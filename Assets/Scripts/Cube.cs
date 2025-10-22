using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public bool isAlive = false;
    private Renderer cubeRenderer;

    void Start()
    {
        cubeRenderer = GetComponent<Renderer>();
        UpdateColor();
    }

    public void UpdateColor()
    {
        cubeRenderer.material.color = isAlive ? Color.white : Color.black;
    }

    void OnMouseDown()
    {
        if (GameOfLife.instance.isPaused)
        {
            isAlive = !isAlive;
            UpdateColor();
        }
    }
}
