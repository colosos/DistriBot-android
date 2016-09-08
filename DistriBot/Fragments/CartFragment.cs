
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
using Android.Support.V7.Widget;
using Android.Support.V7.App;

namespace DistriBot
{
	public class CartFragment : Fragment
	{

		private RecyclerView recyclerView;
		private ProductsCartRecyclerAdapter adapter;
		private LinearLayoutManager layoutManager;

		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			HasOptionsMenu = true;
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			View view = inflater.Inflate(Resource.Layout.CartFragment, container, false);
			return view;
		}

		public override void OnActivityCreated(Bundle savedInstanceState)
		{
			SetupToolbar();
			base.OnActivityCreated(savedInstanceState);
		}

		private void SetupToolbar()
		{
			var toolbar = View.FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
			var activity = Activity as AppCompatActivity;
			toolbar.InflateMenu(Resource.Menu.CartMenu);
			activity.SetSupportActionBar(toolbar);
		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			switch (item.ItemId)
			{
				case Resource.Id.action_view_products:
					MenuActivity activity = Activity as MenuActivity;
					activity.OnBackPressed();
					return true;

			}
			return false;
		}

		private void CreateAdapter()
		{
			adapter = new ProductsCartRecyclerAdapter();
			recyclerView = View.FindViewById<RecyclerView>(Resource.Id.recyclerView);
			if (recyclerView != null)
			{
				recyclerView.HasFixedSize = true;
				layoutManager = new LinearLayoutManager(Context);
				CreateScrollListener();
				recyclerView.SetLayoutManager(layoutManager);
				recyclerView.SetAdapter(adapter);
			}
		}

		private void CreateScrollListener()
		{
			var onScrollListener = new RecyclerViewOnScrollListener(layoutManager);
			onScrollListener.LoadMoreEvent += (object sender, EventArgs e) =>
			{
			};
			/*
			var onScrollListener = new RecyclerViewOnScrollListener(mLayoutManager);
			onScrollListener.LoadMoreEvent += (object sender, EventArgs e) =>
			{
				LoadProducts(completion: (obj) =>
				{
					products.AddRange(obj);
					mAdapter.NotifyItemRangeInserted(products.Count, obj.Count);
				});
			};

			mRecyclerView.AddOnScrollListener(onScrollListener);*/
		}

		/*
		 private RecyclerView mRecyclerView;
        private LinearLayoutManager mLayoutManager;
        private ProductsRecyclerAdapter mAdapter;
		private List<Product> products = new List<Product>();
		private int lastProduct = 1;
		private int prodQuantity = 10;
		private bool reachedEnd = false;

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
			SetupProductsList();
            SetUpToolbar();
            base.OnActivityCreated(savedInstanceState);
        }


		private void LoadProducts(Action<List<Product>> completion)
		{
			if (!reachedEnd)
			{
				var progressDialogue = Android.App.ProgressDialog.Show(Context, "", "Cargando productos..", true, true);
				ProductServiceManager.GetProducts(lastProduct, prodQuantity, success: (obj) =>
				{
					progressDialogue.Dismiss();
					products.AddRange(obj);
					reachedEnd = obj.Count < prodQuantity;
					lastProduct += obj.Count;
					completion(obj);
				}, failure: (obj) =>
				{
					Android.Widget.Toast.MakeText(Context, "Ha ocurrido un error al cargar los productos", Android.Widget.ToastLength.Short).Show();
				});
			}
		}

        private void SetupProductsList()
		{
			LoadProducts(completion: (obj) =>
			{
				Activity.RunOnUiThread(() =>
				{
					CreateAdapter();
				});
			});
		}

        private void CreateAdapter()
        {
            mAdapter = new ProductsRecyclerAdapter(products);
            mAdapter.ItemClick += OnListItemClick;
            mRecyclerView = View.FindViewById<RecyclerView>(Resource.Id.recyclerView);
            if (mRecyclerView != null)
            {
                mRecyclerView.HasFixedSize = true;
                mLayoutManager = new LinearLayoutManager(Context);

                CreateScrollListener();

                mRecyclerView.SetLayoutManager(mLayoutManager);
                mRecyclerView.SetAdapter(mAdapter);
            }
        }

        void OnListItemClick(object sender, int position)
        {
            if (position >= 0)
            {
                var p = products[position];
                //Llamo el metodo de la actividad menu que muestra el fragment del detalle del producto
                MenuActivity actividad = Activity as MenuActivity;
                actividad.ShowFragment(new ProductDetailFragment(p), "Detalle");

            }
        }

        private void CreateScrollListener()
        {
            var onScrollListener = new RecyclerViewOnScrollListener(mLayoutManager);
            onScrollListener.LoadMoreEvent += (object sender, EventArgs e) =>
            {
				LoadProducts(completion: (obj) =>
				{
					products.AddRange(obj);
					mAdapter.NotifyItemRangeInserted(products.Count, obj.Count);
				});                
            };

            mRecyclerView.AddOnScrollListener(onScrollListener);
        }

		private void resetPages()
		{
			lastProduct = 1;
			prodQuantity = 10;
			reachedEnd = false;
		}	
		 */
	}
}

