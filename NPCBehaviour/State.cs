using UnityEngine;

public class State : ScriptableObject {

    public GameObject interestedIn;
    public GameObject secondaryInterest;
    public bool hostileToInterests;

    [HideInInspector] public GameObject lastThreat;   //use this when influencing - could be good for WHO to hide from or attack

    protected Enemy owner;
    protected EnemySpawnPoint spawn;
    protected AbilityComponent abilComponent;

    protected Transform target;
    protected AgentStats targetStats;
    
    /// Used to create a Scriptable Object instance
    public State CreateState(Enemy owner, GameObject lastInfluence)
    {
        State newState = (State)Instantiate(Resources.Load("State Objects/" + name));
        newState.EnterState(owner, lastInfluence);
        return newState;
    }

    /// Use as a constructor to init local variables
    protected virtual void EnterState(Enemy owner, GameObject lastInfluence)
    {
        this.owner = owner;
        lastThreat = lastInfluence;
        spawn = owner.Spawn;
        abilComponent = owner.abilityComponent;
    }
}
