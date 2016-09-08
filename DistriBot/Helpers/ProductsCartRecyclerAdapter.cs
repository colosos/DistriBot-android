using System;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;


namespace DistriBot
{
	public class ProductsCartRecyclerAdapter : RecyclerView.Adapter
	{

		private Order order;

		public event EventHandler<int> ItemClick;

		public ProductsCartRecyclerAdapter(Order order) : base()
		{
			this.order = order;
		}

		public override int ItemCount
		{
			get
			{
				return order.Products.Count;
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
			var item = order.Products[position];
			ProductCartView myHolder = holder as ProductCartView;

		}

		public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
		{
			throw new NotImplementedException();
		}

		public class ProductCartView : RecyclerView.ViewHolder
		{
			public View MainView { get; set; }
			public TextView Name { get; set; }
			public TextView Quantity { get; set; }
			public TextView Price { get; set; }

			public ProductCartView(View view, Action<int> listener) : base(view)
			{
				MainView = view;
				view.Click += (sender, e) => listener(base.Position);
			}
		}
	}
}

/*
 *

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var product = products[position];
            ProductView myHolder = holder as ProductView;
            myHolder.Name.Text = product.Name;
            myHolder.UnitPrice.Text = product.UnitPrice.ToString();
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View row = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ProductRow, parent, false);

            TextView txtName = row.FindViewById<TextView>(Resource.Id.Text1);
            TextView txtUnitPrice = row.FindViewById<TextView>(Resource.Id.Text2);

            return new ProductView(row, OnClick) { Name = txtName, UnitPrice = txtUnitPrice};
        }
 */
