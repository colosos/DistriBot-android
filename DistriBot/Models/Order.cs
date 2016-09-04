using System;
using System.Collections.Generic;

namespace DistriBot
{
	public class Order
	{
		public int Id { get; set; }
		public int ClientId { get; set; }
		public List<Tuple<int, double>> Products { get; set; } // <ProductId, ProductQuantity>
		public double Price { get; set; }

		public Order()
		{
		}
	}
}
