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
			editor.PutString("token-session", token);
			editor.Apply();
		}

		public static string GetSessionToken()
		{
			ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(Application.Context);
			return prefs.GetString("token-session", "DEFAULT");
		}
	}
}

