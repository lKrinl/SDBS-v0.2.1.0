using UnityEngine;
using UnityEngine.Events;

namespace UniCraft.HealthMechanism
{
    /// <summary>
    /// Module to replicate the health behaviour
    /// </summary>
    [AddComponentMenu("UniCraft/HealthSystem")]
    [DisallowMultipleComponent]
    public class HPSystem : MonoBehaviour
    {
        ////////// Attribute //////////
        
        //Default Setting//
        public const int DeathHP = 0;
        public const int MaxHPLimit = 999;
        
        //Information//
        [SerializeField] protected int MaxHP = 100;
        [SerializeField] protected int HP = 80;
        
        //Unity Event//
        [SerializeField] protected UnityEvent OnTakeDamageEvents = null;
        [SerializeField] protected UnityEvent OnRecoverHPEvents = null;
        
        [SerializeField] protected UnityEvent OnDeathEvents = null;
        [SerializeField] protected UnityEvent OnResurrectionEvents = null;

        //Property//
        public int GetHP => HP;
        public int GetMaxHP => MaxHP;
        public bool IsDead => (HP == DeathHP);
        
        ////////// Method //////////

        ///API///

        //Damage//
        
        public virtual void TakeDamage(int damage)
        {
            HP = Mathf.Clamp(HP - damage, DeathHP, MaxHP);
            if ( HP == DeathHP )
            {
                Die();
            }
            else
            {
                OnTakeDamageEvents.Invoke();
            }
        }

        public virtual void RecoverHP(int heal)
        {
            HP = Mathf.Clamp(HP + heal, DeathHP, MaxHP);
            OnRecoverHPEvents.Invoke();
        }
        
        //Life//

        public virtual void Die()
        {
            OnDeathEvents.Invoke();
        }

        public virtual void Resuscitate(int heal)
        {
            HP = Mathf.Clamp(HP + heal, DeathHP, MaxHP);
            OnResurrectionEvents.Invoke();
        }
    }
}
