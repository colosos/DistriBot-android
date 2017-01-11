
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
	public class SalesmanOrdersFragment : Fragment
	{

		private List<Order> orders = new List<Order>();
		private RecyclerView recyclerView;
		private OrdersRecyclerAdapter adapter;
		private LinearLayoutManager layoutManager;

		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			View view = inflater.Inflate(Resource.Layout.SalesmanOrdersFragment, container, false);
			return view;
		}

		public override void OnActivityCreated(Bundle savedInstanceState)
		{
			SetupOrdersList();
			SetupToolbar();
			base.OnActivityCreated(savedInstanceState);
		}



		private void SetupToolbar()
		{
			if (View != null)
			{
				var toolbar = View.FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
				var activity = Activity as AppCompatActivity;
				activity.SetSupportActionBar(toolbar);
				activity.SupportActionBar.Title = "Lista de pedidos";
			}
		}

		private void SetupOrdersList()
		{
			LoadOrders((List<Order> obj) =>
			{
				Activity.RunOnUiThread(() =>
				{
					CreateAdapter();
				});
			});
		}

		private void LoadOrders(Action<List<Order>> completion)
		{
			var progressDialogue = Android.App.ProgressDialog.Show(Context, "", "Cargando pedidos", true, true);
			var salesman = SessionManager.GetSalesmanUsername();
			OrderServiceManager.GetOrdersBySalesman(salesman, success: (obj) =>
			{
				progressDialogue.Dismiss();
				orders.AddRange(obj);
				completion(obj);
			}, failure: (obj) =>
			{
				progressDialogue.Dismiss();
				Toast.MakeText(Context, "Ha ocurrido un error al cargar los pedidos", ToastLength.Long).Show();
			});
		}

		private void CreateAdapter()
		{
			adapter = new OrdersRecyclerAdapter(orders);
			adapter.ItemClick += OnListItemClick;
			recyclerView = View.FindViewById<RecyclerView>(Resource.Id.recyclerView);
			if (recyclerView != null)
			{
				recyclerView.HasFixedSize = true;
				layoutManager = new LinearLayoutManager(Context);
				recyclerView.SetLayoutManager(layoutManager);
				recyclerView.SetAdapter(adapter);
			}
		}

		void OnListItemClick(object sender, int position)
		{
			if (position >= 0)
			{
				var order = orders[position];
				OrderServiceManager.GetOrder(order.Id, success: (obj) =>
				{
					Order orderToEdit = obj;
					MenuActivity mActivity = Activity as MenuActivity;
					mActivity.ShowFragment(new OrderProductsFragment(orderToEdit), "OrderProductsFragment");
				}, failure: (obj) =>
				{
					Toast.MakeText(Context, "Ha ocurrido un error al cargar el pedido", ToastLength.Long).Show();	
				});
			}
		}
	}
}

/*
 * AlertDialog.Builder alert = new AlertDialog.Builder(this.Activity);
				alert.SetTitle("Info");
				alert.SetMessage("Desea realizar un pedido para " + obj.Name + "?");
				alert.SetPositiveButton("Si", (senderAlert, args) =>
				{
					CartManager.GetInstance().Order.Client = obj;
					MenuActivity actividad = Activity as MenuActivity;
					actividad.ShowFragment(new ProductsFragment(true), "ProductsFragment");
				});

				alert.SetNegativeButton("No", (senderAlert, args) => { });
				 
				Activity.RunOnUiThread(() =>
				{
					alert.Show();
				});
 */
