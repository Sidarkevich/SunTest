using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

public class LoadingBar : MonoBehaviour
{
    [SerializeField] private float _loadingTime;
    [SerializeField] private int _thresholdsCount;
    [SerializeField] private Image _fillImage;

    public void StartAnimation()
    {
        _fillImage.fillAmount = 0;
        var thresholds = new List<float>();

        for (int i = 0; i < _thresholdsCount; i++)
        {
            thresholds.Add(Random.Range(0.0f, 0.9f));
        }
        thresholds.Sort();

        Sequence sequence = DOTween.Sequence();
        foreach (var hold in thresholds)
        {
            Debug.Log("Hold: " + hold);
            sequence.Append(_fillImage.DOFillAmount(hold * _loadingTime, _loadingTime / _thresholdsCount+1));
        }
        sequence.Append(_fillImage.DOFillAmount(1, _loadingTime / _thresholdsCount+1));
    }
}
