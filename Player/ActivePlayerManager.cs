using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivePlayer : MonoBehaviour {

    protected bool canFlinch = true;

    public Animator baseAnim;
    public Rigidbody body;

    public float speed = 2;
    protected Vector3 frontVector;

    public bool isGrounded;
    public bool isAlive = true;

    public float maxHP = 100.0f;
    public float currentHP = 100.0f;

    //holds attack info. class declaration at bottom
    public AttackData normalAttack;

    //variables for knockdown
    protected Coroutine knockdownRoutine;
    public bool isKnockedOut;

    //variables for health bar
    public LifeBar lifeBar;
    public Sprite playerThumbnail;

    public GameObject hitValuePrefab;

    public AudioClip deathClip;
    public AudioClip hitClip;

    public AudioSource audioSource;

    protected ActivePlayerCollider activeplayerCollider;

    protected virtual void Start()
    {
        currentHP = maxHP;
        isAlive = true;
        baseAnim.SetBool("IsAlive", isAlive);

        activeplayerCollider = GetComponent<ActivePlayerCollider>();
        activeplayerCollider.SetColliderStance(true);
    }

    public virtual void Update()
    {
        //keeps shadow grounded
        Vector3 shadowPosition = shadow.transform.position;
        shadowPosition.y = 0;
        shadow.transform.position = shadowPosition;
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.name == "Court")
        {
            isGrounded = true;
            baseAnim.SetBool("IsGrounded", isGrounded);
            DidLand();
        }
    }

    protected virtual void OnCollisionExit(Collision collision)
    {
        if (collision.collider.name == "Court")
        {
            isGrounded = false;
            baseAnim.SetBool("IsGrounded", isGrounded);
        }
    }

    protected virtual void DidLand()
    {

    }

    public virtual void Attack()
    {
        ///To do: (if(ballEquipped)) & Input Key for attack///
        baseAnim.SetTrigger("Attack");
    }

    public virtual void DidHitObject(Collider collider, Vector3 hitPoint, Vector3 hitVector)
    {
        EnemyPlayer enemyplayer = collider.GetComponent<Player>();
        if (isAlive && !isKnockedOut && enemyplayer != null && enemyplayer.CanBeHit() && collider.tag != gameObject.tag)
        {
            if (collider.attachedRigidbody != null)
                HitEnemyPlayer(enemyplayer, hitPoint, hitVector);
        }
    }

    protected virtual void HitEnemyPlayer(EnemyPlayer enemyplayer, Vector3 hitPoint, Vector3 hitVector)
    {
        //Debug.Log(gameObject.name + " HIT " + actor.gameObject.name);
        enemyplayer.EvaluateAttackData(normalAttack, hitVector, hitPoint);
        PlaySFX(hitClip);
    }

    public virtual void TakeDamage(float value, Vector3 hitVector, bool knockdown = false)
    {
        if (isAlive && currentLife <= 0)
            Die();
        else if (knockdown)
        {
            if (knockdownRoutine == null)
            {
                Vector3 pushbackVector = (hitVector + Vector3.up * 0.75f).normalized;
                body.AddForce(pushbackVector * 250);
                knockdownRoutine = StartCoroutine(KnockdownRoutine());
            }
        }
        else if (canFlinch)
            baseAnim.SetTrigger("IsStunned");

        //update lifebar
        lifeBar.EnableLifeBar(true);
        lifeBar.SetProgress(currentHP / maxHP);
        Color color = baseSprite.color;
        if (currentHP < 0)
            color.a = 0.75f;
        lifeBar.SetThumbnail(activeplayerThumbnail, color);
    }

    protected virtual void Die()
    {
        //stop any running knockdown routines if actor is dead
        if (knockdownRoutine != null)
            StopCoroutine(knockdownRoutine);

        isAlive = false;
        baseAnim.SetBool("IsAlive", isAlive);
        StartCoroutine(DeathFlicker());
        PlaySFX(deathClip);
        activeplayerCollider.SetColliderStance(false);
    }

    private IEnumerator DeathFlicker()
    {
        int i = 5;
        while (i > 0)
        if (gameObject != null)
            Destroy(gameObject, 0.1f);
    }

    protected virtual IEnumerator KnockdownRoutine()
    {
        isKnockedOut = true;
        baseAnim.SetTrigger("Knockdown");
        actorCollider.SetColliderStance(false);
        yield return new WaitForSeconds(1.0f);
        actorCollider.SetColliderStance(true);
        baseAnim.SetTrigger("GetUp");
        knockdownRoutine = null;
    }

    public void DidGetUp()
    {
        isKnockedOut = false;
    }

    public bool CanBeHit()
    {
        if (isAlive && isKnockedOut == false)
            return true;
        return false;
    }

    public virtual bool CanWalk()
    {
        return true;
    }

    public virtual void FaceTarget(Vector3 targetPoint)
    {
        FlipSprite(transform.position.x - targetPoint.x > 0);
    }

    //applies the pushback force variable of the AttackData to the Rigidbody of this Actor.
    //multiplies data.force by the direction of the hit, because the actor needs to lunge backward in the opposite direction of the hero’s punch
    //Lastly, it executes TakeDamage using the AttackData’s attackDamage and hitVector values.
    public virtual void EvaluateAttackData(AttackData data, Vector3 hitVector, Vector3 hitPoint)
    {
        body.AddForce(data.force * hitVector);
        TakeDamage(data.attackDamage, hitVector, data.knockdown);
        ShowHitEffects(data.attackDamage, hitPoint);
    }

    protected void ShowHitEffects(float value, Vector3 position)
    {
        ///To do: dmgOBJ prefab script///
        GameObject dmgObj = Instantiate(hitDMGPrefab);
        dmgObj.transform.position = position;

        //Creates a new instance of the hitValuePrefab and sets its text to the amount of
        //damage taken. After one second, it triggers the DestroyTimer script.
        GameObject obj = Instantiate(hitValuePrefab);
        obj.GetComponent<Text>().text = value.ToString();
        obj.GetComponent<DestroyTimer>().EnableTimer(1.0f);
    }

    public void PlaySFX(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}

[System.Serializable]
public class AttackData
{
    public float attackDamage = 10;
    public float force = 50;
    public bool knockdown = false;
}
