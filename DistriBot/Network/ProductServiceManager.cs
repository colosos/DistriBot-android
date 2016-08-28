using System;
using System.Collections.Generic;

namespace DistriBot
{
	public class ProductServiceManager
	{
		private static string relativeUrl = "Products";

		public static void GetProducts(int desde, int cantidad, Action<List<Product>> success, Action<string> failure)
		{
			relativeUrl += "?desde=" + desde +"&cantidad=" + cantidad;
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
