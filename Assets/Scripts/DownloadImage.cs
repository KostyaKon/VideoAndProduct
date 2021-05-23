using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

/// <summary>
/// Class that uploads a picture at a given url
/// </summary>
public class DownloadImage : MonoBehaviour
{
    public Image image;

    public void GetImage(string url)
    {
        StartCoroutine(GetImageUrl(url));
    }


    IEnumerator GetImageUrl(string MediaUrl)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
            Debug.Log(request.error);
        else
        {
            Texture2D tex = ((DownloadHandlerTexture)request.downloadHandler).texture;
            Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(tex.width / 2, tex.height / 2));
            image.overrideSprite = sprite;
        }
    }
}
