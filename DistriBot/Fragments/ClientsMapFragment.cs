
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
using Android.Gms.Maps;
using Android.Support.V4.App;
using Android.Gms.Maps.Model;

namespace DistriBot
{
	public class ClientsMapFragment : Fragment, IOnMapReadyCallback
	{

		private GoogleMap mMap;
		private List<Client> clients = new List<Client>();

		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			View view = inflater.Inflate(Resource.Layout.ClientsMapFragment, container, false);
			return view;
		}

		public override void OnActivityCreated(Bundle savedInstanceState)
		{
			SetUpMap();
			base.OnActivityCreated(savedInstanceState);
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
			// mMap.MyLocationChange += MMap_MyLocationChange;
		}

		void MMap_MyLocationChange(object sender, GoogleMap.MyLocationChangeEventArgs e)
		{
			// LatLng latlng = new LatLng(e.Location.Latitude, e.Location.Longitude);
			// CameraUpdate camera = CameraUpdateFactory.NewLatLngZoom(latlng, 10);
			// mMap.AnimateCamera(camera);
		}

		void LoadClients()
		{
			Client c1 = new Client("Alejandro", -34.881203, -56.074917);
			Client c2 = new Client("Federico", -34.892619, -56.083213);
			Client c3 = new Client("Andres", -34.914684, -56.150516);
			Client c4 = new Client("Juan Pablo", -34.875489, -56.159022);
			MarkerOptions m1 = new MarkerOptions();
			m1.SetPosition(new LatLng(c1.Latitude, c1.Longitude));
			m1.SetTitle(c1.Name);
			MarkerOptions m2 = new MarkerOptions();
			m2.SetPosition(new LatLng(c2.Latitude, c2.Longitude));
			m2.SetTitle(c2.Name);
			MarkerOptions m3 = new MarkerOptions();
			m3.SetPosition(new LatLng(c3.Latitude, c3.Longitude));
			m3.SetTitle(c3.Name);
			MarkerOptions m4 = new MarkerOptions();
			m4.SetPosition(new LatLng(c4.Latitude, c4.Longitude));
			m4.SetTitle(c4.Name);
			mMap.AddMarker(m1);
			mMap.AddMarker(m2);
			mMap.AddMarker(m3);
			mMap.AddMarker(m4);
			CameraUpdate camera = CameraUpdateFactory.NewLatLngZoom(new LatLng(c2.Latitude, c2.Longitude), 10);
			mMap.AnimateCamera(camera);

			/*
			ClientServiceManager.GetClients(success: (obj) =>
			{
				clients = obj;
			}, failure: (obj) =>
			{
				//TODO: Show error message
			});
			foreach (Client client in clients)
			{
				MarkerOptions markerOptions = new MarkerOptions();
				markerOptions.SetPosition(new LatLng(client.Latitude, client.Longitude));
				mMap.AddMarker(markerOptions);
			}
			*/
		}
	}
}

