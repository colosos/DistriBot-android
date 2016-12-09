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
		public List<Product> ProductsHelper { get; set; }
		public bool Delivered { get; set; }

		public Order()
		{
			Products = new List<Tuple<int, double, double>>();
			ProductsHelper = new List<Product>();
		}

		public static Order OrderFromJson(JsonValue json)
		{
			int id = json["id"];
			double price = json["price"];
			Client client = Client.ClientFromJson(json["client"]);
			List<Tuple<int, double, double>> products = ProductsListFromJson(json["productsList"]);
			bool delivered = false;
			if (json["deliveredDate"] != null)
			{
				delivered = true;
			}

			Order order = new Order();
			order.Id = id;
			order.Client = client;
			order.Price = price;
			order.Delivered = delivered;
			foreach (Tuple<int, double, double> product in products)
			{
				order.Products.Add(product);
			}
			return order;
		}

		public static Order EagerOrderFromJson(JsonValue json)
		{
			int id = json["id"];
			double price = json["price"];
			Client client = Client.ClientFromJson(json["client"]);
			List<Tuple<int, double, double>> products = ProductsListFromJson(json["productsList"]);
			List<Product> productsHelper = ProductsFromJson(json["productsList"]);
			bool delivered = false;
			if (json["deliveredDate"] != null)
			{
				delivered = true;
			}

			Order order = new Order();
			order.Id = id;
			order.Client = client;
			order.Price = price;
			order.Products.AddRange(products);
			order.ProductsHelper.AddRange(productsHelper);
			order.Delivered = delivered;
			return order;
		}

		public static Order LazyOrderFromJson(JsonValue json)
		{
			int id = json["id"];
			double price = json["price"];
			Client client = Client.ClientFromJson(json["client"]);
			bool delivered = false;
			if (json["deliveredDate"] != null)
			{
				delivered = true;
			}

			Order order = new Order();
			order.Id = id;
			order.Client = client;
			order.Price = price;
			order.Products = new List<Tuple<int, double, double>>();
			order.Delivered = delivered;
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

		public static List<Order> LazyOrdersFromJson(JsonValue jsonArray)
		{
			List<Order> orders = new List<Order>();
			foreach (JsonValue json in jsonArray)
			{
				orders.Add(LazyOrderFromJson(json));
			}
			return orders;
		}

		public static List<Order> EagerOrdersFromJson(JsonValue jsonArray)
		{
			List<Order> orders = new List<Order>();
			foreach (JsonValue json in jsonArray)
			{
				orders.Add(EagerOrderFromJson(json));
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

		private static List<Product> ProductsFromJson(JsonValue jsonArray)
		{
			List<Product> products = new List<Product>();
			foreach (JsonValue json in jsonArray)
			{
				JsonValue product = json["product"];
				int id = product["id"];
				string name = product["name"];
				string description = product["description"];
				double price = product["price"];
				string unit = product["unit"];

				products.Add(new Product(id, name, price, description, unit));
			}
			return products;
		}
	}
}