using UnityEngine;

namespace _Project.Scripts.Data
{
    [CreateAssetMenu(menuName = "Settings/Game Setting", fileName = "New Game Settings")]
    public class GameSettings : ScriptableObject
    {
        [Header("LEVEL GENERATION")]
        [Range(3, 5)] public int slotStickCount;
        public int bonusLevelIslandCount;
        public Color[] stickColors;
        public float oppositeIslandsDistance = 4f;
        public float nearIslandsDistance = 1.5f;
        public float zCenterPosition = 2.7f;
        public float normalIslandOffset = 2f;
        public float bonusLevelRadius = 2.4f;

        [Space(10)] [Header("STICKS")] public float stickMoveSpeed = 1.5f;
        public float stickRotateSpeed = 9f;
        public float lastMoveTime = 1f;
        public float lastRotateTime = 0.5f;
        public float groupMoveDelayTime = 0.5f;
    }
}