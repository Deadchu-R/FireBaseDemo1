using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject loginCanvas;
    [SerializeField] private GameObject userInterfaceCanvas;
    [SerializeField] private Button loginButton;
    [SerializeField] private TMP_InputField usernameInputField;
    [SerializeField] private TMP_InputField passwordInputField;
    [SerializeField] private string fireBaseURL;
    private void Awake()
    {
        loginCanvas.SetActive(true);
        userInterfaceCanvas.SetActive(false);
        loginButton.interactable = false;
       
    }

    private void Update()
    {
        if (usernameInputField.text.Length > 0 && passwordInputField.text.Length > 0 && usernameInputField.text != "Username" && passwordInputField.text != "Password")
        {
            loginButton.interactable = true;
        }
        else
        {
            loginButton.interactable = false;
        }
    }
    public void Login()
    {
       GetData();
    }
    [ContextMenu("Get Data")]
    private void GetData()
    {
         StartCoroutine(GetRequest(fireBaseURL));
    }
    public  IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            Debug.Log("<color=green>sended WEB requesrt--></color>");

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(  ":  Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError( ":HTTP HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(webRequest.downloadHandler.text);
                    LoginValidation(webRequest);
                    break;
            }
        }
    }
    void LoginValidation(UnityWebRequest webRequest) 
    {


            DocumentWrapper playersData = JsonUtility.FromJson<DocumentWrapper>(webRequest.downloadHandler.text);
        foreach (Document player in playersData.documents)
        {
            if (player.fields.UserName.stringValue.Equals(usernameInputField.text) && player.fields.Password.stringValue.Equals(passwordInputField.text))
            {
                Debug.Log("Loged In Successfully");
                loginCanvas.SetActive(false);
                userInterfaceCanvas.SetActive(true);
            }
        }
    }

}

[System.Serializable]
public class DocumentWrapper
{
    public Document[] documents;

    public DocumentWrapper(Document[] documents)
    {
        this.documents = documents;
    }
}

[System.Serializable]
public class Document
{
    public string name;
    public Fields fields;
    public string createTime;
    public string updateTime;

    public Document(string name, Fields fields, string createTime,string updateTime)
    {
        this.name = name;
        this.fields = fields;
        this.createTime = createTime;
        this.updateTime = updateTime;
    }
}

[System.Serializable]
public class Fields
{
    public Password Password;
    public UserName UserName;
}

[System.Serializable]
public class Password
{
    public string stringValue;
}

[System.Serializable]
public class UserName
{
    public string stringValue;
}








