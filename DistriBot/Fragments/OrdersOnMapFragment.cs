﻿using System;
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
using System.Json;

namespace DistriBot
{
	public class OrdersOnMapFragment : Fragment, IOnMapReadyCallback
	{

		private GoogleMap mMap;
		private MapView mapView;

		private Location currentLocation;

		private Dictionary<Client, Marker> clientsDictionary = new Dictionary<Client, Marker>();
		private List<Order> orders = new List<Order>();
		private List<Client> clients = new List<Client>();

		private bool routeParameter;

		public OrdersOnMapFragment(List<Order> ordersList, Location location)
		{
			orders.AddRange(ordersList);
			currentLocation = location;
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
			RouteServiceManager.GetRouteParameter(success: (obj) =>
			{
				routeParameter = obj == 1;
			}, failure: (obj) =>
			{
				Toast.MakeText(Context, "Ha ocurrido un error al cargar un parámetro de ruta", ToastLength.Long).Show();
			});
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
			var progressDialogue = Android.App.ProgressDialog.Show(Context, "", "Cargando ruta de reparto", true, true);
			LoadClients();
			progressDialogue.Dismiss();

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
					DeliverymanMenuActivity actividad = Activity as DeliverymanMenuActivity;
					actividad.OnBackPressed();
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
				toolbar.InflateMenu(Resource.Menu.OrdersOnMapMenu);
				activity.SetSupportActionBar(toolbar);
				activity.SupportActionBar.Title = "Ruta de reparto";
			}
		}

		private void LoadClients()
		{
			var deliveryman = SessionManager.GetDeliverymanUsername();
			RouteServiceManager.GetRouteClients(deliveryman, success: (obj) =>
			{
				clients.AddRange(obj);
				int count = 0;
				Activity.RunOnUiThread(() =>
				{
					DisplayDeliveryRoute();
					foreach (Client client in clients)
					{
						MarkerOptions markerOptions = new MarkerOptions();
						var latitude = client.Latitude;
						var longitude = client.Longitude;
						markerOptions.SetPosition(new LatLng(latitude, longitude));
						Marker marker = mMap.AddMarker(markerOptions);
						clientsDictionary.Add(client, marker);
						count++;
						if (count == clients.Count)
						{
							if (currentLocation != null)
							{
								LatLng latlng = new LatLng(currentLocation.Latitude, currentLocation.Longitude);
								CameraUpdate camera = CameraUpdateFactory.NewLatLngZoom(latlng, 12);
								mMap.AnimateCamera(camera);
							}
							else
							{
								LatLng latlng = new LatLng(latitude, longitude);
								CameraUpdate camera = CameraUpdateFactory.NewLatLngZoom(latlng, 12);
								mMap.AnimateCamera(camera);
							}
						}
					}
				});
			}, failure: (obj) =>
			{
				Toast.MakeText(Context, "Ha ocurrido un error al cargar los clientes", ToastLength.Long).Show();
			});
		}

		public void DisplayDeliveryRoute()
		{
			if (currentLocation != null)
			{
				Tuple<string, string> initialPosition = new Tuple<string, string>(currentLocation.Latitude.ToString(), currentLocation.Longitude.ToString());
				List<Tuple<string, string>> list = new List<Tuple<string, string>>();
				foreach (Client client in clients)
				{
					Tuple<string, string> tuple = new Tuple<string, string>(client.Latitude.ToString(), client.Longitude.ToString());
					list.Add(tuple);
				}
				Tuple<string, string> finalPosition;
				if (routeParameter)
				{
					finalPosition = new Tuple<string, string>(initialPosition.Item1, initialPosition.Item2);
				}
				else
				{
					finalPosition = list[list.Count - 1];
				}
				HTTPHelper.GetInstance().GetDeliveryRoute(list, initialPosition, finalPosition, routeParameter, success: (obj) =>
				{
					if (routeParameter)
					{
						JsonValue routes = obj["routes"];
						JsonArray waypoints = routes[0]["waypoint_order"] as JsonArray;
						int waypoint = waypoints[waypoints.Count - 1];
						var destination = list[waypoint - 1];
						HTTPHelper.GetInstance().GetDeliveryRoute(list, initialPosition, destination, routeParameter, success: (json) =>
						{
							string encodedPoints = Route.GetOverviewPolyLine(json);
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
								var polylineoption = new PolylineOptions().InvokeColor(Android.Graphics.Color.DarkBlue)
																		  .Geodesic(true)
																		  .Add(latlngPoints);
								mMap.AddPolyline(polylineoption);
							});
						}, failure: (json) =>
						{
							Toast.MakeText(Context, "Ha ocurrido un error al armar la ruta", ToastLength.Long).Show();
						});
					}
					else
					{
						string encodedPoints = Route.GetOverviewPolyLine(obj);
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
							var polylineoption = new PolylineOptions().InvokeColor(Android.Graphics.Color.DarkBlue)
																	  .Geodesic(true)
																	  .Add(latlngPoints);
							mMap.AddPolyline(polylineoption);
						});
					}
				}, failure: (obj) =>
				{
					Toast.MakeText(Context, "Ha ocurrido un error al armar la ruta", ToastLength.Long).Show();
				});
			}
			else
			{
				Toast.MakeText(Context, "Ha ocurrido un error al obtener su ubicacion", ToastLength.Long).Show();
			}
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

		void MMap_MarkerClick(object sender, GoogleMap.MarkerClickEventArgs e)
		{
			
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