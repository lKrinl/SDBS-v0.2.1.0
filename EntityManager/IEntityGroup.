using System.Collections.Generic;

namespace GameFramework.Entity
{
    public interface IEntityGroup
    {
        string Name
        {
            get;
        }


        int EntityCount
        {
            get;
        }


        float InstanceAutoReleaseInterval
        {
            get;
            set;
        }


        int InstanceCapacity
        {
            get;
            set;
        }

        float InstanceExpireTime
        {
            get;
            set;
        }


        int InstancePriority
        {
            get;
            set;
        }


        IEntityGroupHelper Helper
        {
            get;
        }

        bool HasEntity(int entityId);

        bool HasEntity(string entityAssetName);

        IEntity GetEntity(int entityId);

        IEntity GetEntity(string entityAssetName);

        IEntity[] GetEntities(string entityAssetName);

        void GetEntities(string entityAssetName, List<IEntity> results);

        IEntity[] GetAllEntities();

        void GetAllEntities(List<IEntity> results);

        void SetEntityInstanceLocked(object entityInstance, bool locked);

        void SetEntityInstancePriority(object entityInstance, int priority);
    }
}
