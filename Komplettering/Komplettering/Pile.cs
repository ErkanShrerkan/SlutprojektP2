using System;
using System.Collections.Generic;
using System.Text;

namespace Komplettering
{
    class Pile<T>
    {
        T[] array;

        public void Add(T item)
        {

        }

        public int Count
        {
            get { return array.Length; }
            set
            {
                array = new T[value];
            }
        }
    }
}
