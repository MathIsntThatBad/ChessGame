using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabAndReset : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset;

    // Update is called once per frame
    void Update()
    {
        // Check for left mouse button down
        if (Input.GetMouseButtonDown(0))
        {
            // Cast a ray from the mouse position into the scene
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            // Check if the ray hits this sprite
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                // Calculate the offset between the sprite's position and the mouse position
                offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                // Start dragging
                isDragging = true;
            }
        }

        // Check for left mouse button up
        if (Input.GetMouseButtonUp(0))
        {
            // Stop dragging
            isDragging = false;
        }

        // If dragging, update sprite position to follow the mouse position with the offset
        if (isDragging)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = transform.position.z; // Ensure the same z-coordinate as the sprite
            transform.position = mousePos + offset;
        }
    }
}