using System;
using System.Collections.Generic;
using UnityEngine;

//Relating to the player and their actions
public class PlayerManager : MonoBehaviour {

    private static PlayerManager _PlayerManager;
    public static PlayerManager Instance { get { return _PlayerManager; } }

    public GameObject player;   //reference for runtime
    public PlayerController playerControls;
    public Item[] equipped;

    public Action onTargetDestroyed;    //for targeting, xp, etc...

    public PlayerStats playerStats;
    public AbilityComponent playerAbilComponent;

    public List<Abilities> UnlockedAbilities { get { return playerAbilComponent.unlockedAbilities; } }
    public Dictionary<Stats, Stat> PlayerStatModifiers { get { return playerStats.statModifiers; } }
    
    private void Awake()
    {
        //Singleton setup
        if (_PlayerManager == null)
        {
            _PlayerManager = this;
        }
        else if (_PlayerManager != this)
        {
            Destroy(gameObject);
        }
        //DontDestroyOnLoad(gameObject);

        playerStats = player.GetComponent<PlayerStats>();
        playerAbilComponent = player.GetComponent<AbilityComponent>();
        playerControls = player.GetComponent<PlayerController>();
    }


    void EquipWeapon(Item item)
    {
        item.gameObject.SetActive(true);
        item.transform.parent = player.transform;
        item.transform.position = new Vector3(player.transform.position.x,
            (player.transform.position.y + player.transform.localScale.y),
            player.transform.position.z);
    }
}
