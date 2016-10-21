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
	public class OrdersOnMapFragment : Fragment, IOnMapReadyCallback
	{

		private GoogleMap mMap;
		private MapView mapView;

		private Dictionary<Order, Marker> ordersDictionary = new Dictionary<Order, Marker>();
		private List<Order> ordersList = new List<Order>();

		public OrdersOnMapFragment(List<Order> orders)
		{
			foreach (Order order in orders)
			{
				ordersList.Add(order);
			}
		}

		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			HasOptionsMenu = true;
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			View view = inflater.Inflate(Resource.Layout.OrdersOnMapFragment, container, false);
			return view;
		}

		public override void OnActivityCreated(Bundle savedInstanceState)
		{
			SetupMap(savedInstanceState);
			base.OnActivityCreated(savedInstanceState);
		}

		private void SetupMap(Bundle savedInstanceState)
		{
			mapView = View.FindViewById<MapView>(Resource.Id.map);
			mapView.OnCreate(savedInstanceState);
			if (mMap == null)
			{
				mapView.GetMapAsync(this);
			}
		}

		public void OnMapReady(GoogleMap googleMap)
		{
			mMap = googleMap;
			mMap.MyLocationEnabled = true;
			LoadOrders();

			mMap.MarkerClick += MMap_MarkerClick;
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
				case Resource.Id.action_view_list:
					MenuActivity actividad = Activity as MenuActivity;
					actividad.OnBackPressed();
					return true;
			}
			return base.OnOptionsItemSelected(item);
		}

		private void SetupToolbar()
		{
			var toolbar = View.FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
			var activity = Activity as AppCompatActivity;
			toolbar.InflateMenu(Resource.Menu.OrdersOnMapMenu);
			activity.SetSupportActionBar(toolbar);
		}

		private void LoadOrders()
		{
			foreach (Order order in ordersList)
			{
				Activity.RunOnUiThread(() =>
				{
					MarkerOptions markerOptions = new MarkerOptions();
					var latitude = order.Client.Latitude;
					var longitude = order.Client.Longitude;
					markerOptions.SetPosition(new LatLng(latitude, longitude));
					Marker marker = mMap.AddMarker(markerOptions);
					ordersDictionary.Add(order, marker);
				});
			}
			//LatLng latlng = new LatLng(currentLocation.Latitude, currentLocation.Longitude);
			//CameraUpdate camera = CameraUpdateFactory.NewLatLngZoom(latlng, 12);
			//mMap.AnimateCamera(camera);
		}

		void MMap_MarkerClick(object sender, GoogleMap.MarkerClickEventArgs e)
		{
			List<Tuple<string, string>> list = new List<Tuple<string, string>>();
			foreach (Order order in ordersList)
			{
				Tuple<string, string> tuple = new Tuple<string, string>(order.Client.Latitude.ToString(), order.Client.Longitude.ToString());
				list.Add(tuple);
			}
			HTTPHelper.GetInstance().TestDirections(list, success: (obj) =>
			{
				string encodedPoints = Route.RouteFromJson(obj);
				List<LatLng> lstDecodedPoints = DecodePolylinePoints(encodedPoints);
				var latlngPoints = new LatLng[lstDecodedPoints.Count];
				int index = 0;
				foreach (LatLng latlng in lstDecodedPoints)
				{
					latlngPoints[index] = new LatLng(latlng.Latitude, latlng.Longitude);
					index++;
				}

				Activity.RunOnUiThread(() =>
				{
					var polylineoption = new PolylineOptions().InvokeColor(Android.Graphics.Color.Red)
															  .Geodesic(true)
															  .Add(latlngPoints);
					mMap.AddPolyline(polylineoption);
				});
			}, failure: (obj) =>
			{
				
			});
		}

		private List<LatLng> DecodePolylinePoints(string encodedPoints)
		{
			var poly = new List<LatLng>();
			if (encodedPoints == null || encodedPoints == "")
			{
				return null;
			}
			char[] polylinechars = encodedPoints.ToCharArray();
			int index = 0;
			int currentLat = 0;
			int currentLng = 0;
			int next5bits;
			int sum;
			int shifter;
			while (index < polylinechars.Length)
			{
				sum = 0;
				shifter = 0;
				do
				{
					next5bits = (int)polylinechars[index++] - 63;
					sum |= (next5bits & 31) << shifter;
					shifter += 5;	
				} while (next5bits >= 32 && index < polylinechars.Length);
				if (index >= polylinechars.Length)
				{
					break;
				}
				currentLat += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);

				sum = 0;
				shifter = 0;
				do
				{
					next5bits = (int)polylinechars[index++] - 63;
					sum |= (next5bits & 31) << shifter;
					shifter += 5;
				} while (next5bits >= 32 && index < polylinechars.Length);
				if (index >= polylinechars.Length && next5bits >= 32)
				{
					break;
				}
				currentLng += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);

				double lat = Convert.ToDouble(currentLat) / 100000.0;
				double lng = Convert.ToDouble(currentLng) / 100000.0;
				LatLng p = new LatLng(lat, lng);
				poly.Add(p);
			}
			return poly;
		}

		public override void OnResume()
		{
			mapView.OnResume();
			base.OnResume();
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

	}
}


/*
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

*/
