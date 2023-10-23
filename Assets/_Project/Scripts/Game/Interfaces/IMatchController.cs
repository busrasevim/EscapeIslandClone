namespace _Project.Scripts.Game.Interfaces
{
    public interface IMatchController
    {
        public void SelectIsland(Island island);
        public void SetIslands(Island[] allIslands);
        public void DeselectAll();
    }
}