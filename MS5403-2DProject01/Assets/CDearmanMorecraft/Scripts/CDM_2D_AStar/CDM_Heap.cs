using System.Collections;
using UnityEngine;
using System;

public class Heap<T> where T : IHeapItem<T>
{
    public T[] items;
    public int in_itemCount;

    public Heap(int _maxHeapSize)
    {
        items = new T[_maxHeapSize];
    }

    public void Add(T item)
    {
        item.HeapIndex = in_itemCount;
        items[in_itemCount] = item;
        SortItemUp(item);
        in_itemCount++;
    }

    public T RemoveFirst()
    {
        T _firstItem = items[0];
        in_itemCount--;

        items[0] = items[in_itemCount];
        items[0].HeapIndex = 0;

        SortItemDown(items[0]);

        return _firstItem;
    }

    public void UpdateItem(T item)
    {
        SortItemUp(item);
    }

    public int Count
    {
        get
        {
            return in_itemCount;
        }
    }

    public bool Contains(T item)
    {
        return Equals(items[item.HeapIndex], item);
    }

    void SortItemDown(T item)
    {
        while (true)
        {
            int _childIndexL = item.HeapIndex * 2 + 1;
            int _childIndexR = item.HeapIndex * 2 + 2;
            int _swapIndex = 0;

            if (_childIndexL < in_itemCount)
            {
                _swapIndex = _childIndexL;

                if (_childIndexR < in_itemCount)
                {
                    if (items[_childIndexL].CompareTo(items[_childIndexR]) < 0)
                    {
                        _swapIndex = _childIndexR;
                    }
                }

                if (item.CompareTo(items[_swapIndex]) < 0)
                {
                    Swap(item, items[_swapIndex]);
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }
    }

    void SortItemUp(T item)
    {
        int _in_ParentIndex = (item.HeapIndex - 1) / 2;

        while (true)
        {
            T _parentItem = items[_in_ParentIndex];
            
            if (item.CompareTo(_parentItem) > 0)
            {
                Swap(item, _parentItem);
            }
            else
            {
                break;
            }

            _in_ParentIndex = (item.HeapIndex - 1) / 2;
        }
    }

    void Swap(T itemA, T itemB)
    {
        items[itemA.HeapIndex] = itemB;
        items[itemB.HeapIndex] = itemB;

        int _itemAIndex = itemA.HeapIndex;

        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = _itemAIndex;
    }
}

public interface IHeapItem<T> : IComparable<T>
{
    int HeapIndex
    {
        get;
        set;
    }
}
