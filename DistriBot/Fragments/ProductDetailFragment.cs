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
using FFImageLoading; using FFImageLoading.Views;
using FFImageLoading.Transformations;

namespace DistriBot
{
    public class ProductDetailFragment : Fragment, View.IOnClickListener
    {
		
        private FloatingActionButton ftAddToCart;
		private Product product;
		private ImageViewAsync imgView;

		public ProductDetailFragment(Product p)
		{
			product = p;
		}

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.ProductDetailFragment, container, false);
			view.FindViewById<TextView>(Resource.Id.txtProductName).Text = product.Name;
			view.FindViewById<TextView>(Resource.Id.txtProductDescription).Text = product.Description;
			view.FindViewById<TextView>(Resource.Id.txtProductUnitPrice).Text = product.UnitPrice.ToString();
			view.FindViewById<TextView>(Resource.Id.txtProductUnit).Text = " / " + product.MeasurementUnit;

			imgView = view.FindViewById<ImageViewAsync>(Resource.Id.productImage);
			LoadImage();

            ftAddToCart = view.FindViewById<FloatingActionButton>(Resource.Id.floating_button);
            ftAddToCart.SetOnClickListener(this);

            return view;
        }
        
        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            SetUpToolbar();
            base.OnActivityCreated(savedInstanceState);
        }

		private void LoadImage()
		{
			var progressDialogue = Android.App.ProgressDialog.Show(Context, "", "Cargando imagen", true, true);
			string url = product.ImageUrlV1;
			ImageService.Instance.LoadUrl(url)
						.Finish((obj) =>
			{
				progressDialogue.Dismiss();
			})
			            .Transform(new CircleTransformation(8, "#CFD8DC"))
			            .Into(imgView);
		}

        private void SetUpToolbar()
        {
            var toolbar = View.FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            toolbar.Title = product.Name;
            var activity = Activity as AppCompatActivity;
            activity.SetSupportActionBar(toolbar);
            activity.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
			activity.SupportActionBar.Title = product.Name;
        }

        public void OnClick(View v)
        {
            switch (v.Id)
            {
                case Resource.Id.floating_button:
                    Toast.MakeText(Context, "Agregar al pedido", ToastLength.Short).Show();
                    break;
            }
        }

	}
}
