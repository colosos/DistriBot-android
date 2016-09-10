using System;
using Android.Preferences;
using Android.Content;
using Android.App;

namespace DistriBot
{
	public class SessionManager
	{
		public SessionManager()
		{
		}

		public static void SaveTimestamp(DateTime date)
		{
			ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(Application.Context);
			ISharedPreferencesEditor editor = prefs.Edit();
			editor.PutLong("login-timestamp", date.Ticks);
			editor.Apply();
		}

		public static DateTime GetTimestamp()
		{
			ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(Application.Context);
			return new DateTime(prefs.GetLong("login-timestamp", 0));
		}

		public static void SaveTokenSession(string token)
		{
			ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(Application.Context);
			ISharedPreferencesEditor editor = prefs.Edit();
			editor.PutString("distribot-token-session", token);
			editor.Apply();
		}

		public static string GetSessionToken()
		{
			ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(Application.Context);
			return prefs.GetString("distribot-token-session", "");
		}

		public static void SaveTokenType(string tokenType)
		{
			ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(Application.Context);
			ISharedPreferencesEditor editor = prefs.Edit();
			editor.PutString("token-type", tokenType);
			editor.Apply();
		}

		public static string GetTokenType()
		{
			ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(Application.Context);
			return prefs.GetString("token-type", "");
		}

		public static bool IsUserLoggedIn()
		{
			TimeSpan diff = DateTime.Now.Subtract(GetTimestamp());
			bool tokenValid = diff.Days < 1;
			return GetSessionToken() != null && GetSessionToken() != "" && tokenValid;
		}
	}
}
