using Svelto.Tasks;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using System.Timers;

namespace Svelto.ECS.Example.Parallelism
{
    class BoidsEngine :
MultiNodesEngine<BoidNode, PrintTimeNode>
#if FOURTH_TIER_EXAMPLE
 ,IStructNodeEngine<BoidNode>
#endif
,ICallBackOnAddEngine, Context.IWaitForFrameworkDestruction
    {
#if TURBO_EXAMPLE
        public const uint NUM_OF_THREADS = 8; //must be divisible by 4 for this exercise as I am not handling reminders
#endif
        
        public BoidsEngine()
        {}

        IEnumerator Update()
        {
            while (true)
            {
#if TURBO_EXAMPLE
                //note: yielding here is meaningless as it runs on a sync scheduler
                //which will stop the execution of the thread until the operation is
                //done. I just didn't want to confuse you.
                //Note that while the task runs inside the Sync Runner, the 
                //MultiThreadedParallelTaskCollection run on several threads
                //therefore main threads waits until the other threads are finished
                //I know it sounds complex, but look, is very simple right?
                yield return _multiParallelTask.ThreadSafeRunOnSchedule(_syncRunner);
#else
                //yield on the sync scheduler as I want hold the main thread on purpose.
                //In real life you wouldn't use sync scheduler as you don't want to 
                //stall the main thread.
                //note: RunOnSchedule (and ThreadSafeRunOnSchedule) allows to continue
                //the operation on another runner without stalling the current one.
                //yielding it allows the current operation to wait for the result.
                yield return _boidEnumerator.ThreadSafeRunOnSchedule(StandardSchedulers.syncScheduler);
#endif
                //run the cached enumerator on the next coroutine phase, yield until it's done. 
                //The thread will spin until is done. Yielding an enumerator on the same
                //runner actually executes it immediatly.
                yield return _testEnumerator;

                //since _testEnumerator runs synchronously, we need to yield for a frame
                //at least once, otherwise this enumerator becomes totally synchronously
                //transforming into an infinite loop.
                //I understand this can look all obscure if you don't know how yield works :(
                yield return null;
            }
        }


#if FOURTH_TIER_EXAMPLE
        public void CreateStructNodes(SharedStructNodeLists sharedStructNodeLists)
        {
            _structNodes = new StructNodes<BoidNode>(sharedStructNodeLists);
        }
#endif

        protected override void AddNode(BoidNode node)
        {
#if FIRST_TIER_EXAMPLE || SECOND_TIER_EXAMPLE || THIRD_TIER_EXAMPLE
            _nodes.Add(node);
#endif
#if FOURTH_TIER_EXAMPLE
            _structNodes.Add(node);
#endif
        }

        protected override void RemoveNode(BoidNode node)
        {
            throw new NotImplementedException();
        }

        protected override void AddNode(PrintTimeNode node)
        {
            _printNode = node;
        }

        protected override void RemoveNode(PrintTimeNode node)
        {
            throw new NotImplementedException();
        }

        IEnumerator WaitForNodesAdded()
        {
//Engines are usually designed to be able to cope with dynamic adding and removing of entities, 
//but in this case I needed to know when the entities are ready to be processed. This wouldn't be 
//strictly necessary if I coded the engine in a different way, but I decided to keep it simpler and more readable. 
//That's why the engine starts immediately a task that waits for the nodes to be added(it assumes that all the 
//entities are created on the same frame).This demo aims to be allocation free during the main execution, that's 
//why all the tasks are prepared before hand. In this step, we prepare just one task that runs the main operations 
//that must be executed on the entities.         
            int count;
#if FOURTH_TIER_EXAMPLE
            BoidNode[] _nodes;
#endif
            do
            {
#if FIRST_TIER_EXAMPLE || SECOND_TIER_EXAMPLE || THIRD_TIER_EXAMPLE
                count = _nodes.Count;
#endif
#if FOURTH_TIER_EXAMPLE
                _nodes =_structNodes.GetList(out count);           
#endif
                yield return null;
            } while (count == 0);

#if TURBO_EXAMPLE
            int numberOfThreads = (int)Mathf.Min(NUM_OF_THREADS, count);

            var countn = count / numberOfThreads;

            _multiParallelTask = new MultiThreadedParallelTaskCollection(numberOfThreads);
            _syncRunner = new SyncRunner(true);

            for (int i = 0; i < numberOfThreads; i++)
                _multiParallelTask.Add(new BoidEnumerator(_nodes, countn * i, countn));
#else
            _boidEnumerator = new BoidEnumerator(_nodes, 0, count);
#endif
            _testEnumerator = new TestEnumerator(_printNode);

            Update().ThreadSafeRunOnSchedule(StandardSchedulers.updateScheduler);
            
        }

        public void Ready()
        {
            TaskRunner.Instance.Run(WaitForNodesAdded());
        }

        public void OnFrameworkDestroyed()
        {
#if TURBO_EXAMPLE && UNITY_EDITOR 
//not needed on an actual client, but unity doesn't stop other threads when the execution is stopped
            _multiParallelTask.ClearAndKill();
#endif
        }

        /// <summary>
        /// This is just to be sure everything is actually running
        /// it also shows how you can run instruction on the mainthread
        /// that operates on data computed on other threads. It's all about
        /// which runners the single IEnumerator/ITaskRoutine runs on!
        /// </summary>
        class TestEnumerator : IEnumerator
        {
            public object Current { get { return null; } }

            public TestEnumerator(PrintTimeNode node)
            {
                _printNode = node;
            }

            public bool MoveNext()
            {
                if (_totalCount != NumberOfEntities.value * 4)
                    throw new Exception("something went wrong");
                else
                    _totalCount = 0;

                _printNode.component.iterations++;

                return false;
            }

            public void Reset()
            {
                throw new NotImplementedException();
            }

            PrintTimeNode _printNode;
        }

        /// <summary>
        /// The meaningless set of operations that 
        /// run on a set of nodes
        /// </summary>
        class BoidEnumerator : IEnumerator
        {
            private int _countn;
            private int _start;
#if FIRST_TIER_EXAMPLE || SECOND_TIER_EXAMPLE || THIRD_TIER_EXAMPLE
            private DataStructures.FasterList<BoidNode> _nodes;
#else
            BoidNode[] _nodes;
#endif

            public object Current  {   get { return null;  }  }
#if FIRST_TIER_EXAMPLE || SECOND_TIER_EXAMPLE || THIRD_TIER_EXAMPLE
            public BoidEnumerator(DataStructures.FasterList<BoidNode> nodes, int start, int countn)
            {
#else
            public BoidEnumerator(BoidNode[] nodes, int start, int countn)
            {
#endif
                _nodes = nodes;
                _start = start;
                _countn = countn;
            }

            public bool MoveNext()
            {
#if THIRD_TIER_EXAMPLE
                var entities = _nodes.ToArrayFast();
#else
                var entities = _nodes;
#endif
                Vector3 realTarget = new Vector3();
                realTarget.Set(1,2,3);

                for (int j = 0; j < 4; j++)
                {
                    var count = _start + _countn;
                    for (int index = _start; index < count; index++)
                    {
#if SECOND_TIER_EXAMPLE || THIRD_TIER_EXAMPLE
                        IBoidComponent boidNode = entities[index].node;

                        var position = boidNode.position;

                        var x = (realTarget.x - position.x);
                        var y = (realTarget.y - position.y);
                        var z = (realTarget.z - position.z);

                        var sqrdmagnitude = x * x + y * y + z * z;

                        boidNode.position.Set(x / sqrdmagnitude, y / sqrdmagnitude, z / sqrdmagnitude);
#elif FOURTH_TIER_EXAMPLE
                        var x = (realTarget.x - entities[index].position.x);
                        var y = (realTarget.y - entities[index].position.y);
                        var z = (realTarget.z - entities[index].position.z);

                        var sqrdmagnitude = x * x + y * y + z * z;
                        entities[index].position.x = x * sqrdmagnitude;
                        entities[index].position.y = y * sqrdmagnitude;
                        entities[index].position.z = z * sqrdmagnitude;
#endif
#if FIRST_TIER_EXAMPLE
                        var position = entities[index].node.position;

                        var direction = realTarget - position;
                        var sqrdmagnitude = direction.sqrMagnitude;

                        entities[index].node.position = direction / (sqrdmagnitude);
#endif
                        System.Threading.Interlocked.Increment(ref _totalCount);
                    }
                }

                return false;
            }

            public void Reset()
            { }
        }
#if TURBO_EXAMPLE
        SyncRunner _syncRunner;
#else
        BoidEnumerator _boidEnumerator;
#endif

#if FIRST_TIER_EXAMPLE || SECOND_TIER_EXAMPLE || THIRD_TIER_EXAMPLE
        DataStructures.FasterList<BoidNode> _nodes = new DataStructures.FasterList<BoidNode>();
#endif
#if FOURTH_TIER_EXAMPLE
        StructNodes<BoidNode> _structNodes;
#endif
        static int _totalCount;

#if TURBO_EXAMPLE
        MultiThreadedParallelTaskCollection _multiParallelTask;       
#endif
        TestEnumerator _testEnumerator;
        PrintTimeNode _printNode;
    }
}