using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private ProgressUI _progressUI;
    private StartUI _startUI;
    private LevelCompletedUI _levelCompletedUI;
    private LevelFailedUI _levelFailedUI;

    public void GameStart(int level)
    {
        _startUI.Show();
        _progressUI.UpdateLevelTexts(level);
    }
    
    public void LevelStart()
    {
        _startUI.Hide();
        _progressUI.Show();
    }

    public void LevelCompleted()
    {
        _levelCompletedUI.Show();
    }

    public void LevelFailed()
    {
        _levelFailedUI.Show();
    }
}
