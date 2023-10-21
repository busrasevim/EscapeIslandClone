using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MatchController
{
    private Island _selectedIsland;
    private Island[] _allIslands;
    [Inject] private LineManager _lineManager;

    public void SelectIsland(Island island)
    {
        if (_selectedIsland == island)
        {
            DeselectAll();
            return;
        }
        
        if (_selectedIsland && !_selectedIsland.IsIslandEmpty())
        {
            if (island.IsIslandOkay(_selectedIsland))
            {
                var emptySlotCount = island.GetEmptySlotCount();
                var groups = _selectedIsland.GetAvailableGroups(emptySlotCount);
                var line = _lineManager.SetLine(_selectedIsland.transform,island.transform);
                island.GroupTransition(groups, line);
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
        foreach (var island in _allIslands)
        {
            island.Deselect();
        }
    }
}
