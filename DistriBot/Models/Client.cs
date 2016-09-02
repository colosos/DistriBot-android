using System;
using System.Collections.Generic;
using System.Json;

namespace DistriBot
{
	public class Client
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public double Latitude { get; set; }
		public double Longitude { get; set; }
		public string Address { get; set; }
		public string Phone { get; set; }
		public string Email { get; set; }
		public double CreditBalance { get; set; }

		public Client(int id, string name, double latitude, double longitude, string address,
		              string phone, string email, double creditBalance)
		{
			Id = id;
			Name = name;
			Latitude = latitude;
			Longitude = longitude;
			Address = address;
			Phone = phone;
			Email = email;
			CreditBalance = creditBalance;
		}

		public static List<Client> ClientsFromJson(JsonValue jsonArray)
		{
			List<Client> clients = new List<Client>();
			foreach (JsonValue json in jsonArray)
			{
				clients.Add(ClientFromJson(json));
			}
			return clients;
		}

		public static Client ClientFromJson(JsonValue json)
		{
			int id = json["id"];
			string name = json["name"];
			double latitude = json["latitude"];
			double longitude = json["longitude"];
			string address = json["address"];
			string phone = json["phone"];
			string email = json["emailAddress"];
			double creditBalance = json["creditBalance"];
			return new Client(id, name, latitude, longitude, address, phone, email, creditBalance);
		}
	}
}
