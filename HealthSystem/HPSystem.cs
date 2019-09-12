using UnityEngine;
using UnityEngine.Events;

namespace UniCraft.HealthMechanism
{
    /// <summary>
    /// Module to replicate the health behaviour
    /// </summary>
    [AddComponentMenu("UniCraft/HealthSystem")]
    [DisallowMultipleComponent]
    public class HealthSystem : MonoBehaviour
    {
        ////////// Attribute //////////
        
        //Default Setting//
        public const int DeathHealth = 0;
        public const int MaxHealthLimit = 999;
        
        //Information//
        [SerializeField] protected int MaxHealth = 100;
        [SerializeField] protected int Health = 80;
        
        //Unity Event//
        [SerializeField] protected UnityEvent OnTakeDamageEvents = null;
        [SerializeField] protected UnityEvent OnRecoverHealthEvents = null;
        
        [SerializeField] protected UnityEvent OnDeathEvents = null;
        [SerializeField] protected UnityEvent OnResurrectionEvents = null;

        //Property//
        public int GetHealth => Health;
        public int GetMaxHealth => MaxHealth;
        public bool IsDead => (Health == DeathHealth);
        
        ////////// Method //////////

        ///API///

        //Damage//
        
        public virtual void TakeDamage(int damage)
        {
            Health = Mathf.Clamp(Health - damage, DeathHealth, MaxHealth);
            if ( Health == DeathHealth )
            {
                Die();
            }
            else
            {
                OnTakeDamageEvents.Invoke();
            }
        }

        public virtual void RecoverHealth(int heal)
        {
            Health = Mathf.Clamp(Health + heal, DeathHealth, MaxHealth);
            OnRecoverHealthEvents.Invoke();
        }
        
        //Life//

        public virtual void Die()
        {
            OnDeathEvents.Invoke();
        }

        public virtual void Resuscitate(int heal)
        {
            Health = Mathf.Clamp(Health + heal, DeathHealth, MaxHealth);
            OnResurrectionEvents.Invoke();
        }
    }
}
