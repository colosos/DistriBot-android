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
    public class ProductDetailFragment : Fragment
    {
		
		private Product product;
		private ImageViewAsync imgView;
		private View firstRecommendendProduct;
		private View secondRecommendendProduct;
		private View thirdRecommendendProduct;

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
			view.FindViewById<TextView>(Resource.Id.txtProductDescription).Text = product.Description;
			view.FindViewById<TextView>(Resource.Id.txtProductUnitPrice).Text = product.UnitPrice.ToString();
			view.FindViewById<TextView>(Resource.Id.txtProductUnit).Text = " / " + product.MeasurementUnit;

			firstRecommendendProduct = view.FindViewById<View>(Resource.Id.firstProduct);
			secondRecommendendProduct = view.FindViewById<View>(Resource.Id.secondProduct);
			thirdRecommendendProduct = view.FindViewById<View>(Resource.Id.thirdProduct);

			imgView = view.FindViewById<ImageViewAsync>(Resource.Id.productImage);
			LoadImage();
			LoadRecommendedProducts();

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

		private void LoadRecommendedProducts()
		{
			var progressDialogue = Android.App.ProgressDialog.Show(Context, "", "Cargando productos recomendados", true, true);
			ProductServiceManager.GetRecommendedProductsFromProduct(product.Id, success: (obj) =>
			{
				Activity.RunOnUiThread(() =>
				{
					progressDialogue.Dismiss();
					firstRecommendendProduct.FindViewById<TextView>(Resource.Id.Text1).Text = obj[0].Name;
					firstRecommendendProduct.FindViewById<TextView>(Resource.Id.Text2).Text = "$" + obj[0].UnitPrice;
					secondRecommendendProduct.FindViewById<TextView>(Resource.Id.Text1).Text = obj[1].Name;
					secondRecommendendProduct.FindViewById<TextView>(Resource.Id.Text2).Text = "$" + obj[1].UnitPrice;
					thirdRecommendendProduct.FindViewById<TextView>(Resource.Id.Text1).Text = obj[2].Name;
					thirdRecommendendProduct.FindViewById<TextView>(Resource.Id.Text2).Text = "$" + obj[2].UnitPrice;
				});
			}, failure: (obj) =>
			{
				progressDialogue.Dismiss();
				Toast.MakeText(Context, "Ha ocurrido un error al cargar los productos recomendados", ToastLength.Long).Show();
			});
		}

        private void SetUpToolbar()
        {
			if (View != null)
			{
				var toolbar = View.FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
				toolbar.Title = product.Name;
				var activity = Activity as AppCompatActivity;
				activity.SetSupportActionBar(toolbar);
				activity.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
				activity.SupportActionBar.Title = product.Name;
			}
        }

	}
}
