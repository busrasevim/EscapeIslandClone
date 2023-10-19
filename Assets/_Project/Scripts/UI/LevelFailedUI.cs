using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LevelFailedUI : UIPanel
{
    [SerializeField] private CanvasGroup tapToContinueButtonCanvasGroup;
    [Inject] private GameManager _gameManager;
    
    public override void Show(float buttonDelay = 0)
    {
        canvasGroup.Show(0.5f);

        tapToContinueButtonCanvasGroup.Show(0.25f, buttonDelay);
    }

    public override void Hide()
    {
        canvasGroup.Hide();
        tapToContinueButtonCanvasGroup.Hide();
    }

    public void PressedTapToContinue() //butona verilecek
    {
        Hide();
        _gameManager.RestartLevel();
    }
}
