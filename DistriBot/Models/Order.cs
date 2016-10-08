using System;
using System.Collections.Generic;
using System.Json;

namespace DistriBot
{
	public class Order
	{
		public int Id { get; set; }
		public int ClientId { get; set; }
		public List<Tuple<int, double, double>> Products { get; set; } // <ProductId, ProductQuantity, Subtotal>
		public double Price { get; set; }

		public Order()
		{
			Products = new List<Tuple<int, double, double>>();
		}

		public static Order OrderFromJson(JsonValue json)
		{
			int id = json["id"];
			return new Order();
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
	}
}


/*
 Model Order en backend:
 Id, Client, Creation date, DeliveredDate, ProductsList, Price, Salesman
*/
