using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.UI
{
    public abstract class UIPanel : MonoBehaviour
    {
        [SerializeField] protected CanvasGroup canvasGroup;

        public abstract void Show(float buttonDelay = 0);

        public abstract void Hide();
    
        public virtual async UniTask ShowAfterSeconds(float delay)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay));
            Show();
        }
    }
}
