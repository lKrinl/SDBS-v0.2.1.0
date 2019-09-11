using GameFramework.ObjectPool;
using GameFramework.Resource;
using System;
using System.Collections.Generic;

namespace GameFramework.Entity
{
    public interface IEntityManager
    {

        int EntityCount
        {
            get;
        }


        int EntityGroupCount
        {
            get;
        }

        event EventHandler<ShowEntitySuccessEventArgs> ShowEntitySuccess;

        event EventHandler<ShowEntityFailureEventArgs> ShowEntityFailure;

        event EventHandler<ShowEntityUpdateEventArgs> ShowEntityUpdate;

        event EventHandler<ShowEntityDependencyAssetEventArgs> ShowEntityDependencyAsset;

        event EventHandler<HideEntityCompleteEventArgs> HideEntityComplete;

        void SetObjectPoolManager(IObjectPoolManager objectPoolManager);

        void SetResourceManager(IResourceManager resourceManager);

        void SetEntityHelper(IEntityHelper entityHelper);

        bool HasEntityGroup(string entityGroupName);

        IEntityGroup GetEntityGroup(string entityGroupName);

        IEntityGroup[] GetAllEntityGroups();

        void GetAllEntityGroups(List<IEntityGroup> results);

        bool AddEntityGroup(string entityGroupName, float instanceAutoReleaseInterval, int instanceCapacity, float instanceExpireTime, int instancePriority, IEntityGroupHelper entityGroupHelper);

        bool HasEntity(int entityId);

        bool HasEntity(string entityAssetName);

        IEntity GetEntity(int entityId);

        IEntity GetEntity(string entityAssetName);

        IEntity[] GetEntities(string entityAssetName);

        void GetEntities(string entityAssetName, List<IEntity> results);

        IEntity[] GetAllLoadedEntities();

        void GetAllLoadedEntities(List<IEntity> results);

        int[] GetAllLoadingEntityIds();

        void GetAllLoadingEntityIds(List<int> results);

        bool IsLoadingEntity(int entityId);

        bool IsValidEntity(IEntity entity);

        void ShowEntity(int entityId, string entityAssetName, string entityGroupName);

        void ShowEntity(int entityId, string entityAssetName, string entityGroupName, int priority);

        void ShowEntity(int entityId, string entityAssetName, string entityGroupName, object userData);

        void ShowEntity(int entityId, string entityAssetName, string entityGroupName, int priority, object userData);

        void HideEntity(int entityId);

        void HideEntity(int entityId, object userData);

        void HideEntity(IEntity entity);

        void HideEntity(IEntity entity, object userData);

        void HideAllLoadedEntities();

        void HideAllLoadedEntities(object userData);

        void HideAllLoadingEntities();

        IEntity GetParentEntity(int childEntityId);

        IEntity GetParentEntity(IEntity childEntity);

        IEntity[] GetChildEntities(int parentEntityId);

        void GetChildEntities(int parentEntityId, List<IEntity> results);

        IEntity[] GetChildEntities(IEntity parentEntity);

        void GetChildEntities(IEntity parentEntity, List<IEntity> results);

        void AttachEntity(int childEntityId, int parentEntityId);

        void AttachEntity(int childEntityId, int parentEntityId, object userData);

        void AttachEntity(int childEntityId, IEntity parentEntity);

        void AttachEntity(int childEntityId, IEntity parentEntity, object userData);

        void AttachEntity(IEntity childEntity, int parentEntityId);

        void AttachEntity(IEntity childEntity, int parentEntityId, object userData);

        void AttachEntity(IEntity childEntity, IEntity parentEntity);

        void AttachEntity(IEntity childEntity, IEntity parentEntity, object userData);

        void DetachEntity(int childEntityId);

        void DetachEntity(int childEntityId, object userData);

        void DetachEntity(IEntity childEntity);

        void DetachEntity(IEntity childEntity, object userData);

        void DetachChildEntities(int parentEntityId);

        void DetachChildEntities(int parentEntityId, object userData);

        void DetachChildEntities(IEntity parentEntity);

        void DetachChildEntities(IEntity parentEntity, object userData);
    }
}
