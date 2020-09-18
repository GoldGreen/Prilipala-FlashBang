using UnityEngine;

public class SlicedGrid
{
    private int cols;

    private Vector2 size;
    private Vector2 startedPosition;

    public SlicedGrid(int cols, Vector2 size, Vector2 startedPosition)
    {
        this.cols = cols;
        this.size = size;
        this.startedPosition = startedPosition;
    }

    public Vector2 this[int index] => startedPosition + new Vector2((index % cols) * size.x, -(index / cols) * size.y);
}