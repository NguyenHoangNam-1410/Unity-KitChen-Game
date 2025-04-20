using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private CuttingCounter cuttingCounter;
    [SerializeField] private Image barImage;


    private void Start()
    {
        cuttingCounter.OnProgessChanged += CuttingCounter_OnProgessChanged;
        barImage.fillAmount = 0f;
        Hide();
    }

    private void CuttingCounter_OnProgessChanged(object sender, CuttingCounter.OnProgessChangedEventArg e)
    {
        barImage.fillAmount = e.progressNormalize;

        if(e.progressNormalize == 0f || e.progressNormalize == 1f)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
