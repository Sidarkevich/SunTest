using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;

public class ContentLoader : MonoBehaviour
{
    public UnityEvent<List<Sprite>> ContentLoadedEvent;
    [SerializeField] private int _allowableErrorsCount;

    private Dictionary<int, Sprite> _downloaded = new Dictionary<int, Sprite>();
    private List<Sprite> _content;
    public List<Sprite> Downloaded => _content;

    public void LoadData()
    {
        StartCoroutine(LoadTextureFromServer());
    }

    IEnumerator LoadTextureFromServer()
    {
        Debug.Log("Start downloading..");

        var errorsCount = 0;
        var nextId = 1;

        while (errorsCount < _allowableErrorsCount)
        {
            using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture($"http://data.ikppbb.com/test-task-unity-data/pics/{nextId}.jpg"))
            {
                yield return uwr.SendWebRequest();

                if (uwr.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log($"{uwr.error} while loading: {nextId}");

                    errorsCount++;
                }
                else
                {
                    var texture = DownloadHandlerTexture.GetContent(uwr);
                    _downloaded.Add(nextId, Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero));
                }

                nextId++;
            }
        }

        _content = new List<Sprite>(_downloaded.Values);
        Debug.Log("Download finished! Result: " + _content.Count);

        ContentLoadedEvent?.Invoke(_content);
    }
}
