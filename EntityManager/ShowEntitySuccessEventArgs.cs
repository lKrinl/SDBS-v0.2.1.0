namespace GameFramework.Entity
{
    public sealed class ShowEntitySuccessEventArgs : GameFrameworkEventArgs
    {
        public ShowEntitySuccessEventArgs()
        {
            Entity = null;
            Duration = 0f;
            UserData = null;
        }

        public IEntity Entity
        {
            get;
            private set;
        }

        public float Duration
        {
            get;
            private set;
        }

        public object UserData
        {
            get;
            private set;
        }

        public static ShowEntitySuccessEventArgs Create(IEntity entity, float duration, object userData)
        {
            ShowEntitySuccessEventArgs showEntitySuccessEventArgs = ReferencePool.Acquire<ShowEntitySuccessEventArgs>();
            showEntitySuccessEventArgs.Entity = entity;
            showEntitySuccessEventArgs.Duration = duration;
            showEntitySuccessEventArgs.UserData = userData;
            return showEntitySuccessEventArgs;
        }

        public override void Clear()
        {
            Entity = null;
            Duration = 0f;
            UserData = null;
        }
    }
}
