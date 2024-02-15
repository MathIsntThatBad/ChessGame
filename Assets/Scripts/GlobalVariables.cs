using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariables : MonoBehaviour
{
    public static ChessData globalChessData;

    public static string black = "black";
    public static string white = "white";

    public TextAsset jsonFile;

    void Awake()
    {
        if (jsonFile != null)
        {
            string jsonString = jsonFile.text;

            if (string.IsNullOrEmpty(jsonString))
            {
                UnityEngine.Debug.LogError("JSON string is null or empty.");
                return;
            }

            globalChessData = ChessJsonUtility.fromJSON(jsonString);
        }
        else
        {
            UnityEngine.Debug.LogError("jsonFile is not assigned.");
        }
    }
}
