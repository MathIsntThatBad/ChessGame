using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
        /*
        //string wsUrl = "wss://synchronerealitaeten2324.informatik.uni-bremen.de:8543/chess/game1";
        //string wsUrl = "wss://94.242.206.52:8543/chess/game1";
        string wsUrl = "ws://localhost:8080";
        ws = new WebSocket(wsUrl);
        ws.Log.Level = LogLevel.Trace;
        UnityEngine.Debug.LogError("WebSocketConnection");
        ws.OnMessage += (sender, e) =>
        {
            UnityEngine.Debug.LogError("WebSocket Server Response: " + e.Data);
            // Process the WebSocket received data
            ChessData blab = ChessJsonUtility.fromJSON(e.Data);
        };
        ws.OnError += (sender, e) =>
        {
            UnityEngine.Debug.LogError("WebSocket Error: " + e.Message);
        };
        ws.OnOpen += (sender, e) =>
        {
            UnityEngine.Debug.LogError("WebSocket Opened");
            ws.Send("Test lololol");
        };
        ws.Connect();
        */



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
        Chessmana cm = obj.GetComponent<Chessmana>();
        cm.name = name;
        cm.SetXBoard(x);
        cm.SetYBoard(y);
        cm.Activate();
        return obj;
    }

    public void SetPosition(GameObject obj)
    {
        Chessmana cm = obj.GetComponent<Chessmana>();
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
        if (currentPlayer == "white")
        {
            currentPlayer = "black";
        }
        else
        {
            currentPlayer = "white";
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



    //
    public void JSONToBoard()
    {
        if (jsonFile != null)
        {
            string jsonString = jsonFile.text;

            if (string.IsNullOrEmpty(jsonString))
            {
                UnityEngine.Debug.LogError("JSON string is null or empty.");
                return;
            }

            cData = ChessJsonUtility.fromJSON(jsonString);
        }
        else
        {
            UnityEngine.Debug.LogError("jsonFile is not assigned.");
        }


        /*Das hier soll testen ob es schon möglich ist den JSON-String vom Server zu erhalte*/


        //Debugging
        /*
        if (cData.possibleMoves==null) UnityEngine.Debug.LogError("Empty List");
        if (cData.possibleMoves != null)
        {
                foreach (var move in cData.possibleMoves)
                {
                    UnityEngine.Debug.LogError("Move: " + move);
                }            
        }*/
        //UnityEngine.Debug.LogError("Number of moves: " + move.moves.Count);


        string s = cData.FEN;
        string[] data = s.Split(' ');
        string[] lines = data[0].Split('/');


        //Debugging
        //UnityEngine.Debug.LogError(data[1]);
        //Declare the current actor
        if (data[1] == "w")
        {
            currentPlayer = "white";
        }
        else
        {
            currentPlayer = "black";
        }
        decPlayers(lines);
    }

    public void decPlayers(string[] lines)
    {
        int xOff = 7;
        int yOff = 7;
        for (int x = 0; x<lines.Length; x++)
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
                        playerBlack[countBlack] = Create(cChar.ToString(), y, xOff-x);
                        countBlack++;
                    }
                    if (char.IsUpper(cChar))
                    {
                        playerWhite[countWhite] = Create(cChar.ToString(), y, xOff-x);
                        countWhite++;
                    }
                    y++;
                }
            }
        }
    } 
}
