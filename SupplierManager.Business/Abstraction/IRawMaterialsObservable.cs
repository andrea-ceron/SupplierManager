using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupplierManager.Business.Abstraction;

public interface IRawMaterialsObservable
{
	IObservable<int> AddRawMaterial { get; }

}
