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
    public static class Principal
    {
        private static int _PCSpeedinMs = 1000;

        public static int PCSpeedinMs
        {
            get
            {
                return _PCSpeedinMs;
            }

            set
            {
                _PCSpeedinMs = value;
            }
        }
    }
}