using System;
using System.Collections.Generic;
using System.Json;

namespace DistriBot
{
	public class OrderServiceManager
	{
		private static string relativeUrl = "Orders";

		public static void AddOrder(Order order, Action success, Action failure)
		{
			List<Tuple<int, double>> products = new List<Tuple<int, double>>();
			foreach (Tuple<int, double, double> tuple in order.Products)
			{
				Tuple<int, double> product = new Tuple<int, double>(tuple.Item1, tuple.Item2);
				products.Add(product);
			}

			//Esta llamando al plan B, la idea es llamar a PostRequest
			//y pasarle el json body integro.
			HTTPHelper.GetInstance().PostOrderRequest(relativeUrl,
													  BuildProductListJson(products),
													  BuildClientJson(order.ClientId),
													  order.Price,
													  success: (obj) =>
													  {
														  success();
													  },
													  failure: (obj) =>
													  {
														  failure();
			});
		}

		private static JsonValue BuildProductListJson(List<Tuple<int, double>> products)
		{
			JsonArray productList = new JsonArray();
			foreach (Tuple<int, double> product in products)
			{
				KeyValuePair<string, JsonValue> quantity = new KeyValuePair<string, JsonValue>("quantity", product.Item2);
				KeyValuePair<string, JsonValue> productId = new KeyValuePair<string, JsonValue>("id", product.Item1);
				KeyValuePair<string, JsonValue> productRow = new KeyValuePair<string, JsonValue>("product", new JsonObject(productId));
				JsonObject productJson = new JsonObject(productRow);
				productJson.Add(quantity);
				productList.Add(productJson);
			}

			return productList;
		}

		private static JsonValue BuildClientJson(int clientId)
		{
			return new JsonObject(new KeyValuePair<string, JsonValue>("id", clientId));
		}

		//private static JsonValue BuildOrderJson(int clientId, List<Tuple<int, double>> products, double price)
		//{
		//	JsonArray productList = new JsonArray();
		//	foreach (Tuple<int, double> product in products)
		//	{
		//		KeyValuePair<string, JsonValue> quantity = new KeyValuePair<string, JsonValue>("quantity", product.Item2);
		//		KeyValuePair<string, JsonValue> productId = new KeyValuePair<string, JsonValue>("id", product.Item1);
		//		KeyValuePair<string, JsonValue> productRow = new KeyValuePair<string, JsonValue>("product", new JsonObject(productId));
		//		JsonObject productJson = new JsonObject(productRow);
		//		productJson.Add(quantity);
		//		productList.Add(productJson);
		//	}
		//	JsonObject parameters = new JsonObject();
		//	parameters.Add("client", new JsonObject(new KeyValuePair<string, JsonValue>("id", clientId)));
		//	parameters.Add("productList", productList);
		//	parameters.Add("price", price);

		//	return parameters;
		//}
	}
}

