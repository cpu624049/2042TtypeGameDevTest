public class BoardModel
{
    public int Width { get; private set; }
    public int Height { get; private set; }

    private int[,] cells;

    public BoardModel(int width, int height)
    {
        Width = width;
        Height = height;
        cells = new int[width, height];
    }

    public void Clear()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                cells[x, y] = 0;
            }
        }
    }

    public int GetCell(int x, int y)
    {
        return cells[x, y];
    }

    public void SetCell(int x, int y, int value)
    {
        cells[x, y] = value;
    }

    public bool IsEmpty(int x, int y)
    {
        return cells[x, y] == 0;
    }
}