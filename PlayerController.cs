using UnityEngine;
using System.Collections;

public class PlayerControls : MonoBehaviour
{
    CharacterController characterController;

    public float speed = 6.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;

    private Vector3 moveDirection = Vector3.zero;
    private GameObject GameBall;
    private List<Vector3> velocities;
    private int velocityArraySize = 10;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        velocities = new List<Vector3>();
        GameBall = GameObject.Find("GameBall");
        GameState.playerStartingPosition = Camera.main.transform.position;
        GameState.playerPosition = GameState.playerStartingPosition;
    }

    void Update()
    {
        Vector3 pos = transform.position;
        pos.x += moveSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;
        pos.z += moveSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
         float deltaX = GameState.playerPosition.x - GameState.playerStartingPosition.x;
        Vector3 velocity = (newPosition - GameState.playerPosition) / Time.deltaTime;
        velocities.Insert(0, velocity);
        if (velocities.Count > velocityArraySize) velocities.RemoveAt(velocityArraySize);
        Vector3 avgVelocity = new Vector3(0,0,0);
        for (int i = 0; i < velocities.Count; i++) {
            avgVelocity += velocities[i] / velocities.Count;
        }
        GameState.playerSmoothVelocity = avgVelocity;
        GameState.playerVelocity = velocity;
        GameState.playerPosition = newPosition;
        transform.position = GameState.playerPosition;
        /* Quaternion ballQuat =  Quaternion.LookRotation(
            ball.transform.position - GameState.playerPosition - 2.0f * Vector3.forward,
            Vector3.up
        );*/
        Quaternion targetQuat = Quaternion.LookRotation(
            GameState.mouseTarget - GameState.playerPosition,
            Vector3.up
        );
    }
}

