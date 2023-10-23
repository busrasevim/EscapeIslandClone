using System;
using System.Collections;
using _Project.Scripts.Managers;
using _Project.Scripts.Utils;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.UI
{
    public class LevelCompletedUI : UIPanel
    {
        [SerializeField] private CanvasGroup tapToContinueButtonCanvasGroup;
        [Inject] private GameManager _gameManager;

        public async void ShowAfterSeconds(float delay, float buttonDelay = 2f)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay));
            Show(buttonDelay);
        }
    
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

        public void PressedTapToContinue()
        {
            Hide();
            _gameManager.RestartLevel();
        }
    }
}
