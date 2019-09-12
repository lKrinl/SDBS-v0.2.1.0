using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : MonoBehaviour {

    ///<summary>///
    //Scriptable movement reserved for Azazel//
    ///</summary>///
    
    float startY;
    [Range(0f, 1f)] public float flyRange = 1f;

    private void Start()
    {
        startY = transform.position.y;
    }

    private void Update()
    {
        //Speed is time, range is flyRange
        transform.position = new Vector3(transform.position.x, startY + Mathf.Sin(Time.time) * flyRange, transform.position.z);
    }
}
