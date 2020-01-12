using System;
using System.Collections;
using System.Collections.Generic;

namespace KouXiaGu.BinaryHeap
{

    /// <summary>
    /// 二叉堆;
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BinaryHeap<T> : ICollection<T>, IEnumerable<T>
    {
        public BinaryHeap()
        {
            collection = new List<T>();
            collection.Add(default(T));
        }
        public BinaryHeap(int capacity)
        {
            collection = new List<T>(capacity);
            collection.Add(default(T));
        }

        /// <summary>
        /// 根索引值;
        /// </summary>
        const int rootIndex = 1;

        List<T> collection;

        /// <summary>
        /// 结构存在有效的元素个数;
        /// </summary>
        public int Count
        {
            get
            {
                return collection.Count - 1;
            }
        }

        /// <summary>
        /// 根节点,根据排序的最大值或者最小值;
        /// </summary>
        public T Root
        {
            get
            {
                return collection[rootIndex];
            }
        }

        int lastNodeIndex
        {
            get
            {
                return collection.Count - 1;
            }
        }

        bool ICollection<T>.IsReadOnly
        {
            get
            {
                return ((ICollection<T>)this.collection).IsReadOnly;
            }
        }

        /// <summary>
        /// 比较 other 是否置于 current 之前;
        /// </summary>
        protected abstract bool Compare(T current, T other);

        /// <summary>
        /// 按循序加入到数据结构;
        /// </summary>
        public void Add(T item)
        {
            collection.Add(item);
            BubbleUp(lastNodeIndex);
        }

        public bool Remove(T item)
        {
            if (Count == 0)
                return false;

            int index = collection.IndexOf(item, rootIndex, Count);
            if (index == -1)
                return false;

            collection[index] = collection[lastNodeIndex];
            BubbleDown(index);
            collection.RemoveAt(lastNodeIndex);

            return true;
        }

        /// <summary>
        /// 输出并且移除根节点;
        /// </summary>
        public T Extract()
        {
            if (Count == 0)
                throw new InvalidOperationException("已经不存在元素;");

            T result = collection[rootIndex];
            collection[rootIndex] = collection[lastNodeIndex];
            BubbleDown(rootIndex);
            collection.RemoveAt(lastNodeIndex);

            return result;
        }

        public bool Contains(T item)
        {
            return collection.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            collection.CopyTo(array, arrayIndex);
        }

        public void Clear()
        {
            collection.Clear();
            collection.Add(default(T));
        }

        void BubbleUp(int index)
        {
            int currentIndex = index;
            int parentIndex = GetParentIndex(index);
            T current = collection[currentIndex];

            while (currentIndex > rootIndex)
            {
                if (Compare(collection[parentIndex], current))
                {
                    collection[currentIndex] = collection[parentIndex];
                    currentIndex = parentIndex;
                    parentIndex = GetParentIndex(parentIndex);
                }
                else
                {
                    break;
                }
            }
            collection[currentIndex] = current;
        }

        void BubbleDown(int index)
        {
            int currentIndex = index;
            int leftChildIndex = GetLeftChildIndex(index);
            T current = collection[currentIndex];

            while (leftChildIndex <= lastNodeIndex)
            {
                if (leftChildIndex < lastNodeIndex && Compare(collection[leftChildIndex], collection[leftChildIndex + 1]))
                {
                    leftChildIndex++;
                }

                if (Compare(current ,collection[leftChildIndex]))
                {
                    collection[currentIndex] = collection[leftChildIndex];
                    currentIndex = leftChildIndex;
                    leftChildIndex = GetLeftChildIndex(leftChildIndex);
                }
                else
                {
                    break;
                }
            }
            collection[currentIndex] = current;
        }

        int GetParentIndex(int index)
        {
            return (int)decimal.Floor(index / 2);
        }

        int GetLeftChildIndex(int index)
        {
            return (2 * index);
        }

        int GetRightChildIndex(int index)
        {
            return (2 * index + 1);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return collection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return collection.GetEnumerator();
        }

    }

    public class MinHeap<T> : BinaryHeap<T>
        where T : IComparable<T>
    {
        public MinHeap() { }
        public MinHeap(int capacity) : base(capacity) { }

        protected override bool Compare(T current, T other)
        {
            return other.CompareTo(current) < 0;
        }
    }

    public class MaxHeap<T> : BinaryHeap<T>
        where T : IComparable<T>
    {
        public MaxHeap() { }
        public MaxHeap(int capacity) : base(capacity) { }

        protected override bool Compare(T current, T other)
        {
            return other.CompareTo(current) > 0;
        }
    }

}