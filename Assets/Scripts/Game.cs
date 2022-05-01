using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

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

    private void NewCells()
    {
        for (var y = 0; y < GRID_HEIGHT; y++)
        {
            for (var x = 0; x < GRID_WIDTH; x++)
            {
                Cell cell = grid[x, y];
                cell.SetState(RandomAliveCell());
                cell.SetEnvironment(CellEnvironment.Empty);
            }
        }
    }

    private void CountNeighbours()
    {
        for (var y = 0; y < GRID_HEIGHT; y++)
        {
            for (var x = 0; x < GRID_WIDTH; x++)
            {
                var neighbours = new List<Cell>();
                grid[x, y].numNeighbours = 0;

                // Top
                if (y + 1 < GRID_HEIGHT && grid[x, y + 1].IsNotEmpty())
                {
                    neighbours.Add(grid[x, y + 1]);
                }

                // Right
                if (x + 1 < GRID_WIDTH && grid[x + 1, y].IsNotEmpty())
                {
                    neighbours.Add(grid[x + 1, y]);
                }

                // Bot
                if (y - 1 >= 0 && grid[x, y - 1].IsNotEmpty())
                {
                    neighbours.Add(grid[x, y - 1]);
                }

                // Left
                if (x - 1 >= 0 && grid[x - 1, y].IsNotEmpty())
                {
                    neighbours.Add(grid[x - 1, y]);
                }

                // Top-Right
                if (x + 1 < GRID_WIDTH && y + 1 < GRID_HEIGHT && grid[x + 1, y + 1].IsNotEmpty())
                {
                    neighbours.Add(grid[x + 1, y + 1]);
                }

                // Top-Left
                if (x - 1 >= 0 && y + 1 < GRID_HEIGHT && grid[x - 1, y + 1].IsNotEmpty())
                {
                    neighbours.Add(grid[x - 1, y + 1]);
                }

                // Bot-Right
                if (x + 1 < GRID_WIDTH && y - 1 >= 0 && grid[x + 1, y - 1].IsNotEmpty())
                {
                    neighbours.Add(grid[x + 1, y - 1]);
                }

                // Bot-Left
                if (x - 1 >= 0 && y - 1 >= 0 && grid[x - 1, y - 1].IsNotEmpty())
                {
                    neighbours.Add(grid[x - 1, y - 1]);
                }

                ProcessNeigbours(grid[x, y], neighbours);
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
                        ProcessEnvironment(grid[x, y]);
                    }
                }
                else
                {
                    if (grid[x, y].numNeighbours == 3)
                    {
                        grid[x, y].SetState(CellState.Black);
                        ProcessEnvironment(grid[x, y]);
                    }
                }
            }
        }
    }

    private void ProcessNeigbours(Cell currentCell, List<Cell> neighbours)
    {
        currentCell.numNeighbours = neighbours.Count;

        switch (currentCell.State)
        {
            case CellState.Empty:
                break;
            case CellState.Black:
                if (neighbours.Any(cell => cell.State == CellState.Red) && neighbours.Count <= 2)
                    currentCell.SetState(CellState.Red);
                else if (neighbours.Any(cell => cell.State == CellState.Yellow) && neighbours.Count >= 2)
                    currentCell.SetState(CellState.Yellow);
                break;
            case CellState.Yellow:
                var countYellow = neighbours.Count(cell => cell.State == CellState.Yellow);
                var countRed = neighbours.Count(cell => cell.State == CellState.Red);
                if (countYellow > countRed && countRed != 0)
                {
                    currentCell.SetState(CellState.Yellow);
                }
                else if (countRed >= countYellow && countYellow != 0)
                {
                    currentCell.SetState(CellState.Red);
                }

                break;
            case CellState.Red:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void ProcessEnvironment(Cell cell)
    {
        if (cell.Environment == CellEnvironment.Pink && cell.IsNotEmpty())
        {
            cell.SetState(CellState.Red);
        }

        if (cell.Environment == CellEnvironment.White && cell.IsNotEmpty())
        {
            cell.SetState(CellState.Yellow);
        }
        
        if (cell.Environment == CellEnvironment.Green && cell.IsNotEmpty())
        {
            cell.SetState(CellState.Black);
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

        if (Input.GetKeyUp(KeyCode.R))
        {
            NewCells();
            simulationEnabled = false;
        }
    }
}