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
			string grant_type = "password";
			string relativeUrl = "";
			JsonObject parameters = new JsonObject();
			parameters.Add("username", username);
			parameters.Add("password", password);
			parameters.Add("grant_type", grant_type);
			HTTPHelper.GetInstance().PostRequest(relativeUrl, parameters, success: (obj) =>
			{
				//TODO: Success
			}, failure: (obj) =>
			{
				//TODO: Failure
			});

		}
	}
}

