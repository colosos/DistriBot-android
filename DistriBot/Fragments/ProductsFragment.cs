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
            base.OnActivityCreated(savedInstanceState);
        }

        //private void LoadProducts()
        //{
        //    //TODO: Show spinner while loading products
        //    ProductServiceManager.GetProducts(1, success: (obj) =>
        //    {
        //        products = obj;
        //        this.RunOnUiThread(() =>
        //        {
        //            CreateAdapter();
        //        });
        //    }, failure: (obj) =>
        //    {
        //        //TODO: Show error message.
        //    });
        //}

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
                Android.Widget.Toast.MakeText(Context, p.Name, Android.Widget.ToastLength.Short).Show();

                // Elimino el item que se selecciona y se le avisa especificamente eso al recyclerview adapter
                products.RemoveAt(position);
                mAdapter.NotifyItemRemoved(position);
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