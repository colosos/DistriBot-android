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

namespace DistriBot
{
    public class ProductDetailFragment : Fragment
    {
        public Product ProductoDetalle { get; set; }
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.ProductDetailFragment, container, false);
            view.FindViewById<TextView>(Resource.Id.tvProductnName).Text = ProductoDetalle.Name;
            view.FindViewById<TextView>(Resource.Id.tvProductUnitPrice).Text = ProductoDetalle.UnitPrice.ToString();
            return view;
        }

        public ProductDetailFragment (Product product)
        {
            ProductoDetalle = product;
        }
    }
}