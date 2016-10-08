
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace DistriBot
{
	public class DeliveryMapFragment : Fragment, IOnMapReadyCallback
	{

		private GoogleMap mMap;
		private MapView mapView;

		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			return base.OnCreateView(inflater, container, savedInstanceState);
		}

		public override void OnActivityCreated(Bundle savedInstanceState)
		{
			SetUpMap(savedInstanceState);
			base.OnActivityCreated(savedInstanceState);
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

		public void OnMapReady(GoogleMap googleMap)
		{
			mMap = googleMap;
		}
	}
}

