using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class DataLoader : MonoBehaviour
{
    const string url = "http://data.ikppbb.com/test-task-unity-data/pics/33.jpg";

    [SerializeField] private Image[] _images;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadTextureFromServer());
    }

    IEnumerator LoadTextureFromServer()
    {
        for (int i = 1; i < 11; i++)
        {
            using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture($"http://data.ikppbb.com/test-task-unity-data/pics/{i}.jpg"))
            {
                yield return uwr.SendWebRequest();

                if (uwr.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(uwr.error);
                }
                else
                {
                    // Get downloaded asset bundle
                    var texture = DownloadHandlerTexture.GetContent(uwr);
                    _images[i-1].sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                }
            }
        }
    }
}
