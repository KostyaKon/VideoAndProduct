using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

/// <summary>
/// A class that sends a request to the server, receives a description of the product, 
/// and displays information about the product
/// </summary>
public class PostMethod : MonoBehaviour
{
    [SerializeField]
    private string urlProduct = "https://6u3td6zfza.execute-api.us-east-2.amazonaws.com/prod/v1/gcom/ad";

    public Text nameProduct, priceProduct;
    public GameObject userPanel;

    public void PostRequest()
    {
        StartCoroutine(PostData());
    }

    private IEnumerator PostData()
    {
        WWWForm formData = new WWWForm();

        PostStruct post = new PostStruct();
        // преобразуем стракт в json строку
        string json = JsonUtility.ToJson(post);
        byte[] postBytes = Encoding.UTF8.GetBytes(json);
        UploadHandler uploadHandler = new UploadHandlerRaw(postBytes);

        using (UnityWebRequest request = UnityWebRequest.Post(urlProduct, formData))
        {
            request.uploadHandler = uploadHandler;
            request.SetRequestHeader("Content-Type", "application/json; charset=UTF-8");
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
                Debug.Log(request.error);
            else
            {
                string str = request.downloadHandler.text;
                str = str.Replace('\'', '\"');

                // преобразуем ответ в стракт
                PostStruct postFromServer = JsonUtility.FromJson<PostStruct>(str);
                // скачиваем картинку и присваиваем полям значения с ответа
                GetComponent<DownloadImage>().GetImage(postFromServer.item_image);
                nameProduct.text = postFromServer.item_name;
                priceProduct.text = postFromServer.price.ToString() + " " + postFromServer.currency_sign;
                userPanel.SetActive(true);
            }
        }
    }
}
