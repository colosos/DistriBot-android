using System;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using System.Collections.Generic;


namespace DistriBot
{
	public class OrdersRecyclerAdapter : RecyclerView.Adapter
	{

		private List<Tuple<Client, Order>> orders;

		public event EventHandler<int> ItemClick;

		public OrdersRecyclerAdapter(List<Tuple<Client, Order>> orderList) : base()
		{
			orders = new List<Tuple<Client, Order>>();
			foreach (Tuple<Client, Order> order in orderList)
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
			myHolder.Client.Text = order.Item1.Name;
			myHolder.Address.Text = order.Item1.Address;
			myHolder.Price.Text = order.Item2.Price.ToString();
		}

		public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
		{
			View row = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.OrderRow, parent, false);
			TextView txtClientName = row.FindViewById<TextView>(Resource.Id.txtClientName);
			TextView txtClientAddress = row.FindViewById<TextView>(Resource.Id.txtClientAddress);
			TextView txtTotalPrice = row.FindViewById<TextView>(Resource.Id.txtTotalPrice);
			return new OrderView(row, OnClick) { Client = txtClientName, Address = txtClientAddress, Price = txtTotalPrice };
		}


		public class OrderView : RecyclerView.ViewHolder
		{

			public View MainView { get; set; }
			public TextView Client { get; set; }
			public TextView Address { get; set; }
			public TextView Price { get; set; }

			public OrderView(View view, Action<int> listener) : base(view)
			{
				MainView = view;
				view.Click += (sender, e) => listener(Position);	
			}
		}
	}
}