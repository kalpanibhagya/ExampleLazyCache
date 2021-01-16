using System;
using System.Collections.Generic;
using System.Text;

namespace ExampleLazyCache
{
	public interface IProductCache
	{
		abstract void InitRest();
		abstract Product GetProduct(string id);
	}
}
