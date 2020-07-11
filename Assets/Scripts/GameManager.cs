using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform cellParent;
    [SerializeField]
    Color blue;
    [SerializeField]
    Color red;
    [SerializeField]
    Transform objectParent;
    public CatObject objectToMove;
    [SerializeField]
    GameObject cell;
    GameObject ins;
    [SerializeField]
    GameObject[] objectPrefabs;
    [SerializeField]
    Vector2 oGCellPos;
    public Cell[] cells = new Cell[64];
    
    public List<CatObject> catObjects = new List<CatObject>();
    int crescendoA;
    int randomInt;
    int posValue;
    
    void Start()
    {
        #region Instantiate Cells

        oGCellPos = new Vector2(oGCellPos.x + 0.09f, oGCellPos.y + 0.06f);

        for (int i = 0; i < 64; i++)
        {
            if(crescendoA == 8)
            {
                crescendoA = 0;
                oGCellPos = new Vector2(oGCellPos.x + 0.09f * 9, oGCellPos.y + 0.06f * 7);
            }

            ins = Instantiate(cell, Vector2.zero, cell.transform.rotation);
            ins.transform.parent = cellParent;
            oGCellPos.x -= 0.09f;
            oGCellPos.x = Mathf.Round(oGCellPos.x * 100) / 100;
            oGCellPos.y -= 0.06f;
            oGCellPos.y = Mathf.Round(oGCellPos.y * 100) / 100;
            ins.transform.localPosition = oGCellPos;
            ins.transform.localScale = Vector3.one;
            cells[i] = ins.GetComponentInChildren<Cell>();
            cells[i].posValue = posValue;
            posValue += 1;
            crescendoA += 1;
        }

        #endregion

        #region Instantiate Valuables

        for(int i = 0; i < objectPrefabs.Length; i++)
        {
            do
            {
                randomInt = Random.Range(31, 63);
            }
            while (cells[randomInt].occupied);

            ins = Instantiate(objectPrefabs[i], Vector2.zero, objectPrefabs[i].transform.rotation);
            ins.transform.parent = objectParent;
            ins.transform.localPosition = new Vector2(Mathf.Round(cells[randomInt].transform.localPosition.x * 100) / 100, (Mathf.Round(cells[randomInt].transform.localPosition.y * 100) / 100) + 0.03f);
            ins.transform.localScale = Vector3.one;
            ins.GetComponent<CatObject>().cellPosition = randomInt;
            cells[randomInt].occupied = true;
            catObjects.Add(ins.GetComponent<CatObject>());
        }

        #endregion
    }

    public void EnableCells(CatObject toMove)
    {
        objectToMove = toMove;
       
        for(int i = 0; i < catObjects.Count; i++)
        {
            catObjects[i].GetComponent<BoxCollider2D>().enabled = false;
        }

        cellParent.gameObject.SetActive(true);
        cells[toMove.cellPosition].GetComponentInChildren<Animator>().SetBool("jump", true);
    }

    public void DisableCells()
    {
        cellParent.gameObject.SetActive(false);

        for (int i = 0; i < catObjects.Count; i++)
        {
            catObjects[i].GetComponent<BoxCollider2D>().enabled = true;
        }
    }

    public int[] GetAdjacentCells(int cell)
    {
        int c = 0;
        bool is_top;
        bool is_bot;
        bool is_lft;
        bool is_rgt;

        // Fill empty vector
        int[] adjacentCells = new int[Definitions.NO_OF_ADJACENT_CELLS];
        for (int i = 0; i < adjacentCells.Length; i++)
        {
            adjacentCells[i] = Definitions.NOT_AN_ADJACENT_SLOT;
        }

        // Detect postion of the cell in the board
        is_top = (cell / Definitions.BOARD_SIZE == 0);
        is_bot = (cell / Definitions.BOARD_SIZE == Definitions.BOARD_SIZE - 1);
        is_lft = (cell % Definitions.BOARD_SIZE == 0);
        is_rgt = (cell % Definitions.BOARD_SIZE == Definitions.BOARD_SIZE - 1);

        // Detect adjacent cells based on position
        if (!is_top)
        {
            adjacentCells[c++] = cell - Definitions.BOARD_SIZE;
        }

        if (!is_bot)
        {
            adjacentCells[c++] = cell + Definitions.BOARD_SIZE;
        }

        if (!is_lft)
        {
            adjacentCells[c++] = cell - 1;
        }

        if (!is_rgt)
        {
            adjacentCells[c++] = cell + 1;
        }

        if (!(is_top && is_lft))
        {
            adjacentCells[c++] = cell - Definitions.BOARD_SIZE - 1;
        }

        if (!(is_top && is_rgt))
        {
            adjacentCells[c++] = cell - Definitions.BOARD_SIZE + 1;
        }

        if (!(is_bot && is_lft))
        {
            adjacentCells[c++] = cell + Definitions.BOARD_SIZE - 1;
        }

        if (!(is_bot && !is_rgt))
        {
            adjacentCells[c++] = cell + Definitions.BOARD_SIZE + 1;
        }

        return adjacentCells;
    }
}
