using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;

namespace DistriBot
{
	[Activity(Label = "DistriBot", MainLauncher = true, Icon = "@mipmap/icon", Theme = "@style/Theme.AppCompat.Light.NoActionBar.FullScreen")]
	public class MainActivity : AppCompatActivity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
			var btnLogin = FindViewById<Button>(Resource.Id.btnLogin);
            var btnForgotPassword = FindViewById<TextView>(Resource.Id.tvForgotPasswordLink);
            var etUsername = FindViewById<EditText>(Resource.Id.etUsername);
            var etPassword = FindViewById<EditText>(Resource.Id.etPassword);

            btnLogin.Click += delegate
            {
                Toast.MakeText(this, string.Format("Usuario: {0} Contraseña: {1}", etUsername.Text, etPassword.Text), ToastLength.Short).Show();
            };

            btnForgotPassword.Click += delegate
            {
                Toast.MakeText(this, "¡Qué lástima!", ToastLength.Short).Show();
            };

            //         btnLogin.Click += delegate { HTTPHelper.GetInstance().GetRequest("getMostActiveUsers", null, success: (obj) =>
            //{
            //	//TODO: Success
            //}, failure: (json) =>
            //{
            //	//TODO: Failure	
            //});
            //};
        }
	}
}


