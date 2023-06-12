using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;

public class ContentLoader : MonoBehaviour
{
    public UnityEvent<List<Sprite>> ContentLoadedEvent;
    [SerializeField] private int _allowableErrorsCount;

    private Dictionary<int, Sprite> _downloaded = new Dictionary<int, Sprite>();
    private List<Sprite> _content;
    public List<Sprite> Downloaded => _content;

    private void Start()
    {
        LoadAsync();
        //StartCoroutine(LoadTextureFromServer());
    }

    private async void LoadAsync()
    {
        Debug.Log("Start downloading..");

        var errorsCount = 0;
        var nextId = 1;

        while (errorsCount < _allowableErrorsCount)
        {
            try 
            {
                var sprite = await LoadSpriteAsync($"http://data.ikppbb.com/test-task-unity-data/pics/{nextId}.jpg");
                _downloaded.Add(nextId, sprite);
                Debug.Log("Downloaded: " + nextId);
            }
            catch
            {
                Debug.Log("Failed while downloading " + nextId);
                errorsCount++;
            }

            nextId++;
        }

        _content = new List<Sprite>(_downloaded.Values);
        Debug.Log("Download finished! Result: " + _content.Count);

        ContentLoadedEvent?.Invoke(_content);
    }

    private async UniTask<Sprite> LoadSpriteAsync(string url)
    {
        using var request = UnityWebRequestTexture.GetTexture(url);

        await request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            var texture = DownloadHandlerTexture.GetContent(request);
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        }
        else
        {
            return null;
        }
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
