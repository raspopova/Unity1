using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOfLife : MonoBehaviour
{
    public static GameOfLife instance;

    public GameObject cubePrefab;
    public int gridWidth = 20;
    public int gridHeight = 20;
    public bool isPaused = true;
    public float speed = 0.5f;

    private Cube[,] grid;
    private float timer = 0f;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        CreateGrid();
        SetupCamera();
    }

    void Update()
    {
        if (!isPaused && grid != null)
        {
            timer += Time.deltaTime;
            if (timer >= 0.5f - speed * 0.45f)
            {
                timer = 0f;
                NextGeneration();
            }
        }
    }

    void CreateGrid()
    {

        grid = new Cube[gridWidth, gridHeight];

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Vector3 pos = new Vector3(x, y, 0);
                GameObject cube = Instantiate(cubePrefab, pos, Quaternion.identity);
                
                Cube cubeComponent = cube.GetComponent<Cube>();
                grid[x, y] = cubeComponent;
            }
        }
    }

    void SetupCamera()
    {
        float posX = (gridWidth - 1) * 0.5f;
        float posY = (gridHeight - 1) * 0.5f;
        Camera.main.transform.position = new Vector3(posX, posY, -10f);
        
        float size = Mathf.Max(gridWidth, gridHeight) * 0.6f;
        Camera.main.orthographicSize = size;
        
    }

    void NextGeneration()
    {
        if (grid == null) return;

        bool[,] newState = new bool[gridWidth, gridHeight];

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                if (grid[x, y] != null)
                {
                    int neighbors = CountNeighbors(x, y);
                    
                    if (grid[x, y].isAlive)
                        newState[x, y] = neighbors == 2 || neighbors == 3;
                    else
                        newState[x, y] = neighbors == 3;
                }
            }
        }

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                if (grid[x, y] != null)
                {
                    grid[x, y].isAlive = newState[x, y];
                    grid[x, y].UpdateColor();
                }
            }
        }
    }

    int CountNeighbors(int x, int y)
    {
        int count = 0;

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0) continue;

                int nx = x + i;
                int ny = y + j;

                if (nx >= 0 && nx < gridWidth && ny >= 0 && ny < gridHeight)
                {
                    if (grid[nx, ny] != null && grid[nx, ny].isAlive)
                        count++;
                }
            }
        }

        return count;
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
    }

    public void RandomGrid()
    {
        if (grid == null) return;

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                if (grid[x, y] != null)
                {
                    grid[x, y].isAlive = Random.value > 0.5f;
                    grid[x, y].UpdateColor();
                }
            }
        }
    }

    public void ClearGrid()
    {
        if (grid == null) return;

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                if (grid[x, y] != null)
                {
                    grid[x, y].isAlive = false;
                    grid[x, y].UpdateColor();
                }
            }
        }
    }

    public void ChangeSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
}
