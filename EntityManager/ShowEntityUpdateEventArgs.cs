namespace GameFramework.Entity
{
    public sealed class ShowEntityUpdateEventArgs : GameFrameworkEventArgs
    {
        public ShowEntityUpdateEventArgs()
        {
            EntityId = 0;
            EntityAssetName = null;
            EntityGroupName = null;
            Progress = 0f;
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

        public string EntityGroupName
        {
            get;
            private set;
        }

        public float Progress
        {
            get;
            private set;
        }

        public object UserData
        {
            get;
            private set;
        }

        public static ShowEntityUpdateEventArgs Create(int entityId, string entityAssetName, string entityGroupName, float progress, object userData)
        {
            ShowEntityUpdateEventArgs showEntityUpdateEventArgs = ReferencePool.Acquire<ShowEntityUpdateEventArgs>();
            showEntityUpdateEventArgs.EntityId = entityId;
            showEntityUpdateEventArgs.EntityAssetName = entityAssetName;
            showEntityUpdateEventArgs.EntityGroupName = entityGroupName;
            showEntityUpdateEventArgs.Progress = progress;
            showEntityUpdateEventArgs.UserData = userData;
            return showEntityUpdateEventArgs;
        }

        public override void Clear()
        {
            EntityId = 0;
            EntityAssetName = null;
            EntityGroupName = null;
            Progress = 0f;
            UserData = null;
        }
    }
}
