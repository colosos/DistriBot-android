
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using SupportFragment = Android.Support.V4.App.Fragment;

namespace DistriBot
{
	[Activity(Label = "Lista de pedidos", Theme = "@style/DefaultTheme")]
	public class DeliverymanMenuActivity : AppCompatActivity
	{

		private OrdersListFragment ordersListFragment;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.DeliverymanMenu);
			ordersListFragment = new OrdersListFragment();
			var trans = SupportFragmentManager.BeginTransaction();
			trans.Add(Resource.Id.fragmentContainer, ordersListFragment, "OrdersListFragment");
			trans.Show(ordersListFragment);
			trans.Commit();
		}
	}
}

