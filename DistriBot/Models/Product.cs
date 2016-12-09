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
		public bool Recommended { get; set; }

		public Product(int id, string name, double unitPrice, string description, string unit)
		{
			Id = id;
			Name = name;
			UnitPrice = unitPrice;
			MeasurementUnit = unit;
			Description = description;
			ImageUrlV1 = "https://acanabarrostorage.blob.core.windows.net/products/" + id + "/v1/prod.jpg";
			ImageUrlV1 = "https://acanabarrostorage.blob.core.windows.net/products/" + id + "/v2/prod.jpg";
			Recommended = false;
		}

		public static List<Product> ProductsFromJson(JsonValue jsonArray, bool recommendation)
		{
			List<Product> products = new List<Product>();
			foreach (JsonValue json in jsonArray)
			{
				int id = json["id"];
				string name = json["name"];
				string description = json["description"];
				double price = json["price"];
				string unit = json["unit"];

				Product product = new Product(id, name, price, description, unit);
				if (recommendation)
				{
					product.Recommended = true;
				}
				products.Add(product);
			}

			return products;
		}
	}
}
