using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class InGameUI : UIPanel
{
    [Inject] private GameManager _gameManager;
    
    public override void Show(float buttonDelay = 0)
    {
        canvasGroup.Show();
    }

    public override void Hide()
    {
        canvasGroup.Hide();
    }

    public void PressedRestartButton()
    {
        _gameManager.RestartLevel();
    }
}
