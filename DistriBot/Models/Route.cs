using System;
using System.Collections.Generic;

namespace DistriBot
{
	public class Route
	{

		public Dictionary<Client, Order> clientsOrders { get; set; }

		public Route()
		{
			clientsOrders = new Dictionary<Client, Order>();
		}
	}
}

