using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUI : UIPanel
{
    public override void Show(float buttonDelay = 0)
    {
        canvasGroup.Show();
    }

    public override void Hide()
    {
        canvasGroup.Hide();
    }
}
