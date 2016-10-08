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
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Locations;
using Android.Support.V7.App;

namespace DistriBot
{
	public class ClientsOnMapFragment : Fragment, View.IOnTouchListener, IOnMapReadyCallback
    {

		private GoogleMap mMap;
		private MapView mapView;

		private Location currentLocation;

		private Dictionary<Client, Marker> clientsDictionary = new Dictionary<Client, Marker>();
		private List<Client> clients = new List<Client>();

		private float mLastPosY;

		public FrameLayout mClientsDetailFragmentContainer;
		public ClientsDetailFragment mClientDetailFragment;


        public override void OnCreate(Bundle savedInstanceState)
        {
			base.OnCreate(savedInstanceState);
            HasOptionsMenu = true;
        }


		public ClientsOnMapFragment(List<Client> clientsList, Location location)
        {
			foreach (Client client in clientsList)
			{
				clients.Add(client);	
			}
			currentLocation = location;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
			View view = inflater.Inflate(Resource.Layout.ClientsOnMapFragment, container, false);
            mClientsDetailFragmentContainer = view.FindViewById<FrameLayout>(Resource.Id.clientsDetailFragmentContainer);
            mClientsDetailFragmentContainer.SetOnTouchListener(this);
            return view;
        }

		public override void OnActivityCreated(Bundle savedInstanceState)
		{
            SetUpMap(savedInstanceState);
            base.OnActivityCreated(savedInstanceState);
        }

        public override void OnResume()
		{
            mapView.OnResume();
            base.OnResume();
		}

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            base.OnCreateOptionsMenu(menu, inflater);
            SetupToolbar();
        }

        private void SetupToolbar()
        {
            var toolbar = View.FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            var activity = Activity as AppCompatActivity;
            toolbar.InflateMenu(Resource.Menu.MenuClientsOnMap);
            activity.SetSupportActionBar(toolbar);
        }

        private void SetUpMap(Bundle savedInstanceState)
		{
            mapView = View.FindViewById<MapView>(Resource.Id.map);
            mapView.OnCreate(savedInstanceState);
            if (mMap == null)
			{
                mapView.GetMapAsync(this);
            }
		}

        public override void OnDestroy()
        {
            base.OnDestroy();
            mapView.OnDestroy();
        }

        public override void OnLowMemory()
        {
            base.OnLowMemory();
            mapView.OnLowMemory();
        }

        public void OnMapReady(GoogleMap googleMap)
		{
			mMap = googleMap;
			mMap.MyLocationEnabled = true;
			LoadClients();

			mMap.MarkerClick += MMap_MarkerClick;
		}

		void MMap_MarkerClick(object sender, GoogleMap.MarkerClickEventArgs e)
		{
			var clientsList = clientsDictionary.Where(c => c.Value.Equals(e.Marker)).Select(c => c.Key);
			foreach (Client client in clientsList)
			{
				mClientDetailFragment = new ClientsDetailFragment(client);
				var trans = Activity.SupportFragmentManager.BeginTransaction();
				trans.Replace(mClientsDetailFragmentContainer.Id, mClientDetailFragment, "ClientsDetailFragment");
				trans.Commit();
			}
			if (mClientsDetailFragmentContainer.TranslationY + 2 >= mClientsDetailFragmentContainer.Height)
			{
				mClientsDetailFragmentContainer.TranslationY = 500;
				var interpolator = new Android.Views.Animations.OvershootInterpolator(5);
				mClientsDetailFragmentContainer.Animate().SetInterpolator(interpolator)
											   .TranslationYBy(-130)
											   .SetDuration(500);
			}
			LatLng latlng = new LatLng(e.Marker.Position.Latitude, e.Marker.Position.Longitude);
			CameraUpdate camera = CameraUpdateFactory.NewLatLngZoom(latlng, 12);
			mMap.AnimateCamera(camera);
		}

		void LoadClients()
		{
            foreach (Client client in clients)
            {
                Activity.RunOnUiThread(() =>
                {
                    MarkerOptions markerOptions = new MarkerOptions();
                    markerOptions.SetPosition(new LatLng(client.Latitude, client.Longitude));
                    Marker marker = mMap.AddMarker(markerOptions);
                    clientsDictionary.Add(client, marker);
                });
            }
			LatLng latlng = new LatLng(currentLocation.Latitude, currentLocation.Longitude);
			CameraUpdate camera = CameraUpdateFactory.NewLatLngZoom(latlng, 12);
			mMap.AnimateCamera(camera);
		}

		public bool OnTouch(View v, MotionEvent e)
		{
			switch (e.Action)
			{
				case MotionEventActions.Down:
					mLastPosY = e.GetY();
					return true;
				case MotionEventActions.Move:
					var currentPosition = e.GetY();
					var deltaY = mLastPosY - currentPosition;
					var transY = v.TranslationY;
					transY -= deltaY;
					if (transY < 0)
					{
						transY = 0;
					}
					v.TranslationY = transY;
					return true;
				default:
					return v.OnTouchEvent(e);
			}
		}

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.action_view_list:
                    MenuActivity actividad = Activity as MenuActivity;
                    actividad.OnBackPressed();
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }
    }
}
