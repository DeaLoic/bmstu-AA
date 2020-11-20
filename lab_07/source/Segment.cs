using System;
using System.Collections.Generic;
using System.Text;

namespace lab_07
{
    class Segment : IComparable
    {
        public char key;
        public int index = 0;
        public int size = 0;

        public Segment(char key, int index, int size)
        {
            this.key = key;
            this.index = index;
            this.size = size;
        }

        public int CompareTo(object second)
        {
            return key.CompareTo(((Segment)second).key);
        }

        public int CompareTo(char n)
        {
            return key.CompareTo(n);
        }
    }


}
