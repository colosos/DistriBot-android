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
		public string MeasurementUnit { get; set; }
		public string Description { get; set; }

		public Product(int id, string name, double unitPrice, string description, string unit)
		{
			this.Id = id;
			this.Name = name;
			this.UnitPrice = unitPrice;
			this.MeasurementUnit = unit;
			this.Description = description;
		}

		public static List<Product> ProductsFromJson(JsonValue jsonArray)
		{
			List<Product> products = new List<Product>();
			foreach (JsonValue json in jsonArray)
			{
				int id = json["id"];
				string name = json["name"];
				string description = json["description"];
				double price = json["price"];
				string unit = json["unit"];

				products.Add(new Product(id, name, price, description, unit));
			}

			return products;
		}
	}
}
