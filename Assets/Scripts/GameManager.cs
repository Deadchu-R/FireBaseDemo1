using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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
    private string fireBaseText;
    private List<string> fireBaseList;
    

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
        Debug.Log("LoginStringIs: " + fireBaseURL);
       GetData();
           //wait for data to be retrieved from firebase before continuing to next line of code
         fireBaseList = new List<string>(fireBaseText.Split(','));
           //FIREBASE URE: https://firestore.googleapis.com/v1/projects/testingoftiltan/databases/(default)/documents/Players
           //Split the string into a list of documents (id's)
           //foreach (string document in fireBaseList)
           // check if password and username match
           // if they do, then load the game
    
      
        loginCanvas.SetActive(false);
        userInterfaceCanvas.SetActive(true);


    }
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
                    Debug.Log(  ":\nReceived: " + webRequest.downloadHandler.text);
                    fireBaseText = webRequest.downloadHandler.text.ToString();
                    break;
            }
        }
    }
}

