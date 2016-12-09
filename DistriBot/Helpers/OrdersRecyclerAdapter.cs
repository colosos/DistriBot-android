using System;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using System.Collections.Generic;


namespace DistriBot
{
	public class OrdersRecyclerAdapter : RecyclerView.Adapter
	{

		private List<Order> orders;
		public event EventHandler<int> ItemClick;

		public OrdersRecyclerAdapter(List<Order> ordersList) : base()
		{
			orders = new List<Order>();
			foreach (Order order in ordersList)
			{
				orders.Add(order);
			}
		}

		public override int ItemCount
		{
			get
			{
				return orders.Count;
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
			var order = orders[position];
			OrderView myHolder = holder as OrderView;
			myHolder.Client.Text = order.Client.Name;
			myHolder.Address.Text = order.Client.Address;
			myHolder.Price.Text = order.Price.ToString();
			if (order.Delivered)
			{
				myHolder.Delivered.Text = "Entregado";
			}
			else
			{
				myHolder.Delivered.Text = "";
			}
		}

		public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
		{
			View row = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.OrderRow, parent, false);
			TextView txtClientName = row.FindViewById<TextView>(Resource.Id.txtClientName);
			TextView txtClientAddress = row.FindViewById<TextView>(Resource.Id.txtClientAddress);
			TextView txtTotalPrice = row.FindViewById<TextView>(Resource.Id.txtTotalPrice);
			TextView txtDelivered = row.FindViewById<TextView>(Resource.Id.txtEntregado);
			return new OrderView(row, OnClick) { Client = txtClientName, Address = txtClientAddress, 
				Price = txtTotalPrice, Delivered = txtDelivered };
		}


		public class OrderView : RecyclerView.ViewHolder
		{

			public View MainView { get; set; }
			public TextView Client { get; set; }
			public TextView Address { get; set; }
			public TextView Price { get; set; }
			public TextView Delivered { get; set; }

			public OrderView(View view, Action<int> listener) : base(view)
			{
				MainView = view;
				view.Click += (sender, e) => listener(Position);	
			}
		}
	}
}