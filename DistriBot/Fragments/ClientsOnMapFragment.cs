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

namespace DistriBot
{
	public class ClientsOnMapFragment : Fragment, View.IOnTouchListener, IOnMapReadyCallback, ILocationListener
    {
		private GoogleMap mMap;
		private Dictionary<Client, Marker> clientsDictionary = new Dictionary<Client, Marker>();
		private LocationManager locationManager;
		private Location currentLocation;
		private float mLastPosY;

		public FrameLayout mClientsDetailFragmentContainer;
		public ClientsDetailFragment mClientDetailFragment;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
			InitializeLocationManager();
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
			base.OnActivityCreated(savedInstanceState);
			SetUpMap();
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
				locationManager.RequestLocationUpdates(locationProvider, 2000, 1, this);
			}
		}

		public override void OnPause()
		{
			base.OnPause();
			locationManager.RemoveUpdates(this);
		}

		private void InitializeLocationManager()
		{
			locationManager = Activity.GetSystemService(Context.LocationService) as LocationManager;
		}

		private void SetUpMap()
		{
			if (mMap == null)
			{
				Activity.FragmentManager.FindFragmentById<MapFragment>(Resource.Id.map).GetMapAsync(this);
			}
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
			var clients = clientsDictionary.Where(c => c.Value.Equals(e.Marker)).Select(c => c.Key);
			foreach (Client client in clients)
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
			ClientServiceManager.GetClients(success: (obj) =>
			{
				foreach (Client client in obj)
				{
					Activity.RunOnUiThread(() =>
					{
						MarkerOptions markerOptions = new MarkerOptions();
						markerOptions.SetPosition(new LatLng(client.Latitude, client.Longitude));
						Marker marker = mMap.AddMarker(markerOptions);
						clientsDictionary.Add(client, marker);
					});
				}
			}, failure: (obj) =>
			{
				//TODO: Show error message
			});
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

		public void OnLocationChanged(Location location)
		{
			currentLocation = location;
			LatLng latlng = new LatLng(currentLocation.Latitude, currentLocation.Longitude);
			CameraUpdate camera = CameraUpdateFactory.NewLatLngZoom(latlng, 12);
			mMap.AnimateCamera(camera);
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
