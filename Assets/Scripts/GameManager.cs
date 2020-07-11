using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Transform cellParent;
    public Color blue;
    [SerializeField]
    Color red;
    [SerializeField]
    Text selectText;
    [SerializeField]
    Transform objectParent;
    public CatObject objectToMove;
    public List<CatObject> catObjects = new List<CatObject>();
    [SerializeField]
    GameObject cell;
    GameObject ins;
    [SerializeField]
    GameObject[] objectPrefabs;
    [SerializeField]
    Vector2 oGCellPos;
    public Cell[] cells = new Cell[Definitions.BOARD_SIZE * Definitions.BOARD_SIZE];
    int crescendoA;
    int randomInt;
    int posValue;
    List<int> viableCells = new List<int>();
    List<int> startingViableCells = new List<int>();


    void Start()
    {
        #region Instantiate Cells

        oGCellPos = new Vector2(oGCellPos.x + 0.09f, oGCellPos.y + 0.06f);

        for (int i = 0; i < 64; i++)
        {
            if (crescendoA == 8)
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
            ins.transform.localPosition = new Vector3(oGCellPos.x, oGCellPos.y, i * -0.001f);
            ins.transform.localScale = Vector3.one;
            cells[i] = ins.GetComponentInChildren<Cell>();
            cells[i].zOffset = i * -0.001f;
            cells[i].posValue = posValue;
            posValue += 1;
            crescendoA += 1;
        }

        #endregion

        #region Instantiate Valuables

        for (int i = 0; i < objectPrefabs.Length; i++)
        {
            do
            {
                randomInt = Random.Range(31, 63);
            }
            while (cells[randomInt].occupied);

            ins = Instantiate(objectPrefabs[i], Vector2.zero, objectPrefabs[i].transform.rotation);
            ins.transform.parent = objectParent;
            ins.transform.localPosition = new Vector3(Mathf.Round(cells[randomInt].transform.localPosition.x * 1000) / 1000 + objectPrefabs[i].GetComponent<CatObject>().xOffset, (Mathf.Round(cells[randomInt].transform.localPosition.y * 1000) / 1000) + objectPrefabs[i].GetComponent<CatObject>().yOffset, -0.001f * randomInt);
            ins.transform.localScale = Vector3.one;
            ins.GetComponent<CatObject>().cellPosition = randomInt;
            cells[randomInt].occupied = true;
            catObjects.Add(ins.GetComponent<CatObject>());
        }

        #endregion
    }

    public void EnableCells(CatObject toMove, int movementType)
    {
        objectToMove = toMove;
        viableCells.Clear();

        for (int i = 0; i < catObjects.Count; i++)
        {
            catObjects[i].GetComponent<Collider2D>().enabled = false;
        }

        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].GetComponentInChildren<SpriteRenderer>().color = red;
        }

        #region Determine Viable Cells

        switch (movementType)
        {
            case 0:
                startingViableCells = GetMovementRadius(toMove.cellPosition, 2);

                for (int i = 0; i < startingViableCells.Count; i++)
                {
                    if (!cells[startingViableCells[i]].occupied)
                    {
                        viableCells.Add(startingViableCells[i]);
                    }
                }

                for (int i = 0; i < viableCells.Count; i++)
                {
                    cells[viableCells[i]].GetComponentInChildren<SpriteRenderer>().color = blue;
                }

                break;

            case 1:
                startingViableCells = GetAdjacentCells(toMove.cellPosition);

                for (int i = 0; i < startingViableCells.Count; i++)
                {
                    if (!cells[startingViableCells[i]].occupied)
                    {
                        viableCells.Add(startingViableCells[i]);
                    }
                }

                for (int i = 0; i < viableCells.Count; i++)
                {
                    cells[viableCells[i]].GetComponentInChildren<SpriteRenderer>().color = blue;
                }

                break;
        }

        #endregion

        selectText.text = Definitions.CHOOSE_TXT;

        cellParent.gameObject.SetActive(true);
        cells[toMove.cellPosition].GetComponentInChildren<Animator>().SetBool("jump", true);
    }

    public void DisableCells()
    {
        cellParent.gameObject.SetActive(false);

        for (int i = 0; i < catObjects.Count; i++)
        {
            catObjects[i].GetComponent<Collider2D>().enabled = true;
        }

        selectText.text = Definitions.SELECT_TXT;
    }

    public List<int> GetAdjacentCells(int cell)
    {
        bool is_top;
        bool is_bot;
        bool is_lft;
        bool is_rgt;

        List<int> adjacentCells = new List<int>();

        // Detect postion of the cell in the board
        is_top = (cell / Definitions.BOARD_SIZE == 0);
        is_bot = (cell / Definitions.BOARD_SIZE == Definitions.BOARD_SIZE - 1);
        is_lft = (cell % Definitions.BOARD_SIZE == 0);
        is_rgt = (cell % Definitions.BOARD_SIZE == Definitions.BOARD_SIZE - 1);

        // Detect adjacent cells based on position
        if (!is_top)
        {
            adjacentCells.Add(cell - Definitions.BOARD_SIZE);
        }

        if (!is_bot)
        {
            adjacentCells.Add(cell + Definitions.BOARD_SIZE);
        }

        if (!is_lft)
        {
            adjacentCells.Add(cell - 1);
        }

        if (!is_rgt)
        {
            adjacentCells.Add(cell + 1);
        }

        if (!is_top && !is_lft)
        {
            adjacentCells.Add(cell - Definitions.BOARD_SIZE - 1);
        }

        if (!is_top && !is_rgt)
        {
            adjacentCells.Add(cell - Definitions.BOARD_SIZE + 1);
        }

        if (!is_bot && !is_lft)
        {
            adjacentCells.Add(cell + Definitions.BOARD_SIZE - 1);
        }

        if (!is_bot && !is_rgt)
        {
            adjacentCells.Add(cell + Definitions.BOARD_SIZE + 1);
        }

        return adjacentCells;
    }

    public List<int> GetMovementRadius(int cell, int radius)
    {
        // This function expects a cell order of top-right to bottom-left
        int dstToTop;
        int dstToBot;
        int dstToLft;
        int dstToRgt;

        int coordX = 0;
        int coordY = 0;

        int increments;
        int iterationCount;
        int maxIterations;
        bool iterateOnX;
        bool positiveVal;
        bool outOfBounds;

        int newCell;

        List<int> cellsInRadius = new List<int>();

        // Detect distances from the cell to the edges of the board
        dstToTop = cell / Definitions.BOARD_SIZE;
        dstToBot = (Definitions.BOARD_SIZE - 1) - (cell / Definitions.BOARD_SIZE);
        dstToRgt = cell % Definitions.BOARD_SIZE;
        dstToLft = (Definitions.BOARD_SIZE - 1) - (cell % Definitions.BOARD_SIZE);

        Debug.Log("Top Bot Lft Rgt");
        Debug.Log(dstToTop + " " + dstToBot + " " + dstToLft + " " + dstToRgt);

        // Iterate in an helicoidal manner, limited by the distances
        maxIterations = 1 + radius * 4;
        iterationCount = 0;
        increments = 1;
        iterateOnX = true;
        positiveVal = true;

        for (int j = 0; j < maxIterations; j++)
        {
            // Depending on the state of the helix update the coordinates
            for (int i = 0; i < increments; i++)
            {
                if (iterateOnX)
                {
                    if (positiveVal)
                    {
                        coordX++;
                    }
                    else
                    {
                        coordX--;
                    }
                }
                else
                {
                    if (positiveVal)
                    {
                        coordY++;
                    }
                    else
                    {
                        coordY--;
                    }

                }

                // Check boundaries
                outOfBounds = false;

                if (coordX > dstToRgt)
                {
                    outOfBounds = true;
                }

                if (coordX < -dstToLft)
                {
                    outOfBounds = true;
                }

                if (coordY > dstToTop)
                {
                    outOfBounds = true;
                }

                if (coordY < -dstToBot)
                {
                    outOfBounds = true;
                }

                // Following the coordinates add the value to the list if they are not out of bounds
                if (!outOfBounds)
                {
                    newCell = cell - coordX - (coordY * Definitions.BOARD_SIZE);

                    Debug.Log(" X " + coordX + " Y " + coordY + " NCELL " + newCell + " OCELL " + cell);

                    if (!cellsInRadius.Contains(newCell))
                    {
                        cellsInRadius.Add(newCell);
                    }
                }
            }

            // Control the state of the helix
            if (iterationCount % 2 == 0)
            {
                positiveVal = !positiveVal;
            }
            else
            {
                if (j != maxIterations - 2)
                {
                    increments++;
                }
            }

            iterateOnX = !iterateOnX;
            iterationCount++;
        }

        return cellsInRadius;
    }

    public int getLowestArrayValue(int[] array)
    {
        int lowestValue = 1;

        for (int i = 1; i < array.Length; i++)
        {
            if (array[i] < lowestValue)
            {
                lowestValue = array[i];
            }
        }

        return lowestValue;
    }

    public bool isValueInArray(int value, int[] array)
    {
        bool valueFound = false;

        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] == value)
            {
                valueFound = true;
                break;
            }
        }

        return valueFound;
    }
}