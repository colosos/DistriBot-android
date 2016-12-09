using System;
using System.Collections.Generic;
using System.Json;

namespace DistriBot
{
	public class OrderServiceManager
	{
		private static string relativeUrl = "Orders";

		public static void GetOrders(Action<List<Order>> success, Action<string> failure)
		{
			HTTPHelper.GetInstance().GetRequest(relativeUrl, null, success: (obj) =>
			{
				success(Order.OrdersFromJson(obj));
			}, failure: (obj) =>
			{
				failure("Failed to get orders");
			});
		}

		public static void GetOrder(int id, Action<Order> success, Action<string> failure)
		{
			string url = relativeUrl + "/" + id;
			HTTPHelper.GetInstance().GetRequest(url, null, success: (obj) =>
			{
				success(Order.EagerOrderFromJson(obj));
			}, failure: (obj) =>
			{
				failure("Error al cargar el pedido");
			});
		}

		public static void GetOrdersToDeliver(string deliveryman, Action<List<Order>> success, Action<string> failure)
		{
			string url = "getOrdersForDeliveryMan?username=" + deliveryman;
			HTTPHelper.GetInstance().GetRequest(url, null, success: (obj) =>
			{
				success(Order.EagerOrdersFromJson(obj));
			}, failure: (obj) =>
			{
				failure("Error al cargar los pedidos");
			});
		}

		public static void GetOrdersBySalesman(string salesman, Action<List<Order>> success, Action<string> failure)
		{
			string url = "OrdersBySalesman?nameSalesman=" + salesman;
			HTTPHelper.GetInstance().GetRequest(url, null, success: (obj) =>
			{
				success(Order.LazyOrdersFromJson(obj));
			}, failure: (obj) =>
			{
				failure("Error al cargar los pedidos");
			});
		}

		public static void DeliverOrder(Order order, Action success, Action failure)
		{
			string url = "deliverOrder?idOrder=" + order.Id + "&flag=true";
			HTTPHelper.GetInstance().PostDeliverOrderRequest(url, success: () =>
			{
				success();
			}, failure: () =>
			{
				failure();
			});
		}

		public static void EditOrder(Order order, Action success, Action failure)
		{
			
		}

		public static void DeleteOrder(int id, Action success, Action failure)
		{
			string url = relativeUrl + "/" + id;
			HTTPHelper.GetInstance().DeleteRequest(url, null, success: (obj) =>
			{
				success();
			}, failure: (obj) =>
			{
				failure();
			});
		}

		public static void AddOrder(Order order, Action success, Action failure)
		{
			var salesman = SessionManager.GetSalesmanUsername();
			List<Tuple<int, double>> products = new List<Tuple<int, double>>();
			foreach (Tuple<int, double, double> tuple in order.Products)
			{
				Tuple<int, double> product = new Tuple<int, double>(tuple.Item1, tuple.Item2);
				products.Add(product);
			}
			HTTPHelper.GetInstance().PostOrderRequest(relativeUrl, BuildOrderJson(order.Client.Id, products, salesman, order.Price),
													  success: () =>
			{
				success();
			}, failure: () =>
			{
				failure();
			});
		}

		private static JsonValue BuildOrderJson(int clientId, List<Tuple<int, double>> products, string salesman, double price)
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
			JsonObject parameters = new JsonObject();
			parameters.Add("client", new JsonObject(new KeyValuePair<string, JsonValue>("id", clientId)));
			parameters.Add("productsList", productList);
			parameters.Add("salesman", new JsonObject(new KeyValuePair<string, JsonValue>("username", salesman)));
			parameters.Add("price", price);

			return parameters;
		}
	}
}

