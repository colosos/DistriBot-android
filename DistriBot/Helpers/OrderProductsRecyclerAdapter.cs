using System;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using System.Collections.Generic;

namespace DistriBot
{
	public class OrderProductsRecyclerAdapter : RecyclerView.Adapter
	{

		private Order order;
		private List<Product> products;

		public event EventHandler<int> ItemClick;

		public OrderProductsRecyclerAdapter(Order o) : base()
		{
			order = o;
			products = new List<Product>();
			products.AddRange(order.ProductsHelper);
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
			{
				ItemClick(this, position);
			}
		}

		public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
		{
			var product = products[position];
			var item = order.Products.Find(p => p.Item1 == product.Id);
			OrdersProductView myHolder = holder as OrdersProductView;
			myHolder.Name.Text = product.Name;
			myHolder.Quantity.Text = item.Item2 + product.MeasurementUnit;
			myHolder.Subtotal.Text = " - $" + item.Item3;
		}

		public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
		{
			View row = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ProductCartRow, parent, false);
			TextView txtProductName = row.FindViewById<TextView>(Resource.Id.txtProductName);
			TextView txtQuantity = row.FindViewById<TextView>(Resource.Id.txtQuantity);
			TextView txtSubtotal = row.FindViewById<TextView>(Resource.Id.txtSubtotal);
			return new OrdersProductView(row, OnClick) { Name = txtProductName, Quantity = txtQuantity, Subtotal = txtSubtotal };
		}

		public class OrdersProductView : RecyclerView.ViewHolder
		{
			public View MainView { get; set; }
			public TextView Name { get; set; }
			public TextView Quantity { get; set; }
			public TextView Subtotal { get; set; }

			public OrdersProductView(View view, Action<int> listener) : base(view)
			{
				MainView = view;
				view.Click += (sender, e) => listener(Position);
			}
		}
	}
}
