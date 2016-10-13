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
		public string ImageUrlV1 { get; set; }
		public string ImageUrlV2 { get; set; }

		public Product(int id, string name, double unitPrice, string description, string unit)
		{
			Id = id;
			Name = name;
			UnitPrice = unitPrice;
			MeasurementUnit = unit;
			Description = description;
			ImageUrlV1 = "https://storagedistribot.blob.core.windows.net/clients/" + id + "/v1";
			ImageUrlV1 = "https://storagedistribot.blob.core.windows.net/clients/" + id + "/v2";
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
