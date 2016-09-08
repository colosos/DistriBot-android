using System;
using System.Collections.Generic;

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
	}
}
