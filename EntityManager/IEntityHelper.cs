namespace GameFramework.Entity
{
    public interface IEntityHelper
    {
        object InstantiateEntity(object entityAsset);

        IEntity CreateEntity(object entityInstance, IEntityGroup entityGroup, object userData);

        void ReleaseEntity(object entityAsset, object entityInstance);
    }
}
