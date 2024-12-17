using System;
using System.Collections.Generic;

namespace PuzzleGame;

public class BinaryHeapPriorityQueue<T>
{
    private readonly List<(T Item, int Priority)> _heap = [];

    public int Count => _heap.Count;

    public void Enqueue(T item, int priority)
    {
        _heap.Add((item, priority));
        BubbleUp(_heap.Count - 1);
    }

    public T Dequeue()
    {
        if (_heap.Count == 0) throw new InvalidOperationException("The queue is empty.");

        var item = _heap[0].Item;
        _heap[0] = _heap[^1];
        _heap.RemoveAt(_heap.Count - 1);

        if (_heap.Count > 0)
        {
            BubbleDown(0);
        }

        return item;
    }

    public bool Contains(T item)
    {
        return _heap.Exists(x => EqualityComparer<T>.Default.Equals(x.Item, item));
    }

    public void UpdatePriority(T item, int newPriority)
    {
        for (var i = 0; i < _heap.Count; i++)
        {
            if (EqualityComparer<T>.Default.Equals(_heap[i].Item, item))
            {
                _heap[i] = (item, newPriority);
                BubbleUp(i);
                BubbleDown(i);
                return;
            }
        }

        throw new InvalidOperationException("Item not found in the priority queue.");
    }

    private void BubbleUp(int index)
    {
        while (index > 0)
        {
            var parentIndex = (index - 1) / 2;
            if (_heap[index].Priority >= _heap[parentIndex].Priority) break;

            Swap(index, parentIndex);
            index = parentIndex;
        }
    }

    private void BubbleDown(int index)
    {
        var lastIndex = _heap.Count - 1;

        while (true)
        {
            var leftChild = 2 * index + 1;
            var rightChild = 2 * index + 2;
            var smallest = index;

            if (leftChild <= lastIndex && _heap[leftChild].Priority < _heap[smallest].Priority)
            {
                smallest = leftChild;
            }

            if (rightChild <= lastIndex && _heap[rightChild].Priority < _heap[smallest].Priority)
            {
                smallest = rightChild;
            }

            if (smallest == index) break;

            Swap(index, smallest);
            index = smallest;
        }
    }

    private void Swap(int index1, int index2)
    {
        (_heap[index1], _heap[index2]) = (_heap[index2], _heap[index1]);
    }
}
