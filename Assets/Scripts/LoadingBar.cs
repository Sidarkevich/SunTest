using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class LoadingBar : MonoBehaviour
{
    [SerializeField] private float _loadingTime;
    [SerializeField] private Image _fillImage;

    public void StartAnimation()
    {

    }

    private IEnumerator AnimationCoroutine()
    {
        yield return new WaitForSeconds(_loadingTime);
    }
}
