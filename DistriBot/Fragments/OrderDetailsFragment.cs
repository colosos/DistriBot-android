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
using Android.Support.Design.Widget;

namespace DistriBot
{
	public class OrderDetailsFragment : Fragment
	{

		private Order order;
		private RecyclerView recyclerView;
		private OrderProductsRecyclerAdapter adapter;
		private LinearLayoutManager layoutManager;

		public OrderDetailsFragment(Order pOrder)
		{
			order = pOrder;
		}

		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			View view = inflater.Inflate(Resource.Layout.OrderDetailsFragment, container, false);
			view.FindViewById<TextView>(Resource.Id.txtTotal).Text = "Total $" + order.Price;
			FloatingActionButton btnEntregar = view.FindViewById<FloatingActionButton>(Resource.Id.btnEntregar);
			btnEntregar.Click += BtnEntregar_Click;
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
			if (View != null)
			{
				var toolbar = View.FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
				toolbar.Title = order.Client.Name;
				var activity = Activity as AppCompatActivity;
				activity.SetSupportActionBar(toolbar);
				activity.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
				activity.SupportActionBar.Title = order.Client.Name;
			}
		}

		private void CreateAdapter()
		{
			adapter = new OrderProductsRecyclerAdapter(order);
			recyclerView = View.FindViewById<RecyclerView>(Resource.Id.recyclerView);
			if (recyclerView != null)
			{
				recyclerView.HasFixedSize = true;
				layoutManager = new LinearLayoutManager(Context);
				recyclerView.SetLayoutManager(layoutManager);
				recyclerView.SetAdapter(adapter);
			}
		}

		void BtnEntregar_Click(object sender, EventArgs e)
		{
			OrderServiceManager.DeliverOrder(order, success: () =>
			{
				Activity.RunOnUiThread(() =>
				{
					Toast.MakeText(this.Activity, "Pedido entregado exitosamente", ToastLength.Long).Show();
				});
				DeliverymanMenuActivity activity = Activity as DeliverymanMenuActivity;
				activity.ClearStackFragment();
				activity.ShowFragment(new OrdersListFragment(), "OrdersListFragment");
			}, failure: () =>
			{
				Activity.RunOnUiThread(() =>
				{
					Toast.MakeText(this.Activity, "Hubo un error al entregar el pedido", ToastLength.Long).Show();
				});
			});
		}
	}
}
