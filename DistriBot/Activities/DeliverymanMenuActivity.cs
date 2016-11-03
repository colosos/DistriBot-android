
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

		private SupportFragment currentFragment;
		private LinkedList<SupportFragment> currentStack;

		private OrdersListFragment ordersListFragment;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.DeliverymanMenu);

			var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
			SetSupportActionBar(toolbar);
			SupportActionBar.Hide();

			currentStack = new LinkedList<SupportFragment>();

			ordersListFragment = new OrdersListFragment();
			currentFragment = ordersListFragment;
			var trans = SupportFragmentManager.BeginTransaction();
			trans.Add(Resource.Id.fragmentContainer, ordersListFragment, "OrdersListFragment");
			trans.Show(ordersListFragment);
			trans.Commit();
		}

		public void ShowFragment(SupportFragment fragment, String tag)
		{
			var trans = SupportFragmentManager.BeginTransaction();
			trans.Hide(currentFragment);
			trans.Add(Resource.Id.fragmentContainer, fragment, tag);
			trans.Commit();

			currentStack.AddFirst(currentFragment);
			currentFragment = fragment;
		}

		public override void OnBackPressed()
		{
			if (currentStack.Count > 0)
			{
				var trans = SupportFragmentManager.BeginTransaction();
				trans.Hide(currentFragment);

				SupportFragment fragment = currentStack.First.Value;
				currentStack.RemoveFirst();

				trans.Show(fragment);
				trans.Commit();
				currentFragment = fragment;
			}
			else
			{
				base.OnBackPressed();

			}
		}
	}
}