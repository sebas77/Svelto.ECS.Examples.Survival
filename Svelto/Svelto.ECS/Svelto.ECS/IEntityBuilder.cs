using System;
using System.Collections.Generic;
using Svelto.ECS.Internal;

namespace Svelto.ECS
{
    public interface IEntityBuilder
    {
        void BuildEntityAndAddToList(ref ITypeSafeDictionary dictionary, EGID entityID, object[] implementors);
        ITypeSafeDictionary Preallocate(ref ITypeSafeDictionary dictionary, int size);

        Type GetEntityType();
        
        IEnumerable<DataBindsToAttribute> Attributes { get; }
    }
}