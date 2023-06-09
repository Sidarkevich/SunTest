using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class DataLoader : MonoBehaviour
{
    const string url = "http://data.ikppbb.com/test-task-unity-data/pics/33.jpg";

    [SerializeField] private int _loadCount;
    [SerializeField] private Cell _prefab;
    [SerializeField] private Transform _content;

    [SerializeField] private ImageScreen _screen;

    void Start()
    {
        StartCoroutine(LoadTextureFromServer());
    }

    IEnumerator LoadTextureFromServer()
    {
        var errorsCount = 0;
        var nextId = 1;

        while (errorsCount < 2)
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
                    // Get downloaded asset bundle
                    var texture = DownloadHandlerTexture.GetContent(uwr);
                    var cell = Instantiate(_prefab, Vector3.zero, Quaternion.identity, _content);
                    cell.Setup(Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero));

                    cell.CellSelectedEvent.AddListener(_screen.Setup);
                }

                nextId++;
            }
        }
    }
}
