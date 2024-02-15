using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessManger : MonoBehaviour
{
    //JSON ACCESS
    private ChessData chessData;
    public TextAsset JSONFile;
    public Dictionary<string, List<string>> possibleMovesMap;


    public GameObject controller;
    public GameObject movePlate;

    private int xBoard = -1; //-1
    private int yBoard = -1; //-1

    private string player;

    public Sprite bBauer, bDame, bSpringer, bTurm, bKoenig, bLaeufer;
    public Sprite wBauer, wDame, wSpringer, wTurm, wKoenig, wLaeufer;

    private float fixTShoot = 1.1f; //0.66
    private float fixTShoot2 = (1.1f / 0.66f) * (-2.3f); //-2.3f

    //Map the names to the right chess pieces
    public void Activate()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");

        /*New Workflow with GlobalVariables*/
        chessData = GlobalVariables.globalChessData;


        SetCoordinates();

        switch (this.name)
        {
            case "p": this.GetComponent<SpriteRenderer>().sprite = bBauer; player = GlobalVariables.black; break;
            case "q": this.GetComponent<SpriteRenderer>().sprite = bDame; player = GlobalVariables.black; break;
            case "n": this.GetComponent<SpriteRenderer>().sprite = bSpringer; player = GlobalVariables.black; break;
            case "r": this.GetComponent<SpriteRenderer>().sprite = bTurm; player = GlobalVariables.black; break;
            case "k": this.GetComponent<SpriteRenderer>().sprite = bKoenig; player = GlobalVariables.black; break;
            case "b": this.GetComponent<SpriteRenderer>().sprite = bLaeufer; player = GlobalVariables.black; break;

            case "P": this.GetComponent<SpriteRenderer>().sprite = wBauer; player = GlobalVariables.white; break;
            case "Q": this.GetComponent<SpriteRenderer>().sprite = wDame; player = GlobalVariables.white; break;
            case "N": this.GetComponent<SpriteRenderer>().sprite = wSpringer; player = GlobalVariables.white; break;
            case "R": this.GetComponent<SpriteRenderer>().sprite = wTurm; player = GlobalVariables.white; break;
            case "K": this.GetComponent<SpriteRenderer>().sprite = wKoenig; player = GlobalVariables.white; break;
            case "B": this.GetComponent<SpriteRenderer>().sprite = wLaeufer; player = GlobalVariables.white; break;
        }
    }

    public void SetCoordinates()
    {
        float x = xBoard;
        float y = yBoard;

        x *= fixTShoot;
        y *= fixTShoot;

        x += fixTShoot2;
        y += fixTShoot2;

        this.transform.position = new Vector3(x, y, -1.0f);
    }

    public int GetXBoard()
    {
        return xBoard;
    }

    public int GetYBoard()
    {
        return yBoard;
    }

    public void SetXBoard(int x)
    {
        xBoard = x;
    }

    public void SetYBoard(int y)
    {
        yBoard = y;
    }
    //On click remove/initiate old/new plates
    private void OnMouseUp()
    {
        if (!controller.GetComponent<Game>().IsGameOver() && controller.GetComponent<Game>().GetCurrentPlayer() == player)
        {
            DestroyMovePlates();

            InitiateMovePlates();
        }
    }

    public void DestroyMovePlates()
    {
        GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate");
        for (int i = 0; i < movePlates.Length; i++)
        {
            Destroy(movePlates[i]);
        }
    }

    //Code that runs when a piece is selected
    public void InitiateMovePlates()
    {
        //On click calculate the possible moves for the board
        calculatePossibelMoves(chessData.possibleMoves);
        //Check if the selected piece is listed as "possibleToMove"
        if (possibleToMove())
        {
            //Debugging
            string notation = DecodingUtil.XYToNotation(xBoard, yBoard);
            UnityEngine.Debug.LogError(notation);
            generateAllPlates();
        }
    }

    //Decide wether the piece attacks or not 
    public void PointMovePlate(int x, int y)
    {
        Game sc = controller.GetComponent<Game>();
        if (sc.PositionOnBoard(x, y))
        {
            GameObject cp = sc.GetPosition(x, y);

            if (cp == null)
            {
                MovePlateAttackSpawn(x, y,false);
            }
            else if (cp.GetComponent<ChessManger>().player != player)
            {
                MovePlateAttackSpawn(x, y,true);
            }
        }
    }

    //Man kann beide eig combinen, hier noch n parameter mehr mit bool isTrue und dann n if(..) zum true setzen
    /*public void MovePlateSpawn(int matrixX, int matrixY)
    {
        float x = matrixX;
        float y = matrixY;

        x *= fixTShoot;
        y *= fixTShoot;

        x += fixTShoot2;
        y += fixTShoot2;

        GameObject mp = Instantiate(movePlate, new Vector3(x, y, -3.0f), Quaternion.identity);
        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.SetReference(gameObject);
        mpScript.SetCoordinates(matrixX, matrixY);
    }*/

    //Dont delete above yet, first check for functionality
    public void MovePlateAttackSpawn(int matrixX, int matrixY, bool isTrue)
    {
        float x = matrixX;
        float y = matrixY;

        x *= fixTShoot;
        y *= fixTShoot;

        x += fixTShoot2;
        y += fixTShoot2;

        GameObject mp = Instantiate(movePlate, new Vector3(x, y, -3.0f), Quaternion.identity);
        MovePlate mpScript = mp.GetComponent<MovePlate>();
        if(isTrue) mpScript.attack = true;
        mpScript.SetReference(gameObject);
        mpScript.SetCoordinates(matrixX, matrixY);
    }


    /*
     Section where the fun begins, calculate the possible moves, check if its possible to move a piece
     and the last method is to generate the patterns given a certain key
     */

    //Maps the possible moves to the given piece in a dictionary
    public void calculatePossibelMoves(List<string> allMoves)
    {
        possibleMovesMap = new Dictionary<string, List<string>>();
        foreach (var str in allMoves)
        {
            List<string> lineOfMoves = DecodingUtil.splitIntoPieces(str);
            string first = lineOfMoves[0];
            //UnityEngine.Debug.LogError(first);
            lineOfMoves.RemoveAt(0);
            possibleMovesMap.Add(first, lineOfMoves);
        }
    }

    //Check if the clicked piece can take a move
    public bool possibleToMove()
    {
        string selectedPiecePosition = DecodingUtil.XYToNotation(xBoard, yBoard);
        if (possibleMovesMap.TryGetValue(selectedPiecePosition, out List<string> moves))
        {
            string result = string.Join(", ", moves);
        }
        return possibleMovesMap.ContainsKey(selectedPiecePosition);
    }

    //Get the current key out of the map and generate plates for all entries
    public void generateAllPlates()
    {
        string selectedPiecePosition = DecodingUtil.XYToNotation(xBoard, yBoard);
        if(possibleMovesMap.TryGetValue(selectedPiecePosition, out List<string> moves))
        {
            string result = string.Join(", ", moves);
            //UnityEngine.Debug.LogError("Selected " + this.name + " moves: " + result);
            foreach (var move in moves)
            {
                Tuple<int, int> xyCoordinate = DecodingUtil.NotationToXY(move);
                int x = xyCoordinate.Item1;
                int y = xyCoordinate.Item2;
                PointMovePlate(xBoard+(x-xBoard),yBoard+(y-yBoard));
            }
        }
    }
}
