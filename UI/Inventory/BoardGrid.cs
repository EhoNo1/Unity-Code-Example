using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(GridLayoutGroup))]
public class BoardGrid : MonoBehaviour
{
    private GridLayout gridLayout;
    public GameObject cellPrefab;
    [SerializeField] private TextMeshProUGUI nameText;
    private Vector2Int size;
    private InventoryCell[,] cellGrid;

    void Start()
    {
        gridLayout = GetComponent<GridLayout>();
        BoardData board = BoardLibrary.library[Random.Range(0,BoardLibrary.library.Count)];
        nameText.text = board.name;
        SetGridSize(board.size);
        Debug.Log(board.mask.mask[0, 0]);
        SetGrid(board.mask);
        Debug.Log(board.mask);
    }



    public void SetGridSize(int width, int height)
    {
        SetGridSize(new Vector2Int(width, height));
    }
    public void SetGridSize(Vector2Int size)
    {
        ClearGrid();
        RectTransform rect = GetComponent<RectTransform>();
        rect.sizeDelta = size * 50;
        this.size = size;
        SpawnGrid();
    }

    public void ClearGrid()
    {
        cellGrid = null;
        while (transform.childCount > 0)
        {
            Destroy(transform.GetChild(0));
        }
    }

    public void SpawnGrid()
    {
        cellGrid = new InventoryCell[size.x, size.y];
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                GameObject go = Instantiate(cellPrefab);
                go.transform.SetParent(transform);
                InventoryCell ic = go.GetComponent<InventoryCell>();
                ic.position = new Vector2Int(x, y);
                cellGrid[x, y] = ic;
            }
        }
    }

    public void SetGrid(BoardUsabilityMask mask)
    {
        for (int y = 0; y < mask.size.y; y++)
        {
            for (int x = 0; x < mask.size.x; x++)
            {
                cellGrid[x, y].SetCellUsability(mask.mask[x, y]);
            }
        }
    }
}
