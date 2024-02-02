using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
//Websocketing
using WebSocketSharp;

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
    /*private static WebSocket ws;

    public static void WebSocketConnection()
    {
        string wsUrl = "wss://synchronerealitaeten2324.informatik.uni-bremen.de:8543/chess/game1";
        //string wsUrl = "wss://94.242.206.52:8543/chess/game1";
        ws = new WebSocket(wsUrl);
        ws.Log.Level = LogLevel.Trace;
        ws.Connect();
        UnityEngine.Debug.LogError("WebSocketConnection");
        ws.OnMessage += (sender, e) =>
        {
            UnityEngine.Debug.LogError("WebSocket Server Response: " + e.Data);
            // Process the WebSocket received data
            ChessData chessData = fromJSON(e.Data);
        };

        ws.OnError += (sender, e) =>
        {
            UnityEngine.Debug.LogError("WebSocket Error: " + e.Message);
        };
        ws.OnOpen += (sender, e) =>
        {
            UnityEngine.Debug.LogError("WebSocket Opened");
        };
    }*/


    public static ChessData fromJSON(string jsonString) {
        return JsonUtility.FromJson<ChessData>(alterJSONString(jsonString)); 
    }
        

    //Use this to write the move that was taken back to a JSON
    public static void WriteTakenMoveToJSON(ChessData data, string start, string end)
    {
        data.moveSuggestion = start + " " + end;
        string json = JsonUtility.ToJson(data, true);
        string filePath = System.IO.Path.Combine(UnityEngine.Application.dataPath, "Resources", "save" + ".json");
        System.IO.File.WriteAllText(filePath, json);
    }


    public static string alterJSONString(string jsonString)
    {
        string endSubstring = "\"promoteTo\":";
        string startSubstring = "\"possibleMoves\":";
        int startIndex = jsonString.IndexOf(startSubstring);
        int endIndex = jsonString.IndexOf(endSubstring);
        string extractedSubstring = jsonString.Substring(startIndex + startSubstring.Length, endIndex - (startIndex + startSubstring.Length));
        extractedSubstring = Regex.Replace(extractedSubstring, @"\s", "")
            .Replace("[[", "[")
            .Replace("]]", "]")
            .Replace("\",\"", "")
            .Replace("],[", ",");
        //UnityEngine.Debug.LogError(extractedSubstring);
        return jsonString.Substring(0, startIndex + startSubstring.Length) + extractedSubstring + jsonString.Substring(endIndex);
    }
}