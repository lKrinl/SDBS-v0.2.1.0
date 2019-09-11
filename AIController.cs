using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentControls : MonoBehaviour
{
    private Vector3 target;
    void Start()
    {
        GameState.opponentStartingPosition = transform.position;
        GameState.opponentPosition = transform.position;
    }
    void Update()
    {
        if (GameState.ballPosition.z < Constants.CENTER && GameState.ballVelocity.z < 0) {
            target.x = GameState.ballPosition.x + Constants.AI_TARGET_OFFSET.x;
            target.y = GameState.ballPosition.y + Constants.AI_TARGET_OFFSET.y;
            target.z = GameState.opponentPosition.z;
        } else {
            target = GameState.opponentStartingPosition;
        }
        Vector3 direction = target - GameState.opponentPosition;
        if (direction.magnitude < 0.1f) return;
        direction = direction.normalized;
        GameState.opponentPosition.x += direction.x * Constants.AI_SPEED;
        GameState.opponentPosition.y += direction.y * Constants.AI_SPEED;
        transform.position = GameState.opponentPosition;
    }
}
