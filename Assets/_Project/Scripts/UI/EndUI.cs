using UnityEngine;

namespace _Project.Scripts.UI
{
    public class EndUI : UIPanel
    {
        [SerializeField] private LevelCompletedUI levelCompletedUI;
        [SerializeField] private LevelFailedUI levelFailedUI;
        private bool _isWin;
    
        public override void Show(float buttonDelay = 0)
        {
            if (_isWin)
            {
                levelCompletedUI.Show();
            }
            else
            {
                levelFailedUI.Show();
            }
        
            gameObject.SetActive(true);
        }

        public override void Hide()
        {
            levelCompletedUI.Hide();
            levelFailedUI.Hide();
            gameObject.SetActive(false);
        }

        public void SetWin(bool isWin)
        {
            _isWin = isWin;
        }
    }
}
