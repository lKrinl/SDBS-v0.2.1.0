using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants
{
    public static Vector3 MAX_BOUNDS = new Vector3(22, 100, 34);
    public static Vector3 MIN_BOUNDS = new Vector3(-30, -0.75f, -44);
    public static Vector2 MIN_FIELD = new Vector2(-14.9f, -25.2f);
    public static Vector2 MAX_FIELD = new Vector2(5.2f, 14.6f);
    public static Vector3 BAT_OFFSET = new Vector3(0.0f, -9f, -18.0f);
    public static Vector3 BALL_INITIAL_VELOCITY = new Vector3(0, 1.5f, 0);
    public static Vector3 AI_TARGET_OFFSET = new Vector3(0,-1.25f,0);
    public static float CENTER = -5.3f;
    public static float CAMERA_SPEED = 10.2f;
    public static float CAMERA_SCROLL_SPEED = 10.0f;
    public static float GRAVITY = 0.18f;
    public static float FRICTION = 0.9f;
    public static float IMPACT = 1.4f;
    public static float AI_SPEED = 0.15f;
    public static int ROUNDS_PER_GAME = 5;
    public enum State {
        LOADING_ASSETS = 0,
        MAIN_MENU = 1,
        IN_QUEUE = 2,
        IN_RANKED_GAME = 3,
        IN_PRACTICE_GAME = 4,
        IN_SUMMARY = 5
    }
}
