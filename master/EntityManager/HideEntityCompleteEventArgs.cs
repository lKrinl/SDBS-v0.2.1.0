namespace GameFramework.Entity
{
    public sealed class HideEntityCompleteEventArgs : GameFrameworkEventArgs
    {
        public HideEntityCompleteEventArgs()
        {
            EntityId = 0;
            EntityAssetName = null;
            EntityGroup = null;
            UserData = null;
        }


        public int EntityId
        {
            get;
            private set;
        }


        public string EntityAssetName
        {
            get;
            private set;
        }


        public IEntityGroup EntityGroup
        {
            get;
            private set;
        }


        public object UserData
        {
            get;
            private set;
        }


        public static HideEntityCompleteEventArgs Create(int entityId, string entityAssetName, IEntityGroup entityGroup, object userData)
        {
            HideEntityCompleteEventArgs hideEntityCompleteEventArgs = ReferencePool.Acquire<HideEntityCompleteEventArgs>();
            hideEntityCompleteEventArgs.EntityId = entityId;
            hideEntityCompleteEventArgs.EntityAssetName = entityAssetName;
            hideEntityCompleteEventArgs.EntityGroup = entityGroup;
            hideEntityCompleteEventArgs.UserData = userData;
            return hideEntityCompleteEventArgs;
        }


        public override void Clear()
        {
            EntityId = 0;
            EntityAssetName = null;
            EntityGroup = null;
            UserData = null;
        }
    }
}
