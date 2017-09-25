using Svelto.DataStructures;
using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace Svelto.ECS.Example.Flock
{
    public interface IBoidComponent
    {
        Vector3 velocity { get; set; }
    }
}