using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BallSensor : MonoBehaviour {

    public bool ballIsNearby;

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Ball")
            ballIsNearby = true;
    }

    public void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Ball")
            ballIsNearby = false;
    }
}
