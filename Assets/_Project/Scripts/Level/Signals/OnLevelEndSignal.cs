namespace _Project.Scripts.Level.Signals
{
    public struct OnLevelEndSignal
    {
        public readonly bool IsWin;

        public OnLevelEndSignal(bool isWin)
        {
            IsWin = isWin;
        }
    }
}