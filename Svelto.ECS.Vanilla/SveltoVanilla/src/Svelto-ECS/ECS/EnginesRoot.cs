using System;
using System.Collections;
using System.Collections.Generic;
using Svelto.DataStructures;
using Svelto.ECS.Internal;
using Svelto.ECS.EntityViewSchedulers;
using Svelto.Utilities;
#if EXPERIMENTAL
using Svelto.ECS.Experimental;
using Svelto.ECS.Experimental.Internal;
#endif

#if ENGINE_PROFILER_ENABLED && UNITY_EDITOR
using Svelto.ECS.Profiler;
#endif

namespace Svelto.ECS
{
    public sealed class EnginesRoot : IEntityFunctions, IEntityFactory
    {
        public EnginesRoot(EntityViewSubmissionScheduler entityViewScheduler)
        {
            _entityViewEngines = new Dictionary<Type, FasterList<IHandleEntityViewEngine>>();
#if EXPERIMENTAL            
            _activableViewEntitiesEngines = new Dictionary<Type, FasterList<IHandleActivableEntityEngine>>();
#endif    
            _otherEngines = new FasterList<IEngine>();

            _entityViewsDB = new Dictionary<Type, ITypeSafeList>();
            _metaEntityViewsDB = new Dictionary<Type, ITypeSafeList>();
            _groupEntityViewsDB = new Dictionary<int, Dictionary<Type, ITypeSafeList>>();
            _entityViewsDBdic = new Dictionary<Type, ITypeSafeDictionary>();
            
            _entityViewsToAdd = new DoubleBufferedEntityViews<Dictionary<Type, ITypeSafeList>>();           
            _metaEntityViewsToAdd = new DoubleBufferedEntityViews<Dictionary<Type, ITypeSafeList>>();
            _groupedEntityViewsToAdd = new DoubleBufferedEntityViews<Dictionary<int, Dictionary<Type, ITypeSafeList>>>();
#if ENGINE_PROFILER_ENABLED && UNITY_EDITOR
            _addEntityViewToEngine = AddEntityViewToEngine;
            _removeEntityViewFromEngine = _removeEntityViewFromEngine;
#endif                 
            _engineEntityViewDB = new EngineEntityViewDB(_entityViewsDB, _entityViewsDBdic, _metaEntityViewsDB, _groupEntityViewsDB);

            _scheduler = entityViewScheduler;
            _scheduler.Schedule(SubmitEntityViews);
#if EXPERIMENTAL            
            _sharedStructEntityViewLists = new SharedStructEntityViewLists();
            _sharedGroupedStructEntityViewLists = new SharedGroupedStructEntityViewsLists();

            _structEntityViewEngineType = typeof(IStructEntityViewEngine<>);
            _groupedStructEntityViewsEngineType = typeof(IGroupedStructEntityViewsEngine<>);
            
            _implementedInterfaceTypes = new Dictionary<Type, Type[]>();
#endif
#if UNITY_EDITOR
            UnityEngine.GameObject debugEngineObject = new UnityEngine.GameObject("Engine Debugger");
            debugEngineObject.gameObject.AddComponent<EngineProfilerBehaviour>();
#endif
        }
        
        public IEntityFactory GenerateEntityFactory()
        {
            return new GenericEntityFactory(new DataStructures.WeakReference<EnginesRoot>(this));
        }

        public IEntityFunctions GenerateEntityFunctions()
        {
            return new GenericEntityFunctions(new DataStructures.WeakReference<EnginesRoot>(this));
        }
        
        public void BuildEntity<T>(int entityID, object[] implementors) where T:IEntityDescriptor, new()
        {
            EntityFactory.BuildEntityViews(entityID, _entityViewsToAdd.current, EntityDescriptor<T>.EntityViewsToBuild, implementors);
        }

        /// <summary>
        /// A meta entity is a way to manage a set of entitites that are not easily 
        /// queriable otherwise. For example you may want to group existing entities
        /// by size and type and then use the meta entity entityView to manage the data 
        /// shared among the single entities of the same type and size. This will 
        /// prevent the scenario where the coder is forced to parse all the entities to 
        /// find the ones of the same size and type. 
        /// Since the entities are managed through the shared entityView, the same
        /// shared entityView must be found on the single entities of the same type and size.
        /// The shared entityView of the meta entity is then used by engines that are meant 
        /// to manage a group of entities through a single entityView. 
        /// The same engine can manage several meta entities entityViews too.
        /// The Engine manages the logic of the Meta EntityView data and other engines
        /// can read back this data through the normal entity as the shared entityView
        /// will be present in their descriptor too.
        /// It's a way to control a group of Entities through a entityView only.
        /// This set of entities can share exactly the same entityView reference if 
        /// built through this function. In this way, if you need to set a variable
        /// on a group of entities, instead to inject N entityViews and iterate over
        /// them to set the same value, you can inject just one entityView, set the value
        /// and be sure that the value is shared between entities.
        /// </summary>
        /// <param name="metaEntityID"></param>
        /// <param name="ed"></param>
        /// <param name="implementors"></param>
        public void BuildMetaEntity<T>(int metaEntityID, object[] implementors) where T:IEntityDescriptor, new()
        {
            EntityFactory.BuildEntityViews(metaEntityID, _entityViewsToAdd.current, 
                                           EntityDescriptor<T>.EntityViewsToBuild, implementors);
        }

        /// <summary>
        /// Using this function is like building a normal entity, but the entityViews
        /// are grouped by groupID to be better processed inside engines and
        /// improve cache locality. Only IGroupStructEntityViewWithID entityViews are grouped
        /// other entityViews are managed as usual.
        /// </summary>
        /// <param name="entityID"></param>
        /// <param name="groupID"></param>
        /// <param name="ed"></param>
        /// <param name="implementors"></param>
        public void BuildEntityInGroup<T>(int entityID, int groupID, object[] implementors) where T:IEntityDescriptor, new()
        {
            EntityFactory.BuildGroupedEntityViews(entityID, groupID, 
                                                  _groupedEntityViewsToAdd.current,
                                                  EntityDescriptor<T>.EntityViewsToBuild,
                                                  implementors);
        }
        
        public void RemoveEntity<T>(int entityID) where T : IEntityDescriptor, new()
        {
            InternalRemove(EntityDescriptor<T>.EntityViewsToBuild, entityID, _entityViewsDB);
        }

        public void RemoveMetaEntity<T>(int metaEntityID) where T : IEntityDescriptor, new()
        {
            InternalRemove(EntityDescriptor<T>.EntityViewsToBuild, metaEntityID, _metaEntityViewsDB);
        }

        public void RemoveEntityFromGroup<T>(int entityID, int groupID) where T : IEntityDescriptor, new()
        {
            InternalRemove(EntityDescriptor<T>.EntityViewsToBuild, entityID, _groupEntityViewsDB[groupID]);
        }
#if EXPERIMENTAL
        public void SetEntityActiveState<T>(int entityID, bool state) where T : IEntityDescriptor, new()
        {
            InternalSetActiveState(EntityDescriptor<T>.EntityViewsToBuild, entityID, state, _entityViewsDB);
        }

        public void SetMetaEntityActiveState<T>(int metaEntityID, bool state) where T : IEntityDescriptor, new()
        {
            InternalSetActiveState(EntityDescriptor<T>.EntityViewsToBuild, metaEntityID, state, _metaEntityViewsDB);
        }

        public void SetEntityInGroupActiveState<T>(int entityID, int groupID, bool state) where T : IEntityDescriptor, new()
        {
            InternalSetActiveState(EntityDescriptor<T>.EntityViewsToBuild, entityID, state, _groupEntityViewsDB[groupID]);
        }
#endif        
        public void AddEngine(IEngine engine)
        {
#if UNITY_EDITOR
            EngineProfiler.AddEngine(engine);
#endif
            var engineType = engine.GetType();
#if EXPERIMENTAL
            bool engineAdded;
    
            var implementedInterfaces = engineType.GetInterfaces();
            
            CollectImplementedInterfaces(implementedInterfaces);
    
            engineAdded = CheckSpecialEngine(engine);
#endif
            var viewEngine = engine as IHandleEntityViewEngine;
            
            if (viewEngine != null)
                CheckEntityViewsEngine(viewEngine, engineType);
            else            
                _otherEngines.Add(engine);
            
            var queryableEntityViewEngine = engine as IQueryingEntityViewEngine;
            if (queryableEntityViewEngine != null)
            {
                queryableEntityViewEngine.entityViewsDB = _engineEntityViewDB;
                queryableEntityViewEngine.Ready();
            }
        }
       
#if EXPERIMENTAL
         void CollectImplementedInterfaces(Type[] implementedInterfaces)
        {
            _implementedInterfaceTypes.Clear();

            var type = typeof(IHandleEntityViewEngine);

            for (int index = 0; index < implementedInterfaces.Length; index++)
            {
                var interfaceType = implementedInterfaces[index];

                if (type.IsAssignableFrom(interfaceType) == false)
                    continue;

                if (false == interfaceType.IsGenericTypeEx())
                {
                    continue;
                }

                var genericTypeDefinition = interfaceType.GetGenericTypeDefinition();

                _implementedInterfaceTypes.Add(genericTypeDefinition, interfaceType.GetGenericArguments());
            }
        }
    
        bool CheckSpecialEngine(IEngine engine)
        {
            if (_implementedInterfaceTypes.Count == 0) return false;

            bool engineAdded = false;

            if (_implementedInterfaceTypes.ContainsKey(_structEntityViewEngineType))
            {
                ((IStructEntityViewEngine)engine).CreateStructEntityViews
                    (_sharedStructEntityViewLists);
            }

            if (_implementedInterfaceTypes.ContainsKey(_groupedStructEntityViewsEngineType))
            {
                ((IGroupedStructEntityViewsEngine)engine).CreateStructEntityViews
                    (_sharedGroupedStructEntityViewLists);
            }

            return engineAdded;
        }
#endif
        void CheckEntityViewsEngine(IEngine engine, Type engineType)
        {
            var baseType = engineType.GetBaseType();

            if (baseType.IsGenericTypeEx())
            {
                var genericArguments = baseType.GetGenericArguments();
                AddEngine(engine as IHandleEntityViewEngine, genericArguments, _entityViewEngines);
#if EXPERIMENTAL
                var activableEngine = engine as IHandleActivableEntityEngine;
                if (activableEngine != null)
                    AddEngine(activableEngine, genericArguments, _activableViewEntitiesEngines);
#endif    

                return;
            }

            throw new Exception("Not Supported Engine");
        }

        static void AddEngine<T>(T engine, Type[] types,
                              Dictionary<Type, FasterList<T>> engines)
        {
            for (int i = 0; i < types.Length; i++)
            {
                FasterList<T> list;

                var type = types[i];

                if (engines.TryGetValue(type, out list) == false)
                {
                    list = new FasterList<T>();

                    engines.Add(type, list);
                }

                list.Add(engine);
            }
        }

        void AddEntityViewsToTheDBAndSuitableEngines(Dictionary<Type, ITypeSafeList> entityViewsToAdd,
            Dictionary<Type, ITypeSafeList> entityViewsDB)
        {
            foreach (var entityViewList in entityViewsToAdd)
            {
                AddEntityViewToDB(entityViewsDB, entityViewList);

                if (entityViewList.Value.isQueryiableEntityView)
                {
                    AddEntityViewToEntityViewsDictionary(_entityViewsDBdic, entityViewList.Value, entityViewList.Key);
                }
            }

            foreach (var entityViewList in entityViewsToAdd)
            {
                if (entityViewList.Value.isQueryiableEntityView)
                {
                    int count;
                    
                    var entityViews = entityViewList.Value.ToArrayFast(out count);
                    
                    for (int i = 0; i < count; i++)
                        AddEntityViewToTheSuitableEngines(_entityViewEngines, entityViews[i],
                                                          entityViewList.Key);
                }
            }
        }

        void AddGroupEntityViewsToTheDBAndSuitableEngines(Dictionary<int, Dictionary<Type, ITypeSafeList>> groupedEntityViewsToAdd,
                                                      Dictionary<int, Dictionary<Type, ITypeSafeList>> groupEntityViewsDB,
                                                      Dictionary<Type, ITypeSafeList> entityViewsDB)
        {
            foreach (var group in groupedEntityViewsToAdd)
            {
                AddEntityViewsToTheDBAndSuitableEngines(group.Value, entityViewsDB);

                AddEntityViewsToGroupDB(groupEntityViewsDB, @group);
            }
        }

        static void AddEntityViewsToGroupDB(Dictionary<int, Dictionary<Type, ITypeSafeList>> groupEntityViewsDB, 
                                      KeyValuePair<int, Dictionary<Type, ITypeSafeList>> @group)
        {
            Dictionary<Type, ITypeSafeList> groupedEntityViewsByType;

            if (groupEntityViewsDB.TryGetValue(@group.Key, out groupedEntityViewsByType) == false)
                groupedEntityViewsByType = groupEntityViewsDB[@group.Key] = new Dictionary<Type, ITypeSafeList>();

            foreach (var entityView in @group.Value)
            {
                groupedEntityViewsByType.Add(entityView.Key, entityView.Value);
            }
        }

        static void AddEntityViewToDB(Dictionary<Type, ITypeSafeList> entityViewsDB, KeyValuePair<Type, ITypeSafeList> entityViewList)
        {
            ITypeSafeList dbList;

            if (entityViewsDB.TryGetValue(entityViewList.Key, out dbList) == false)
                dbList = entityViewsDB[entityViewList.Key] = entityViewList.Value.Create();

            dbList.AddRange(entityViewList.Value);
        }
        
        static void AddEntityViewToEntityViewsDictionary(Dictionary<Type, ITypeSafeDictionary> entityViewsDBdic, 
                                             ITypeSafeList entityViews, Type entityViewType) 
        {
            ITypeSafeDictionary entityViewsDic;
            
            if (entityViewsDBdic.TryGetValue(entityViewType, out entityViewsDic) == false)
                entityViewsDic = entityViewsDBdic[entityViewType] = entityViews.CreateIndexedDictionary();

            entityViewsDic.FillWithIndexedEntityViews(entityViews);
        }

        static void AddEntityViewToTheSuitableEngines(Dictionary<Type, FasterList<IHandleEntityViewEngine>> entityViewEngines, EntityView entityView, Type entityViewType)
        {
            FasterList<IHandleEntityViewEngine> enginesForEntityView;

            if (entityViewEngines.TryGetValue(entityViewType, out enginesForEntityView))
            {
                int count;
                var fastList = FasterList<IHandleEntityViewEngine>.NoVirt.ToArrayFast(enginesForEntityView, out count);
                for (int j = 0; j < count; j++)
                {
#if ENGINE_PROFILER_ENABLED && UNITY_EDITOR
                    
                    EngineProfiler.MonitorAddDuration(_addEntityViewToEngine, (enginesForEntityView[j] as IEntityViewEngine), entityView);
#else
                    fastList[j].Add(entityView);
#endif
                }
            }
        }        
        
#if ENGINE_PROFILER_ENABLED && UNITY_EDITOR
        static void AddEntityViewToEngine(IEntityViewEngine engine, EntityViewWithID entityView)
        {
            engine.Add(entityView);
        }

        static void RemoveEntityViewFromEngine(IEntityViewEngine engine, EntityViewWithID entityView)
        {
            engine.Remove(entityView);
        }
#endif
        
#if EXPERIMENTAL        
        void InternalSetActiveState(IEntityViewBuilder[] entityViewBuilders, int entityID, bool state, 
                                    Dictionary<Type, ITypeSafeList> entityViewsDB)
        {
            int entityViewBuildersCount = entityViewBuilders.Length;
            
            for (int i = 0; i < entityViewBuildersCount; i++)
            {
                Type entityViewType = entityViewBuilders[i].GetType();

                ITypeSafeList entityViews = entityViewsDB[entityViewType];

                if (entityViews.isQueryiableEntityView)
                {
                    var typeSafeDictionary = _entityViewsDBdic[entityViewType];
                    var entityView = typeSafeDictionary.GetIndexedEntityView(entityID);
                    
                    typeSafeDictionary.Remove(entityID);

                    if (state == false)
                        DisableEntityViewFromEngines(_activableViewEntitiesEngines, entityView, entityViewType);
                    else
                        EnableEntityViewFromEngines(_activableViewEntitiesEngines, entityView, entityViewType);
                }
            }
        }
#endif
        void InternalRemove(IEntityViewBuilder[] entityViewBuilders, int entityID, 
                            Dictionary<Type, ITypeSafeList> entityViewsDB)
        {
            int entityViewBuildersCount = entityViewBuilders.Length;
            
            for (int i = 0; i < entityViewBuildersCount; i++)
            {
                Type entityViewType = entityViewBuilders[i].GetEntityViewType();

                ITypeSafeList entityViews = entityViewsDB[entityViewType];
                entityViews.UnorderedRemove(entityID);

                if (entityViews.isQueryiableEntityView)
                {
                    var typeSafeDictionary = _entityViewsDBdic[entityViewType];
                    var entityView = typeSafeDictionary.GetIndexedEntityView(entityID);
                    
                    typeSafeDictionary.Remove(entityID);

                    RemoveEntityViewFromEngines(_entityViewEngines, entityView, entityViewType);
                }
            }
        }
                
        static void RemoveEntityViewFromEngines(Dictionary<Type, FasterList<IHandleEntityViewEngine>> entityViewEngines, 
                                                EntityView entityView, Type entityViewType)
        {
            FasterList<IHandleEntityViewEngine> enginesForEntityView;

            if (entityViewEngines.TryGetValue(entityViewType, out enginesForEntityView))
            {
                int count;
                var fastList = FasterList<IHandleEntityViewEngine>.NoVirt.ToArrayFast(enginesForEntityView, out count);
                
                for (int j = 0; j < count; j++)
                {
#if ENGINE_PROFILER_ENABLED && UNITY_EDITOR
                    EngineProfiler.MonitorRemoveDuration(_removeEntityViewFromEngine, (enginesForEntityView[j] as IEntityViewEngine), entityView);
#else
                    fastList[j].Remove(entityView);
#endif
                }
            }
        }
#if EXPERIMENTAL        
        static void DisableEntityViewFromEngines(Dictionary<Type, FasterList<IHandleActivableEntityEngine>> entityViewEngines, 
                                                 EntityView entityView, Type entityViewType)
        {
            FasterList<IHandleActivableEntityEngine> enginesForEntityView;

            if (entityViewEngines.TryGetValue(entityViewType, out enginesForEntityView))
            {
                int count;
                var fastList = FasterList<IHandleActivableEntityEngine>.NoVirt.ToArrayFast(enginesForEntityView, out count);
                
                for (int j = 0; j < count; j++)
                {
                    fastList[j].Disable(entityView);
                }
            }
        }

        static void EnableEntityViewFromEngines(Dictionary<Type, FasterList<IHandleActivableEntityEngine>> entityViewEngines, 
                                                EntityView entityView, Type entityViewType)
        {
            FasterList<IHandleActivableEntityEngine> enginesForEntityView;

            if (entityViewEngines.TryGetValue(entityViewType, out enginesForEntityView))
            {
                int count;
                var fastList = FasterList<IHandleActivableEntityEngine>.NoVirt.ToArrayFast(enginesForEntityView, out count);
                
                for (int j = 0; j < count; j++)
                {
                    fastList[j].Enable(entityView);
                }
            }
        }
#endif
        void SubmitEntityViews()
        {
            bool newEntityViewsHaveBeenAddedWhileIterating =
                _metaEntityViewsToAdd.current.Count > 0 
                || _entityViewsToAdd.current.Count > 0
                || _groupedEntityViewsToAdd.current.Count > 0;

            int numberOfReenteringLoops = 0;

            while (newEntityViewsHaveBeenAddedWhileIterating)
            {
                //use other as source from now on
                //current will be use to write new entityViews
                _entityViewsToAdd.Swap();
                _metaEntityViewsToAdd.Swap();
                _groupedEntityViewsToAdd.Swap();

                if ( _entityViewsToAdd.other.Count > 0)
                    AddEntityViewsToTheDBAndSuitableEngines(_entityViewsToAdd.other, _entityViewsDB);
                
                if ( _metaEntityViewsToAdd.other.Count > 0)
                    AddEntityViewsToTheDBAndSuitableEngines(_metaEntityViewsToAdd.other, _metaEntityViewsDB);
                
                if (_groupedEntityViewsToAdd.other.Count > 0)
                    AddGroupEntityViewsToTheDBAndSuitableEngines(_groupedEntityViewsToAdd.other, _groupEntityViewsDB, _entityViewsDB);
                
                //other can be cleared now
                _entityViewsToAdd.other.Clear();
                _metaEntityViewsToAdd.other.Clear();
                _groupedEntityViewsToAdd.other.Clear();

                //has current new entityViews?
                newEntityViewsHaveBeenAddedWhileIterating =
                    _metaEntityViewsToAdd.current.Count > 0 
                    || _entityViewsToAdd.current.Count > 0
                    || _groupedEntityViewsToAdd.current.Count > 0;

                if (numberOfReenteringLoops > 5)
                    throw new Exception("possible infinite loop found creating Entities inside IEntityViewsEngine Add method, please consider building entities outside IEntityViewsEngine Add method");

                numberOfReenteringLoops++;
            } 
        }

        readonly Dictionary<Type, FasterList<IHandleEntityViewEngine>> _entityViewEngines;
#if EXPERIMENTAL        
        readonly Dictionary<Type, FasterList<IHandleActivableEntityEngine>> _activableViewEntitiesEngines;
#endif    

        readonly FasterList<IEngine> _otherEngines;

        readonly Dictionary<Type, ITypeSafeList> _entityViewsDB;
        readonly Dictionary<Type, ITypeSafeList> _metaEntityViewsDB;
        readonly Dictionary<int, Dictionary<Type, ITypeSafeList>> _groupEntityViewsDB;
        
        readonly Dictionary<Type, ITypeSafeDictionary> _entityViewsDBdic;

        readonly DoubleBufferedEntityViews<Dictionary<Type, ITypeSafeList>> _entityViewsToAdd;
        readonly DoubleBufferedEntityViews<Dictionary<Type, ITypeSafeList>> _metaEntityViewsToAdd;
        readonly DoubleBufferedEntityViews<Dictionary<int, Dictionary<Type, ITypeSafeList>>> _groupedEntityViewsToAdd;
      
        readonly EntityViewSubmissionScheduler _scheduler;
#if EXPERIMENTAL
        readonly Type _structEntityViewEngineType;
        readonly Type _groupedStructEntityViewsEngineType;
        
        readonly SharedStructEntityViewLists _sharedStructEntityViewLists;
        readonly SharedGroupedStructEntityViewsLists _sharedGroupedStructEntityViewLists;

        readonly Dictionary<Type, Type[]>  _implementedInterfaceTypes;
#endif    
 
#if ENGINE_PROFILER_ENABLED && UNITY_EDITOR       
        static Action<IHandleEntityViewEngine, EntityViewWithID> _addEntityViewToEngine;
        static Action<IHandleEntityViewEngine, EntityViewWithID> _removeEntityViewFromEngine;
#endif
        readonly EngineEntityViewDB _engineEntityViewDB;

        class DoubleBufferedEntityViews<T> where T : class, IDictionary, new()
        {
            readonly T _entityViewsToAddBufferA = new T();
            readonly T _entityViewsToAddBufferB = new T();

            public DoubleBufferedEntityViews()
            {
                this.other = _entityViewsToAddBufferA;
                this.current = _entityViewsToAddBufferB;
            }

            public T other  { get; private set; }
            public T current { get; private set; }
           
            public void Swap()
            {
                var toSwap = other;
                other = current;
                current = toSwap;
            }
        }
        
        class GenericEntityFactory : IEntityFactory
        {
            DataStructures.WeakReference<EnginesRoot> _weakEngine;

            public GenericEntityFactory(DataStructures.WeakReference<EnginesRoot> weakReference)
            {
                _weakEngine = weakReference;
            }

            public void BuildEntity<T>(int entityID, object[] implementors = null) where T : IEntityDescriptor, new()
            {
                _weakEngine.Target.BuildEntity<T>(entityID, implementors);
            }

            public void BuildMetaEntity<T>(int metaEntityID, object[] implementors = null) where T : IEntityDescriptor, new()
            {
                _weakEngine.Target.BuildMetaEntity<T>(metaEntityID, implementors);
            }

            public void BuildEntityInGroup<T>(int entityID, int groupID, object[] implementors = null) where T : IEntityDescriptor, new()
            {
                _weakEngine.Target.BuildEntityInGroup<T>(entityID, groupID, implementors);
            }
        }
        
        class GenericEntityFunctions : IEntityFunctions
        {
            public GenericEntityFunctions(DataStructures.WeakReference<EnginesRoot> weakReference)
            {
                _weakReference = weakReference;
            }

            public void RemoveEntity<T>(int entityID) where T : IEntityDescriptor, new()
            {
                _weakReference.Target.RemoveEntity<T>(entityID);
            }

            public void RemoveMetaEntity<T>(int metaEntityID) where T : IEntityDescriptor, new()
            {
                _weakReference.Target.RemoveEntity<T>(metaEntityID);
            }

            public void RemoveEntityFromGroup<T>(int entityID, int groupID) where T : IEntityDescriptor, new()
            {
                _weakReference.Target.RemoveEntity<T>(entityID);
            }

            public void SetEntityActiveState<T>(int entityID, bool state) where T : IEntityDescriptor, new()
            {
                _weakReference.Target.RemoveEntity<T>(entityID);
            }

            public void SetMetaEntityActiveState<T>(int metaEntityID, bool state) where T : IEntityDescriptor, new()
            {
                _weakReference.Target.RemoveEntity<T>(metaEntityID);
            }

            public void SetEntityInGroupActiveState<T>(int entityID, int @group, bool state) where T : IEntityDescriptor, new()
            {
                _weakReference.Target.RemoveEntity<T>(entityID);
            }
            
            readonly DataStructures.WeakReference<EnginesRoot> _weakReference;
        }
    }
}