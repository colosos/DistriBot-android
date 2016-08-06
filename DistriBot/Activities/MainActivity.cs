using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using Android.Content;

namespace DistriBot
{
	[Activity(Label = "DistriBot", Icon = "@mipmap/icon", Theme = "@style/Theme.AppCompat.Light.NoActionBar.FullScreen")]
	public class MainActivity : AppCompatActivity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

			var btnLogin = FindViewById<Button>(Resource.Id.btnLogin);
            var btnForgotPassword = FindViewById<TextView>(Resource.Id.tvForgotPasswordLink);
            var etUsername = FindViewById<EditText>(Resource.Id.etUsername);
            var etPassword = FindViewById<EditText>(Resource.Id.etPassword);

            btnLogin.Click += delegate
            {
				if (etUsername.Text == "" || etPassword.Text == "")
				{
					Toast.MakeText(this, "El nombre de usuario y/o la contraseña son incorrectos", ToastLength.Long).Show();
				}
				else 
				{
					SalesmanServiceManager.Login(etUsername.Text, etPassword.Text, success: () => 
					{
						var menuActivity = new Intent(this, typeof(MenuActivity));
						StartActivity(menuActivity);
					}, failure: () => 
					{
						Toast.MakeText(this, "El nombre de usuario y/o la contraseña son incorrectos", ToastLength.Long).Show();	
					});
				}
            };

            btnForgotPassword.Click += delegate
            {
                Toast.MakeText(this, "¡Qué lástima!", ToastLength.Short).Show();
            };

        }
	}
}


