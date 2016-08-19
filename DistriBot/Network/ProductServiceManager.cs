using System;
using System.Collections.Generic;

namespace DistriBot
{
	public class ProductServiceManager
	{
		private static string relativeUrl = "Products";

		public static void GetProducts(int page, Action<List<Product>> success, Action<string> failure)
		{
			HTTPHelper.GetInstance().GetRequest(relativeUrl, null, success: (obj) =>
			{
				success(Product.ProductsFromJson(obj));
			}, failure: (obj) =>
			{
				failure("Error message");
			});
		}
	}
}
