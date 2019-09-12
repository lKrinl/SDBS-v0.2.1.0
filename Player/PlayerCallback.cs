using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCallback : MonoBehaviour {

    public Player player;

    public void DidGetUp()
    {
        player.DidGetUp();
    }
}
