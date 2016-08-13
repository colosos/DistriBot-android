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
using Android.Support.V7.App;
using Android.Support.V7.Widget;

namespace DistriBot
{
	[Activity(Label = "Productos", MainLauncher = true, Theme = "@style/DefaultTheme")]
	public class ProductsActivity : AppCompatActivity
	{
        private RecyclerView mRecyclerView;
        private LinearLayoutManager mLayoutManager;
        private ProductsRecyclerAdapter mAdapter;

        List<Product> products = new List<Product>()
		{
			new Product(1, "Salame", 12.3),
			new Product(2, "Queso", 34),
			new Product(3, "Azucar", 53),
			new Product(4, "Agua", 3.11),
            new Product(5, "Coca", 3.11),
            new Product(6, "Pomelo", 3.11),
            new Product(7, "Cerveza", 3.11),
            new Product(8, "Hamburguesas", 3.11),
            new Product(9, "Panchos", 3.11),
            new Product(10, "Nuggets", 3.11),
            new Product(11, "Papas", 3.11),
            new Product(12, "Jamón", 3.11),
            new Product(13, "Mortadela", 3.11)
        };

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.Products);

           
            
            mAdapter = new ProductsRecyclerAdapter(products);
            mAdapter.ItemClick += OnListItemClick;

            mRecyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView);
            if (mRecyclerView != null)
            {
                mRecyclerView.HasFixedSize = true;
                mLayoutManager = new LinearLayoutManager(this);

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

                mRecyclerView.SetLayoutManager(mLayoutManager);
                mRecyclerView.SetAdapter(mAdapter);
            }

            
        }

        void OnListItemClick(object sender, int position)
        {
            if (position >= 0)
            {
                var p = products[position];
                Android.Widget.Toast.MakeText(this, p.Name, Android.Widget.ToastLength.Short).Show();

                // Elimino el item que se selecciona y se le avisa especificamente eso al recyclerview adapter
                products.RemoveAt(position);
                mAdapter.NotifyItemRemoved(position);

            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
		{
			MenuInflater.Inflate(Resource.Menu.home, menu);
			return base.OnCreateOptionsMenu(menu);
		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			Toast.MakeText(this, "Top ActionBar pressed: " + item.TitleFormatted, ToastLength.Short).Show();
			return base.OnOptionsItemSelected(item);
		}
	}
}

