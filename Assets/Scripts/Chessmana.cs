using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chessmana : MonoBehaviour
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

    public void Activate()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");

        SetCoordinates();

        switch (this.name)
        {
            case "p": this.GetComponent<SpriteRenderer>().sprite = bBauer; player = "black"; break;
            case "q": this.GetComponent<SpriteRenderer>().sprite = bDame; player = "black"; break;
            case "n": this.GetComponent<SpriteRenderer>().sprite = bSpringer; player = "black"; break;
            case "r": this.GetComponent<SpriteRenderer>().sprite = bTurm; player = "black"; break;
            case "k": this.GetComponent<SpriteRenderer>().sprite = bKoenig; player = "black"; break;
            case "b": this.GetComponent<SpriteRenderer>().sprite = bLaeufer; player = "black"; break;

            case "P": this.GetComponent<SpriteRenderer>().sprite = wBauer; player = "white"; break;
            case "Q": this.GetComponent<SpriteRenderer>().sprite = wDame; player = "white"; break;
            case "N": this.GetComponent<SpriteRenderer>().sprite = wSpringer; player = "white"; break;
            case "R": this.GetComponent<SpriteRenderer>().sprite = wTurm; player = "white"; break;
            case "K": this.GetComponent<SpriteRenderer>().sprite = wKoenig; player = "white"; break;
            case "B": this.GetComponent<SpriteRenderer>().sprite = wLaeufer; player = "white"; break;
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

    //Das passsiert wenn man so 'ne Figur anklickt
    public void InitiateMovePlates()
    {
        
        if (JSONFile != null)
        {
            string jsonString = JSONFile.text;
            if (string.IsNullOrEmpty(jsonString))
            {
                UnityEngine.Debug.LogError("JSON string is null or empty.");
                return;
            }
            /*DAS HIER MUSS UNBEDINGT EINEN NEUEN PLATZ FINDEN DAS IST ECHT NICT NICE*/
            chessData = ChessJsonUtility.fromJSON(jsonString);
        }
        else
        {
            UnityEngine.Debug.LogError("jsonFile is not assigned.");
        }
        //On click calculate the possible moves for the board
        calculatePossibelMoves(chessData.possibleMoves);
        //Check if the selected piece is listed as "possibleToMove"
        if (possibleToMove())
        {
            //Debugging
            string notation = DecodingUtil.XYToNotation(xBoard, yBoard);
            UnityEngine.Debug.LogError(notation);
            //END
            //Generate all plates 
            generateAllPlates();
        }
    }

    //AB hier wieder brauchen 
    public void PointMovePlate(int x, int y)
    {
        Game sc = controller.GetComponent<Game>();
        if (sc.PositionOnBoard(x, y))
        {
            GameObject cp = sc.GetPosition(x, y);

            if (cp == null)
            {
                MovePlateSpawn(x, y);
            }
            else if (cp.GetComponent<Chessmana>().player != player)
            {
                MovePlateAttackSpawn(x, y);
            }
        }
    }

    //Man kann beide eig combinen, hier noch n parameter mehr mit bool isTrue und dann n if(..) zum true setzen
    public void MovePlateSpawn(int matrixX, int matrixY)
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
    }

    public void MovePlateAttackSpawn(int matrixX, int matrixY)
    {
        float x = matrixX;
        float y = matrixY;

        x *= fixTShoot;
        y *= fixTShoot;

        x += fixTShoot2;
        y += fixTShoot2;

        GameObject mp = Instantiate(movePlate, new Vector3(x, y, -3.0f), Quaternion.identity);
        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.attack = true;
        mpScript.SetReference(gameObject);
        mpScript.SetCoordinates(matrixX, matrixY);
    }
    /*
     Section where the fun begins, calculate the possible moves, check if its possible to move a piece
     and the last method is to generate the patterns given a certain key
     */


    //Version die nested array verwendet 
    async public void calculatePossibelMoves(string[][]allMoves)
    {
        possibleMovesMap = new Dictionary<string, List<string>>();
        for(int i = 0; i < allMoves.Length; i++)
        {
            List<string> movesPerPiece = new List<string>();
            for(int j = 1; j < allMoves[i].Length; j++)
            {
                movesPerPiece.Add(allMoves[i][j]);
            }
            possibleMovesMap.Add(allMoves[i][0], movesPerPiece);
        }
    }

    //Version die Liste verwendet
    async public void calculatePossibelMoves(List<string> allMoves)
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

    //Mal schaun ob das ding überhaupt darf 
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
