using UnityEngine;

public class GridPositions
{
    private int rows;
    private int cols;

    private Vector2[,] positions;

    public GridPositions(int rows, int cols, Vector2 size, Vector2 gridPositon)
    {
        this.rows = rows;
        this.cols = cols;

        positions = new Vector2[this.rows, this.cols];

        var started = gridPositon - new Vector2(size.x * (cols - 1) / 2, -size.y * (rows - 1) / 2);

        for (int i = 0; i < this.rows; i++)
        {
            for (int j = 0; j < this.cols; j++)
            {
                positions[i, j] = started + new Vector2(j * size.x, -i * size.y);
            }
        }
    }

    public Vector2 this[int index] => positions[index / cols, index % cols];
}