using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;

public class AnalyticsManager : MonoBehaviour
{
    private const bool IS_A_BUILD = false;
    private const string SERVER_URL = "http://44.202.60.121/upload_data";

    public static IEnumerator PostMethod(string jsonData)
    {
        if (IS_A_BUILD)
        {
            using (UnityWebRequest request = UnityWebRequest.Put(SERVER_URL, jsonData))
            {
                request.method = UnityWebRequest.kHttpVerbPOST;
                request.SetRequestHeader("Content-Type", "application/json");
                request.SetRequestHeader("Accept", "application/json");

                yield return request.SendWebRequest();

                if (!request.isNetworkError && request.responseCode == (int)HttpStatusCode.OK)
                    Debug.Log("Data successfully sent to the server");
                else
                    Debug.Log("Error sending data to the server: Error " + request.responseCode);
            }
        }
        else
        {
            Debug.Log("Mock sent the following: " + jsonData);
        }
    }
}
