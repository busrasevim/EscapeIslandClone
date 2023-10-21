using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchController
{
    private Island _selectedIsland;

    public void SelectIsland(Island island)
    {
        if (_selectedIsland && !_selectedIsland.IsIslandEmpty())
        {
            if (island.IsIslandOkay(_selectedIsland))
            {
                //pre deki o renge ait hepsi yeni island a gidecek
                var emptySlotCount = island.GetEmptySlotCount();
                var groups = _selectedIsland.GetAvailableGroups(emptySlotCount);
                island.GroupTransition(groups);
            }
            
            DeselectAll();
        }
        else
        {
            _selectedIsland = island;
        }
    }

    public void DeselectAll()
    {
        _selectedIsland = null;
    }
}
