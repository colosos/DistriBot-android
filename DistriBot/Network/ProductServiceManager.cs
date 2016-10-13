using System;
using System.Collections.Generic;

namespace DistriBot
{
	public class ProductServiceManager
	{

		public static string ImageUrl = "https://storagedistribot.blob.core.windows.net/clients/";

		public static void GetProducts(int desde, int cantidad, Action<List<Product>> success, Action<string> failure)
		{
			string url = "Products?desde=" + desde + "&cantidad=" + cantidad;
			HTTPHelper.GetInstance().GetRequest(url, null, success: (obj) =>
			{
				success(Product.ProductsFromJson(obj));
			}, failure: (obj) =>
			{
				failure("Error message");
			});
		}
	}
}
