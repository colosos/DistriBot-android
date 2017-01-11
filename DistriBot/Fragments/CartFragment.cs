
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
	public class CartFragment : Fragment, View.IOnClickListener
	{
		private RecyclerView recyclerView;
		private ProductsCartRecyclerAdapter adapter;
		private LinearLayoutManager layoutManager;

		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			HasOptionsMenu = true;
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			View view = inflater.Inflate(Resource.Layout.CartFragment, container, false);
			view.FindViewById<TextView>(Resource.Id.txtTotal).Text = "Total $" + CartManager.GetInstance().Order.Price;
			FloatingActionButton btnConfirm = view.FindViewById<FloatingActionButton>(Resource.Id.btnConfirmar);
			btnConfirm.Click += BtnConfirm_Click;
			return view;
		}

		public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
		{
			SetupToolbar();
			base.OnCreateOptionsMenu(menu, inflater);
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
				if (CartManager.GetInstance().Order.Client != null)
				{
					activity.SupportActionBar.Title = CartManager.GetInstance().Order.Client.Name;
				}
				else
				{
					activity.SupportActionBar.Title = "Carrito de productos";
				}	
			}
		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			switch (item.ItemId)
			{
				case Resource.Id.action_delete_order:
					AlertDialog.Builder alert = new AlertDialog.Builder(this.Activity);
					alert.SetMessage("Esta seguro que desea cancelar el pedido?");
					alert.SetPositiveButton("Confirmar", (senderAlert, args) =>
					{
						CartManager.GetInstance().ResetCart();
						MenuActivity activity = Activity as MenuActivity;
						activity.ClearPresaleStackFragment();
						activity.ShowFragment(new ClientsListFragment(), "ClientsListFragment");
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
			adapter = new ProductsCartRecyclerAdapter();
			recyclerView = View.FindViewById<RecyclerView>(Resource.Id.recyclerView);
			if (recyclerView != null)
			{
				recyclerView.HasFixedSize = true;
				layoutManager = new LinearLayoutManager(Context);
				recyclerView.SetLayoutManager(layoutManager);
				recyclerView.SetAdapter(adapter);
			}
		}

		void BtnConfirm_Click(object sender, EventArgs e)
		{
			CartManager cart = CartManager.GetInstance();
			OrderServiceManager.AddOrder(cart.Order, success: () =>
			{
				Activity.RunOnUiThread(() =>
				{
					Toast.MakeText(this.Activity, "El pedido se ha registrado exitosamente", ToastLength.Long).Show();
				});
				CartManager.GetInstance().ResetCart();
				MenuActivity activity = Activity as MenuActivity;
				activity.ClearPresaleStackFragment();
				activity.ShowFragment(new ClientsListFragment(), "ClientsListFragment");
			}, failure: () =>
			{
				Activity.RunOnUiThread(() =>
				{
					Toast.MakeText(this.Activity, "Hubo un error al generar el pedido", ToastLength.Long).Show();
				});
			});
		}

		public void OnClick(View v)
		{
			MenuActivity activity = Activity as MenuActivity;
			activity.OnBackPressed();
		}
	}
}
