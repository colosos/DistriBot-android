using System;
using System.Json;
using RestSharp;

namespace DistriBot
{
	public class HTTPHelper
	{
		RestClient client;
		private static HTTPHelper instance = null;
		private static readonly object synclock = new object();
		
		private HTTPHelper()
		{
			client = new RestClient("https://microsoft-apiappb014d742205d461bab0e6eaa303215f7.azurewebsites.net/api/");
		}

		public static HTTPHelper GetInstance()
		{
			if (instance == null)
			{
				lock (synclock)
				{
					if (instance == null)
					{
						instance = new HTTPHelper();
					}

				}
			}
			return instance;
		}

		public void DeleteRequest(string relativeUrl, JsonValue parameters, Action<JsonValue> success, Action<JsonValue> failure)
		{
			RestRequest request = new RestRequest(relativeUrl, Method.DELETE);

			if (parameters != null)
			{
				request.AddBody(parameters);
			}

			client.ExecuteAsync(request, response =>
			{
				var json = JsonValue.Parse(response.Content);
				if ((int)response.StatusCode >= 200 && (int)response.StatusCode <= 210)
				{
					success(json);
				}
				else
				{
					failure(json);
				}
			});
		}

		public void GetRequest(string relativeUrl, JsonValue parameters, Action<JsonValue> success, Action<JsonValue> failure)
		{
			RestRequest request = new RestRequest(relativeUrl, Method.GET);
			request.AddHeader("Authorization", GetFormattedToken());
			request.AddHeader("Accept", "application/json");
			request.AddHeader("Content-Type", "application/json");
			if (parameters != null)
			{
				request.AddBody(parameters);
			}

			client.ExecuteAsync(request, response =>
			{
				var json = JsonValue.Parse(response.Content);
				if ((int)response.StatusCode >= 200 && (int)response.StatusCode <= 210)
				{
					success(json);
				}
				else
				{
					failure(json);
				}
			});
		}

		public void PostRequest(string relativeUrl, JsonValue parameters, Action<JsonValue> success, Action<JsonValue> failure)
		{
			RestRequest request = new RestRequest(relativeUrl, Method.POST);

			if (parameters != null)
			{
				request.AddHeader("Accept", "application/json");
				request.Parameters.Clear();
				request.AddParameter("application/json", parameters, ParameterType.RequestBody);
			}
			client.ExecuteAsync(request, response =>
			{
				var json = JsonValue.Parse(response.Content);
				if ((int)response.StatusCode >= 200 && (int)response.StatusCode <= 210)
				{
					success(json);
				}
				else
				{
					failure(json);
				}
			});
		}

		public void PutRequest(string relativeUrl, JsonValue parameters, Action<JsonValue> success, Action<JsonValue> failure)
		{
			RestRequest request = new RestRequest(relativeUrl, Method.PUT);

			if (parameters != null)
			{
				request.AddBody(parameters);
			}

			client.ExecuteAsync(request, response =>
			{
				var json = JsonValue.Parse(response.Content);
				if ((int)response.StatusCode >= 200 && (int)response.StatusCode <= 210)
				{
					success(json);
				}
				else
				{
					failure(json);
				}
			});
		}

		public string GetFormattedToken()
		{
			//return SessionManager.GetTokenType() + " " + SessionManager.GetSessionToken();
			return "bearer beM7jYPiMIwXV8_taF3tCmAso9S2ifJR6-jnPhCztPCYuOQ981T4tCvuNKXF5brdYOzkYKuXttPjdnjIanOKylLFRAGmcuspCwB8Xjjy5-g3UzBEp2XOrigAu7AUcOkOTaPsfZzgomHJsBZxGia1pkLZfOiNX40Vk7MqA8f7PVsS9GoK8s1RuxS1VIV3AGnwitPFg3hZJai24EL_Vt_nJq9xpd-PQQE9tJGyh7p3bO4";
		}

		public void PostLoginRequest(string relativeUrl, string username, string password, Action<JsonValue> success, Action<JsonValue> failure)
		{
			RestRequest request = new RestRequest(relativeUrl, Method.POST);

			request.AddHeader("Accept", "application/json");
			request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
			request.AddParameter("UserName", username);
			request.AddParameter("Password", password);
			request.AddParameter("grant_type","password");

			client.ExecuteAsync(request, response =>
			{
				var json = JsonValue.Parse(response.Content);
				if ((int)response.StatusCode >= 200 && (int)response.StatusCode <= 210)
				{
					success(json);
				}
				else
				{
					failure(json);
				}
			});
		}
	}
}

