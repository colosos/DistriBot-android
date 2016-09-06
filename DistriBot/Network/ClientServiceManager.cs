using System;
using System.Collections.Generic;

namespace DistriBot
{
	public class ClientServiceManager
	{

		private static string relativeUrl = "Clients";

		public ClientServiceManager()
		{
		}

		public static void GetClients(Action<List<Client>> success, Action<string> failure)
		{
			HTTPHelper.GetInstance().GetRequest(relativeUrl, null, success: (obj) =>
			{
				success(Client.ClientsFromJson(obj));
			}, failure: (obj) =>
			{
				failure("Failed to get clients");
			});
		}

		public static void GetClientsPaginated(int from, int quantity, Action<List<Client>> success, Action<string> failure)
		{
			string url = relativeUrl + "?desde=" + from + "&cantidad=" + quantity;
			HTTPHelper.GetInstance().GetRequest(url, null, success: (obj) =>
			{
				success(Client.ClientsFromJson(obj));
			}, failure: (obj) =>
			{
				failure("Failed to get clients");
			});
		}

		public static void GetNearestClient(double lat, double lon, Action<Client> success, Action<string> failure)
		{
			string url = relativeUrl + "/nearest?lat=" + lat + "&lon=" + lon;
			HTTPHelper.GetInstance().GetRequest(url, null, success: (obj) =>
			{
				success(Client.ClientFromJson(obj));
			}, failure: (obj) =>
			{
				failure(obj);
			});
		}
	}
}
