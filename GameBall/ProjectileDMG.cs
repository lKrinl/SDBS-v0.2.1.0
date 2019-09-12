using System;
using UnityEngine;

namespace ThirdPersonController
{
    public delegate void HitDelegate(Vector3 point, Vector3 normal, Collider hitCollider);
    
    public interface IProjectile
    {
        /// Invoked when this projectile hits something.
        event HitDelegate hit;
        
        void OnSpawn(GameObject owner, float damage);
    }
}
