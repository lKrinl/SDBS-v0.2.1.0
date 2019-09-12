namespace Peril.Physics
{
    public interface ICollisionBody
    {

        int RefId { get; set; }

        //Skip this body when testing for collisions//
        bool Sleeping { get; }

        //The body's collision shape//
        ICollisionShape CollisionShape { get; }

        //Called each frame of collision//
        void OnCollision(CollisionResult result, ICollisionBody other);

    }
}

