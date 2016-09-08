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
			return prefs.GetString("distribot-token-session", "DEFAULT");
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
			return prefs.GetString("token-type", "DEFAULT");
		}

		public static bool IsUserLoggedIn()
		{
			return GetSessionToken() != null && GetSessionToken() != "" && GetSessionToken() != "DEFAULT";
		}
	}
}
