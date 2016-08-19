using System;
using System.Collections.Generic;
using System.Json;

namespace DistriBot
{
	public class Client
	{

		public string Name { get; set; }
		public double Latitude { get; set; }
		public double Longitude { get; set; }

		public Client(string name, double latitude, double longitude)
		{
			Name = name;
			Latitude = latitude;
			Longitude = longitude;
		}

		public static List<Client> ClientsFromJson(JsonValue jsonArray)
		{
			List<Client> clients = new List<Client>();
			foreach (JsonValue json in jsonArray)
			{
				string name = json["name"];
				double latitude = json["latitude"];
				double longitude = json["longitude"];
				clients.Add(new Client(name, latitude, longitude));
			}
			return clients;
		}
	}
}

