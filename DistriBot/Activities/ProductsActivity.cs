using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

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
	[Activity(Label = "Productos", Theme = "@style/DefaultTheme")]
	public class ProductsActivity : AppCompatActivity
	{
        private RecyclerView mRecyclerView;
        private LinearLayoutManager mLayoutManager;
        private ProductsRecyclerAdapter mAdapter;
		//private List<Product> products = new List<Product>();
		private List<Product> products = new List<Product>()
		{
			new Product(14, "Chocolate", 3.11),
			new Product(15, "Arroz", 3.11),
			new Product(16, "Banana", 3.11),
			new Product(17, "Manzana", 3.11),
			new Product(18, "Limón", 3.11)
		};

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.Products);

			LoadProducts();
        }

		private void LoadProducts()
		{
			//TODO: Show spinner while loading products
			ProductServiceManager.GetProducts(1, success: (obj) =>
			{
				products = obj;
				this.RunOnUiThread(() =>
				{
					CreateAdapter();	
				});
			}, failure: (obj) =>
			{
				//TODO: Show error message.
			});
		}

		private void CreateAdapter()
		{
			mAdapter = new ProductsRecyclerAdapter(products);
			mAdapter.ItemClick += OnListItemClick;

			mRecyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView);
			if (mRecyclerView != null)
			{
				mRecyclerView.HasFixedSize = true;
				mLayoutManager = new LinearLayoutManager(this);

				CreateScrollListener();

				mRecyclerView.SetLayoutManager(mLayoutManager);
				mRecyclerView.SetAdapter(mAdapter);
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
