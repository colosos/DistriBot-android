using System;
using System.Json;


namespace DistriBot
{
	public class SalesmanServiceManager
	{
		
		public SalesmanServiceManager()
		{
		}

		public static void Login(string username, string password, Action success, Action failure)
		{
			string relativeUrl = "login";
			string grant_type = "password";
			JsonObject parameters = new JsonObject();
			parameters.Add("username", username);
			parameters.Add("password", password);
			parameters.Add("grant_type", grant_type);
			HTTPHelper.GetInstance().PostRequest(relativeUrl, parameters, success: (obj) =>
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

