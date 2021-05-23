using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

/// <summary>
/// A class that sends user data to the server to purchase a product
/// </summary>
public class SendBuy : MonoBehaviour
{
    [SerializeField]
    private string urlUserBuy = "https://6u3td6zfza.execute-api.us-east-2.amazonaws.com/prod/v1/gcom/action";

    public Text userEmail, userCard, userExpirationDate, requestText;

    public void PostRequest()
    {
        StartCoroutine(PostData());
    }

    private IEnumerator PostData()
    {
        WWWForm formData = new WWWForm();

        UserStruct post = new UserStruct()
        {
            email = userEmail.text,
            cardNumber = userCard.text,
            expirationDate = userExpirationDate.text
        };
        string json = JsonUtility.ToJson(post);
        byte[] postBytes = Encoding.UTF8.GetBytes(json);
        UploadHandler uploadHandler = new UploadHandlerRaw(postBytes);

        using (UnityWebRequest request = UnityWebRequest.Post(urlUserBuy, formData))
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
                SendBuyStruct postFromServer = JsonUtility.FromJson<SendBuyStruct>(str);

                requestText.text = postFromServer.user_message;
                requestText.gameObject.SetActive(true);
            }
        }
    }
}
