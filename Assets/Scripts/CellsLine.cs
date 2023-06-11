using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CellsLine : MonoBehaviour
{
    [SerializeField] private Cell[] _cells;

    public void Setup(List<Sprite> sprites, UnityAction<Sprite> clickCallback)
    {
        for (int i = 0; i < _cells.Length; i++)
        {
            _cells[i].Setup(sprites[i]);
            _cells[i].CellSelectedEvent.AddListener(clickCallback);
        }
    }
}
