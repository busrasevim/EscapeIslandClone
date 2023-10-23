using _Project.Scripts.Game.Interfaces;
using Lean.Common;
using Zenject;

namespace _Project.Scripts.Game
{
    public class MatchController : IMatchController
    {
        private Island _selectedIsland;
        private Island _secondSelectedIsland;
        private Island[] _allIslands;
        [Inject] private LineManager _lineManager;
        private LeanSelect _leanSelect;
        
        

        public void SelectIsland(Island island)
        {
            if (_selectedIsland == island)
            {
                DeselectAll();
                return;
            }

            _secondSelectedIsland = island;
            var isSelectedIslandValid = _selectedIsland && !_selectedIsland.IsIslandEmpty();
            if (isSelectedIslandValid)
            {
                if (island.IsIslandOkayForMatch(_selectedIsland.GetFirstGroupColor()))
                {
                    var emptySlotGroupCount = island.GetEmptySlotGroupCount();
                    var stickGroups = _selectedIsland.GetAvailableStickGroups(emptySlotGroupCount);
                    var line = _lineManager.SetLine(_selectedIsland.transform,island.transform);
                    island.StickGroupTransition(stickGroups, line);
                }
            
                DeselectAll();
            }
            else
            {
                _selectedIsland = island;
                if(_selectedIsland.IsIslandEmpty())
                    DeselectAll();
            }
        }

        public void SetIslands(Island[] allIslands)
        {
            _allIslands = allIslands;
        }
    
        public void DeselectAll()
        {
            _selectedIsland = null;
            _secondSelectedIsland = null;
            foreach (var island in _allIslands)
            {
                island.Deselect();
            }
        }
    }
}
