using Svelto.DataStructures;
using Svelto.ECS.Example.Flock.Nodes;
using Svelto.Tasks;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Svelto.ECS.Example.Flock.Engines
{
    class BoidsEngine : INodeEngine<SettingsNode>, IQueryableNodeEngine
    {
public const uint NUM_OF_THREADS = 128;

        const float GridSize = 0.5f;
        const float scale = 1;

        public BoidsEngine()
        {
            //            _hashspace.Clear();

            //          GameObject go = new GameObject("test");
            //        go.AddComponent<Test>()._hashspace = _hashspace;
            _Target = GameObject.FindObjectOfType<SelectRandomPosition>().transform;
        }

        void UpdatePosF()
        {
            //  PInvokeWrapper.PIXBeginEventEx(0, "UpdatePos");
            UnityEngine.Profiling.Profiler.BeginSample("UpdatePos");
            var nodes = nodesDB.QueryNodes<BoidNode>();

            var count = nodes.Count;

            for (int i = count - 1; i >= 0; i--)
            {
                BoidNode boidNode = nodes[i];
                var transformTS = boidNode.transformTS;
                Transform t = boidNode.transform.T;

                t.position = transformTS.position;
                t.rotation = transformTS.rotation;
            }
            UnityEngine.Profiling.Profiler.EndSample();
            //      PInvokeWrapper.PIXEndEventEx();

        }
/*
        void InitializeHashStructure(BoidNode[] nodes, int start, int countn)
        {
            for (int index = start; index < countn; index++)
            {
                BoidNode boidNode = nodes[index];
                var transformTS = boidNode.transform;

                Vector3 position = transformTS.T.position;

                FasterListThreadSafe<BoidNode> hashset;
                int hashpos = HashPos(position.x, position.y, position.z);
                hashset = _hashspace.GetOrAdd(hashpos, Create);
                hashset.Add(boidNode);
            }
        }
/*
        private IEnumerator ProcessBoidsPerSpaceG(int key, FasterListThreadSafe<BoidNode> boidList)
        {
            UpdateHash hash; _updateHashes.TryDequeue(out hash);

            hash.boidList = boidList; hash.key = key;

            return hash;
        }

        private void ProcessBoidsPerSpace(int key, FasterListThreadSafe<BoidNode> boidList)
        {
            BoidNode boidNode; 
            try
            {
                for (int i = 0; i < boidList.Count; ++i)
                {
                    boidNode = boidList[i];

                    var transformTS = boidNode.transformTS;
                    Vector3 position = transformTS.position = transformTS.position2;

                    int hashpos = HashPos(position.x, position.y, position.z);

                    if (hashpos != key)
                    {
                        boidList.UnorderredRemoveAt(i);
                        i--;
                        if (boidList.Count == 0)
                        {
                            _hashspace.TryRemove(key, out boidList);
                            pool.Enqueue(boidList);
                        }
                        
                        FasterListThreadSafe<BoidNode> hashset;
                        hashset = _hashspace.GetOrAdd(hashpos, Create);
                        hashset.Add(boidNode);
                    }
                }
            } catch (Exception e)
{
                UnityEngine.Debug.LogException(e);
            }
        }*/

        private FasterListThreadSafe<BoidNode> Create(int arg)
        {
            FasterListThreadSafe<BoidNode> hashset;
            //if (pool.TryDequeue(out hashset) == false)
                    hashset = new FasterListThreadSafe<BoidNode>(new FasterList<BoidNode>());
            return hashset;
        }

        int HashPos(float x, float y, float z)
        {
            var x1 = (FastRound((x / GridSize)));
            var y1 = (FastRound((y / GridSize)));
            var z1 = (FastRound((z / GridSize)));

            return x1 | y1 << 10 | z1 << 20;
        }

        const int BIG_ENOUGH_INT = 16 * 1024;
        const double BIG_ENOUGH_ROUND = BIG_ENOUGH_INT + 0.5;

        static int FastRound(float f)
        {
            var value = (int)(f + BIG_ENOUGH_ROUND) - BIG_ENOUGH_INT + 256;

            if (value >= 512 || value < 0)
            {
                throw new Exception("say what " + value + " " + f);
            }

            return value;
        }

        public void Add(SettingsNode node)
        {
            sts = node.settings.settings;

      /*      for (int i = 0; i < sts.BirdsCount + 1; i++)
            {
                pool.Enqueue(new FasterListThreadSafe<BoidNode>(new FasterList<BoidNode>()));
            }*/

            var boidNodes = nodesDB.QueryNodes<BoidNode>();
            var nodes = new BoidNode[boidNodes.Count];

            boidNodes.CopyTo(nodes, 0);

            int numberOfOperations = (int)Mathf.Min(128, boidNodes.Count) ; uint numberOfThreads = (uint)Mathf.Min(NUM_OF_THREADS, boidNodes.Count) ;

            var countn = nodes.Length / numberOfOperations;

       /*     _updateHashes = new ConcurrentQueue<UpdateHash>();

            for (int i = 0; i < 40000; i++) _updateHashes.Enqueue(new UpdateHash(this));*/

            parallelBoidComputationTasks = new MultiThreadParallelTaskCollection(numberOfThreads);

            for (int i = 0; i < numberOfOperations; i++)
                parallelBoidComputationTasks.Add(new BoidEnumerator(this, nodes, countn * i, countn * (i + 1)));

      //      InitializeHashStructure(nodes, 0, nodes.Length);



            FixedUpdate().ThreadSafeRunOnSchedule(StandardSchedulers.mainThreadScheduler);
        }

        public void Remove(SettingsNode node)
        {
            throw new NotImplementedException();
        }

        public IEngineNodeDB nodesDB { set; private get; }
        

        IEnumerator FixedUpdate()
        {
            Stopwatch sw = Stopwatch.StartNew();
            while (true)
            {
                _deltaTime = (float)sw.ElapsedMilliseconds / 1000.0f;

                sw.Reset(); sw.Start();

                _realTarget = _Target.position;

                var waitForBoidComputation = parallelBoidComputationTasks.ThreadSafeRunOnSchedule(StandardSchedulers.multiThreadScheduler);

                UpdatePosF();

                while (waitForBoidComputation.MoveNext());

                yield return null;
            }
        }

        class BoidEnumerator : IEnumerator
        {
            private int _countn;
            private int _start;
            private BoidNode[] _nodes;
            private BoidsEngine _engine;
            BoidNode[] colliders = new BoidNode[50];

            public object Current
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public BoidEnumerator(BoidsEngine engine, BoidNode[] nodes, int start, int countn)
            {
                _nodes = nodes;
                _start = start;
                _countn = countn;
                _engine = engine;
            }

            public bool MoveNext()
            {
                PInvokeWrapper.PIXBeginEventEx(0, "BoidOperations");

                for (int index = _start; index < _countn; index++)
                {
                    BoidNode boidNode = _nodes[index];

                    var transformTS = boidNode.transformTS;
                    var boid = boidNode.boid;
                    var curPos = transformTS.position;
                    var newtarget = _engine._realTarget - curPos;
                    
                    boid.velocity += (newtarget.normalized / 10);
                    transformTS.position += boid.velocity * _deltaTime;
if (boid.velocity.sqrMagnitude > 0.01f)
                    transformTS.rotation = Quaternion.Slerp(transformTS.rotation, Quaternion.LookRotation(boid.velocity), _deltaTime);
                }

                PInvokeWrapper.PIXEndEventEx();
                return false;
            }

            public void Reset()
            { }
        }


  /*      int OverlapSphereNonAlloc(BoidNode[] colliders, Vector3 curPos, float viewRadius, int max)
        {
            float x3 = curPos.x;
            int x1 = FastRound((x3) / GridSize);
            float y3 = curPos.y;
            int y1 = FastRound((y3) / GridSize);
            float z3 = curPos.z;
            int z1 = FastRound((z3) / GridSize);
            
            int i = 0;
            FasterListThreadSafe<BoidNode> bods;
            for (int x = x1 - 1; x <= x1+1; x++)
                for (int y = y1 - 1; y <= y1 + 1; y++)
                    for (int z = z1 - 1; z <= z1 + 1; z++)
                    {
                        if (_hashspace.TryGetValue(x | (y << 10) | (z << 20), out bods))
                        {
                            bods.CopyTo(colliders, i);
                            i += bods.Count;
                            if (i > max)
                                return max;
                        }
                    }

            return i;
        }
 /*       class Test : MonoBehaviour
        {
            public ConcurrentDictionary<int, FasterListThreadSafe<BoidNode>> _hashspace = new ConcurrentDictionary<int, FasterListThreadSafe<BoidNode>>();

            void OnDrawGizmos()
            {
                foreach (KeyValuePair<int, FasterListThreadSafe<BoidNode>> asd in _hashspace)
                {
                    var item = asd.Key;
                    int x = (item & (0x3ff)) - 256;
                    item >>= 10;
                    int y = (item & (0x3ff)) - 256;
                    item >>= 10;
                    int z = (item & (0x3ff)) - 256;
                    Gizmos.color = new Color(1, 0, 0, 0.5F);
                    Gizmos.DrawCube(new Vector3(x * GridSize, y * GridSize, z * GridSize), new Vector3(GridSize, GridSize, GridSize));
                }
            }
        }*/

        Main.BoidSettingsEx sts;
        static private float _deltaTime;
        private DateTime then;

       // ConcurrentDictionary<int, FasterListThreadSafe<BoidNode>> _hashspace = new ConcurrentDictionary<int, FasterListThreadSafe<BoidNode>>();
        //ConcurrentQueue<FasterListThreadSafe<BoidNode>> pool = new ConcurrentQueue<FasterListThreadSafe<BoidNode>>();

     //   MultiThreadParallelTaskCollection parallelHashSpaceUpdateOperations = new MultiThreadParallelTaskCollection(NUM_OF_THREADS);
        MultiThreadParallelTaskCollection parallelBoidComputationTasks;
        private Transform _Target;
        private Vector3 _realTarget;
        //       ConcurrentQueue<UpdateHash> _updateHashes;
    }
}