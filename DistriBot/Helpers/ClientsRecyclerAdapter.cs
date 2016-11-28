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
            var client = clients[position];
            ClientView myHolder = holder as ClientView;
            myHolder.Name.Text = client.Name;
			myHolder.Balance.Text = "$" + client.CreditBalance;
			if (client.CreditBalance > 0)
			{
				myHolder.Balance.SetTextColor(Android.Graphics.Color.ParseColor("#ff00c853"));
			}
			if (client.CreditBalance == 0)
			{
				myHolder.Balance.SetTextColor(Android.Graphics.Color.ParseColor("#747474"));
			}
			if (client.CreditBalance < 0)
			{
				myHolder.Balance.SetTextColor(Android.Graphics.Color.ParseColor("#ffd32f2f"));
			}
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View row = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ClientRow, parent, false);

            TextView tvName = row.FindViewById<TextView>(Resource.Id.tvName);
			TextView tvBalance = row.FindViewById<TextView>(Resource.Id.tvBalance);

			return new ClientView(row, OnClick) { Name = tvName, Balance = tvBalance };
        }

        public class ClientView : RecyclerView.ViewHolder
        {
            public View MainView { get; set; }
            public TextView Name { get; set; }
            public TextView Balance { get; set; }

            public ClientView(View view, Action<int> listener) : base(view)
            {
                MainView = view;
                view.Click += (sender, e) => listener(base.Position);
            }
        }

    }
}