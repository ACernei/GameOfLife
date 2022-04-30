using UnityEngine;
using Debug = UnityEngine.Debug;

public class Game : MonoBehaviour
{
    private const int GRID_WIDTH = 22;
    private const int GRID_HEIGHT = 12;
    private const int GRID_SIZE = 16;
    private float timer;
    private Cell[,] grid;

    public Cell cellPrefab;
    public float speed = 0.1f;
    public bool simulationEnabled;

    private void Start()
    {
        grid = new Cell[GRID_WIDTH, GRID_HEIGHT];
        PlaceCells();
    }

    private void Update()
    {
        if (simulationEnabled)
        {
            if (timer >= speed)
            {
                timer = 0f;
                CountNeighbours();
                PopulationControl();
            }
            else
            {
                timer += Time.deltaTime;
            }
        }

        UserInput();
    }

    private void PlaceCells()
    {
        for (var y = 0; y < GRID_HEIGHT; y++)
        {
            for (var x = 0; x < GRID_WIDTH; x++)
            {
                var cell = Instantiate(cellPrefab, transform);
                cell.transform.position = new Vector2(x * GRID_SIZE, y * GRID_SIZE);
                cell.SetState(RandomAliveCell());
                cell.SetEnvironment(CellEnvironment.Empty);
                grid[x, y] = cell;
            }
        }
    }

    private void CountNeighbours()
    {
        for (var y = 0; y < GRID_HEIGHT; y++)
        {
            for (var x = 0; x < GRID_WIDTH; x++)
            {
                var numNeighbours = 0;
                // Top
                if (y + 1 < GRID_HEIGHT && grid[x, y + 1].IsNotEmpty())
                {
                    numNeighbours++;
                }

                // Right
                if (x + 1 < GRID_WIDTH && grid[x + 1, y].IsNotEmpty())
                {
                    numNeighbours++;
                }

                // Bot
                if (y - 1 >= 0 && grid[x, y - 1].IsNotEmpty())
                {
                    numNeighbours++;
                }

                // Left
                if (x - 1 >= 0 && grid[x - 1, y].IsNotEmpty())
                {
                    numNeighbours++;
                }

                // Top-Right
                if (x + 1 < GRID_WIDTH && y + 1 < GRID_HEIGHT && grid[x + 1, y + 1].IsNotEmpty())
                {
                    numNeighbours++;
                }

                // Top-Left
                if (x - 1 >= 0 && y + 1 < GRID_HEIGHT && grid[x - 1, y + 1].IsNotEmpty())
                {
                    numNeighbours++;
                }

                // Bot-Right
                if (x + 1 < GRID_WIDTH && y - 1 >= 0 && grid[x + 1, y - 1].IsNotEmpty())
                {
                    numNeighbours++;
                }

                // Bot-Left
                if (x - 1 >= 0 && y - 1 >= 0 && grid[x - 1, y - 1].IsNotEmpty())
                {
                    numNeighbours++;
                }

                grid[x, y].numNeighbours = numNeighbours;
            }
        }
    }

    private void PopulationControl()
    {
        for (var y = 0; y < GRID_HEIGHT; y++)
        {
            for (var x = 0; x < GRID_WIDTH; x++)
            {
                if (grid[x, y].IsNotEmpty())
                {
                    if (grid[x, y].numNeighbours != 2 && grid[x, y].numNeighbours != 3)
                    {
                        grid[x, y].SetState(CellState.Empty);
                    }
                }
                else
                {
                    if (grid[x, y].numNeighbours == 3)
                    {
                        grid[x, y].SetState(CellState.Black);
                    }
                }
            }
        }
    }

    private static CellState RandomAliveCell()
    {
        var rand = Random.Range(0, 100);

        if (rand > 75)
            return CellState.Black;

        return CellState.Empty;
    }

    private void UserInput()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            Vector2 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            int x = Mathf.RoundToInt(mousePoint.x / GRID_SIZE);
            int y = Mathf.RoundToInt(mousePoint.y / GRID_SIZE);
            Debug.Log($"{x}({Mathf.RoundToInt(mousePoint.x)}):{y}({Mathf.RoundToInt(mousePoint.y)})");
            if (x >= 0 && y >= 0 && x < GRID_WIDTH && y < GRID_HEIGHT)
            {
                if (Input.GetMouseButtonDown(0))
                    grid[x, y].SetState(grid[x, y].State.Next());
                else if (Input.GetMouseButtonDown(1))
                    grid[x, y].SetEnvironment(grid[x, y].Environment.Next());
            }
        }

        if (Input.GetKeyUp(KeyCode.P))
        {
            simulationEnabled = !simulationEnabled;
        }
    }
}