using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIEnemyControls : MonoBehaviour
{
    private Vector3 target;
    void Start()
    {
        GameState.ai_enemyStartingPosition = transform.position;
        GameState.ai_enemyPosition = transform.position;
    }
    void Update()
    {
        if (GameState.ballPosition.z < Constants.CENTER && GameState.ballVelocity.z < 0) {
            target.x = GameState.ballPosition.x + Constants.AI_TARGET_OFFSET.x;
            target.y = GameState.ballPosition.y + Constants.AI_TARGET_OFFSET.y;
            target.z = GameState.ai_enemyPosition.z;
        } else {
            target = GameState.ai_enemyStartingPosition;
        }
        Vector3 direction = target - GameState.ai_enemyPosition;
        if (direction.magnitude < 0.1f) return;
        direction = direction.normalized;
        GameState.ai_enemyPosition.x += direction.x * Constants.AI_SPEED;
        GameState.ai_enemyPosition.y += direction.y * Constants.AI_SPEED;
        transform.position = GameState.ai_enemyPosition;
    }
}
