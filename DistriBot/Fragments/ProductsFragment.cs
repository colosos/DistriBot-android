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
    public class ProductsFragment : Fragment
    {
        private RecyclerView mRecyclerView;
        private LinearLayoutManager mLayoutManager;
        private ProductsRecyclerAdapter mAdapter;

        private List<Product> products = new List<Product>()
        {
            new Product(14, "Chocolate", 3.11),
            new Product(15, "Arroz", 3.11),
            new Product(16, "Banana", 3.11),
            new Product(17, "Manzana", 3.11),
            new Product(18, "Limón", 3.11)
        };
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here

        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            View view = inflater.Inflate(Resource.Layout.Products, container, false);
            return view;
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            CreateAdapter();
            //LoadProducts();
            var toolbar = View.FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            var activity = Activity as AppCompatActivity;
            activity.SetSupportActionBar(toolbar);
            base.OnActivityCreated(savedInstanceState);
        }

        private void LoadProducts()
        {
            var progressDialogue = Android.App.ProgressDialog.Show(Context, "Espere un momento por favor", "Se están cargando los productos", true, true);
            ProductServiceManager.GetProducts(1, success: (obj) =>
            {
                progressDialogue.Dismiss();
                products = obj;
                Activity.RunOnUiThread(() =>
                {
                    CreateAdapter();
                });
            }, failure: (obj) =>
            {
                Android.Widget.Toast.MakeText(Context, "Ha ocurrido un error al cargar los productos", Android.Widget.ToastLength.Short).Show();
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
                //Load more stuff here
                List<Product> addProducts = new List<Product>()
                {
                        new Product(14, "Chocolate", 3.11),
                        new Product(15, "Arroz", 3.11),
                        new Product(16, "Banana", 3.11),
                        new Product(17, "Manzana", 3.11),
                        new Product(18, "Limón", 3.11)
                };
                products.AddRange(addProducts);
                mAdapter.NotifyItemRangeInserted(products.Count, addProducts.Count);
            };

            mRecyclerView.AddOnScrollListener(onScrollListener);
        }
    }
}