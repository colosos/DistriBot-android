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
	}
}

