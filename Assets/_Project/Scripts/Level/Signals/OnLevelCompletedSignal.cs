namespace _Project.Scripts.Level.Signals
{
    public struct OnLevelCompletedSignal
    {
        public readonly int LevelIndex;

        public OnLevelCompletedSignal(int levelIndex)
        {
            LevelIndex = levelIndex;
        }
    }
}