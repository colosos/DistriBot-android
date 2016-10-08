using System;
using System.Collections.Generic;

namespace DistriBot
{
	public class CartManager
	{
		private static CartManager instance = null;
		private static readonly object synclock = new object();

		public Order Order { get; set; }
		public List<Product> Products { get; set; }
		public int Salesman { get; set; }

		private CartManager()
		{
			Order = new Order();
			Products = new List<Product>();
		}

		public static CartManager GetInstance()
		{
			if (instance == null)
			{
				lock (synclock)
				{
					if (instance == null)
					{
						instance = new CartManager();
					}

				}
			}
			return instance;
		}

		public void ResetCart()
		{
			this.Order = null;
			this.Products = null;
			this.Salesman = 0;
		}
	}
}

