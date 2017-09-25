using UnityEngine;
using System;
using Svelto.DataStructures;
using System.Collections.Concurrent;

namespace Svelto.ECS.Example.Flock
{
    public class Boid : MonoBehaviour, IBoidComponent, IImplementor, ITransformComponent, IThreadSafeTransformComponent
    {
        [Serializable]
        public class DebugSettings
        {
            public bool enableDrawing = false;

            public bool obstaclesAvoidanceDraw = false;
            public Color obstaclesAvoidanceColor = Color.red;

            public bool velocityDraw = false;
            public Color velocityColor = Color.grey;

            public bool positionForceDraw = false;
            public Color positionForceColor = Color.cyan;

            public bool alignmentForceDraw = false;
            public Color alignmentForceColor = Color.yellow;

            public bool cohesionForceDraw = false;
            public Color cohesionForceColor = Color.magenta;

            public bool collisionsAvoidanceForceDraw = false;
            public Color collisionsAvoidanceForceColor = Color.green;

            public bool attractionForceDraw = false;
            public Color attractionForceColor = Color.green;

            public bool totalForceDraw = false;
            public Color totalForceColor = Color.black;
        }

        DebugSettings dbgSts = null;
        Transform _transform;
        private Vector3 _position;

        public DebugSettings DebugSettingsRef
        {
            get { return dbgSts; }
            set { dbgSts = value; }
        }

        public Vector3 velocity { set; get; }

        public Transform T
        {
            get
            {
                return _transform;
            }
        }

        public ConcurrentDictionary<int, BoidNode> lastHashSet
        {
            get;

            set;
        }

        
        public int lastCount
        {
            get;

            set;
        }

        public int lastPos
        {
            get;

            set;
        }

        public Vector3 position
        {
            get { return _position; }
            set { _position = value; }
        }

        public Quaternion rotation
        {
            get;

            set;
        }

        public bool processed
        {
            get;

            set;
        }

        void Start()
        {
            _transform = transform;
            position = _transform.position;
            rotation = _transform.rotation;
            if (dbgSts == null)
            {
                Debug.LogWarning("Boid initialized with standalone debug settings copy");
                dbgSts = new DebugSettings();
            }
        }
    }
}