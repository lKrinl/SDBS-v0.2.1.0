using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class PlayerCollider : MonoBehaviour {

    public Vector3 standingColliderCenter;
    public Vector3 standingColliderSize;

    public Vector3 downColliderCenter;
    public Vector3 downColliderSize;

    private CapsuleCollider playerCollider;

    private void Awake()
    {
        actorCollider = GetComponent<CapsuleCollider>();
    }

    public void SetColliderStance(bool isStanding)
    {
        if (isStanding)
        {
            actorCollider.center = standingColliderCenter;
            actorCollider.size = standingColliderSize;
        }
        else
        {
            actorCollider.center = downColliderCenter;
            actorCollider.size = downColliderSize;
        }
    }
}
