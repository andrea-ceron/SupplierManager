﻿using CustomerManager.Repository.Model;
using Kafka.Utility.Abstractions.Clients;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SupplierManager.Repository.Abstraction;


namespace SupplierManager.Business.Kafka;
public class ProducerServiceWithSubscription(
    ILogger<KafkaUtility.Services.ProducerServiceWithSubscription> logger
    , IProducerClient<string, string> producerClient
    , IOptions<KafkaTopicsOutput> optionsTopics
    , IServiceProvider serviceProvider
    , IServiceScopeFactory serviceScopeFactory
    , IUniprPagamentiObservable observable)
    : KafkaUtility.Services.ProducerServiceWithSubscription(logger, serviceProvider)
{

    /// <summary>
    /// Effettuiamo la subscribe dell'observable NuovoTransactionalOutbox: ogni qualvolta
    /// viene inoltrata una notifica tramite l'observer <see cref="IUniprPagamentiObserver"/>
    /// eseguiamo l'azione definita, ovvero marchiamo come completato il tcs invocando il
    /// metodo TrySetResult()
    /// </summary>
    /// <param name="tcs"></param>
    /// <returns></returns>
    protected override IDisposable Subscribe(TaskCompletionSource tcs)
    {
        return observable.NuovoVersamento.Subscribe((change) => tcs.TrySetResult());
    }

    /// <inheritdoc/>
    protected override IEnumerable<string> GetTopics()
    {
        return optionsTopics.Value.GetTopics();
    }

    protected override async Task OperationsAsync(CancellationToken cancellationToken) {
        using IServiceScope scope = serviceScopeFactory.CreateScope();
        IRepository repository = scope.ServiceProvider.GetRequiredService<IRepository>();

        logger.LogInformation("Acquisizione dei TransactionalOutbox da elaborare...");
        IEnumerable<TransactionalOutbox> transactionalOutboxList = (await repository.GetAllTransactionalOutboxAsync(cancellationToken)).OrderBy(x => x.Id);
        if (!transactionalOutboxList.Any()) {
            logger.LogInformation($"Non ci sono TransactionalOutbox da elaborare");
            return;
        }

        logger.LogInformation("Ci sono {Count} TransactionalOutbox da elaborare", transactionalOutboxList.Count());

        foreach (TransactionalOutbox tran in transactionalOutboxList) {
            string groupMsg = $"del record {nameof(TransactionalOutbox)} con " +
                    $"{nameof(TransactionalOutbox.Id)} = {tran.Id}, " +
                    $"{nameof(TransactionalOutbox.Tabella)} = '{tran.Tabella}' e " +
                    $"{nameof(TransactionalOutbox.Messaggio)} = '{tran.Messaggio}'";

            logger.LogInformation("Elaborazione {groupMsg}...", groupMsg);

            try {

                logger.LogInformation("Determinazione del topic...");
                string topic = tran.Tabella switch {
                    nameof(Versamento) => optionsTopics.Value.Versamenti,
                    _ => throw new ArgumentOutOfRangeException($"La tabella {tran.Tabella} non è prevista come topic nel Producer")
                };

                logger.LogInformation("Scrittura del messaggio Kafka sul topic '{topic}'...", topic);
                await producerClient.ProduceAsync(topic, tran.Id.ToString(), tran.Messaggio, cancellationToken);

                logger.LogInformation("Eliminazione {groupMsg}...", groupMsg);
                await repository.DeleteTransactionalOutboxAsync(tran.Id, cancellationToken);

                await repository.SaveChanges(cancellationToken);
                logger.LogInformation("Record eliminato");

            } catch (Exception ex) {
                logger.LogError(ex, "Si è verificata un'eccezione nel metodo ProducerService.OperationsAsync durante l'elaborazione {groupMsg}: {ex}", groupMsg, ex);
            }
        }
    }
}
