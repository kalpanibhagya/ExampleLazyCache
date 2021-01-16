using LazyCache;
using LazyCache.Providers;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;


namespace ExampleLazyCache
{
	public class Product
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public decimal Price { get; set; }

		public Product(string id, string name, decimal price)
		{
			Id = id;
			Name = name;
			Price = price;
		}

	}
	public class ProductCache
	{
		IAppCache cache = new CachingService();
		public List<Product> dummyData = new List<Product>();

		public void InitResetCache()
		{
			cache.CacheProvider.Dispose();

			var provider = new MemoryCacheProvider(
							new MemoryCache(
							   new MemoryCacheOptions()));

			
			cache = new CachingService(provider);

			dummyData = new List<Product>();
			dummyData.Add(new Product("product-01", "High Back Leather Chair", 34475.00m));
			dummyData.Add(new Product("product-02", "Mid Back Leather Chair", 24475.00m));
			dummyData.Add(new Product("product-03", "Low Back Leather Chair", 14475.00m));
			dummyData.Add(new Product("product-04", "Visitor Chair", 24400.00m));
			dummyData.Add(new Product("product-05", "Lecture hall chair", 7475.00m));

		}

		public Product checkCache(string productId)
		{
			Func<Product> loadedProduct = () => LoadProduct(productId);

			Product cachedResult = cache.GetOrAdd(productId, loadedProduct, DateTimeOffset.UtcNow.AddMinutes(15));

			return cachedResult;
		}

		
		public Product LoadProduct(string productId)
		{
			foreach (Product p in dummyData)
			{
				if(p.Id == productId)
				{
					return p;
				}
			}

			return null;
		}

	}

	public class ProductHandler
	{
		public ProductCache productCache = new ProductCache();
		public Product GetProduct(string id)
		{
			return productCache.checkCache(id);
		}

		public void InitReset()
		{
			productCache.InitResetCache();
		}
	}


	public class Program
	{
		public static void Main(string[] args)
		{
			ProductHandler productHandler = new ProductHandler();
			productHandler.InitReset();
				
			Product product1 = productHandler.GetProduct("product-03");
			Product product2 = productHandler.GetProduct("product-01");

			Console.WriteLine("Product Name : " + product1.Name + "    Product Price : " + product1.Price);
			Console.WriteLine("Product Name : " + product2.Name + "    Product Price : " + product2.Price);
		}
	}
}
