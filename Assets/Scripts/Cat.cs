using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : MonoBehaviour
{
    // Personality structure
    struct Personality
    {
        public float[] stamina;
        public float[] sleepChances;
        public float[] lokerChances;
        public float[] appealModifiers;
    }

    // Path structure
    struct Path
    {
        public float cost;
        public int[] route;
        public int routeSize;
    }

    // Personality
    private Personality personality = new Personality();

    // Cell where the cat is located
    public int cellPosition;

    // Other internal components
    private GameManager manager;

    void Start()
    {
        // Initialize internal components
        manager = FindObjectOfType<GameManager>();

        // Create the personality for the cat
        GeneratePersonality();
    }

    void Update()
    {

    }

    void GeneratePersonality()
    {
        float baseStamina;
        float baseSleep;
        float baseLoker;

        personality.stamina = new float[Definitions.NO_OF_TOTD_SLOTS];
        personality.sleepChances = new float[Definitions.NO_OF_TOTD_SLOTS];
        personality.lokerChances = new float[Definitions.NO_OF_TOTD_SLOTS];
        personality.appealModifiers = new float[Definitions.NO_OF_OBJECTS];

        // Select a random fur 

        // Generate base stats
        baseStamina = Random.Range(Definitions.MIN_STAMINA_BASE, Definitions.MAX_STAMINA_BASE);
        baseSleep = Random.Range(Definitions.MIN_SLEEP_BASE, Definitions.MAX_SLEEP_BASE);
        baseLoker = Random.Range(Definitions.MIN_LOKER_BASE, Definitions.MAX_LOKER_BASE);

        // Generate totd stats
        for (int i = 0; i < Definitions.NO_OF_TOTD_SLOTS; i++)
        {
            personality.stamina[i] = baseStamina * Random.Range(Definitions.MIN_STAMINA_MOD, Definitions.MAX_STAMINA_MOD);
            personality.sleepChances[i] = baseSleep * Random.Range(Definitions.MIN_SLEEP_MOD, Definitions.MAX_SLEEP_MOD);
            personality.lokerChances[i] = baseLoker * Random.Range(Definitions.MIN_LOKER_MOD, Definitions.MAX_LOKER_MOD);
        }

        // Generate appeal
        for (int i = 0; i < Definitions.NO_OF_OBJECTS; i++)
        {
            personality.appealModifiers[i] = Random.Range(Definitions.MIN_APPEAL_MOD, Definitions.MAX_APPEAL_MOD);
        }
    }

    int GetNextCell(int position, int stamina)
    {
        // TODO 
        return 0;
    }

    Path GetOptimalPathToCell(int cell)
    {
        // Implements the Dijkstra algorithm 
        Path optimalPath = new Path();
        List<int> unvisitedNodes = new List<int>();
        List<int> visitedNodes = new List<int>();
        float[] tentativeDistance = new float[Definitions.NO_OF_BOARD_CELLS];
        int[] predecessors = new int[Definitions.NO_OF_BOARD_CELLS];
        List<int> neighbors = new List<int>();
        float currentTentativeDistance;
        int currentNode;
        List<int> temporaryRoute = new List<int>();
        int temporaryRouteSize;
        int temporaryPredecessor;
        int timeout = 0;

        // Initialize arrays & lists
        for (int i = 0; i < Definitions.NO_OF_BOARD_CELLS; i++)
        {
            unvisitedNodes[i] = i;
            tentativeDistance[i] = Definitions.NODE_DIST_INFINITY;
            predecessors[i] = Definitions.NODE_NOT_IN_LIST;
        }
        
        // Set tentative distance to current node to 0
        tentativeDistance[this.cellPosition] = 0;

        while (!visitedNodes.Contains(cell) || timeout < Definitions.MAX_DIJKSTRA_ITERATIONS)
        {   
            // Select the next node
            currentNode = GetLowestArrayValueInsidePos(tentativeDistance, unvisitedNodes);

            // Get its neighbors
            neighbors = manager.GetAdjacentCells(currentNode);

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
                currentTentativeDistance = tentativeDistance[currentNode] + manager.cells[neighbors[i]].cost;

                if (tentativeDistance[neighbors[i]] > currentTentativeDistance)
                {
                    tentativeDistance[neighbors[i]] = currentTentativeDistance;
                    predecessors[neighbors[i]] = currentNode;
                }
            }

            // Once all is updated move the node from the unvisited list to the visited list
            unvisitedNodes.Remove(currentNode);
            visitedNodes.Add(currentNode);

            timeout++;
        }

        if (timeout >= Definitions.MAX_DIJKSTRA_ITERATIONS)
        {
            Debug.Log("Error: Reached max allowed number of Dijkstra iterations");
        }

        // Compile results
        optimalPath.cost = tentativeDistance[cell];

        temporaryRouteSize = 0;
        do
        {
            temporaryPredecessor = predecessors[cell];
            temporaryRoute.Add(temporaryPredecessor);
            temporaryRouteSize++;
        }
        while (temporaryPredecessor != this.cellPosition);

        optimalPath.route = temporaryRoute.ToArray();
        optimalPath.routeSize = temporaryRouteSize;

        return optimalPath;
    }

    int GetLowestArrayValueInsidePos(float[] mainArray, List<int> containingList)
    {
        int lowestValuePos;
        bool isValueInArray;
        do
        {
            lowestValuePos = manager.GetLowestArrayValuePos(mainArray);
            isValueInArray = containingList.Contains(lowestValuePos);

            if (!isValueInArray)
            {
                mainArray[lowestValuePos] = Definitions.NODE_DIST_INFINITY;
            }
        }
        while (!isValueInArray);

        return lowestValuePos;
    }
}
