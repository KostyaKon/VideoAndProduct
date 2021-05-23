using System.Collections;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Video;

/// <summary>
/// This class sends a Get request to the specified url address, receives a vast.xml file, 
/// finds the url address of the video, downloads it to the root folder of the project, 
/// and launches the video.
/// </summary>
public class GetMethod : MonoBehaviour
{
    [SerializeField]
    private string url = "https://6u3td6zfza.execute-api.us-east-2.amazonaws.com/prod/ad/vast";

    private VideoPlayer vp;
    private string urlVideo, nameDownloadVideo;

    void Start()
    {
        vp = GetComponent<VideoPlayer>();
        vp.enabled = false;
    }

    public void GetRequest()
    {
        StartCoroutine(GetData());
    }

    IEnumerator GetData()
    {
        // создаём get запрос
        using(UnityWebRequest request = UnityWebRequest.Get(url))
        {
            // ждём ответа от сервера
            yield return request.SendWebRequest();
            // если ошибка, выдаём в консоль ошибку
            if (request.isNetworkError || request.isHttpError)
                Debug.Log(request.error);
            else
            {
                // в Video Player выставляем, что источником видео будет url адресс
                vp.source = VideoSource.Url;
                // получаем url адресс видео, передаём её в Video Player и активируем Video Player
                urlVideo = new ReadXML().GetUrlVideo(request.downloadHandler.text);
                if(!(urlVideo == "Error"))
                {
                    vp.url = urlVideo;
                    vp.enabled = true;

                    // скачиваем видео
                    nameDownloadVideo = "video.webm";
                    WebClient myWebClient = new WebClient();
                    myWebClient.DownloadFile(urlVideo, nameDownloadVideo);
                }
                else
                {
                    Debug.Log(urlVideo);
                }
            }
        }
    }
}
