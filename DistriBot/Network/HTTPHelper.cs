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
			client = new RestClient("https://microsoft-apiapp5f18a6cbd08e4a60b5d735c9bbf275ff.azurewebsites.net/api/");
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

			// easily add HTTP Headers
			//request.AddHeader("header", "value");

			// execute the request
			//IRestResponse response = client.Execute(request);
			//var content = response.Content; // raw content as string

			// or automatically deserialize result
			// return content type is sniffed but can be explicitly set via RestClient.AddHandler();
			//IRestResponse<Person> response2 = client.Execute<Person>(request);
			//var name = response2.Data.Name;

			// or download and save file to disk
			//client.DownloadData(request).SaveAs(path);

			// easy async support
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

			// async with deserialization
			//var asyncHandle = client.ExecuteAsync<Person>(request, response =>
			//{
			//	Console.WriteLine(response.Data.Name);
			//});
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

	}
}

