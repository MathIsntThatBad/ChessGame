using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
/**/
using UnityEngine.Networking;
/**/
using static System.Net.Mime.MediaTypeNames;

[Serializable]
public class ChessData
{
    public string date;
    public string result;
    public string site;
    public string white;
    public string moveSuggestion;
    public List<string> possibleMoves;
    public string promoteTo;
    public string black;
    public string history;
    public string @event;
    public List<string> occurredPositions;
    public string FEN;
}
[Serializable]
public class NestedArrayWrapper
{
    public List<string> data;
}
[Serializable]
public static class ChessJsonUtility
{

    public static IEnumerator FetchDataFromServer()
    {
        string serverUrl = "wss://synchronerealitaeten2324.informatik.uni-bremen.de:8543/chess/game1";
        UnityWebRequest webRequest = UnityWebRequest.Get(serverUrl);

        // Send the request and wait for a response
        yield return webRequest.SendWebRequest();

        if (webRequest.result == UnityWebRequest.Result.Success)
        {
            // Successful response
            string responseData = webRequest.downloadHandler.text;
            UnityEngine.Debug.LogError("Server Response: " + responseData);
            ChessData chessData = fromJSON(responseData);
            //Respone data should contain the json string
        }
        else
        {
            // Error handling
            UnityEngine.Debug.LogError("Error: " + webRequest.error);
        }
    }


    public static ChessData fromJSON(string jsonString) {
        return JsonUtility.FromJson<ChessData>(jsonString); 
    }
        

    //Use this to write the move that was taken back to a JSON
    public static void WriteTakenMoveToJSON(ChessData data, string start, string end)
    {
        data.moveSuggestion = start + " " + end;
        string json = JsonUtility.ToJson(data, true);
        string filePath = System.IO.Path.Combine(UnityEngine.Application.dataPath, "Resources", "save" + ".json");
        System.IO.File.WriteAllText(filePath, json);
    }
}