using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapping
{
    class priorityQueue
    {
        int Heapsize;
        Tuple<Tuple<float, int>, int>[] arr;

        public priorityQueue()
        {
            Heapsize = 1;
            arr = new Tuple<Tuple<float, int>, int>[200000];
        }
        public void minheapify(int index)
        {
            if (Heapsize <= 2)
            {
                return;
            }
            int leftindex = 2 * index;
            int rightindex = 2 * index + 1;
            int smallest;
            if (leftindex <= Heapsize && arr[leftindex].Item1.Item1 <= arr[index].Item1.Item1)
            {
                smallest = leftindex;
            }
            else
            {
                smallest = index;
            }
            if (rightindex <= Heapsize && arr[rightindex].Item1.Item1 <= arr[index].Item1.Item1)
            {
                smallest = rightindex;
            }
            if (smallest != index)
            {
                Tuple<Tuple<float, int>, int> temp = new Tuple<Tuple<float, int>, int>(arr[index].Item1, arr[index].Item2);
                arr[index] = arr[smallest];
                arr[smallest] = temp;
                minheapify(smallest);
            }

        }

        public Tuple<float, int> heapmin()
        {
            return arr[1].Item1;
        }
        public Tuple<float, int> heap_extract_min()
        {
            if (Heapsize < 2)
            {
                return null;
            }
            Tuple<float, int> tempvertex = new Tuple<float, int>(arr[1].Item1.Item1, arr[1].Item1.Item2);
            arr[1] = arr[Heapsize - 1];
            Heapsize = Heapsize - 1;
            minheapify(1);
            return tempvertex;
        }
        public void decreasekey(int index, float key)
        {
            if (key > arr[index].Item1.Item1)
            {
                return;
            }
            Tuple<float, int> newkey = new Tuple<float, int>(key, arr[index].Item1.Item2);
            Tuple<Tuple<float, int>, int> newvalue = new Tuple<Tuple<float, int>, int>(newkey, arr[index].Item2);
            arr[index] = newvalue;
            int parentindex = index / 2;
            while (index > 1 && arr[parentindex].Item1.Item1 > arr[index].Item1.Item1)
            {
                Tuple<float, int> item1 = new Tuple<float, int>(arr[parentindex].Item1.Item1, arr[parentindex].Item1.Item2);
                Tuple<Tuple<float, int>, int> temp = new Tuple<Tuple<float, int>, int>(item1, arr[parentindex].Item2);
                arr[parentindex] = arr[index];
                arr[index] = temp;
            }
            index = parentindex;
        }
        public void insert(float key, int number)
        {
            Heapsize = Heapsize + 1;
            Tuple<float, int> vertex = new Tuple<float, int>(float.MaxValue, number);
            Tuple<Tuple<float, int>, int> newvalue = new Tuple<Tuple<float, int>, int>(vertex, Heapsize - 1);
            arr[Heapsize - 1] = newvalue;
            decreasekey(Heapsize - 1, key);
        }

        public bool Isempty()
        {
            return (Heapsize == 1);
        }

    }
}
