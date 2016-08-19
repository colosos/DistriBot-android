using System;
using System.Json;
using System.Collections.Generic;

namespace DistriBot
{
	public class Product
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public double UnitPrice { get; set; }

		public Product(int id, string name, double unitPrice)
		{
			this.Id = id;
			this.Name = name;
			this.UnitPrice = unitPrice;
		}

		public static List<Product> ProductsFromJson(JsonValue jsonArray)
		{
			List<Product> products = new List<Product>();
			foreach (JsonValue json in jsonArray)
			{
				int id = json["id"];
				string name = json["name"];
				double price = json["price"];

				products.Add(new Product(id, name, price));
			}

			return products;
		}
	}
}
