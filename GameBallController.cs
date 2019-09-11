using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BallController : MonoBehaviour
{
    private TrailRenderer trailRenderer;
    private Vector3 scale;
    private AudioSource audioSource;
    private Color blueTrailColor = new Color(0.1f, 0.1f, 0.8f, 0.5f);
    private Color redTrailColor = new Color(0.8f, 0.1f, 0.1f, 0.5f);
    private List<AudioClip> sounds = new List<AudioClip>();
    System.Random rnd = new System.Random();
    void Start()
    {
        trailRenderer = GameObject.Find("Trail").GetComponent<TrailRenderer>();
        trailRenderer.endColor = Color.white;
        trailRenderer.startColor = blueTrailColor;
        GameState.ballStartingPosition = transform.position;
        GameState.ballPosition = GameState.ballStartingPosition;
        scale = transform.localScale;
        audioSource = gameObject.GetComponent<AudioSource>();
        sounds.Add((AudioClip)Resources.Load("Sound/hit1"));
        sounds.Add((AudioClip)Resources.Load("Sound/hit2"));
        sounds.Add((AudioClip)Resources.Load("Sound/hit3"));
        sounds.Add((AudioClip)Resources.Load("Sound/hit4"));
    }
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name == "TennisBatBlue") {
            // Player swing
            GameState.bounces = 0;
            GameState.turn = 0;
            Vector3 hitDirection = (GameState.mouseTarget - GameState.playerPosition).normalized;
            float s = GameState.playerSmoothVelocity.magnitude;
            // float s = GameState.swingPower * 20.0f;
            GameState.swingPower = 0;
            GameState.ballVelocity.x = Constants.IMPACT * s * hitDirection.x; //GameState.playerSmoothVelocity.x;
            GameState.ballVelocity.z = Constants.IMPACT * s * hitDirection.z; //GameState.playerSmoothVelocity.z;
            GameState.ballVelocity.y = GameState.ballVelocity.y * 0.25f + Constants.IMPACT * s * 0.5f;
            trailRenderer.startColor = blueTrailColor;
            scale.z = 0.5f;
            transform.localScale = scale;
            GameState.ballPosition += GameState.ballVelocity * .02f;
            audioSource.PlayOneShot(sounds[rnd.Next(0, 3)]);
        } else if (collider.gameObject.name == "TennisBatRed") {
            // Opponent swing
            GameState.bounces = 0;
            GameState.turn = 1;
            Vector3 hitDirection = (new Vector3(UnityEngine.Random.Range(-14.5f, 3.0f), 0, 7) - GameState.opponentPosition).normalized;
            float s = GameState.ballVelocity.magnitude;
            GameState.ballVelocity.x = s * hitDirection.x; //GameState.playerSmoothVelocity.x;
            GameState.ballVelocity.z = s * hitDirection.z; //GameState.playerSmoothVelocity.z;
            GameState.ballVelocity.y = 0.5f * s;
            trailRenderer.startColor = redTrailColor;
            scale.z = 0.5f;
            transform.localScale = scale;
            GameState.ballPosition += GameState.ballVelocity * .02f;
            audioSource.PlayOneShot(sounds[rnd.Next(0, 3)]);
        } else if (collider.gameObject.name == "TennisNet") {
            endRound();
        }
    }
    Vector3 getCollisionVector(Vector3 oldPosition, Vector3 newPosition, Vector3 normal, float distanceToBoundary) 
    {
        float l = Vector3.Distance(newPosition, oldPosition);
        Vector3 d = (newPosition - oldPosition) / l;
        Vector3 D = d - 2 * Vector3.Dot(d, normal) * normal;
        float L = distanceToBoundary / Vector3.Dot(normal, d);
        return oldPosition + (l - L) * d + L * D; 
    }
    bool isOutsideGameField(Vector3 position) {
        return !(position.x < Constants.MAX_FIELD.x 
            && position.x > Constants.MIN_FIELD.x
            && position.z < Constants.MAX_FIELD.y
            && position.z > Constants.MIN_FIELD.y);
    }
    void Update()
    {
        if (GameState.gamePaused) return;
        // Gravity
        GameState.ballVelocity.y -= Constants.GRAVITY;
        // Position update
        Vector3 newPosition = GameState.ballPosition + GameState.ballVelocity * Time.deltaTime;
        // Boundary
        if (newPosition.x > Constants.MAX_BOUNDS.x) {
            //newPosition = getCollisionVector(GameState.ballPosition, newPosition, new Vector3(1, 0, 0), Math.Abs(newPosition.x - Constants.MAX_BOUNDS.x)); 
            //GameState.ballVelocity.x *= -1;
            endRound();
        }
        else if (newPosition.x < Constants.MIN_BOUNDS.x) {
            //newPosition = getCollisionVector(GameState.ballPosition, newPosition, new Vector3(-1, 0, 0), Math.Abs(newPosition.x - Constants.MIN_BOUNDS.x)); 
            //GameState.ballVelocity.x *= -1;
            endRound();
        }
        else if (newPosition.y > Constants.MAX_BOUNDS.y) {
            //newPosition = getCollisionVector(GameState.ballPosition, newPosition, new Vector3(0, 1, 0), Math.Abs(newPosition.y - Constants.MAX_BOUNDS.y)); 
            //GameState.ballVelocity.y *= -1;
            endRound();
        }
        else if (newPosition.y < Constants.MIN_BOUNDS.y) {
            newPosition = getCollisionVector(GameState.ballPosition, newPosition, new Vector3(0, -1, 0), Math.Abs(newPosition.y - Constants.MIN_BOUNDS.y)); 
            GameState.ballVelocity.y *= -1 * Constants.FRICTION;
            GameState.bounces++;
            scale.y = 0.5f;
            transform.localScale = scale;
            if (GameState.bounces > 1 || isOutsideGameField(newPosition)) {
                endRound();
                return;
            } else if (newPosition.z > Constants.CENTER) {
                if (GameState.turn == 1) GameState.bounces = 0;
                GameState.turn = 0;
                trailRenderer.startColor = blueTrailColor;
            } else {
                if (GameState.turn == 0) GameState.bounces = 0;
                GameState.turn = 1;
                trailRenderer.startColor = redTrailColor;
            }
            audioSource.PlayOneShot(sounds[3]);
        }
        else if (newPosition.z > Constants.MAX_BOUNDS.z) {
            //newPosition = getCollisionVector(GameState.ballPosition, newPosition, new Vector3(0, 0, 1), Math.Abs(newPosition.z - Constants.MAX_BOUNDS.z)); 
            //GameState.ballVelocity.z *= -1;
            endRound();
        }
        else if (newPosition.z < Constants.MIN_BOUNDS.z) {
            //newPosition = getCollisionVector(GameState.ballPosition, newPosition, new Vector3(0, 0, -1), Math.Abs(newPosition.z - Constants.MIN_BOUNDS.z)); 
            //GameState.ballVelocity.z *= -1;
            endRound();
        }
        // position = Vector3.Min(Constants.MAX_BOUNDS, position);
        // position = Vector3.Max(Constants.MIN_BOUNDS, position);
        // Update transform
        if (scale.y < 1.0) {
            scale.y += 0.05f;
            scale.y = Math.Min(scale.y, 1.0f);
            transform.localScale = scale;
        }
        if (scale.z < 1.0) {
            scale.z += 0.05f;
            scale.z = Math.Min(scale.z, 1.0f);
            transform.localScale = scale;
        }
        GameState.ballPosition = newPosition;
        transform.position = GameState.ballPosition;
    }
    void endRound() {
        GameState.currentRound++;
        if (GameState.turn == 0) {
            GameState.opponentScore++;
            GameState.lastRoundWinner = 1;
        } else {
            GameState.playerScore++;
            GameState.lastRoundWinner = 0;
        }
        if (GameState.currentRound <= Constants.ROUNDS_PER_GAME 
            && GameState.playerScore <= Constants.ROUNDS_PER_GAME/2
            && GameState.opponentScore <= Constants.ROUNDS_PER_GAME/2) {
            nextRound();
        } else endGame();
    }
    void nextRound() {
        trailRenderer.startColor = blueTrailColor;
        // transform.position = GameState.ballStartingPosition;
        EventManager.TriggerEvent("NEXT_ROUND");
    }
    void endGame() {
        trailRenderer.startColor = blueTrailColor;
        EventManager.TriggerEvent("END_GAME");
    }
}
