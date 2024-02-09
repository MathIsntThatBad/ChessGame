using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleOnHover : MonoBehaviour
{
    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
    }

    private void OnMouseEnter()
    {
        transform.localScale = originalScale * 1.2f; // You can adjust the scaling factor here
    }

    private void OnMouseExit()
    {
        transform.localScale = originalScale;
    }
}
