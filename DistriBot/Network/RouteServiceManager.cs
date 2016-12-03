using System;
using System.Collections.Generic;
using System.Json;

namespace DistriBot
{
	public class RouteServiceManager
	{
		public RouteServiceManager()
		{
		}

		public static void GetRouteClients(string deliveryman, Action<List<Client>> success, Action<string> failure)
		{
			int day = Convert.ToInt32(DateTime.Today.DayOfWeek);
			string url = "getRouteForDeliveryMan";
			url += "?username=" + deliveryman;
			url += "&dayOfWeek=" + day;
			HTTPHelper.GetInstance().GetRequest(url, null, success: (obj) =>
			{
				success(Route.RouteFromJson(obj));
			}, failure: (obj) =>
			{
				failure("Failed to get route clients");
			});
		}

		public static void GetRouteParameter(Action<int> success, Action<string> failure)
		{
			string url = "Parms?id=AUTOMATIC_ROUTE";
			HTTPHelper.GetInstance().GetRequest(url, null, success: (obj) =>
			{
				int parameter = obj["value"];
				success(parameter);
			}, failure: (obj) =>
			{
				failure("Error al cargar el parámetro");
			});
		}
	}
}

