using Unity.VisualScripting;
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
                spriteRenderer.color = new Color(1, 1, 1);
                break;
            case CellState.Yellow:
                spriteRenderer.color = new Color(0.6f, 0.65f, 0.55f);
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
                spriteRenderer.color = new Color(0.07f, 0.21f, 0.14f);
                
                break;
            case CellEnvironment.Pink:
                spriteRenderer.color = new Color(0.37f, 0.01f, 0.12f);
                break;
            case CellEnvironment.Green:
                spriteRenderer.color = new Color(0.91f, 0.6f, 0.37f);
                break;
        }

        Environment = environment;
    }

    public bool IsEmpty() => State == CellState.Empty;
    public bool IsNotEmpty() => !IsEmpty();
}