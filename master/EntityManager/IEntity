namespace GameFramework.Entity
{
    public interface IEntity
    {
        int Id
        {
            get;
        }


        string EntityAssetName
        {
            get;
        }


        object Handle
        {
            get;
        }


        IEntityGroup EntityGroup
        {
            get;
        }

        void OnInit(int entityId, string entityAssetName, IEntityGroup entityGroup, bool isNewInstance, object userData);

        void OnRecycle();

        void OnShow(object userData);

        void OnHide(bool isShutdown, object userData);

        void OnAttached(IEntity childEntity, object userData);

        void OnDetached(IEntity childEntity, object userData);

        void OnAttachTo(IEntity parentEntity, object userData);

        void OnDetachFrom(IEntity parentEntity, object userData);

        void OnUpdate(float elapseSeconds, float realElapseSeconds);
    }
}
