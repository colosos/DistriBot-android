
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
	public class OrdersListFragment : Fragment
	{

		private List<Tuple<Client, Order>> list = new List<Tuple<Client, Order>>();
		private RecyclerView recyclerView;
		private OrdersRecyclerAdapter adapter;
		private LinearLayoutManager layoutManager;

		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			HasOptionsMenu = true;
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			View view = inflater.Inflate(Resource.Layout.OrdersListFragment, container, false);
			return view;
		}

		public override void OnActivityCreated(Bundle savedInstanceState)
		{
			SetupOrdersList();
			base.OnActivityCreated(savedInstanceState);
		}

		public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
		{
			base.OnCreateOptionsMenu(menu, inflater);
			SetupToolbar();
		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			/*
			switch (item.ItemId)
			{
				case Resource.Id.action_view_map:
					MenuActivity mActivity = Activity as MenuActivity;
					mActivity.ShowFragment(new ClientsOnMapFragment(clients, currentLocation), "ClientsOnMap");
					return true;

			}
			return base.OnOptionsItemSelected(item);
			*/
			return true;
		}

		private void SetupOrdersList()
		{
			/*LoadOrders((List<Client> obj) =>
			{
				Activity.RunOnUiThread(() =>
				{
					CreateAdapter();
				});
			});
			*/
		}

		/*
		private void LoadOrders(Action<List<Tuple<Client, Order>> completion)
		{
			var progressDialogue = Android.App.ProgressDialog.Show(Context, "", "Cargando pedidos", true, true);
			ClientServiceManager.GetClientsPaginated(lastClient, clientsQuantity, success: (List<Client> obj) =>
			{
				progressDialogue.Dismiss();
				clients.AddRange(obj);
				reachedEnd = obj.Count < clientsQuantity;
				lastClient += obj.Count;
				completion(obj);
			}, failure: (obj) =>
			{
				progressDialogue.Dismiss();
				Android.Widget.Toast.MakeText(Context, "Ha ocurrido un error al cargar los clientes", Android.Widget.ToastLength.Short).Show();
			}); 
		}
		*/

		private void SetupToolbar()
		{
			var toolbar = View.FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
			var activity = Activity as AppCompatActivity;
			toolbar.InflateMenu(Resource.Menu.OrdersListMenu);
			activity.SetSupportActionBar(toolbar);
		}

		private void CreateAdapter()
		{
			adapter = new OrdersRecyclerAdapter(list);
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
