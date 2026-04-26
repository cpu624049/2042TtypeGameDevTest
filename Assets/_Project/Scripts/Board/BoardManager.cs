using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [Header("Board Settings")]
    [SerializeField] private BoardView boardView;
    [SerializeField] private int width = 4;
    [SerializeField] private int height = 4;
    [SerializeField] private int startTileCount = 2;

    private BoardModel boardModel;
    private int lastMoveScore = 0;

    private void Awake()
    {
        boardModel = new BoardModel(width, height);
    }

    public void InitializeBoard()
    {
        boardModel.Clear();
        lastMoveScore = 0;

        Debug.Log("BoardManager.InitializeBoard()");
        RefreshBoardView();
        PrintBoard();
    }

    public void SpawnStartTiles()
    {
        for (int i = 0; i < startTileCount; i++)
        {
            SpawnRandomTile();
        }
    }

    public void SpawnRandomTile()
    {
        Vector2Int[] emptyPositions = GetEmptyPositions();

        if (emptyPositions.Length == 0)
        {
            Debug.Log("빈 칸 없음: 새 타일 생성 생략");
            return;
        }

        Vector2Int spawnPos = emptyPositions[Random.Range(0, emptyPositions.Length)];
        int spawnValue = Random.value < 0.9f ? 2 : 4;

        boardModel.SetCell(spawnPos.x, spawnPos.y, spawnValue);

        Debug.Log($"새 타일 생성: ({spawnPos.x}, {spawnPos.y}) = {spawnValue}");
        RefreshBoardView();
        PrintBoard();
    }

    public bool HandleMove(SwipeDirection direction)
    {
        Debug.Log($"BoardManager.HandleMove() direction = {direction}");

        bool moved = false;
        lastMoveScore = 0;

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
            Debug.Log($"보드 변경 발생 / 이번 턴 점수: {lastMoveScore}");
            RefreshBoardView();
            PrintBoard();
        }
        else
        {
            Debug.Log("보드 변경 없음");
        }

        return moved;
    }

    public int ConsumeLastMoveScore()
    {
        int score = lastMoveScore;
        lastMoveScore = 0;
        return score;
    }

    public bool IsGameOver()
    {
        if (GetEmptyPositions().Length > 0)
            return false;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int current = boardModel.GetCell(x, y);

                if (x + 1 < width && boardModel.GetCell(x + 1, y) == current)
                    return false;

                if (y + 1 < height && boardModel.GetCell(x, y + 1) == current)
                    return false;
            }
        }

        return true;
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
                int mergedValue = line[i] * 2;
                result.Add(mergedValue);
                lastMoveScore += mergedValue;
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

    private Vector2Int[] GetEmptyPositions()
    {
        List<Vector2Int> emptyPositions = new List<Vector2Int>();

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (boardModel.IsEmpty(x, y))
                {
                    emptyPositions.Add(new Vector2Int(x, y));
                }
            }
        }

        return emptyPositions.ToArray();
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

    public bool RemoveLowestTileForContinue()
    {
        int lowestValue = int.MaxValue;
        Vector2Int targetPos = new Vector2Int(-1, -1);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int value = boardModel.GetCell(x, y);

                if (value <= 0)
                    continue;

                if (value < lowestValue)
                {
                    lowestValue = value;
                    targetPos = new Vector2Int(x, y);
                }
            }
        }

        if (targetPos.x < 0)
            return false;

        boardModel.SetCell(targetPos.x, targetPos.y, 0);

        Debug.Log($"이어하기: 가장 낮은 타일 제거 ({targetPos.x}, {targetPos.y}) = {lowestValue}");
        RefreshBoardView();
        PrintBoard();

        return true;
    }

    private void RefreshBoardView()
    {
        if (boardView != null)
        {
            boardView.Refresh(boardModel.CopyCells());
        }
    }
}