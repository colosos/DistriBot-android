using System;
namespace DistriBot
{
	public class LoginServiceManager
	{
		private static string relativeUrl = "login";

		public LoginServiceManager()
		{
			relativeUrl = "login";
		}

		public static void Login(string username, string password, Action<string> success, Action failure)
		{
			HTTPHelper.GetInstance().PostLoginRequest(relativeUrl, username, password, success: (obj) =>
			{
				var role = obj["role"];
				var token = obj["access_token"];
				var tokenType = obj["token_type"];
				SessionManager.SaveTokenSession(token);
				SessionManager.SaveTokenType(tokenType);
				success(role);
			}, failure: (obj) =>
			{
				failure();
			});	
		}
	}
}
