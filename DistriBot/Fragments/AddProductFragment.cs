using System;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;

namespace DistriBot
{
	public class AddProductFragment : DialogFragment
	{

		public AddProductFragment()
		{
			
		}	

		public static AddProductFragment NewInstance(Bundle savedInstanceState)
		{
			var fragment = new AddProductFragment();
			fragment.Arguments = savedInstanceState;
			return fragment;
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			View view = inflater.Inflate(Resource.Layout.AddProductFragment, container, false);
			return view;
		}
	}
}

