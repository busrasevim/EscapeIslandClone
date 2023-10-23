using _Project.Scripts.Managers;
using _Project.Scripts.Utils;
using Zenject;

namespace _Project.Scripts.UI
{
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
}
