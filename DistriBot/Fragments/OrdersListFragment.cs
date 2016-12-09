
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
using Android.Locations;

namespace DistriBot
{
	public class OrdersListFragment : Fragment, ILocationListener
	{

		private List<Order> orders = new List<Order>();
		private RecyclerView recyclerView;
		private OrdersRecyclerAdapter adapter;
		private LinearLayoutManager layoutManager;

		private LocationManager locationManager;
		private Location currentLocation;

		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			InitializeLocationManager();
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

		private void InitializeLocationManager()
		{
			locationManager = Activity.GetSystemService(Context.LocationService) as LocationManager;
		}

		public override void OnResume()
		{
			base.OnResume();
			Criteria locationCriteria = new Criteria();
			string locationProvider;
			locationCriteria.Accuracy = Accuracy.Fine;
			locationCriteria.PowerRequirement = Power.Medium;
			locationProvider = locationManager.GetBestProvider(locationCriteria, true);
			if (locationProvider != null)
			{
				locationManager.RequestSingleUpdate(locationProvider, this, null);
			}
		}

		public override void OnPause()
		{
			base.OnPause();
			locationManager.RemoveUpdates(this);
		}

		public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
		{
			base.OnCreateOptionsMenu(menu, inflater);
			SetupToolbar();
		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			switch (item.ItemId)
			{
				case Resource.Id.action_view_map:
					DeliverymanMenuActivity activity = Activity as DeliverymanMenuActivity;
					activity.ShowFragment(new OrdersOnMapFragment(orders, currentLocation), "OrdersOnMapFragment");
					return true;
			}
			return base.OnOptionsItemSelected(item);
		}

		private void SetupToolbar()
		{
			if (View != null)
			{
				var toolbar = View.FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
				var activity = Activity as AppCompatActivity;
				toolbar.InflateMenu(Resource.Menu.OrdersListMenu);
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
			var deliveryman = SessionManager.GetDeliverymanUsername();
			OrderServiceManager.GetOrdersToDeliver(deliveryman, success: (obj) =>
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
				if (!order.Delivered)
				{
					DeliverymanMenuActivity activity = Activity as DeliverymanMenuActivity;
					activity.ShowFragment(new OrderDetailsFragment(order), "OrderDetailsFragment");
				}
				else
				{
					Toast.MakeText(Context, "El pedido ya ha sido entregado", ToastLength.Long).Show();
				}
			}
		}

		public void OnLocationChanged(Location location)
		{
			currentLocation = location;
		}

		public void OnProviderDisabled(string provider)
		{
			// OnProviderEnabled and OnProviderDisabled - Complementary methods that notify the application when the 
			// user has enabled or disabled the provider (for example, a user may disable GPS to conserve battery).	
		}

		public void OnProviderEnabled(string provider)
		{
			// OnProviderEnabled and OnProviderDisabled - Complementary methods that notify the application when the 
			// user has enabled or disabled the provider (for example, a user may disable GPS to conserve battery).	
		}

		public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
		{
			// Notifies the application when the provider's availability changes, and provides
			// the accompanying status (for example, GPS availability may change when a user walks indoors).
		}
	}
}