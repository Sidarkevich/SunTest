using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellsLine : MonoBehaviour
{
    [SerializeField] private Cell[] _cells;

    public void Setup(List<Sprite> sprites)
    {
        for (int i = 0; i < _cells.Length; i++)
        {
            _cells[i].Setup(sprites[i]);
        }
    }
}
