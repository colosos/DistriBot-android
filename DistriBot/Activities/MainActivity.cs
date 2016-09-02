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

			if (SessionManager.IsUserLoggedIn())
			{
				var menuActivity = new Intent(this, typeof(MenuActivity));
				StartActivity(menuActivity);
			}

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
				var progressDialog = ProgressDialog.Show(this, "", "Autenticando", true);
				progressDialog.SetProgressStyle(ProgressDialogStyle.Spinner);
				new Thread(new ThreadStart(delegate
				{
					LoginServiceManager.Login(etUsername.Text, etPassword.Text, success: (role) =>
					{
						if (role.Equals("salesmen"))
						{
							RunOnUiThread(() =>
							{
								var menuActivity = new Intent(this, typeof(MenuActivity));
								StartActivity(menuActivity);
								progressDialog.Dismiss();
							});
						}
						else
						{
							// TODO: Redirect to Deliveryman menu
						}
					}, failure: () =>
					{
						RunOnUiThread(() =>
						{
							progressDialog.Dismiss();
							Toast.MakeText(this, "El nombre de usuario y/o la contraseña son incorrectos", ToastLength.Long).Show();
						});
					});
				})).Start();
			}
		}
	}
}