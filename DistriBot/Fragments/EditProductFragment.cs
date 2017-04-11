using System;
using System.Collections.Generic;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;

namespace DistriBot
{
	public class EditProductFragment : DialogFragment
	{

		private Product product;
		private Order order;

		private NumberPicker numberPicker;
		private EditText txtQuantity;
		private Button btnAccept;
		private Button btnDelete;
		private Button btnCancel;
		private TextView totalPrice;
		private double subTotal = 0;

		public EditProductFragment(Product p, Order o)
		{
			product = p;
			order = o;
		}

		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			View view = inflater.Inflate(Resource.Layout.EditProductFragment, container, false);
			view.FindViewById<TextView>(Resource.Id.txtProductName).Text = product.Name;
			view.FindViewById<TextView>(Resource.Id.txtUnitPrice).Text = "$ " + product.UnitPrice + "/" + product.MeasurementUnit;
			view.FindViewById<TextView>(Resource.Id.txtUnit).Text = product.MeasurementUnit;
			txtQuantity = view.FindViewById<EditText>(Resource.Id.txtQuantity);
			Tuple<int, double, double> item = order.Products.Find(p => p.Item1 == product.Id);
			txtQuantity.Text = item.Item2.ToString();
			txtQuantity.TextChanged += TxtQuantity_TextChanged;
			totalPrice = view.FindViewById<TextView>(Resource.Id.txtTotalPrice);
			totalPrice.Text = item.Item3.ToString();
			numberPicker = view.FindViewById<NumberPicker>(Resource.Id.numPickQuantity);
			numberPicker.MinValue = 1;
			numberPicker.MaxValue = 20;
			numberPicker.SetBackgroundColor(Android.Graphics.Color.ParseColor("#455A64"));
			numberPicker.ValueChanged += NumberPicker_ValueChanged;

			btnAccept = view.FindViewById<Button>(Resource.Id.btnAccept);
			btnAccept.Click += BtnAccept_Click;
			btnDelete = view.FindViewById<Button>(Resource.Id.btnDelete);
			btnDelete.Click += BtnDelete_Click;
			btnCancel = view.FindViewById<Button>(Resource.Id.btnCancel);
			btnCancel.Click += BtnCancel_Click;

			return view;
		}

		void TxtQuantity_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
		{
			try
			{
				double finalPrice = product.UnitPrice * Convert.ToDouble(e.Text.ToString());
				subTotal = finalPrice;
				totalPrice.Text = "Subtotal: $" + finalPrice.ToString();
			}
			catch (FormatException)
			{
				//Cannot convert string to double.
				totalPrice.Text = "Subtotal: $";
			}
		}

		void NumberPicker_ValueChanged(object sender, NumberPicker.ValueChangeEventArgs e)
		{
			txtQuantity.Text = e.NewVal.ToString();
			txtQuantity.SetSelection(txtQuantity.Text.Length);
		}

		void BtnAccept_Click(object sender, EventArgs e)
		{
			if (subTotal > 0)
			{
				try
				{
					Tuple<int, double, double> productCart = new Tuple<int, double, double>(product.Id, Convert.ToDouble(txtQuantity.Text), subTotal);
					CartManager.GetInstance().Order.Products.Add(productCart);
					CartManager.GetInstance().Order.Price += subTotal;
					CartManager.GetInstance().Products.Add(product);
					Toast.MakeText(this.Activity, "Producto agregado exitosamente", ToastLength.Long).Show();
				}
				catch (FormatException)
				{
					//Cannot convert string to double.
					Toast.MakeText(this.Activity, "Ingrese una cantidad correcta", ToastLength.Long).Show();
				}
				finally
				{
					this.Dismiss();
				}
			}
			else
			{
				Toast.MakeText(this.Activity, "Ingrese una cantidad correcta", ToastLength.Long).Show();
			}
		}

		void BtnDelete_Click(object sender, EventArgs e)
		{

		}

		void BtnCancel_Click(object sender, EventArgs e)
		{
			this.Dismiss();
		}
	}
}

/*	



		void BtnAdd_Click(object sender, EventArgs e)
		{
			if (subTotal > 0)
			{
				try
				{
					Tuple<int, double, double> productCart = new Tuple<int, double, double>(product.Id, Convert.ToDouble(txtQuantity.Text), subTotal);
					CartManager.GetInstance().Order.Products.Add(productCart);
					CartManager.GetInstance().Order.Price += subTotal;
					CartManager.GetInstance().Products.Add(product);
					Toast.MakeText(this.Activity, "Producto agregado exitosamente", ToastLength.Long).Show();
				}
				catch (FormatException)
				{
					//Cannot convert string to double.
					Toast.MakeText(this.Activity, "Ingrese una cantidad correcta", ToastLength.Long).Show();
				}
				finally
				{
					this.Dismiss();
				}
			}
			else
			{
				Toast.MakeText(this.Activity, "Ingrese una cantidad correcta", ToastLength.Long).Show();
			}
		}

 */
