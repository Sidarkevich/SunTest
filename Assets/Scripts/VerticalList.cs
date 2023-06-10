using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalList : MonoBehaviour
{
    [SerializeField] private ContentLoader _loader;

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
 
    void Start()
    {
        _container.anchoredPosition3D = Vector3.zero;

        Vector2 prefabScale = _prefab.rect.size;
        _prefabSize = prefabScale.y + _spacing;

        _visibleCount = Mathf.CeilToInt(_viewPort.rect.size.y / _prefabSize);

        _numItems = Mathf.Min(10, _visibleCount + _numBuffer);

        _container.sizeDelta = new Vector2(prefabScale.x, _prefabSize * _numItems);
        _containerHalfSize = _container.rect.size.y * 0.5f;

        _startPos = _container.anchoredPosition3D - (_offsetVec * _containerHalfSize) + (_offsetVec * prefabScale.y * 0.5f);

        for (int i = 0; i < _numItems; i++)
        {
            var obj = Instantiate(_prefab.gameObject, _container.transform);
            RectTransform rect = obj.GetComponent<RectTransform>();
            rect.anchoredPosition3D = _startPos + (_offsetVec * i * _prefabSize);

            listItems.Add(rect);
            obj.SetActive(true);
        }

        _container.anchoredPosition3D += _offsetVec * (_containerHalfSize - _viewPort.rect.size.y * 0.5f);
    }
}
