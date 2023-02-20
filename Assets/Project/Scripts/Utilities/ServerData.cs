using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

public class ServerData : MonoBehaviour
{
    [System.Serializable]
    public struct Data
    {
        public int gridAmount;
        public int piceAmount;
    }

    public readonly string jsonURL = "https://drive.google.com/uc?export=download&id=1sdoKLjLvLtvLYAurrl-3hIUqxFYRZoBW";
    public static List<Data> serverListData = new List<Data>();

    public static bool serverIsResponded = false;
    IEnumerator Start()
    {
        yield return GetData(jsonURL);
    }
    IEnumerator GetData(string url)
    {
        if (string.IsNullOrEmpty(url))
        {
            serverIsResponded = true;
            yield break;
        }

        if (GameManager.Instance.overrideDefaultLevelData)
        {
            serverIsResponded = true;
            yield break;
        }

        if (serverIsResponded)
        {
            yield break;
        }

        UnityWebRequest request = UnityWebRequest.Get(url);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            // error ...
            serverIsResponded = true;
            Debug.Log("Server Error");
            yield break;
        }
        else
        {
            // success...
            Debug.Log(request.downloadHandler.text);
            serverListData = JsonConvert.DeserializeObject<List<Data>>(request.downloadHandler.text);
            Debug.Log("Server Status Done!");
            serverIsResponded = true;
        }

        // Clean up any resources it is using.
        request.Dispose();
    }
}
