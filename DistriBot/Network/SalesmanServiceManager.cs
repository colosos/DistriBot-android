using System;
using System.Json;

namespace DistriBot
{
	public class SalesmanServiceManager
	{
		private static string relativeUrl;

		public SalesmanServiceManager()
		{
			relativeUrl = "Salesmen";
		}

		//Tal vez este metodo debe ir en otra clase ya que por ahora no es solo para el vendedor
		//Todos los usuarios estan usando el mismo login actualmente
		public static void Login(string username, string password, Action success, Action failure)
		{
			string urlPath = "login";

			HTTPHelper.GetInstance().PostLoginRequest(urlPath, username, password, success: (obj) =>
			{
				var token = obj["access_token"];
				var tokenType = obj["token_type"];
				SessionManager.SaveTokenSession(token);
				SessionManager.SaveTokenType(tokenType);
				success();

			}, failure: (obj) =>
			{
				failure();
			});
		}
	}
}
