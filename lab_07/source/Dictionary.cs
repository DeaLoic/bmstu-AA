using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

namespace lab_07
{
    class Dictionary
    {
        string[] body;
        Segment[] segments;
        int segmentsCount = 0;
        public int fill = 0;
        public string this[int i]
        {
            get { return body[i]; }
            set { body[i] = value; }
        }   

        public Dictionary(string pathToFile)
        {
            if (File.Exists(pathToFile))
            {
                body = new string[100];
                using (StreamReader sr = File.OpenText(pathToFile))
                {
                    string s;
                    fill = 0;
                    while ((s = sr.ReadLine()) != null)
                    {
                        if (fill == body.Length)
                        {
                            Array.Resize(ref body, (int)(fill * 1.5));
                        }
                        this[fill] = s.ToLower();
                        fill++;
                    }
                }
                Array.Resize(ref body, fill);
                Sort();
                FormSegments();
            }
        }

        public void Sort()
        {
            Array.Sort(body);
        }

        public void FormSegments()
        {
            if (fill > 0)
            {
                segments = new Segment[33];
                segmentsCount = 0;
                char key = this[0][0];
                int indexStart = 0;
                for (int i = 1; i < fill; i++)
                {
                    if (key != this[i][0])
                    {
                        if (segmentsCount == segments.Length)
                        {
                            Array.Resize(ref segments, (int)(segmentsCount * 1.5));
                        }
                        segments[segmentsCount] = (new Segment(key, indexStart, i - indexStart));
                        segmentsCount++;
                        key = this[i][0];
                        indexStart = i;
                    }
                }
                segments[segmentsCount] = (new Segment(key, indexStart, fill - indexStart));
                segmentsCount++;
                Array.Resize(ref segments, segmentsCount);
                Array.Sort(segments);
            }
        }
        // return index of key
        public int BruteForce(string key)
        {
            int index = -1;
            bool isFinded = false;
            for (int i = 0; i < fill && !isFinded; i++)
            {
                if (key.CompareTo(this[i]) == 0)
                {
                    isFinded = true;
                    index = i;
                }
            }

            return index;
        }

        public int BinarySearch(string key)
        {
            return BinarySearchBorders(key, 0, fill);
        }

        private int BinarySearchBorders(string key, int left, int right)
        {
            int mid = 0;
            int finded = 1;
            bool isFinded = false;

            while (!isFinded && left < right)
            {
                mid = left + (right - left) / 2;
                finded = key.CompareTo(this[mid]);
                if (finded == 0)
                {
                    isFinded = true;
                }
                else if (finded < 0)
                {
                    right = mid - 1;
                }
                else
                {
                    left = mid + 1;
                }
            }
            mid = isFinded ? mid : -1;
            return mid;
        }

        public int FindBySegments(string key)
        {
            int index = -1;
            char keyChar = key[0];
            Segment segment = Array.Find(segments, (a) => (a.key == keyChar));

            if (segment != null)
                index = BinarySearchBorders(key, segment.index, segment.index + segment.size);
            return index;
        }
    }
}
