using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;

namespace DistriBot
{
	[Activity(Label = "Productos", MainLauncher = true, Theme = "@style/DefaultTheme")]
	public class ProductsActivity : AppCompatActivity
	{
		ListView listView;
		List<Product> products = new List<Product>()
		{
			new Product(1, "Salame", 12.3),
			new Product(2, "Queso", 34),
			new Product(3, "Azucar", 53),
			new Product(4, "Agua", 3.11)
		};

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.Products);

			listView = FindViewById<ListView>(Resource.Id.List);

			listView.Adapter = new ProductsAdapter(this, products);
			listView.ItemClick += OnListItemClick;
		}

		void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
		{
			var p = products[e.Position];
			Android.Widget.Toast.MakeText(this, p.Name, Android.Widget.ToastLength.Short).Show();
		}

		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			MenuInflater.Inflate(Resource.Menu.home, menu);
			return base.OnCreateOptionsMenu(menu);
		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			Toast.MakeText(this, "Top ActionBar pressed: " + item.TitleFormatted, ToastLength.Short).Show();
			return base.OnOptionsItemSelected(item);
		}
	}
}

