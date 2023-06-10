using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalList : MonoBehaviour
{
    [SerializeField] private RectTransform _container;
    [SerializeField] private RectTransform _viewPort;
    [SerializeField] private RectTransform _prefab;
    [SerializeField] private float _spacing;

    private float _prefabSize;
    private float _containerHalfSize;
    private int _visibleCount;
    private Vector3 _startPos;
    private Vector3 _offsetVec = Vector3.down;
    private int _numItems = 0;
    private int _numBuffer = 2;

    private List<RectTransform> listItems = new List<RectTransform>();
    private Dictionary<int, int[]> itemDict = new Dictionary<int, int[]>();
    private List<RectTransform> listItemRect = new List<RectTransform>();

    private List<Sprite> _content;
 
    public void Setup(List<Sprite> content)
    {
        _content = content;

        _container.anchoredPosition3D = Vector3.zero;

        Vector2 prefabScale = _prefab.rect.size;
        _prefabSize = prefabScale.y + _spacing;

        _container.sizeDelta = new Vector2(prefabScale.x, _prefabSize * _content.Count);
        _containerHalfSize = _container.rect.size.y * 0.5f;

        _visibleCount = Mathf.CeilToInt(_viewPort.rect.size.y / _prefabSize);

        _numItems = Mathf.Min(content.Count, _visibleCount + _numBuffer);

        _startPos = _container.anchoredPosition3D - (_offsetVec * _containerHalfSize) + (_offsetVec * prefabScale.y * 0.5f);

        for (int i = 0; i < _numItems; i++)
        {
            var obj = Instantiate(_prefab.gameObject, _container.transform);
            RectTransform rect = obj.GetComponent<RectTransform>();
            rect.anchoredPosition3D = _startPos + (_offsetVec * i * _prefabSize);

            listItems.Add(rect);
            listItemRect.Add(rect);
            itemDict.Add(rect.GetInstanceID(), new int[] { i, i });

            obj.SetActive(true);

            var cell = obj.GetComponent<Cell>();
            cell.Setup(content[i]);
        }

        _container.anchoredPosition3D += _offsetVec * (_containerHalfSize - _viewPort.rect.size.y * 0.5f);
    }

    public void ReorderItemsByPos(float normPos)
    {
        normPos = 1f - normPos;

        int numOutOfView = Mathf.CeilToInt(normPos * (66 - _visibleCount));   //number of elements beyond the left boundary (or top)
        int firstIndex = Mathf.Max(0, numOutOfView - _numBuffer);   //index of first element beyond the left boundary (or top)
        int originalIndex = firstIndex % _numItems;

        int newIndex = firstIndex;
        for (int i = originalIndex; i < _numItems; i++)
        {
            moveItemByIndex(listItemRect[i], newIndex);
            listItems[i].GetComponent<Cell>().Setup(_content[newIndex]);
            newIndex++;
        }
        for (int i = 0; i < originalIndex; i++)
        {
            Debug.Log(newIndex);

            moveItemByIndex(listItemRect[i], newIndex);
            listItems[i].GetComponent<Cell>().Setup(_content[newIndex]);
            newIndex++;
        }
    }

    private void moveItemByIndex(RectTransform item, int index)
    {
        int id = item.GetInstanceID();
        itemDict[id][0] = index;
        item.anchoredPosition3D = _startPos + (_offsetVec * index * _prefabSize);
    }
}
