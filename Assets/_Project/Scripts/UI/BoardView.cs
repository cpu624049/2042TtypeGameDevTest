using System.Collections.Generic;
using UnityEngine;

public class BoardView : MonoBehaviour
{
    [SerializeField] private TileView tilePrefab;
    [SerializeField] private Transform tileParent;
    [SerializeField] private int width = 4;
    [SerializeField] private int height = 4;

    private readonly List<TileView> tileViews = new List<TileView>();

    private void Awake()
    {
        CreateTiles();
    }

    private void CreateTiles()
    {
        if (tilePrefab == null || tileParent == null)
        {
            Debug.LogError("BoardView: tilePrefab 또는 tileParent 참조가 비어 있음");
            return;
        }

        tileViews.Clear();

        for (int i = 0; i < width * height; i++)
        {
            TileView tile = Instantiate(tilePrefab, tileParent);
            tileViews.Add(tile);
        }
    }

    public void Refresh(int[,] cells)
    {
        if (cells == null)
            return;

        int index = 0;

        for (int y = height - 1; y >= 0; y--)
        {
            for (int x = 0; x < width; x++)
            {
                tileViews[index].SetValue(cells[x, y]);
                index++;
            }
        }
    }
}