using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariables : MonoBehaviour
{
    public static ChessData cd;
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

            cd = ChessJsonUtility.fromJSON(jsonString);
        }
        else
        {
            UnityEngine.Debug.LogError("jsonFile is not assigned.");
        }
    }
}
