
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
        private Stack<SupportFragment> mStackFragment;

        protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Menu);

            mProductsFragment = new ProductsFragment();
            mClientsOnMapFragment = new ClientsOnMapFragment();

            mStackFragment = new Stack<SupportFragment>();

            var trans = SupportFragmentManager.BeginTransaction();
            trans.Add(Resource.Id.fragmentContainer, mClientsOnMapFragment, "ClientsOnMapFragment");
            trans.Hide(mClientsOnMapFragment);
            trans.Add(Resource.Id.fragmentContainer, mProductsFragment, "ProductsFragment");
            trans.Commit();
            mCurrentFragment = mProductsFragment;

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
            switch (position)
            {
                case 0:
                    ShowFragment(mProductsFragment);
                    break;
				case 1:
                    break;
                case 2:
                    ShowFragment(mClientsOnMapFragment);
                    break;

            }
        }

        private void ShowFragment(SupportFragment fragment)
        {
            var trans = SupportFragmentManager.BeginTransaction();
            trans.Hide(mCurrentFragment);
            trans.Show(fragment);
            trans.AddToBackStack(null);
            trans.Commit();

            mStackFragment.Push(mCurrentFragment);
            mCurrentFragment = fragment;
        }

        public override void OnBackPressed()
        {
            if(SupportFragmentManager.BackStackEntryCount > 0)
            {
                SupportFragmentManager.PopBackStack();
                mCurrentFragment = mStackFragment.Pop();
            }
            else
            {
                base.OnBackPressed();

            }
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

