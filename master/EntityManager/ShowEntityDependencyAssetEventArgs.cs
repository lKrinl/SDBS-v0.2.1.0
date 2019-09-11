namespace GameFramework.Entity
{
    public sealed class ShowEntityDependencyAssetEventArgs : GameFrameworkEventArgs
    {
        public ShowEntityDependencyAssetEventArgs()
        {
            EntityId = 0;
            EntityAssetName = null;
            EntityGroupName = null;
            DependencyAssetName = null;
            LoadedCount = 0;
            TotalCount = 0;
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

        public string DependencyAssetName
        {
            get;
            private set;
        }

        public int LoadedCount
        {
            get;
            private set;
        }

        public int TotalCount
        {
            get;
            private set;
        }

        public object UserData
        {
            get;
            private set;
        }

        public static ShowEntityDependencyAssetEventArgs Create(int entityId, string entityAssetName, string entityGroupName, string dependencyAssetName, int loadedCount, int totalCount, object userData)
        {
            ShowEntityDependencyAssetEventArgs showEntityDependencyAssetEventArgs = ReferencePool.Acquire<ShowEntityDependencyAssetEventArgs>();
            showEntityDependencyAssetEventArgs.EntityId = entityId;
            showEntityDependencyAssetEventArgs.EntityAssetName = entityAssetName;
            showEntityDependencyAssetEventArgs.EntityGroupName = entityGroupName;
            showEntityDependencyAssetEventArgs.DependencyAssetName = dependencyAssetName;
            showEntityDependencyAssetEventArgs.LoadedCount = loadedCount;
            showEntityDependencyAssetEventArgs.TotalCount = totalCount;
            showEntityDependencyAssetEventArgs.UserData = userData;
            return showEntityDependencyAssetEventArgs;
        }

        public override void Clear()
        {
            EntityId = 0;
            EntityAssetName = null;
            EntityGroupName = null;
            DependencyAssetName = null;
            LoadedCount = 0;
            TotalCount = 0;
            UserData = null;
        }
    }
}
