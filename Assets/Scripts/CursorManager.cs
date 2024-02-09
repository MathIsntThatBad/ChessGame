using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public Texture2D cursorTexture; // Assign your custom cursor texture in the Unity Editor

    void Start()
    {
        // Set the cursor to the custom texture
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
    }
}
