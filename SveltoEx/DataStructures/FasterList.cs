using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Svelto.DataStructures
{
    public sealed class FasterListEnumerator<T> : IEnumerator<T>
    {
        private T[] _buffer;
        private int _size;
        private int _counter;

        public FasterListEnumerator(T[] buffer, int size)
        {
            _size = size;
            _counter = -1;
            _buffer = buffer;
        }

        T IEnumerator<T>.Current
        {
            get { return _buffer[_counter]; }
        }

        object IEnumerator.Current
        {
            get { return _buffer[_counter]; }
        }

        bool IEnumerator.MoveNext()
        {
            return ++_counter < _size;
        }

        void IEnumerator.Reset()
        {
            _counter = 0;
        }

        void IDisposable.Dispose()
        {
        }
    }

    public sealed class FasterList<T> : IEnumerable<T>, IList<T>
    {
        private const int MIN_SIZE = 32;
        private T[] _buffer;
        private int _size;

        public FasterList()
        {
            _size = 0;

            _buffer = new T[MIN_SIZE];
        }

        public FasterList(int initialSize)
        {
            _size = 0;

            _buffer = new T[initialSize];
        }

        public FasterList(ICollection<T> collection)
        {
            _buffer = new T[collection.Count];

            collection.CopyTo(_buffer, 0);

            _size = _buffer.Length;
        }

        public FasterList(FasterList<T> listCopy)
        {
            _buffer = new T[listCopy.Count];

            listCopy.CopyTo(_buffer, 0);

            _size = listCopy.Count;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new FasterListEnumerator<T>(_buffer, Count);
        }

        public int Count
        {
            get { return _size; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public T this[int i]
        {
            get { return _buffer[i]; }
            set { _buffer[i] = value; }
        }

        public int IndexOf(T item)
        {
            var comp = EqualityComparer<T>.Default;

            for (var index = _size - 1; index >= 0; --index)
            {
                if (comp.Equals(_buffer[index], item))
                {
                    return index;
                }
            }

            return -1;
        }

        public void Clear()
        {
            _size = 0;
        }

        public void Add(T item)
        {
            if (_size == _buffer.Length)
                AllocateMore();

            _buffer[_size++] = item;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            Array.Copy(_buffer, 0, array, arrayIndex, Count);
        }

        public void Insert(int index, T item)
        {
            if (index >= _size)
                throw new IndexOutOfRangeException();

            if (_size == _buffer.Length) AllocateMore();

            Array.Copy(_buffer, index, _buffer, index + 1, _size - index);

            _buffer[index] = item;
            ++_size;
        }

        public bool Contains(T item)
        {
            var index = IndexOf(item);

            return index != -1;
        }

        public bool Remove(T item)
        {
            var index = IndexOf(item);

            if (index == -1)
                return false;

            RemoveAt(index);

            return true;
        }

        public void RemoveAt(int index)
        {
            if (index >= _size)
                throw new IndexOutOfRangeException();

            --_size;

            if (index == _size)
                return;

            Array.Copy(_buffer, index + 1, _buffer, index, _size - index);
        }

        public void SetAt(int index, T value)
        {
            if (index >= _buffer.Length)
                AllocateMore(index + 1);

            if (_size <= index)
                _size = index + 1;

            this[index] = value;
        }

        private void AllocateMore()
        {
            var newList = new T[Mathf.Max(_buffer.Length << 1, MIN_SIZE)];
            if (_size > 0) _buffer.CopyTo(newList, 0);
            _buffer = newList;
        }

        private void AllocateMore(int newSize)
        {
            var oldLength = _buffer.Length;

            while (oldLength < newSize)
                oldLength <<= 1;

            var newList = new T[oldLength];
            if (_size > 0) _buffer.CopyTo(newList, 0);
            _buffer = newList;
        }

        private void Trim()
        {
            if (_size < _buffer.Length)
                Array.Resize(ref _buffer, _size);
        }

        public void Release()
        {
            _size = 0;
            _buffer = null;
        }

        public void AddRange(IEnumerable<T> items, int count)
        {
            if (_size + count >= _buffer.Length)
                AllocateMore(_size + count);

            foreach (var item in items)
                _buffer[_size++] = item;
        }

        public void AddRange(ICollection<T> items)
        {
            var count = items.Count;
            if (_size + count >= _buffer.Length)
                AllocateMore(_size + count);

            foreach (var item in items)
                _buffer[_size++] = item;
        }

        public void AddRange(FasterList<T> items)
        {
            var count = items.Count;
            if (_size + count >= _buffer.Length)
                AllocateMore(_size + count);

            Array.Copy(items._buffer, 0, _buffer, _size, count);
            _size += count;
        }

        public void AddRange(T[] items)
        {
            var count = items.Length;
            if (_size + count >= _buffer.Length)
                AllocateMore(_size + count);

            Array.Copy(items, 0, _buffer, _size, count);
            _size += count;
        }

        public bool UnorderredRemove(T item)
        {
            var index = IndexOf(item);

            if (index == -1)
                return false;

            UnorderredRemoveAt(index);

            return true;
        }

        public void UnorderredRemoveAt(int index)
        {
            if (index >= _size)
                throw new IndexOutOfRangeException();

            _buffer[index] = _buffer[--_size];
        }

        public T[] ToArray()
        {
            Trim();
            return _buffer;
        }

        private T[] ToArrayFast()
        {
            return _buffer;
        }

        public ReadOnlyCollection<T> AsReadOnly()
        {
            Trim();
            return Array.AsReadOnly(_buffer);
        }

        public void Sort(Comparison<T> comparer)
        {
            Array.Sort(_buffer, comparer);
        }

        public void Resize(int newSize)
        {
            Array.Resize(ref _buffer, newSize);
        }
    }
}
