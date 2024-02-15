using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using WebSocketSharp;


public class Game : MonoBehaviour
{
    ChessData cData;
    public TextAsset jsonFile;
    public GameObject chesspiece;

    //Poisitions and team for each chesspiece
    private GameObject[,] positions = new GameObject[8, 8];
    private GameObject[] playerBlack = new GameObject[16];
    private GameObject[] playerWhite = new GameObject[16];

    private string currentPlayer = "white";

    private bool gameOver = false;
    private int countBlack = 0;
    private int countWhite = 0;

    private WebSocket ws;


    // Start is called before the first frame update
    void Start()
    {
        //string wsUrl = "wss://synchronerealitaeten2324.informatik.uni-bremen.de:8543/chess/game1";
        string wsUrl = "ws://localhost:8080/entrance";
        ws = new WebSocket(wsUrl);
        ws.Log.Level = LogLevel.Trace;
        ws.Connect();
        UnityEngine.Debug.LogError("WebSocketConnection");
        ws.OnMessage += (sender, e) =>
        {
            UnityEngine.Debug.Log("WebSocket Server Response: " + e.Data);
            // Process the WebSocket received data
            //ChessData chessData = ChessJsonUtility.fromJSON(e.Data);
        };

        ws.OnError += (sender, e) =>
        {
            UnityEngine.Debug.Log("WebSocket Error: " + e.Message);
        };
        ws.OnOpen += (sender, e) =>
        {
            UnityEngine.Debug.Log("WebSocket Opened");
        };


        /*New Workflow with GlobalVariables*/
        cData = GlobalVariables.globalChessData;

        JSONToBoard();
        //Set all piece positions on the position board
        for (int i = 0; i < countWhite; i++)
        {
            SetPosition(playerWhite[i]);
        }

        for (int i = 0; i < countBlack; i++)
        {
            SetPosition(playerBlack[i]);
        }
    }

    public GameObject Create(string name, int x, int y)
    {
        GameObject obj = Instantiate(chesspiece, new Vector3(0, 0, -1), Quaternion.identity);
        ChessManger cm = obj.GetComponent<ChessManger>();
        cm.name = name;
        cm.SetXBoard(x);
        cm.SetYBoard(y);
        cm.Activate();
        return obj;
    }

    public void SetPosition(GameObject obj)
    {
        ChessManger cm = obj.GetComponent<ChessManger>();
        positions[cm.GetXBoard(), cm.GetYBoard()] = obj;
    }

    public void SetPositionEmpty(int x, int y)
    {
        positions[x, y] = null;
    }

    public GameObject GetPosition(int x, int y)
    {
        return positions[x, y];
    }

    public bool PositionOnBoard(int x, int y)
    {
        if (x < 0 || y < 0 || x >= positions.GetLength(0) || y >= positions.GetLength(1)) return false;
        return true;
    }

    public string GetCurrentPlayer()
    {
        return currentPlayer;
    }

    public bool IsGameOver()
    {
        return gameOver;
    }

    public void NextTurn()
    {
        if (currentPlayer == GlobalVariables.white)
        {
            currentPlayer = GlobalVariables.black;
        }
        else
        {
            currentPlayer = GlobalVariables.white;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver == true && Input.GetMouseButtonDown(0))
        {
            gameOver = false;
            SceneManager.LoadScene("Game");
        }
    }

    public void Winner(string playerWinner)
    {
        gameOver = true;

        GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().enabled = true;
        GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().text = playerWinner + " is the winner!";
        GameObject.FindGameObjectWithTag("RestartText").GetComponent<Text>().enabled = true;
    }

    
    public void JSONToBoard()
    {
        string s = cData.FEN;
        string[] data = s.Split(' ');
        string[] lines = data[0].Split('/');

        if (data[1] == "w")
        {
            currentPlayer = GlobalVariables.white;
        }
        else
        {
            currentPlayer = GlobalVariables.black;
        }

        decPlayers(lines);
    }

    //Iterate FEN and decleare the whole board read from the JSON-String
    public void decPlayers(string[] lines)
    {
        int xOff = 7;
        int yOff = 7;
        for (int x = 0; x < lines.Length; x++)
        {
            int y = 0;
            for (int j = 0; j < lines[x].Length; j++)
            {
                char cChar = lines[x][j];
                if (char.IsNumber(cChar))
                {
                    int val = Convert.ToInt32(cChar.ToString());
                    y += val;
                }
                else
                {
                    if (char.IsLower(cChar))
                    {
                        playerBlack[countBlack] = Create(cChar.ToString(), y, xOff - x);
                        countBlack++;
                    }
                    if (char.IsUpper(cChar))
                    {
                        playerWhite[countWhite] = Create(cChar.ToString(), y, xOff - x);
                        countWhite++;
                    }
                    y++;
                }
            }
        }
    }
}
