using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Support.V4.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Locations;

namespace DistriBot
{
    public class ClientsListFragment : Fragment, ILocationListener
    {

        private RecyclerView mRecyclerView;
        private LinearLayoutManager mLayoutManager;
        private ClientsRecyclerAdapter mAdapter;
        private List<Client> clients = new List<Client>();

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
            View view = inflater.Inflate(Resource.Layout.ClientsListFragment, container, false);
            return view;
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            LoadClients();
            base.OnActivityCreated(savedInstanceState);
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
            SetUpToolbar();
        }

		private void InitializeLocationManager()
		{
			locationManager = Activity.GetSystemService(Context.LocationService) as LocationManager;
		}

        private void SetUpToolbar()
        {
            var toolbar = View.FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            var activity = Activity as AppCompatActivity;
            toolbar.InflateMenu(Resource.Menu.MenuClientsList);
            activity.SetSupportActionBar(toolbar);
        }

        void LoadClients()
        {
            var progressDialogue = Android.App.ProgressDialog.Show(Context, "", "Cargando clientes", true, true);
            ClientServiceManager.GetClients(success: (obj) =>
            {
                progressDialogue.Dismiss();
                clients = obj;
                Activity.RunOnUiThread(() =>
                {
					CreateAdapter();
                });

            }, failure: (obj) =>
            {
                Android.Widget.Toast.MakeText(Context, "Ha ocurrido un error al cargar los clientes", Android.Widget.ToastLength.Short).Show();
            });
        }

        private void CreateAdapter()
        {
            mAdapter = new ClientsRecyclerAdapter(clients);
            mAdapter.ItemClick += OnListItemClick;
            mRecyclerView = View.FindViewById<RecyclerView>(Resource.Id.recyclerView);
            if (mRecyclerView != null)
            {
                mRecyclerView.HasFixedSize = true;
                mLayoutManager = new LinearLayoutManager(Context);

                //CreateScrollListener();

                mRecyclerView.SetLayoutManager(mLayoutManager);
                mRecyclerView.SetAdapter(mAdapter);
            }
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.action_view_map:
                    MenuActivity mActivity = Activity as MenuActivity;
					mActivity.ShowFragment(new ClientsOnMapFragment(clients, currentLocation), "ClientsOnMap");
                    return true;

            }
            return false;
        }

        void OnListItemClick(object sender, int position)
        {
            if (position >= 0)
            {
                var c = clients[position];
                MenuActivity actividad = Activity as MenuActivity;
				actividad.Order.ClientId = c.Id;
                actividad.ShowFragment(new ProductsFragment(), "ProductsFragment");
            }
        }

		void SuggestClient()
		{
			double lat = currentLocation.Latitude;
			double lon = currentLocation.Longitude;
			ClientServiceManager.GetNearestClient(lat, lon, success: (Client obj) =>
			{
				AlertDialog.Builder alert = new AlertDialog.Builder(this.Activity);
				alert.SetTitle("Informaci��n");
				alert.SetMessage("Desea realizar un pedido para " + obj.Name + "?");
				alert.SetPositiveButton("Si", (senderAlert, args) =>
				{
					MenuActivity actividad = Activity as MenuActivity;
					actividad.Order.ClientId = obj.Id;
					actividad.ShowFragment(new ProductsFragment(), "ProductsFragment");
				});

				alert.SetNegativeButton("No", (senderAlert, args) => { });
				 
				Activity.RunOnUiThread(() =>
				{
					alert.Show();
				});
			}, failure: (string obj) =>
			{
			});
		}

		public void OnLocationChanged(Location location)
		{
			currentLocation = location;
			SuggestClient();
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

		//private void CreateScrollListener()
		//{
		//    var onScrollListener = new RecyclerViewOnScrollListener(mLayoutManager);
		//    onScrollListener.LoadMoreEvent += (object sender, EventArgs e) =>
		//    {
		//        //Load more stuff here
		//        List<Product> addProducts = new List<Product>()
		//        {
		//                new Product(14, "Chocolate", 3.11),
		//                new Product(15, "Arroz", 3.11),
		//                new Product(16, "Banana", 3.11),
		//                new Product(17, "Manzana", 3.11),
		//                new Product(18, "Lim�n", 3.11)
		//        };
		//        clients.AddRange(addProducts);
		//        mAdapter.NotifyItemRangeInserted(clients.Count, addProducts.Count);
		//    };

		//    mRecyclerView.AddOnScrollListener(onScrollListener);
		//}
	}
}