using System;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;

namespace DistriBot
{
	public class AddProductFragment : DialogFragment
	{

		private Product product;

		private NumberPicker numberPicker;
		private EditText txtQuantity;
		private Button btnAdd;
		private Button btnCancel;

		public AddProductFragment(Product mProduct)
		{
			product = mProduct;
		}	

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			View view = inflater.Inflate(Resource.Layout.AddProductFragment, container, false);

			view.FindViewById<TextView>(Resource.Id.txtProductName).Text = product.Name;
			view.FindViewById<TextView>(Resource.Id.txtUnitPrice).Text = "$ " + product.UnitPrice + "/" + product.MeasurementUnit;
			view.FindViewById<TextView>(Resource.Id.txtUnit).Text = product.MeasurementUnit;
			txtQuantity = view.FindViewById<EditText>(Resource.Id.txtQuantity);

			btnAdd = view.FindViewById<Button>(Resource.Id.btnAddProduct);
			btnAdd.Click += BtnAdd_Click;
			btnCancel = view.FindViewById<Button>(Resource.Id.btnCancel);
			btnCancel.Click += BtnCancel_Click;

			numberPicker = view.FindViewById<NumberPicker>(Resource.Id.numPickQuantity);
			numberPicker.MinValue = 1;
			numberPicker.MaxValue = 20;
			numberPicker.SetBackgroundColor(Android.Graphics.Color.ParseColor("#455A64"));
			numberPicker.ValueChanged += NumberPicker_ValueChanged;
			txtQuantity.Text = numberPicker.Value.ToString();

			return view;
		}

		void NumberPicker_ValueChanged(object sender, NumberPicker.ValueChangeEventArgs e)
		{
			txtQuantity.Text = e.NewVal.ToString();
			txtQuantity.SetSelection(txtQuantity.Text.Length);
		}

		void BtnAdd_Click(object sender, EventArgs e)
		{

		}

		void BtnCancel_Click(object sender, EventArgs e)
		{

		}
	}
}

