
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Support.V4.App;
using Android.Support.V7.Widget;
using Android.Support.V7.App;

namespace DistriBot
{
	public class CartFragment : Fragment
	{

		private Order order;
		private RecyclerView recyclerView;
		private ProductsCartRecyclerAdapter adapter;
		private LinearLayoutManager layoutManager;

		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			order = CartManager.GetInstance().Order;
			View view = inflater.Inflate(Resource.Layout.CartFragment, container, false);
			view.FindViewById<TextView>(Resource.Id.txtTotal).Text = "Total $" + order.Price;
			return view;
		}

		public override void OnActivityCreated(Bundle savedInstanceState)
		{
			CreateAdapter();
			SetupToolbar();
			base.OnActivityCreated(savedInstanceState);
		}

		private void SetupToolbar()
		{
			var toolbar = View.FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
			var activity = Activity as AppCompatActivity;
			activity.SetSupportActionBar(toolbar);
			activity.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
		}

		private void CreateAdapter()
		{
			adapter = new ProductsCartRecyclerAdapter();
			recyclerView = View.FindViewById<RecyclerView>(Resource.Id.recyclerView);
			if (recyclerView != null)
			{
				recyclerView.HasFixedSize = true;
				layoutManager = new LinearLayoutManager(Context);
				recyclerView.SetLayoutManager(layoutManager);
				recyclerView.SetAdapter(adapter);
			}
		}
	}
}

