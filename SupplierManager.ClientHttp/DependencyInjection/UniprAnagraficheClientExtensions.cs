

using Microsoft.Extensions.Configuration;
using SupplierManager.ClientHttp;
using SupplierManager.ClientHttp.Abstraction;

namespace Microsoft.Extensions.DependencyInjection;

public static class UniprAnagraficheClientExtensions {

    public static IServiceCollection AddUniprAnagraficheClient(this IServiceCollection services, IConfiguration configuration) {

        IConfigurationSection confSection = configuration.GetSection(SupplierManagerClientOptions.SectionName);
		SupplierManagerClientOptions options = confSection.Get<SupplierManagerClientOptions>() ?? new();

        services.AddHttpClient<IClientHttp, ClientHttp>(o => {          
            o.BaseAddress = new Uri(options.BaseAddress);
        }).ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler {
            ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
        });

        return services;
    }
}
