using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [Header("Board Settings")]
    [SerializeField] private int width = 4;
    [SerializeField] private int height = 4;

    private BoardModel boardModel;

    private void Awake()
    {
        boardModel = new BoardModel(width, height);
    }

    public void InitializeBoard()
    {
        boardModel.Clear();

        // 테스트용 초기 배치
        SetTestBoard();

        Debug.Log("BoardManager.InitializeBoard()");
        PrintBoard();
    }

    public void HandleMove(SwipeDirection direction)
    {
        Debug.Log($"BoardManager.HandleMove() direction = {direction}");

        bool moved = false;

        switch (direction)
        {
            case SwipeDirection.Left:
                moved = MoveLeft();
                break;

            case SwipeDirection.Right:
                moved = MoveRight();
                break;

            case SwipeDirection.Up:
                moved = MoveUp();
                break;

            case SwipeDirection.Down:
                moved = MoveDown();
                break;
        }

        if (moved)
        {
            Debug.Log("보드 변경 발생");
            PrintBoard();
        }
        else
        {
            Debug.Log("보드 변경 없음");
        }
    }

    private bool MoveLeft()
    {
        bool boardChanged = false;

        for (int y = 0; y < height; y++)
        {
            List<int> originalLine = GetRowLeftToRight(y);
            List<int> processedLine = ProcessLine(originalLine);

            if (!AreLinesEqual(originalLine, processedLine))
            {
                boardChanged = true;
            }

            SetRowLeftToRight(y, processedLine);
        }

        return boardChanged;
    }

    private bool MoveRight()
    {
        bool boardChanged = false;

        for (int y = 0; y < height; y++)
        {
            List<int> originalLine = GetRowRightToLeft(y);
            List<int> processedLine = ProcessLine(originalLine);

            if (!AreLinesEqual(originalLine, processedLine))
            {
                boardChanged = true;
            }

            SetRowRightToLeft(y, processedLine);
        }

        return boardChanged;
    }

    private bool MoveUp()
    {
        bool boardChanged = false;

        for (int x = 0; x < width; x++)
        {
            List<int> originalLine = GetColumnTopToBottom(x);
            List<int> processedLine = ProcessLine(originalLine);

            if (!AreLinesEqual(originalLine, processedLine))
            {
                boardChanged = true;
            }

            SetColumnTopToBottom(x, processedLine);
        }

        return boardChanged;
    }

    private bool MoveDown()
    {
        bool boardChanged = false;

        for (int x = 0; x < width; x++)
        {
            List<int> originalLine = GetColumnBottomToTop(x);
            List<int> processedLine = ProcessLine(originalLine);

            if (!AreLinesEqual(originalLine, processedLine))
            {
                boardChanged = true;
            }

            SetColumnBottomToTop(x, processedLine);
        }

        return boardChanged;
    }

    private List<int> ProcessLine(List<int> line)
    {
        List<int> compressed = Compress(line);
        List<int> merged = Merge(compressed);
        List<int> finalLine = Compress(merged);

        while (finalLine.Count < line.Count)
        {
            finalLine.Add(0);
        }

        return finalLine;
    }

    private List<int> Compress(List<int> line)
    {
        List<int> result = new List<int>();

        for (int i = 0; i < line.Count; i++)
        {
            if (line[i] != 0)
            {
                result.Add(line[i]);
            }
        }

        return result;
    }

    private List<int> Merge(List<int> line)
    {
        List<int> result = new List<int>();

        int i = 0;
        while (i < line.Count)
        {
            if (i < line.Count - 1 && line[i] == line[i + 1])
            {
                result.Add(line[i] * 2);
                i += 2;
            }
            else
            {
                result.Add(line[i]);
                i += 1;
            }
        }

        return result;
    }

    private List<int> GetRowLeftToRight(int y)
    {
        List<int> row = new List<int>();

        for (int x = 0; x < width; x++)
        {
            row.Add(boardModel.GetCell(x, y));
        }

        return row;
    }

    private void SetRowLeftToRight(int y, List<int> values)
    {
        for (int x = 0; x < width; x++)
        {
            boardModel.SetCell(x, y, values[x]);
        }
    }

    private List<int> GetRowRightToLeft(int y)
    {
        List<int> row = new List<int>();

        for (int x = width - 1; x >= 0; x--)
        {
            row.Add(boardModel.GetCell(x, y));
        }

        return row;
    }

    private void SetRowRightToLeft(int y, List<int> values)
    {
        int index = 0;

        for (int x = width - 1; x >= 0; x--)
        {
            boardModel.SetCell(x, y, values[index]);
            index++;
        }
    }

    private List<int> GetColumnTopToBottom(int x)
    {
        List<int> column = new List<int>();

        for (int y = height - 1; y >= 0; y--)
        {
            column.Add(boardModel.GetCell(x, y));
        }

        return column;
    }

    private void SetColumnTopToBottom(int x, List<int> values)
    {
        int index = 0;

        for (int y = height - 1; y >= 0; y--)
        {
            boardModel.SetCell(x, y, values[index]);
            index++;
        }
    }

    private List<int> GetColumnBottomToTop(int x)
    {
        List<int> column = new List<int>();

        for (int y = 0; y < height; y++)
        {
            column.Add(boardModel.GetCell(x, y));
        }

        return column;
    }

    private void SetColumnBottomToTop(int x, List<int> values)
    {
        int index = 0;

        for (int y = 0; y < height; y++)
        {
            boardModel.SetCell(x, y, values[index]);
            index++;
        }
    }

    private bool AreLinesEqual(List<int> a, List<int> b)
    {
        if (a.Count != b.Count)
            return false;

        for (int i = 0; i < a.Count; i++)
        {
            if (a[i] != b[i])
                return false;
        }

        return true;
    }

    private void SetTestBoard()
    {
        // y = 3
        boardModel.SetCell(0, 3, 2);
        boardModel.SetCell(1, 3, 0);
        boardModel.SetCell(2, 3, 2);
        boardModel.SetCell(3, 3, 4);

        // y = 2
        boardModel.SetCell(0, 2, 2);
        boardModel.SetCell(1, 2, 2);
        boardModel.SetCell(2, 2, 4);
        boardModel.SetCell(3, 2, 4);

        // y = 1
        boardModel.SetCell(0, 1, 0);
        boardModel.SetCell(1, 1, 2);
        boardModel.SetCell(2, 1, 0);
        boardModel.SetCell(3, 1, 2);

        // y = 0
        boardModel.SetCell(0, 0, 2);
        boardModel.SetCell(1, 0, 2);
        boardModel.SetCell(2, 0, 2);
        boardModel.SetCell(3, 0, 0);
    }

    private void PrintBoard()
    {
        string log = "Current Board\n";

        for (int y = height - 1; y >= 0; y--)
        {
            for (int x = 0; x < width; x++)
            {
                log += boardModel.GetCell(x, y).ToString().PadLeft(4);
            }

            log += "\n";
        }

        Debug.Log(log);
    }
}