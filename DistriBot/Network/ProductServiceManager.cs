using System;
using System.Collections.Generic;

namespace DistriBot
{
	public class ProductServiceManager
	{

		public static void GetProducts(int desde, int cantidad, Action<List<Product>> success, Action<string> failure)
		{
			string url = "Products?desde=" + desde + "&cantidad=" + cantidad;
			HTTPHelper.GetInstance().GetRequest(url, null, success: (obj) =>
			{
				success(Product.ProductsFromJson(obj, false));
			}, failure: (obj) =>
			{
				failure("Error message");
			});
		}

		public static void GetRecommendedProducts(int clientId, Action<List<Product>> success, Action<string> failure)
		{
			string url = "getRecommendations?CliId=" + clientId;
			HTTPHelper.GetInstance().GetRequest(url, null, success: (obj) =>
			{
				success(Product.ProductsFromJson(obj, true));
			}, failure: (obj) =>
			{
				failure("Error al cargar los productos");
			});
		}

		public static void GetRecommendedProductsFromProduct(int productId, Action<List<Product>> success, Action<string> failure)
		{
			string url = "getRecommendations?CliId=" + productId;
			HTTPHelper.GetInstance().GetRequest(url, null, success: (obj) =>
			{
				success(Product.ProductsFromJson(obj, true));
			}, failure: (obj) =>
			{
				failure("Error al cargar los productos");
			});
		}
	}
}
