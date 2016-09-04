using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Support.V4.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;
using Android.Support.V7.Widget;

namespace DistriBot
{
    public class ClientsListFragment : Fragment
    {

        private RecyclerView mRecyclerView;
        private LinearLayoutManager mLayoutManager;
        private ClientsRecyclerAdapter mAdapter;
        private List<Client> clients = new List<Client>();
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            //esto hace que se llame al OnOptionsItemSelected
            HasOptionsMenu = true;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.ClientsListFragment, container, false);
            return view;
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            LoadClients();
            base.OnActivityCreated(savedInstanceState);
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            base.OnCreateOptionsMenu(menu, inflater);
            SetUpMap();
        }

        private void SetUpMap()
        {
            var toolbar = View.FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            var activity = Activity as AppCompatActivity;
            toolbar.InflateMenu(Resource.Menu.MenuClientsList);
            activity.SetSupportActionBar(toolbar);
        }

        void LoadClients()
        {
            var progressDialogue = Android.App.ProgressDialog.Show(Context, "", "Cargando clientes...", true, true);
            ClientServiceManager.GetClients(success: (obj) =>
            {
                progressDialogue.Dismiss();
                clients = obj;
                Activity.RunOnUiThread(() =>
                {
                    CreateAdapter();
                });

            }, failure: (obj) =>
            {
                Android.Widget.Toast.MakeText(Context, "Ha ocurrido un error al cargar los clientes", Android.Widget.ToastLength.Short).Show();
            });
        }

        private void CreateAdapter()
        {
            mAdapter = new ClientsRecyclerAdapter(clients);
            mAdapter.ItemClick += OnListItemClick;
            mRecyclerView = View.FindViewById<RecyclerView>(Resource.Id.recyclerView);
            if (mRecyclerView != null)
            {
                mRecyclerView.HasFixedSize = true;
                mLayoutManager = new LinearLayoutManager(Context);

                //CreateScrollListener();

                mRecyclerView.SetLayoutManager(mLayoutManager);
                mRecyclerView.SetAdapter(mAdapter);
            }
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.action_view_map:
                    MenuActivity actividad = Activity as MenuActivity;
                    actividad.ShowFragment(new ClientsOnMapFragment(clients), "ClientsOnMap");
                    return true;

            }
            return false;
        }

        void OnListItemClick(object sender, int position)
        {
            if (position >= 0)
            {
                var c = clients[position];
                //Llamo el metodo de la actividad menu que muestra el fragment del detalle del client
                //MenuActivity actividad = Activity as MenuActivity;
                //actividad.ShowFragment(new ProductDetailFragment(p), "Detalle");

            }
        }

        //private void CreateScrollListener()
        //{
        //    var onScrollListener = new RecyclerViewOnScrollListener(mLayoutManager);
        //    onScrollListener.LoadMoreEvent += (object sender, EventArgs e) =>
        //    {
        //        //Load more stuff here
        //        List<Product> addProducts = new List<Product>()
        //        {
        //                new Product(14, "Chocolate", 3.11),
        //                new Product(15, "Arroz", 3.11),
        //                new Product(16, "Banana", 3.11),
        //                new Product(17, "Manzana", 3.11),
        //                new Product(18, "Limón", 3.11)
        //        };
        //        clients.AddRange(addProducts);
        //        mAdapter.NotifyItemRangeInserted(clients.Count, addProducts.Count);
        //    };

        //    mRecyclerView.AddOnScrollListener(onScrollListener);
        //}
    }
}