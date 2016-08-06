using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace DistriBot
{
	public class ProductsAdapter : BaseAdapter<Product>
	{
		List<Product> products;
		Activity context;

		public ProductsAdapter(Activity context, List<Product> products) : base()
		{
			this.context = context;
			this.products = products;
		}

		public override long GetItemId(int position)
		{
			return position;
		}

		public override Product this[int position]
		{
			get
			{
				return products[position];
			}
		}

		public override int Count
		{
			get
			{
				return products.Count;
			}
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			var product = products[position];
			View view = convertView; // re-use an existing view, if one is available
			if (view == null)
				// otherwise create a new one
				view = context.LayoutInflater.Inflate(Resource.Layout.ProductRow, null);

			view.FindViewById<TextView>(Resource.Id.Text1).Text = product.Name;
			view.FindViewById<TextView>(Resource.Id.Text2).Text = product.UnitPrice.ToString();

			return view;
		}
	}



}
