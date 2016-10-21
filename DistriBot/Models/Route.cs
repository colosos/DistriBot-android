using System;
using System.Collections.Generic;
using System.Json;

namespace DistriBot
{
	public class Route
	{

		public List<Client> Clients { get; set; }
		public List<Product> Products { get; set; }
		public List<Order> Orders { get; set; }

		public Route()
		{
			Clients = new List<Client>();
			Products = new List<Product>();
			Orders = new List<Order>();

			// Add Clients
			Client c1 = new Client(1, "Tienda Inglesa", -34.882441, -56.081933, "Av Italia 5820", "26011111", "tinglesa@gmail.com", 10000);
			Client c2 = new Client(2, "Universidad ORT", -34.912883, -56.156804, "Bv España 2633", "27071806", "universidad@ort.com", 20000);
			Client c3 = new Client(3, "IMM", -34.905598, -56.186256, "Av. 18 de Julio 1360", "27111212", "imm@gmail.com", -50000);
			Clients.Add(c1);
			Clients.Add(c2);
			Clients.Add(c3);

			// Add Products
			Product p1 = new Product(1, "Manzana Roja", 10, "Manzana", "Kg");
			Product p2 = new Product(2, "Jamon crudo", 100, "Jamon", "Kg");
			Product p3 = new Product(4, "Cerveza Patricia", 200, "Cerveza", "Lt");
			Products.Add(p1);
			Products.Add(p2);
			Products.Add(p3);

			// Add Orders
			Order o1 = new Order();
			o1.Id = 1;
			o1.Client = c1;
			o1.Price = 500;
			o1.Products.Add(new Tuple<int, double, double>(1, 50, 500));
			Order o2 = new Order();
			o2.Id = 2;
			o2.Client = c2;
			o2.Price = 100;
			o2.Products.Add(new Tuple<int, double, double>(2, 1, 100));
			Order o3 = new Order();
			o3.Id = 3;
			o3.Client = c3;
			o3.Price = 400;
			o3.Products.Add(new Tuple<int, double, double>(3, 2, 400));
			Orders.Add(o1);
			Orders.Add(o2);
			Orders.Add(o3);
		}

		public static string RouteFromJson(JsonValue json)
		{
			JsonValue routes = json["routes"];
			JsonValue overview_polyline = routes[0]["overview_polyline"];
			return overview_polyline["points"];
		}
	}
}

