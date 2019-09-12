using System;
using UnityEngine;
using UnityEngine.AI;

/// e.g.:  Targetable <- InteractableNPC <- (StoryNPCS) || Targetable <- Enemy <- (EnemyNPCs)
public class Enemy : Targetable {

    protected static GameObject player;
    protected static Difficulty gameDifficulty;

    public GameObject deathEffect;

    public Transform target;
    
    //Components
    [HideInInspector] public NavMeshAgent navAgent;
    [HideInInspector] public AbilityComponent abilityComponent;
    [HideInInspector] public Rigidbody rBody;
    [HideInInspector] public AgentStats stats;

    //Properties
    public EnemyFieldSpawnPoint Spawn
    {
        get { return home; }
        set { home = value; transform.SetParent(value.transform); }
    }
    public Vector3 Position { get { return transform.position; } }
    public Vector3 Velocity { get { return navAgent.velocity; } set { navAgent.velocity = value; } }
    public float SightRange { get { return neighbourhoodTracker.TrackingRadius; } }

    protected void Awake()  //Object initialised
    {
        navAgent = GetComponent<NavMeshAgent>();
        abilityComponent = GetComponent<AbilityComponent>();
        rBody = GetComponent<Rigidbody>();
        stats = GetComponent<AgentStats>();
    }

    //References & Event subscriptions
    protected void Start () //Scripts initialised
    {
        if (player == null) player = PlayerManager.Instance.player;
        SetStats(); //Update stats at start

        //Events
        stats.onDeath += Die;
    }

    //Event unsubscriptions
    protected void OnDisable()
    {
        Spawn.RemoveEnemy(this);
        //Event Removals
        stats.onDeath -= Die;
    }

    private void Die()
    {
        MissionManager.Instance.RegisterKill(this);

        abilityComponent.enabled = false;
        emotionChip.enabled = false;
        navAgent.velocity = new Vector3(0f, 0f, 0f);
        navAgent.destination = transform.position;
        GameObject deathFx = Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(deathFx, 2f);
        Destroy(gameObject, .25f);
    }

    //Stats - Related to difficulty/load events
    public void SetStats()
    {
        //Apply stat modifiers with difficulty setting
        stats.ApplyStatModifiers();

        //Set Component Variables (may use stats)
        neighbourhoodTracker.TrackingRadius = Mathf.Sqrt(stats.sqrMaxTargetDistance);
    }


    // ----- NAVIGATION ----- //

    public void MoveTo(Vector3 target)
    {
        if (navAgent != null)
        {
            navAgent.destination = target;
        }
        else
        {
            Vector3 direction = target - Position;
            direction = direction.normalized;
            float ownerSpeed = stats.speed;

            Velocity = direction * ownerSpeed;
        }
        FaceTarget(target);
    }

    public void MoveToRandomWaypoint()
    {
        if(Spawn.spawnAreaWaypoints.Count <= 1)
        {
            MoveTo(RandomNavMeshPoint(SightRange));
        }

        //Get random point, based upon spawner waypoints
        Vector3 patrolPos = Spawn.spawnAreaWaypoints[UnityEngine.Random.Range(0, Spawn.spawnAreaWaypoints.Count)];

        //Prevent waypoint at current location being set as new target
        if (patrolPos == Position)
        {
            MoveToRandomWaypoint();
            return;
        }

        MoveTo(patrolPos);
    }

    private void FaceTarget(Vector3 target)
    {
        Vector3 direction = target - Position;
        direction = direction.normalized;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.fixedDeltaTime * 10f);
        }
    }

    public bool CanSeeTarget(Transform target)
    {
        Ray ray = new Ray(Position, (target.position - Position));
        RaycastHit hit;

        //Ball raycast - disregard layers
        if (target.tag.Equals("Ball"))
        {
            if (Physics.Raycast(ray, out hit, SightRange, LayerMask.GetMask("Default", GameMetaInfo._LAYER_IMMOVABLE_OBJECT), QueryTriggerInteraction.Ignore))
            {
                if (hit.transform.tag.Equals(target.tag))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        else //Target other actors - change layer mask
        {
            if (Physics.Raycast(ray, out hit, SightRange, LayerMask.GetMask(GameMetaInfo._LAYER_AFFECTABLE_OBJECT), QueryTriggerInteraction.Ignore))
            {
                if (hit.transform.tag.Equals(target.tag))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        return false;
    }
}
