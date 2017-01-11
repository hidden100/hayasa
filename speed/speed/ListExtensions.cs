using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace speed.Extensions
{
    public static class ListExtensions
    {
        public static ObservableList<T> Shuffle<T>(this ObservableList<T> list)
        {
            int n = list.Count;
            int tamnhoVariavel = list.Count;
            ObservableList<T> final = new ObservableList<T>();
            for (int i = 0; i < n; i++)
            {
                Random rnd = new Random();
                int random = rnd.Next(0, tamnhoVariavel - 1);

                final.Add(list[random]);
                list.RemoveAt(random);
                tamnhoVariavel = list.Count;
            }

            return final;
        }

        public static ObservableList<T> SubList<T>(this ObservableList<T> list, int initial, int length)
        {
            ObservableList<T> final = new ObservableList<T>();
            for(int i = initial; i < initial + length; i++ )
            {
                final.Add(list[i]);
            }

            return final;
        }

        public static ObservableList<T> TransferElements<T>(this ObservableList<T> list, int numberOfElements, ObservableList<T> toFillList)
        {
            
            if (list == null || list.Count == 0)
                return toFillList;
            if(numberOfElements > list.Count)
            {
                foreach(T element in list)
                {
                    toFillList.Add(element);
                }
            }
            else
            {
                int initialCount = list.Count;
                for(int i = initialCount; initialCount - i < numberOfElements; i--)
                {
                    toFillList.Add(list[i - 1]);
                    list.RemoveAt(i - 1);
                }
            }

            return toFillList;

        }

    }
}