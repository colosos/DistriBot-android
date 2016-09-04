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
    class ClientsRecyclerAdapter : RecyclerView.Adapter
    {
        List<Client> clients;
        public event EventHandler<int> ItemClick;

        public ClientsRecyclerAdapter(List<Client> clients) : base()
		{
            this.clients = clients;
        }

        public override int ItemCount
        {
            get
            {
                return clients.Count;
            }
        }

        void OnClick(int position)
        {
            if (ItemClick != null)
                ItemClick(this, position);
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var product = clients[position];
            ClientView myHolder = holder as ClientView;
            myHolder.Name.Text = product.Name;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View row = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ClientRow, parent, false);

            TextView tvName = row.FindViewById<TextView>(Resource.Id.tvName);
            TextView tvAddress = row.FindViewById<TextView>(Resource.Id.tvAddress);

            return new ClientView(row, OnClick) { Name = tvName, Address = tvAddress };
        }

        public class ClientView : RecyclerView.ViewHolder
        {
            public View MainView { get; set; }
            public TextView Name { get; set; }
            public TextView Address { get; set; }

            public ClientView(View view, Action<int> listener) : base(view)
            {
                MainView = view;
                view.Click += (sender, e) => listener(base.Position);
            }
        }

    }
}