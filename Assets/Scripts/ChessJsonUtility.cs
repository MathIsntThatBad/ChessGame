using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using static System.Net.Mime.MediaTypeNames;

[Serializable]
public class Move
{
    public List<string> moves;
}

[Serializable]
public class OccurredPosition
{
    public List<List<string>> positions;
}

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
    public List<OccurredPosition> occurredPositions;
    public string FEN;
}
[Serializable]
public static class ChessJsonUtility
{
    public static ChessData fromJSON(string jsonString) => JsonUtility.FromJson<ChessData>(jsonString);

    //Use this to write the move that was taken back to a JSON
    public static void WriteTakenMoveToJSON(ChessData data, string start, string end)
    {
        data.moveSuggestion = start + " " + end;
        string json = JsonUtility.ToJson(data, true);
        string filePath = System.IO.Path.Combine(UnityEngine.Application.dataPath, "Resources", "save" + ".json");
        System.IO.File.WriteAllText(filePath, json);

        //Debug.Log("ChessData saved to: " + filePath);
    }
}
