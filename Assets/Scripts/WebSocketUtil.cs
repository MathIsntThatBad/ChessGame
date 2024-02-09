using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

public class WebSocketUtil : MonoBehaviour
{
    private WebSocket ws;
    /*string wsUrl = "wss://synchronerealitaeten2324.informatik.uni-bremen.de:8543/chess/game1";
    //string wsUrl = "wss://94.242.206.52:8543/chess/game1";
    ws = new WebSocket(wsUrl);
    ws.SslConfiguration.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) =>
    {
        return true;
    };
    ws.Log.Level = LogLevel.Trace;
    ws.Connect();
    UnityEngine.Debug.LogError("WebSocketConnection");
    ws.OnMessage += (sender, e) =>
    {
        UnityEngine.Debug.LogError("WebSocket Server Response: " + e.Data);
        // Process the WebSocket received data
        ChessData chessData = ChessJsonUtility.fromJSON(e.Data);
    };

    ws.OnError += (sender, e) =>
    {
        UnityEngine.Debug.LogError("WebSocket Error: " + e.Message);
    };
    ws.OnOpen += (sender, e) =>
    {
        UnityEngine.Debug.LogError("WebSocket Opened");
    };*/
}
