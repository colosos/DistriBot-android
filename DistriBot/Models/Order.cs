using System;
using System.Collections.Generic;
using System.Json;

namespace DistriBot
{
	public class Order
	{
		public int Id { get; set; }
		public Client Client { get; set; }
		public List<Tuple<int, double, double>> Products { get; set; } // <ProductId, ProductQuantity, Subtotal>
		public double Price { get; set; }

		public Order()
		{
			Products = new List<Tuple<int, double, double>>();
		}

		public static Order OrderFromJson(JsonValue json)
		{
			int id = json["id"];
			double price = json["price"];
			Client client = Client.ClientFromJson(json["client"]);
			List<Tuple<int, double, double>> products = ProductsListFromJson(json["productsList"]);

			Order order = new Order();
			order.Id = id;
			order.Client = client;
			order.Price = price;
			foreach (Tuple<int, double, double> product in products)
			{
				order.Products.Add(product);
			}
			return order;
		}

		public static List<Order> OrdersFromJson(JsonValue jsonArray)
		{
			List<Order> orders = new List<Order>();
			foreach (JsonValue json in jsonArray)
			{
				orders.Add(OrderFromJson(json));
			}
			return orders;
		}

		private static List<Tuple<int, double, double>> ProductsListFromJson(JsonValue jsonArray)
		{
			List<Tuple<int, double, double>> products = new List<Tuple<int, double, double>>();
			foreach (JsonValue json in jsonArray)
			{
				double quantity = json["quantity"];
				JsonValue product = json["product"];
				int id = product["id"];
				double price =  product["price"];
				double subtotal = quantity * price;
				products.Add(new Tuple<int, double, double>(id, quantity, subtotal));
			}
			return products;
		}
	}
}