using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class StartUI : UIPanel
{
    [Inject] private GameManager _gameManager;
    [SerializeField] private ProgressUI progressUI;
    
    public override void Show(float buttonDelay = 0)
    {
        canvasGroup.Show(0.1f);
    }

    public override void Hide()
    {
        canvasGroup.Hide();
    }

    public void PressedStartButton()
    {
        StartLevel();
    }

    private void StartLevel()
    {
        Hide();
        _gameManager.StartLevel();
    }

    public void SetLevelText(int levelIndex)
    {
        progressUI.UpdateLevelTexts(levelIndex);
    }
}
