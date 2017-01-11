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
using Android.Support.V7.App;
using Android.Support.Design.Widget;

namespace DistriBot
{
	public class OrderProductsFragment : Fragment, View.IOnClickListener
	{

		private Order order;
		private RecyclerView recyclerView;
		private OrderProductsRecyclerAdapter adapter;
		private LinearLayoutManager layoutManager;

		public OrderProductsFragment(Order pOrder)
		{
			order = pOrder;
		}

		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			HasOptionsMenu = true;
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			View view = inflater.Inflate(Resource.Layout.OrdersProductFragment, container, false);
			view.FindViewById<TextView>(Resource.Id.txtTotal).Text = "Total $" + order.Price;
			FloatingActionButton btnConfirmar = view.FindViewById<FloatingActionButton>(Resource.Id.btnConfirmar);
			FloatingActionButton btnAgregar = view.FindViewById<FloatingActionButton>(Resource.Id.btnAgregar);
			btnConfirmar.Click += BtnConfirmar_Click;
			btnAgregar.Click += BtnAgregar_Click;
			return view;
		}

		public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
		{
			base.OnCreateOptionsMenu(menu, inflater);
			SetupToolbar();
		}

		public override void OnActivityCreated(Bundle savedInstanceState)
		{
			CreateAdapter();
			base.OnActivityCreated(savedInstanceState);
		}

		private void SetupToolbar()
		{
			if (View != null)
			{
				var toolbar = View.FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
				var activity = Activity as AppCompatActivity;
				toolbar.InflateMenu(Resource.Menu.OrderProductsMenu);
				activity.SetSupportActionBar(toolbar);
				toolbar.SetNavigationIcon(Resource.Drawable.abc_ic_ab_back_mtrl_am_alpha);
				toolbar.SetNavigationOnClickListener(this);
				activity.SupportActionBar.Title = order.Client.Name;
			}
		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			switch (item.ItemId)
			{
				case Resource.Id.action_delete_order:
					AlertDialog.Builder alert = new AlertDialog.Builder(this.Activity);
					alert.SetMessage("Esta seguro que desea eliminar el pedido?");
					alert.SetPositiveButton("Confirmar", (senderAlert, args) =>
					{
						OrderServiceManager.DeleteOrder(order.Id, success: () =>
						{
							Activity.RunOnUiThread(() =>
							{
								Toast.MakeText(this.Activity, "El pedido se ha eliminado exitosamente", ToastLength.Long).Show();
							});
							MenuActivity mActivity = Activity as MenuActivity;
							mActivity.ClearDeilveryStackFragment();
							mActivity.ShowFragment(new SalesmanOrdersFragment(), "SalesmanOrdersFragment");
						}, failure: () =>
						{
							Activity.RunOnUiThread(() =>
							{
								Toast.MakeText(this.Activity, "Hubo un error al eliminar el pedido", ToastLength.Long).Show();
							});
						});
					});
					alert.SetNegativeButton("Cancelar", (senderAlert, args) => { });
					Activity.RunOnUiThread(() =>
					{
						alert.Show();
					});
					return true;
			}
			return base.OnOptionsItemSelected(item);
		}

		private void CreateAdapter()
		{
			adapter = new OrderProductsRecyclerAdapter(order);
			recyclerView = View.FindViewById<RecyclerView>(Resource.Id.recyclerView);
			if (recyclerView != null)
			{
				recyclerView.HasFixedSize = true;
				layoutManager = new LinearLayoutManager(Context);
				recyclerView.SetLayoutManager(layoutManager);
				recyclerView.SetAdapter(adapter);
			}
		}

		void BtnConfirmar_Click(object sender, EventArgs e)
		{
			OrderServiceManager.EditOrder(order, success: () =>
			{
				Activity.RunOnUiThread(() =>
				{
					Toast.MakeText(this.Activity, "El pedido se ha editado exitosamente", ToastLength.Long).Show();
				});
				// Clear stack de pedidos y volver al principio
				/*
				 MenuActivity activity = Activity as MenuActivity;
				 activity.ClearPresaleStackFragment();
				 activity.ShowFragment(new ClientsListFragment(), "ClientsListFragment");
				 */
			}, failure: () =>
			{
				Activity.RunOnUiThread(() =>
				{
					Toast.MakeText(this.Activity, "Hubo un error al editar el pedido", ToastLength.Long).Show();
				});
			});
		}

		void BtnAgregar_Click(object sender, EventArgs e)
		{

		}

		public void OnClick(View v)
		{
			MenuActivity activity = Activity as MenuActivity;
			activity.OnBackPressed();
		}
	}
}