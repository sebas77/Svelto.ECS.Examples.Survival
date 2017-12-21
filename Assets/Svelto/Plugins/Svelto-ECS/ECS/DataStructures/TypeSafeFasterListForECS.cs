using System;
using System.Collections;
using System.Collections.Generic;
using Svelto.DataStructures;

namespace Svelto.ECS.Internal
{
    public interface ITypeSafeList: IEnumerable
    {
        void AddRange(ITypeSafeList entityViewListValue);

        ITypeSafeList Create();
        bool isQueryiableEntityView { get; }
        void UnorderedRemove(int index);
        ITypeSafeDictionary CreateIndexedDictionary();
        EntityView[] ToArrayFast(out int count);
    }

    class TypeSafeFasterListForECS<T>: FasterList<T> where T:IEntityView
    {
        protected TypeSafeFasterListForECS()
        {
            _mappedIndices = new Dictionary<int, int>();
        }
        
        public void UnorderedRemove(int mappedIndex)
        {
            var index = _mappedIndices[mappedIndex];
            _mappedIndices.Remove(mappedIndex);

            if (UnorderedRemoveAt(index))
                _mappedIndices[this[index].ID] = index;
        }
        
        public void AddRange(ITypeSafeList entityViewListValue)
        {
            var index = this.Count;
            
            AddRange(entityViewListValue as FasterList<T>);
            
            for (int i = index; i < Count; ++i)
                _mappedIndices[this[i].ID] = this.Count;
        }

        readonly Dictionary<int, int> _mappedIndices;
    }

    class TypeSafeFasterListForECSForStructs<T> : TypeSafeFasterListForECS<T>, ITypeSafeList where T:struct, IEntityStruct
    {
        public ITypeSafeList Create()
        {
            return new TypeSafeFasterListForECSForStructs<T>();
        }

        public bool isQueryiableEntityView
        {
            get { return false; }
        }

        public ITypeSafeDictionary CreateIndexedDictionary()
        {
            throw new Exception("Not Allowed");
        }

        public EntityView[] ToArrayFast(out int count)
        {
            throw new Exception("Not Allowed");
        }
    }
    
    class TypeSafeFasterListForECSForClasses<T> : TypeSafeFasterListForECS<T>, ITypeSafeList where T:EntityView
    {
        public ITypeSafeList Create()
        {
            return new TypeSafeFasterListForECSForClasses<T>();
        }

        public bool isQueryiableEntityView
        {
            get { return true; }
        }

        public ITypeSafeDictionary CreateIndexedDictionary()
        {
            return new TypeSafeDictionary<T>();
        }

        public EntityView[] ToArrayFast(out int count)
        {
            count = this.Count;
            
            return this.ToArrayFast();
        }
    }
}
