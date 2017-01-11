using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android;

namespace speed
{
    [Activity(Label = "speed", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        int count = 1;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            //Button button = FindViewById<Button>(Resource.Id.lookForBT);

            //button.Click += (sender, e) =>
            //{

            //    var intent = new Intent(this, typeof(CallHistoryActivity));

            //    StartActivity(intent);
            //};

            Button buttonPC = FindViewById<Button>(Resource.Id.btnAgainstPC);

            buttonPC.Click += (sender, e) =>
            {

                var intent = new Intent(this, typeof(ChoosePCReactionActivity));

                StartActivity(intent);
            };
        }
    }
}

