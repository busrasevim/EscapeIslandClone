using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[CreateAssetMenu(menuName = "Scriptable Objects/Game Setting", fileName = "Game Settings")]
public class GameSettings : ScriptableObject
{
    public int slotStickCount;
    public int bonusLevelIslandCount;
    public Color[] stickColors;
}
