using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressUI : UIPanel
{
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text currentLevelText;
    [SerializeField] private TMP_Text nextLevelText;
    [SerializeField] private Image progressBarFillImage;

    public void UpdateProgress(float progressFillAmount, bool animated = false, float duration = 1f)
    {
        progressBarFillImage.DOKill();

        if (animated)
        {
            progressBarFillImage.fillAmount = progressFillAmount;
            progressBarFillImage.DOFillAmount(progressFillAmount, duration).SetEase(Ease.OutElastic);
        }
        else
        {
            progressBarFillImage.fillAmount = progressFillAmount;
        }
    }

    public void ResetProgress()
    {
        UpdateProgress(0f);
    }

    public void UpdateLevelTexts(int currentLeveNo)
    {
        levelText.DOFade(1f, 0.25f);
        levelText.text = "Level " + currentLeveNo;

        var nextLevelNo = currentLeveNo + 1;
        currentLevelText.text = currentLeveNo.ToString();
        nextLevelText.text = nextLevelNo.ToString();
    }

    public override void Show(float buttonDelay = 0)
    {
        
    }

    public override void Hide()
    {
        
    }
}
