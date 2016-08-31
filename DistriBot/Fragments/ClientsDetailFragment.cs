
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

namespace DistriBot
{
	public class ClientsDetailFragment : Fragment
	{

		private Client mClient;

		public ClientsDetailFragment(Client client)
		{
			mClient = client;
		}

		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			View view = inflater.Inflate(Resource.Layout.ClientsDetailFragment, container, false);
			view.FindViewById<TextView>(Resource.Id.txtClientName).Text = mClient.Name;
			view.FindViewById<TextView>(Resource.Id.txtClientAddress).Text = mClient.Address;
			view.FindViewById<TextView>(Resource.Id.txtClientPhone).Text = mClient.Phone;
			view.FindViewById<TextView>(Resource.Id.txtClientEmail).Text = mClient.Email;
			TextView txtCreditBalance = view.FindViewById<TextView>(Resource.Id.txtClientCreditBalanceDynamic);
			txtCreditBalance.Text = "$ " + mClient.CreditBalance.ToString(); 
			if (mClient.CreditBalance > 0)
			{
				txtCreditBalance.SetTextColor(Android.Graphics.Color.ParseColor("#ff388e3c"));
			}
			if (mClient.CreditBalance < 0)
			{
				txtCreditBalance.SetTextColor(Android.Graphics.Color.ParseColor("#ffd32f2f"));
			}
			return view;
		}
	}
}

