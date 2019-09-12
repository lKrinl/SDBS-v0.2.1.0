namespace GameFramework.Entity
{
    public sealed class ShowEntityFailureEventArgs : GameFrameworkEventArgs
    {
        public ShowEntityFailureEventArgs()
        {
            EntityId = 0;
            EntityAssetName = null;
            EntityGroupName = null;
            ErrorMessage = null;
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

        public string ErrorMessage
        {
            get;
            private set;
        }

        public object UserData
        {
            get;
            private set;
        }

        public static ShowEntityFailureEventArgs Create(int entityId, string entityAssetName, string entityGroupName, string errorMessage, object userData)
        {
            ShowEntityFailureEventArgs showEntityFailureEventArgs = ReferencePool.Acquire<ShowEntityFailureEventArgs>();
            showEntityFailureEventArgs.EntityId = entityId;
            showEntityFailureEventArgs.EntityAssetName = entityAssetName;
            showEntityFailureEventArgs.EntityGroupName = entityGroupName;
            showEntityFailureEventArgs.ErrorMessage = errorMessage;
            showEntityFailureEventArgs.UserData = userData;
            return showEntityFailureEventArgs;
        }

        public override void Clear()
        {
            EntityId = 0;
            EntityAssetName = null;
            EntityGroupName = null;
            ErrorMessage = null;
            UserData = null;
        }
    }
}
