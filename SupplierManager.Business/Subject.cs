using SupplierManager.Business.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace SupplierManager.Business;

public class Subject : ISubject
{
	private Subject<int> addRawMaterial { get; } = new Subject<int>();
	public IObserver<int> AddRawMaterial => addRawMaterial;
	IObservable<int> IRawMaterialsObservable.AddRawMaterial => addRawMaterial;
}
