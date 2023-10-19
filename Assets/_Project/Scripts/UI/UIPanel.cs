using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIPanel : MonoBehaviour
{
    [SerializeField] protected CanvasGroup canvasGroup;

    public abstract void Show(float buttonDelay = 0);

    public abstract void Hide();

    public virtual IEnumerator ShowAfterSeconds(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        Show();
    }
}
