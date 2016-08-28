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
using Android.Support.V7.Widget;
using Android.Support.V7.App;
using Android.Support.Design.Widget;

namespace DistriBot
{
    public class ProductDetailFragment : Fragment, View.IOnClickListener
    {
        private FloatingActionButton ftAddToCart;
        public Product ProductDetail { get; set; }
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.ProductDetailFragment, container, false);
            view.FindViewById<TextView>(Resource.Id.tvProductnName).Text = ProductDetail.Name;
            view.FindViewById<TextView>(Resource.Id.tvProductUnitPrice).Text = ProductDetail.UnitPrice.ToString();

            ftAddToCart = view.FindViewById<FloatingActionButton>(Resource.Id.floating_button);
            ftAddToCart.SetOnClickListener(this);

            return view;
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            var toolbar = View.FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            toolbar.Title = ProductDetail.Name;
            var activity = Activity as AppCompatActivity;
            activity.SetSupportActionBar(toolbar);
            activity.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            base.OnActivityCreated(savedInstanceState);
        }

        public void OnClick(View v)
        {
            switch (v.Id)
            {
                case Resource.Id.floating_button:
                    Android.Widget.Toast.MakeText(Context, "Agregar al pedido", Android.Widget.ToastLength.Short).Show();
                    break;
            }
        }

        public ProductDetailFragment(Product product)
        {
            ProductDetail = product;
        }
    }
}