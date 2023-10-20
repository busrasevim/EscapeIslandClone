using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Game Setting", fileName = "Game Settings")]
public class GameSettings : ScriptableObject
{
    public int slotStickCount;
    public Color[] stickColors;
}