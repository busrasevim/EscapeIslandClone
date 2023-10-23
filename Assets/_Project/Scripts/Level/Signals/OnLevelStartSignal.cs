namespace _Project.Scripts.Level.Signals
{
    public struct OnLevelStartSignal
    {
        public readonly int StartingLevelIndex;

        public OnLevelStartSignal(int startingLevelIndex)
        {
            StartingLevelIndex = startingLevelIndex;
        }
    }
}