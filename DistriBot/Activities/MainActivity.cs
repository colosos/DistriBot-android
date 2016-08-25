using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using Android.Content;
using System.Threading;

namespace DistriBot
{
	[Activity(Label = "DistriBot", MainLauncher = true, Icon = "@mipmap/icon", Theme = "@style/Theme.AppCompat.Light.NoActionBar.FullScreen")]
	public class MainActivity : AppCompatActivity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);

			var btnLogin = FindViewById<Button>(Resource.Id.btnLogin);
            var btnForgotPassword = FindViewById<TextView>(Resource.Id.tvForgotPasswordLink);

			btnLogin.Click += BtnLogin_Click;
            
            btnForgotPassword.Click += delegate
            {
                Toast.MakeText(this, "¡Qué lástima!", ToastLength.Short).Show();
            };

        }

		void BtnLogin_Click(object sender, System.EventArgs e)
		{
			var etUsername = FindViewById<EditText>(Resource.Id.etUsername);
			var etPassword = FindViewById<EditText>(Resource.Id.etPassword);

			if (etUsername.Text == "" || etPassword.Text == "")
			{
				Toast.MakeText(this, "El nombre de usuario y/o la contraseña son incorrectos", ToastLength.Long).Show();
			}
			else
			{
				


				LoginServiceManager.Login(etUsername.Text, etPassword.Text, success: (role) =>
				{
					if (role.Equals("Salesman"))
					{
						var menuActivity = new Intent(this, typeof(MenuActivity));
						StartActivity(menuActivity);
					}
					else
					{
						// TODO: Redirect to Deliveryman menu
					}
				}, failure: () =>
				{
					Toast.MakeText(this, "El nombre de usuario y/o la contraseña son incorrectos", ToastLength.Long).Show();
				});

			}
		}
	}
}