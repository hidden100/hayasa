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
    public class ObservableList<T> : List<T>
    {
        public event EventHandler OnChange;

        public void IChanged()
        {
            OnChange(this, null);
        }
        public void Add(T item)
        {
            base.Add(item);
            if(null != OnChange)
            {
                OnChange(this, null);
            }
        }

        public void Clear()
        {
            base.Clear();
            if(null != OnChange)
            {
                OnChange(this, null);
            }
        }

        public void  RemoveAt(int index)
        {
            base.RemoveAt(index);
            if(null != OnChange)
            {
                OnChange(this, null);
            }
        }


    }
   
}