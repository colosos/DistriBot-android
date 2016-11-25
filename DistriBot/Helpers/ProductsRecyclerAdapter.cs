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
using Android.Support.V7.Widget;

namespace DistriBot
{
    class ProductsRecyclerAdapter : RecyclerView.Adapter
    {
        List<Product> products;
        public event EventHandler<int> ItemClick;

        public ProductsRecyclerAdapter (List<Product> products) : base()
		{
            this.products = products;
        }

        public override int ItemCount
        {
            get
            {
                return products.Count;
            }
        }

        void OnClick(int position)
        {
            if (ItemClick != null)
                ItemClick(this, position);
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var product = products[position];
            ProductView myHolder = holder as ProductView;
            myHolder.Name.Text = product.Name;
            myHolder.UnitPrice.Text = product.UnitPrice.ToString();
			if (product.Recommended)
			{
				myHolder.Recommended.Text = "Recomendado";
			}
			else
			{
				myHolder.Recommended.Text = "";
			}
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View row = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ProductRow, parent, false);
			//row.SetBackgroundColor(Android.Graphics.Color.ParseColor("#ff4dd0e1"));
            TextView txtName = row.FindViewById<TextView>(Resource.Id.Text1);
            TextView txtUnitPrice = row.FindViewById<TextView>(Resource.Id.Text2);
			TextView txtRecommended = row.FindViewById<TextView>(Resource.Id.Text3);

			return new ProductView(row, OnClick) { Name = txtName, UnitPrice = txtUnitPrice, Recommended = txtRecommended };
        }

        public class ProductView : RecyclerView.ViewHolder
        {
            public View MainView { get; set; }
            public TextView Name { get; set; }
            public TextView UnitPrice { get; set; }
			public TextView Recommended { get; set; }
            
            public ProductView (View view, Action<int> listener) : base(view)
            {
                MainView = view;
                view.Click += (sender, e) => listener(base.Position);
            }
        }
    }
}
