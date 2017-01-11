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

namespace speed
{
    [Activity(Label = "Choose PC Reaction")]
    public class ChoosePCReactionActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.ChoosePCReaction);

            Button buttonPC = FindViewById<Button>(Resource.Id.startPCGame);
            EditText PCLevel = FindViewById<EditText>(Resource.Id.txtPCLevel);

            buttonPC.Click += (sender, e) =>
            {

                var intent = new Intent(this, typeof(_1x1GameActivity));
                int temp = 0;
                if (Int32.TryParse(PCLevel.Text, out temp))
                {
                    Principal.PCSpeedinMs = temp;
                }
                StartActivity(intent);
            };
            // Create your application here
        }
    }
}