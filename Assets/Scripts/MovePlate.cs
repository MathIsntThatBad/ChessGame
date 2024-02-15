using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlate : MonoBehaviour
{
    public GameObject controller;

    GameObject reference = null;

    int matrixX;
    int matrixY;

    public bool attack = false;

    // Start is called before the first frame update
    public void Start()
    {
        if (attack)
        {
            //Change to red
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        }
    }

    public void OnMouseUp()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");

        if (attack)
        {
            GameObject cp = controller.GetComponent<Game>().GetPosition(matrixX, matrixY);

            if (cp.name == "wKoenig") controller.GetComponent<Game>().Winner(GlobalVariables.black);
            if (cp.name == "bKoenig") controller.GetComponent<Game>().Winner(GlobalVariables.white);

            Destroy(cp);
        }

        controller.GetComponent<Game>().SetPositionEmpty(reference.GetComponent<ChessManger>().GetXBoard(), reference.GetComponent<ChessManger>().GetYBoard());

        reference.GetComponent<ChessManger>().SetXBoard(matrixX);
        reference.GetComponent<ChessManger>().SetYBoard(matrixY);
        reference.GetComponent<ChessManger>().SetCoordinates();

        controller.GetComponent<Game>().SetPosition(reference);

        controller.GetComponent<Game>().NextTurn();

        reference.GetComponent<ChessManger>().DestroyMovePlates();
    }

    public void SetCoordinates(int x, int y)
    {
        matrixX = x;
        matrixY = y;
    }

    public void SetReference(GameObject obj)
    {
        reference = obj;
    }

    public GameObject GetReference()
    {
        return reference;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
