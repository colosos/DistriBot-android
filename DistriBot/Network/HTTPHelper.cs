using System;
using System.Collections.Generic;
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
			client = new RestClient("http://distribotapi20161029125815.azurewebsites.net/api/");
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
			request.AddHeader("Accept", "application/json");
			request.AddHeader("Authorization", GetFormattedToken());
			request.AddHeader("Content-Type", "application/json");
			if (parameters != null)
			{
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

		public void GetDeliveryRoute(List<Tuple<string, string>> list, Tuple<string, string> initialPosition, Action<JsonValue> success, Action<JsonValue> failure)
		{
			string url = "json?origin=" + initialPosition.Item1 + "," + initialPosition.Item2;
			url += "&destination=" + initialPosition.Item1 + "," + initialPosition.Item2;
			url += "&waypoints=optimize:true|";
			int count = list.Count;
			foreach (Tuple<string, string> waypoint in list)
			{
				url += waypoint.Item1 + "," + waypoint.Item2;
				count -= 1;
				if (count != 0)
				{
					url += "|";
				}	
			}
			url += "&key=AIzaSyAPHhXRQMct1vIrgE-9kQNjlmCFnH0yLNU";
			client = new RestClient("https://maps.googleapis.com/maps/api/directions/");
			RestRequest request = new RestRequest(url, Method.GET);
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


		public void PostOrderRequest(string relativeUrl, JsonValue parameters, Action success, Action failure)
		{
			RestRequest request = new RestRequest(relativeUrl, Method.POST);

			request.AddHeader("Accept", "application/json");
			request.AddHeader("Authorization", GetFormattedToken());
			request.AddHeader("Content-Type", "application/json");
			request.Parameters.Clear();
			request.AddParameter("application/json; charset=utf-8", parameters, ParameterType.RequestBody);
			request.RequestFormat = DataFormat.Json;

			client.ExecuteAsync(request, response =>
			{
				if ((int)response.StatusCode >= 100 && (int)response.StatusCode <= 600)
				{
					if ((int)response.StatusCode >= 200 && (int)response.StatusCode <= 210)
					{
						success();
					}
					else
					{
						failure();
					}
				}
			});
		}

	}
}
