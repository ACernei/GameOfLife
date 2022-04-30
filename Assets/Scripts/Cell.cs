using UnityEditor;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public CellState State { get; private set; } = CellState.Empty;
    public CellEnvironment Environment { get; private set; } = CellEnvironment.Empty;
    public int numNeighbours;
    
    public void SetState(CellState state)
    {
        var spriteRenderer = transform.GetChild(transform.childCount - 1).GetComponent<SpriteRenderer>();
        // var spriteRenderer = GetComponent<SpriteRenderer>();
        switch (state)
        {
            case CellState.Empty:
                spriteRenderer.color = new Color(1, 1, 1, 0);
                break;
            case CellState.Black:
                spriteRenderer.color = new Color(0, 0, 0);
                break;
            case CellState.Yellow:
                spriteRenderer.color = new Color(1, 1, 0);
                break;
            case CellState.Red:
                spriteRenderer.color = new Color(1, 0, 0);
                break;
        }

        State = state;
    }
    
    public void SetEnvironment(CellEnvironment environment)
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();
        // var spriteRenderer = GetComponent<SpriteRenderer>();
        switch (environment)
        {
            case CellEnvironment.Empty:
                spriteRenderer.color = new Color(1, 1, 1, 0);
                break;
            case CellEnvironment.White:
                spriteRenderer.color = new Color(1, 1, 1);
                break;
            case CellEnvironment.Pink:
                spriteRenderer.color = new Color(1, 0, 1);
                break;
        }

        Environment = environment;
    }

    public bool IsEmpty() => State == CellState.Empty;
    public bool IsNotEmpty() => !IsEmpty();
}