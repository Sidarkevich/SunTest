using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Cell : MonoBehaviour
{
    [HideInInspector] public UnityEvent<Sprite> CellSelectedEvent;

    [SerializeField] private Image _image;

    public void Setup(Sprite sprite)
    {
        _image.sprite = sprite;
    }

    public void Clicked()
    {
        CellSelectedEvent?.Invoke(_image.sprite);
    }
}
