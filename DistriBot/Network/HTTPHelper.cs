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

		public void TestDirections(Action<JsonValue> success, Action<JsonValue> failure)
		{
			string latOrigin = "-34.890233";
			string lonOrigin = "-56.121892";
			string latDest = "-34.884319";
			string longDest = "-56.071338";
			string query = "json?origin="+latOrigin+","+lonOrigin+"&destination="+latDest+","+longDest+"&key=AIzaSyAPHhXRQMct1vIrgE-9kQNjlmCFnH0yLNU";
			client = new RestClient("https://maps.googleapis.com/maps/api/directions/");
			RestRequest request = new RestRequest(query, Method.GET);
			request.AddHeader("Authorization", GetFormattedToken());
			request.AddHeader("Accept", "application/json");
			request.AddHeader("Content-Type", "application/json");

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
				request.AddHeader("Authorization", GetFormattedToken());
				request.AddHeader("Content-Type", "application/json");
				request.Parameters.Clear();
				request.AddParameter("application/json; charset=utf-8", parameters, ParameterType.RequestBody);
				request.RequestFormat = DataFormat.Json;
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
			return SessionManager.GetTokenType() + " " + SessionManager.GetSessionToken();
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

		//Esto es un plan B para agregar un pedido.
		public void PostOrderRequest(string relativeUrl, JsonValue productList, JsonValue clientId, double price,Action<JsonValue> success, Action<JsonValue> failure)
		{
			RestRequest request = new RestRequest(relativeUrl, Method.POST);

			request.AddHeader("Content-Type", "application/json");
			request.AddHeader("Authorization", GetFormattedToken());
			request.AddHeader("Accept", "application/json");
			request.AddParameter("client", clientId);
			request.AddParameter("productList", productList);
			request.AddParameter("price", price);
			request.RequestFormat = DataFormat.Json;

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
