using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Movement Setting", fileName = "New Movement Settings")]
public class StickMovementSettings: ScriptableObject
{
    public float stickMoveSpeed = 1.5f;
    public float stickRotateSpeed = 9f;
    public float lastMoveTime = 1f;
    public float lastRotateTime = 0.5f;
    public float groupMoveDelayTime = 0.5f;
}
