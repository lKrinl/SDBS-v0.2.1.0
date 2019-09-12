using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector3 mousePosition;
    private bool mouseDown;
    private Vector3 mouseDelta;
    private Vector3 position;
    private GameObject Court;
    // Start is called before the first frame update
    void Start()
    {
        tennisField = GameObject.Find("TennisField");
        GameState.cameraStartingPosition = Camera.main.transform.position;
        GameState.cameraPosition = GameState.cameraStartingPosition;
        mouseDelta = new Vector3(0, 0, 0);
    }
    // Update is called once per frame
    void Update()
    {
        mouseDelta = Input.mousePosition - mousePosition; 
        mousePosition = Input.mousePosition;
        if (Input.GetMouseButtonDown(0)) {
            mouseDown = true;
            mouseDelta.x = 0; 
            mouseDelta.y = 0;
        } 
        if (Input.GetMouseButtonUp(0)) mouseDown = false;
        /* if (mouseDown) {
           GameState.swingPower = Math.Min(GameState.swingPower + .025f, 1.0f);
        } else {
            GameState.swingPower = Math.Max(GameState.swingPower - .025f, 0.0f);
        }*/
        Vector3 moveDirection = new Vector3(0,0,0);
        if (Input.GetKey(KeyCode.A))
            moveDirection += Vector3.right;
        if (Input.GetKey(KeyCode.D))
            moveDirection += Vector3.left;
        if (Input.GetKey(KeyCode.W))
            moveDirection += Vector3.back;
        if (Input.GetKey(KeyCode.S))
            moveDirection += Vector3.forward;
        if (moveDirection.magnitude > 0)
            GameState.cameraPosition += Constants.CAMERA_SPEED * moveDirection.normalized * Time.deltaTime;
        if (Input.GetKey(KeyCode.Space) && !GameState.cameraJumping) {
            GameState.cameraJumping = true;
            GameState.cameraJumpVelocity = 10.0f;
        }
        if (GameState.cameraJumping) {
            GameState.cameraPosition.y += GameState.cameraJumpVelocity * Time.deltaTime;
            GameState.cameraJumpVelocity -= 3.0f * Constants.GRAVITY;
            if (GameState.cameraPosition.y < GameState.cameraStartingPosition.y) {
                GameState.cameraPosition.y = GameState.cameraStartingPosition.y;
                GameState.cameraJumping = false;
                GameState.cameraJumpVelocity = 0.0f;
            }
        }
        GameState.cameraPosition = Vector3.Min(Constants.MAX_BOUNDS - Constants.BAT_OFFSET, GameState.cameraPosition);
        GameState.cameraPosition = Vector3.Max(Constants.MIN_BOUNDS - Constants.BAT_OFFSET, GameState.cameraPosition);
        GameState.cameraPosition.z = Math.Max(Constants.CENTER - Constants.BAT_OFFSET.z, GameState.cameraPosition.z);
        Camera.main.transform.position = GameState.cameraPosition;
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (tennisField.GetComponent<BoxCollider>().Raycast(ray, out hit, 1000)) {
            Transform objectHit = hit.transform;
            GameState.mouseTarget = hit.point;
        }
    }
}
