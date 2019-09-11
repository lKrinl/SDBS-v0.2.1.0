using GameFramework.ObjectPool;
using System.Collections.Generic;

namespace GameFramework.Entity
{
    internal sealed partial class EntityManager : GameFrameworkModule, IEntityManager
    {

        private sealed class EntityGroup : IEntityGroup
        {
            private readonly string m_Name;
            private readonly IEntityGroupHelper m_EntityGroupHelper;
            private readonly IObjectPool<EntityInstanceObject> m_InstancePool;
            private readonly GameFrameworkLinkedList<IEntity> m_Entities;
            private LinkedListNode<IEntity> m_CachedNode;


            public EntityGroup(string name, float instanceAutoReleaseInterval, int instanceCapacity, float instanceExpireTime, int instancePriority, IEntityGroupHelper entityGroupHelper, IObjectPoolManager objectPoolManager)
            {
                if (string.IsNullOrEmpty(name))
                {
                    throw new GameFrameworkException("Entity group name is invalid.");
                }

                if (entityGroupHelper == null)
                {
                    throw new GameFrameworkException("Entity group helper is invalid.");
                }

                m_Name = name;
                m_EntityGroupHelper = entityGroupHelper;
                m_InstancePool = objectPoolManager.CreateSingleSpawnObjectPool<EntityInstanceObject>(Utility.Text.Format("Entity Instance Pool ({0})", name), instanceCapacity, instanceExpireTime, instancePriority);
                m_InstancePool.AutoReleaseInterval = instanceAutoReleaseInterval;
                m_Entities = new GameFrameworkLinkedList<IEntity>();
                m_CachedNode = null;
            }


            public string Name
            {
                get
                {
                    return m_Name;
                }
            }


            public int EntityCount
            {
                get
                {
                    return m_Entities.Count;
                }
            }


            public float InstanceAutoReleaseInterval
            {
                get
                {
                    return m_InstancePool.AutoReleaseInterval;
                }
                set
                {
                    m_InstancePool.AutoReleaseInterval = value;
                }
            }


            public int InstanceCapacity
            {
                get
                {
                    return m_InstancePool.Capacity;
                }
                set
                {
                    m_InstancePool.Capacity = value;
                }
            }


            public float InstanceExpireTime
            {
                get
                {
                    return m_InstancePool.ExpireTime;
                }
                set
                {
                    m_InstancePool.ExpireTime = value;
                }
            }


            public int InstancePriority
            {
                get
                {
                    return m_InstancePool.Priority;
                }
                set
                {
                    m_InstancePool.Priority = value;
                }
            }


            public IEntityGroupHelper Helper
            {
                get
                {
                    return m_EntityGroupHelper;
                }
            }


            public void Update(float elapseSeconds, float realElapseSeconds)
            {
                LinkedListNode<IEntity> current = m_Entities.First;
                while (current != null)
                {
                    m_CachedNode = current.Next;
                    current.Value.OnUpdate(elapseSeconds, realElapseSeconds);
                    current = m_CachedNode;
                    m_CachedNode = null;
                }
            }


            public bool HasEntity(int entityId)
            {
                foreach (IEntity entity in m_Entities)
                {
                    if (entity.Id == entityId)
                    {
                        return true;
                    }
                }

                return false;
            }


            public bool HasEntity(string entityAssetName)
            {
                if (string.IsNullOrEmpty(entityAssetName))
                {
                    throw new GameFrameworkException("Entity asset name is invalid.");
                }

                foreach (IEntity entity in m_Entities)
                {
                    if (entity.EntityAssetName == entityAssetName)
                    {
                        return true;
                    }
                }

                return false;
            }


            public IEntity GetEntity(int entityId)
            {
                foreach (IEntity entity in m_Entities)
                {
                    if (entity.Id == entityId)
                    {
                        return entity;
                    }
                }

                return null;
            }


            public IEntity GetEntity(string entityAssetName)
            {
                if (string.IsNullOrEmpty(entityAssetName))
                {
                    throw new GameFrameworkException("Entity asset name is invalid.");
                }

                foreach (IEntity entity in m_Entities)
                {
                    if (entity.EntityAssetName == entityAssetName)
                    {
                        return entity;
                    }
                }

                return null;
            }


            public IEntity[] GetEntities(string entityAssetName)
            {
                if (string.IsNullOrEmpty(entityAssetName))
                {
                    throw new GameFrameworkException("Entity asset name is invalid.");
                }

                List<IEntity> results = new List<IEntity>();
                foreach (IEntity entity in m_Entities)
                {
                    if (entity.EntityAssetName == entityAssetName)
                    {
                        results.Add(entity);
                    }
                }

                return results.ToArray();
            }


            public void GetEntities(string entityAssetName, List<IEntity> results)
            {
                if (string.IsNullOrEmpty(entityAssetName))
                {
                    throw new GameFrameworkException("Entity asset name is invalid.");
                }

                if (results == null)
                {
                    throw new GameFrameworkException("Results is invalid.");
                }

                results.Clear();
                foreach (IEntity entity in m_Entities)
                {
                    if (entity.EntityAssetName == entityAssetName)
                    {
                        results.Add(entity);
                    }
                }
            }


            public IEntity[] GetAllEntities()
            {
                List<IEntity> results = new List<IEntity>();
                foreach (IEntity entity in m_Entities)
                {
                    results.Add(entity);
                }

                return results.ToArray();
            }


            public void GetAllEntities(List<IEntity> results)
            {
                if (results == null)
                {
                    throw new GameFrameworkException("Results is invalid.");
                }

                results.Clear();
                foreach (IEntity entity in m_Entities)
                {
                    results.Add(entity);
                }
            }

            /// <summary>
            /// 往实体组增加实体。
            /// </summary>
            /// <param name="entity">要增加的实体。</param>
            public void AddEntity(IEntity entity)
            {
                m_Entities.AddLast(entity);
            }

            /// <summary>
            /// 从实体组移除实体。
            /// </summary>
            /// <param name="entity">要移除的实体。</param>
            public void RemoveEntity(IEntity entity)
            {
                if (m_CachedNode != null && m_CachedNode.Value == entity)
                {
                    m_CachedNode = m_CachedNode.Next;
                }

                if (!m_Entities.Remove(entity))
                {
                    throw new GameFrameworkException(Utility.Text.Format("Entity group '{0}' not exists specified entity '[{1}]{2}'.", m_Name, entity.Id.ToString(), entity.EntityAssetName));
                }
            }

            public void RegisterEntityInstanceObject(EntityInstanceObject obj, bool spawned)
            {
                m_InstancePool.Register(obj, spawned);
            }

            public EntityInstanceObject SpawnEntityInstanceObject(string name)
            {
                return m_InstancePool.Spawn(name);
            }

            public void UnspawnEntity(IEntity entity)
            {
                m_InstancePool.Unspawn(entity.Handle);
            }

            public void SetEntityInstanceLocked(object entityInstance, bool locked)
            {
                if (entityInstance == null)
                {
                    throw new GameFrameworkException("Entity instance is invalid.");
                }

                m_InstancePool.SetLocked(entityInstance, locked);
            }

            public void SetEntityInstancePriority(object entityInstance, int priority)
            {
                if (entityInstance == null)
                {
                    throw new GameFrameworkException("Entity instance is invalid.");
                }

                m_InstancePool.SetPriority(entityInstance, priority);
            }
        }
    }
}
