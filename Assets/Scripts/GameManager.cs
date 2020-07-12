using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Transform cellParent;
    public Color blue;
    public AudioSource buttonSound;
    [SerializeField]
    Color red;
    [SerializeField]
    Text selectText;
    [SerializeField]
    Text turnsText;
    [SerializeField]
    Transform objectParent;
    public CatObject objectToMove;
    public List<CatObject> catObjects = new List<CatObject>();
    [SerializeField]
    GameObject gameOverScreen;
    [SerializeField]
    GameObject cell;
    GameObject ins;
    [SerializeField]
    GameObject[] objectPrefabs;
    [SerializeField]
    Vector2 oGCellPos;
    UIManager uI;
    public Cell[] cells = new Cell[Definitions.BOARD_SIZE * Definitions.BOARD_SIZE];
    public int valuablesLeft = 5;
    int crescendoA;
    int randomInt;
    int posValue;
    int turns;
    [SerializeField]
    Cat cat;
    List<int> viableCells = new List<int>();
    List<int> startingViableCells = new List<int>();


    void Start()
    {
        buttonSound = GetComponent<AudioSource>();
        uI = FindObjectOfType<UIManager>();

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

        cells[0].occupied = true;
        cells[4].occupied = true;
        cells[7].occupied = true;
        cells[20].occupied = true;
        cells[26].occupied = true;

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
        bool obstructed;
        bool outOfBounds;

        int dstToTop;
        int dstToBot;
        int dstToLft;
        int dstToRgt;

        int coordX;
        int coordY;

        int newCell;

        int boardDiagonal = Mathf.CeilToInt(Mathf.Sqrt((Definitions.BOARD_SIZE ^ 2) + (Definitions.BOARD_SIZE ^ 2)));

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

            case 3:
                // Detect distances from the cell to the edges of the board
                dstToTop = toMove.cellPosition / Definitions.BOARD_SIZE;
                dstToBot = (Definitions.BOARD_SIZE - 1) - (toMove.cellPosition / Definitions.BOARD_SIZE);
                dstToRgt = toMove.cellPosition % Definitions.BOARD_SIZE;
                dstToLft = (Definitions.BOARD_SIZE - 1) - (toMove.cellPosition % Definitions.BOARD_SIZE);

                obstructed = false;
                coordX = 0;
                coordY = 0;
                for (int i = 0; i < boardDiagonal; i++)
                {
                    coordX = coordX + 1;
                    coordY = coordY + 1;

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

                    if (!outOfBounds)
                    {
                        newCell = toMove.cellPosition - coordX - (coordY * Definitions.BOARD_SIZE);

                        if (!cells[newCell].occupied && !obstructed)
                        {
                            viableCells.Add(newCell);
                        }
                        else if (i != toMove.cellPosition)
                        {
                            obstructed = true;
                        }
                    }
                }

                obstructed = false;
                coordX = 0;
                coordY = 0;
                for (int i = 0; i < boardDiagonal; i++)
                {
                    coordX = coordX + 1;
                    coordY = coordY - 1;

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

                    if (!outOfBounds)
                    {
                        newCell = toMove.cellPosition - coordX - (coordY * Definitions.BOARD_SIZE);

                        if (!cells[newCell].occupied && !obstructed)
                        {
                            viableCells.Add(newCell);
                        }
                        else if (i != toMove.cellPosition)
                        {
                            obstructed = true;
                        }
                    }
                }

                obstructed = false;
                coordX = 0;
                coordY = 0;
                for (int i = 0; i < boardDiagonal; i++)
                {
                    coordX = coordX - 1;
                    coordY = coordY + 1;

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

                    if (!outOfBounds)
                    {
                        newCell = toMove.cellPosition - coordX - (coordY * Definitions.BOARD_SIZE);

                        if (!cells[newCell].occupied && !obstructed)
                        {
                            viableCells.Add(newCell);
                        }
                        else if (i != toMove.cellPosition)
                        {
                            obstructed = true;
                        }
                    }
                }

                obstructed = false;
                coordX = 0;
                coordY = 0;
                for (int i = 0; i < boardDiagonal; i++)
                {
                    coordX = coordX - 1;
                    coordY = coordY - 1;

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

                    if (!outOfBounds)
                    {
                        newCell = toMove.cellPosition - coordX - (coordY * Definitions.BOARD_SIZE);

                        if (!cells[newCell].occupied && !obstructed)
                        {
                            viableCells.Add(newCell);
                        }
                        else if (i != toMove.cellPosition)
                        {
                            obstructed = true;
                        }
                    }
                }

                for (int i = 0; i < viableCells.Count; i++)
                {
                    cells[viableCells[i]].GetComponentInChildren<SpriteRenderer>().color = blue;
                }

                break;

            case 1:
                startingViableCells = GetAdjacentCells(toMove.cellPosition, 2);

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

            case 4:
                startingViableCells = GetAdjacentCells(toMove.cellPosition, 3);

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

            case 2:
                // Detect distances from the cell to the edges of the board
                dstToTop = toMove.cellPosition / Definitions.BOARD_SIZE;
                dstToBot = (Definitions.BOARD_SIZE - 1) - (toMove.cellPosition / Definitions.BOARD_SIZE);
                dstToRgt = toMove.cellPosition % Definitions.BOARD_SIZE;
                dstToLft = (Definitions.BOARD_SIZE - 1) - (toMove.cellPosition % Definitions.BOARD_SIZE);

                obstructed = false;
                coordX = 0;
                coordY = 0;
                for (int i = 0; i < 4; i++)
                {
                    coordX = coordX + 1;

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

                    if (!outOfBounds)
                    {
                        newCell = toMove.cellPosition - coordX - (coordY * Definitions.BOARD_SIZE);

                        if (!cells[newCell].occupied && !obstructed)
                        {
                            viableCells.Add(newCell);
                        }
                        else if (i != toMove.cellPosition)
                        {
                            obstructed = true;
                        }
                    }
                }

                obstructed = false;
                coordX = 0;
                coordY = 0;
                for (int i = 0; i < 4; i++)
                {
                    coordX = coordX - 1;

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

                    if (!outOfBounds)
                    {
                        newCell = toMove.cellPosition - coordX - (coordY * Definitions.BOARD_SIZE);

                        if (!cells[newCell].occupied && !obstructed)
                        {
                            viableCells.Add(newCell);
                        }
                        else if (i != toMove.cellPosition)
                        {
                            obstructed = true;
                        }
                    }
                }

                obstructed = false;
                coordX = 0;
                coordY = 0;
                for (int i = 0; i < 4; i++)
                {
                    coordY = coordY + 1;

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

                    if (!outOfBounds)
                    {
                        newCell = toMove.cellPosition - coordX - (coordY * Definitions.BOARD_SIZE);

                        if (!cells[newCell].occupied && !obstructed)
                        {
                            viableCells.Add(newCell);
                        }
                        else if (i != toMove.cellPosition)
                        {
                            obstructed = true;
                        }
                    }
                }

                obstructed = false;
                coordX = 0;
                coordY = 0;
                for (int i = 0; i < 4; i++)
                {
                    coordY = coordY - 1;

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

                    if (!outOfBounds)
                    {
                        newCell = toMove.cellPosition - coordX - (coordY * Definitions.BOARD_SIZE);

                        if (!cells[newCell].occupied && !obstructed)
                        {
                            viableCells.Add(newCell);
                        }
                        else if (i != toMove.cellPosition)
                        {
                            obstructed = true;
                        }
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
        StartCoroutine(JumpFalseCo());
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

    public void GameOver()
    {
        cellParent.gameObject.SetActive(false);

        for (int i = 0; i < catObjects.Count; i++)
        {
            catObjects[i].GetComponent<Collider2D>().enabled = false;
        }

        for (int i = 0; i < uI.toActivate.Length; i++)
        {
            uI.toActivate[i].SetActive(false);
        }

        gameOverScreen.SetActive(true);
    }

    public void EndTurn()
    {
        CatObject ToFind;

        turns += 1;
        turnsText.text = Definitions.TURNS_TXT + (Definitions.MAX_TURNS - turns);
        selectText.text = Definitions.CAT_TXT;

        if(Definitions.MAX_TURNS - turns == 0)
        {
            cellParent.gameObject.SetActive(false);

            for (int i = 0; i < catObjects.Count; i++)
            {
                catObjects[i].GetComponent<Collider2D>().enabled = false;
            }

            for (int i = 0; i < uI.toActivate.Length; i++)
            {
                uI.toActivate[i].SetActive(false);
            }

            gameOverScreen.SetActive(true);
        }

        else
        {
            cat.StartAction();

            // Update computers movement
            ToFind = catObjects.Find(x => x.internalMovementType == 5);
            ToFind.CloneMovementType();
        }
    }

    public void NewTurn()
    {
        selectText.text = Definitions.SELECT_TXT;

        for (int i = 0; i < catObjects.Count; i++)
        {
            catObjects[i].GetComponent<Collider2D>().enabled = true;
        }
    }

    public void Retry()
    {
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public List<int> GetAdjacentCells(int cell, int radius=1)
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

    public List<int> GetAdjacentCellsCapped(int cell)
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

        return adjacentCells;
    }

    public int GetDistanceBetweenCells(int cell1, int cell2)
    {
        // Implements the Dijkstra algorithm 
        List<int> unvisitedNodes = new List<int>();
        List<int> visitedNodes = new List<int>();
        int[] tentativeDistance = new int[Definitions.NO_OF_BOARD_CELLS];
        int[] predecessors = new int[Definitions.NO_OF_BOARD_CELLS];
        List<int> neighbors = new List<int>();
        int currentTentativeDistance;
        int currentNode;

        // Initialize arrays & lists
        for (int i = 0; i < Definitions.NO_OF_BOARD_CELLS; i++)
        {
            unvisitedNodes.Add(i);
            tentativeDistance[i] = Definitions.NODE_DIST_INFINITY;
            predecessors[i] = Definitions.NODE_NOT_IN_LIST;
        }

        // Set tentative distance to current node to 0
        tentativeDistance[cell1] = 0;

        for (int l = 0; l < Definitions.MAX_DIJKSTRA_ITERATIONS; l++)
        {
            // Select the next node
            currentNode = GetLowestIntArrayValueInsidePos(tentativeDistance, unvisitedNodes);

            // Get its neighbors
            neighbors = GetAdjacentCellsCapped(currentNode);

            // Eliminate visited neighbors
            for (int i = 0; i < visitedNodes.Count; i++)
            {
                if (neighbors.Contains(visitedNodes[i]))
                {
                    neighbors.Remove(visitedNodes[i]);
                }
            }

            for (int i = 0; i < neighbors.Count; i++)
            {
                // Calculate the tentative distance for this path
                currentTentativeDistance = tentativeDistance[currentNode] + 1;

                if (tentativeDistance[neighbors[i]] > currentTentativeDistance)
                {
                    tentativeDistance[neighbors[i]] = currentTentativeDistance;
                }
            }

            // Once all is updated move the node from the unvisited list to the visited list
            unvisitedNodes.Remove(currentNode);
            visitedNodes.Add(currentNode);

            // Break the loop if the node is found
            if (visitedNodes.Contains(cell2))
            {
                break;
            }
        }

        // Compile results
        return tentativeDistance[cell2];
    }

    int GetLowestIntArrayValueInsidePos(int[] mainArray, List<int> containingList)
    {
        int lowestValuePos;
        bool isValueInArray;
        do
        {
            lowestValuePos = GetLowestIntArrayValuePos(mainArray);
            isValueInArray = containingList.Contains(lowestValuePos);

            if (!isValueInArray)
            {
                mainArray[lowestValuePos] = Definitions.NODE_DIST_INFINITY;
            }
        }
        while (!isValueInArray);

        return lowestValuePos;
    }

    public int GetLowestIntArrayValuePos(int[] array)
    {
        int lowestValue = array[0];
        int lowestValuePos = 0;

        for (int i = 1; i < array.Length; i++)
        {
            if (array[i] < lowestValue)
            {
                lowestValuePos = i;
                lowestValue = array[i];
            }
        }

        return lowestValuePos;
    }

    public int GetLowestArrayValuePos(float[] array)
    {
        float lowestValue = array[0];
        int lowestValuePos = 0;

        for (int i = 1; i < array.Length; i++)
        {
            if (array[i] < lowestValue)
            {
                lowestValuePos = i;
                lowestValue = array[i];
            }
        }

        return lowestValuePos;
    }

    public bool IsValueInArray(int value, int[] array)
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

    IEnumerator JumpFalseCo()
    {
        yield return new WaitForEndOfFrame();
        cells[objectToMove.cellPosition].GetComponentInChildren<Animator>().SetBool("start", true);
        cells[objectToMove.cellPosition].GetComponentInChildren<Animator>().SetBool("jump", false);
    }
}