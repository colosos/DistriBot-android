﻿
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
using Android.Support.V7.App;
using BottomNavigationBar;
using BottomNavigationBar.Listeners;
using SupportFragment = Android.Support.V4.App.Fragment;

namespace DistriBot
{
    [Activity(Label = "MenuActivity", Theme = "@style/DefaultTheme")]
    public class MenuActivity : AppCompatActivity, IOnTabClickListener
    {
        private BottomBar bottomBar;

        private SupportFragment mCurrentFragment;
        private ProductsFragment mProductsFragment;
        private ClientsOnMapFragment mClientsOnMapFragment;

        private LinkedList<SupportFragment> mStackFragmentPreSales;
        private LinkedList<SupportFragment> mStackFragmentCatalogue;
        private LinkedList<SupportFragment> mStackFragmentDeliveryRequests;
        private LinkedList<LinkedList<SupportFragment>> mStackStacks;
        private LinkedList<SupportFragment> mCurrentStack;

        protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Menu);

            backpressed = true;

            mProductsFragment = new ProductsFragment();
            mClientsOnMapFragment = new ClientsOnMapFragment();

            mStackFragmentPreSales = new LinkedList<SupportFragment>();
            mStackFragmentCatalogue = new LinkedList<SupportFragment>();
            mStackFragmentDeliveryRequests = new LinkedList<SupportFragment>();
            mCurrentStack = new LinkedList<SupportFragment>();
            mStackStacks = new LinkedList<LinkedList<SupportFragment>>();

            var trans = SupportFragmentManager.BeginTransaction();
            trans.Add(Resource.Id.fragmentContainer, mClientsOnMapFragment, "ClientsOnMapFragment");
            trans.Hide(mClientsOnMapFragment);
            trans.Add(Resource.Id.fragmentContainer, mProductsFragment, "ProductsFragment");
            trans.Commit();
            mCurrentFragment = mProductsFragment;
            mCurrentStack = mStackFragmentPreSales;

            bottomBar = BottomBar.Attach(this, savedInstanceState);
            bottomBar.SetItems(Resource.Menu.BottomNavBar);
            bottomBar.SetOnTabClickListener(this);
        }

        public void OnTabReSelected(int position)
        {
            //throw new NotImplementedException();
        }

        public void OnTabSelected(int position)
        {
            /* El flag back pressed controla que este metodo no se ejecute
             * más de una vez al iniciar la aplicación o al hacer backpress */
            if (!backpressed)
            {
                switch (position)
                {
                    case 0:
                        ShowTab(mStackFragmentPreSales, mProductsFragment);
                        break;
                    case 1:
                        ShowTab(mStackFragmentCatalogue, mClientsOnMapFragment);
                        break;
                    case 2:
                        break;
                }
            }
            backpressed = false;
        }

        private void ShowTab(LinkedList<SupportFragment> stack, SupportFragment fragment)
        {
            if (stack.Count > 0)
            {
                fragment = stack.First.Value;
                stack.RemoveFirst();
            }

            var trans = SupportFragmentManager.BeginTransaction();
            trans.Hide(mCurrentFragment);
            trans.Show(fragment);
            trans.Commit();

            mCurrentStack.AddFirst(mCurrentFragment);
            mStackStacks.AddFirst(mCurrentStack);
            if (mStackStacks.Contains(stack)) mStackStacks.Remove(stack);
            mCurrentFragment = fragment;
            mCurrentStack = stack;

        }

        /* Este metodo lo llama el fragment contenido por la activity
         * navegar a otro fragment manteniendose dentro de la misma tab */
        public void ShowFragment(SupportFragment fragment, String tag)
        {
            var trans = SupportFragmentManager.BeginTransaction();
            trans.Hide(mCurrentFragment);
            trans.Add(Resource.Id.fragmentContainer, fragment, tag);
            trans.Commit();

            mCurrentStack.AddFirst(mCurrentFragment);
            mCurrentFragment = fragment;
        }

        private bool backpressed;
        public override void OnBackPressed()
        {
            if(mCurrentStack.Count > 0)
            {
                var trans = SupportFragmentManager.BeginTransaction();
                trans.Hide(mCurrentFragment);

                SupportFragment fragment = mCurrentStack.First.Value;
                mCurrentStack.RemoveFirst();

                trans.Show(fragment);
                trans.Commit();
                mCurrentFragment = fragment;
            }
            else
            {
                if(mStackStacks.Count > 0)
                {
                    mCurrentStack = mStackStacks.First.Value;
                    mStackStacks.RemoveFirst();

                    var trans = SupportFragmentManager.BeginTransaction();
                    trans.Hide(mCurrentFragment);

                    SupportFragment fragment = mCurrentStack.First.Value;
                    mCurrentStack.RemoveFirst();

                    trans.Show(fragment);
                    trans.Commit();
                    mCurrentFragment = fragment;

                    /* Lo siguiente es la manera de hacer backpress, cambiar de tab y que la bottom bar muestre
                     * como seleccionada la tab anterior. El flag backpress evita que se ejecute el callback de 
                     * seleccionar la tab */ 
                    backpressed = true;
                    if (mCurrentStack == mStackFragmentPreSales) bottomBar.SelectTabAtPosition(0, false);
                    if (mCurrentStack == mStackFragmentCatalogue) bottomBar.SelectTabAtPosition(1, false);
                    if (mCurrentStack == mStackFragmentDeliveryRequests) bottomBar.SelectTabAtPosition(2, false);
                }
                else
                {
                    base.OnBackPressed();

                }

            }
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    OnBackPressed();
                    break;

            }
            return false;
        }
        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);

            // Necessary to restore the BottomBar's state, otherwise we would
            // lose the current tab on orientation change.
            bottomBar.OnSaveInstanceState(outState);
        }
    }
}

