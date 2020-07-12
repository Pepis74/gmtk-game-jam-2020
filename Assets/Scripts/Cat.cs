using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : MonoBehaviour
{
    // Personality structure
    struct Personality
    {
        public float[] stamina;
        public float[] focus;
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

    // Loker
    public bool isLoker;
    public int turnsLoker;

    // Sleep
    public bool isAsleep;
    public int turnsAsleep;

    // Cell where the cat is located
    public int cellPosition;

    // Cell the cat is aiming for
    public int memory;

    // Other internal components
    private GameManager manager;
    private int totd;

    void Start()
    {
        // Initialize internal components
        manager = FindObjectOfType<GameManager>();

        // Create the personality for the cat
        GeneratePersonality();

        // Initialize loker & sleep
        isLoker = false;
        isAsleep = false;

        // Set objective to empty
        memory = Definitions.OBJECTIVE_EMPTY;

        // Set fixed totd since it has ended up as an unused feature
        totd = 0;
    }

    void Update()
    {
        //SetState();

        // Where to move to (Returns a cell number):
        //GetNextCell(getObjectivePath(GetObjective()));
    }

    void SetState()
    {
        // Randomly select which goes first, sleep or loker
        if (Random.Range(0.0f, 1.0f) < 0.5f)
        {
            SetLoker();
            SetAsleep();
        }
        else
        {
            SetAsleep();
            SetLoker();
        }
    }

    void SetLoker()
    {
        // If already loker every time it's less probable that will continue
        if (isLoker)
        {
            if (Random.Range(0.0f, 1.0f) < Definitions.REMAIN_LOKER_BASE / (float)turnsLoker)
            {
                turnsLoker++;
            }
            else
            {
                isLoker = false;
                turnsLoker = 0;
            }
        }
        else
        {
            if (!isAsleep)
            {
                // Set loker
                if (Random.Range(0.0f, 1.0f) < personality.lokerChances[totd])
                {
                    isLoker = true;
                    turnsLoker = 1;
                }
            }
        }
    }

    void SetAsleep()
    {
        // If already asleep every time it's less probable that will continue
        if (isAsleep)
        {
            if (Random.Range(0.0f, 1.0f) < Definitions.REMAIN_ASLEEP_BASE / (float)turnsAsleep)
            {
                turnsAsleep++;
            }
            else
            {
                isAsleep = false;
                turnsAsleep = 0;
            }
        }
        else
        {
            if (!isLoker)
            {
                // Set asleep
                if (Random.Range(0.0f, 1.0f) < personality.sleepChances[totd])
                {
                    isAsleep = true;
                    turnsAsleep = 1;
                }
            }
        }
    }

    void GeneratePersonality()
    {
        float baseStamina;
        float baseFocus;
        float baseSleep;
        float baseLoker;

        personality.stamina = new float[Definitions.NO_OF_TOTD_SLOTS];
        personality.focus = new float[Definitions.NO_OF_TOTD_SLOTS];
        personality.sleepChances = new float[Definitions.NO_OF_TOTD_SLOTS];
        personality.lokerChances = new float[Definitions.NO_OF_TOTD_SLOTS];
        personality.appealModifiers = new float[Definitions.NO_OF_OBJECTS];

        // Generate base stats
        baseStamina = Random.Range(Definitions.MIN_STAMINA_BASE, Definitions.MAX_STAMINA_BASE);
        baseFocus = Random.Range(Definitions.MIN_FOCUS_BASE, Definitions.MAX_FOCUS_BASE);
        baseSleep = Random.Range(Definitions.MIN_SLEEP_BASE, Definitions.MAX_SLEEP_BASE);
        baseLoker = Random.Range(Definitions.MIN_LOKER_BASE, Definitions.MAX_LOKER_BASE);

        // Generate totd stats
        for (int i = 0; i < Definitions.NO_OF_TOTD_SLOTS; i++)
        {
            personality.stamina[i] = baseStamina * Random.Range(Definitions.MIN_STAMINA_MOD, Definitions.MAX_STAMINA_MOD);
            personality.focus[i] = baseFocus * Random.Range(Definitions.MIN_FOCUS_MOD, Definitions.MAX_FOCUS_MOD);
            personality.sleepChances[i] = baseSleep * Random.Range(Definitions.MIN_SLEEP_MOD, Definitions.MAX_SLEEP_MOD);
            personality.lokerChances[i] = baseLoker * Random.Range(Definitions.MIN_LOKER_MOD, Definitions.MAX_LOKER_MOD);
        }

        // Generate appeal
        for (int i = 0; i < Definitions.NO_OF_OBJECTS; i++)
        {
            personality.appealModifiers[i] = Random.Range(Definitions.MIN_APPEAL_MOD, Definitions.MAX_APPEAL_MOD);
        }
    }

    int GetObjective()
    {
        int objective;
        float[] objectInterest = new float[Definitions.NO_OF_OBJECTS];
        Path[] objectPath = new Path[Definitions.NO_OF_OBJECTS];
        float[] objectDistanceAtt = new float[Definitions.NO_OF_OBJECTS];
        float shortestPath = Definitions.COST_INFINITY;
        float interest = 0.0f;
        float bestObjectiveInterest = 0.0f;
        int bestObjective = 0;

        // Preinitialize some required variables
        for (int i = 0; i < objectPath.Length; i++)
        {
            objectPath[i].cost = Definitions.COST_INFINITY;
        }

        // Precalculate interest & routes
        for (int i = 0; i < manager.catObjects.Count; i++)
        {
            // Get base object interest
            interest = manager.catObjects[i].attractiveness;

            // Apply personality modifier and get optimal paths
            switch (manager.catObjects[i].objName)
            {
                case "ASH URN":
                    interest = interest * personality.appealModifiers[Definitions.ASH_URN_IDX];
                    objectInterest[Definitions.ASH_URN_IDX] = interest;
                    objectPath[Definitions.ASH_URN_IDX] = GetOptimalPathToCell(manager.catObjects[i].cellPosition);
                    break;

                case "AQUARIUM":
                    interest = interest * personality.appealModifiers[Definitions.AQUARIUM_IDX];
                    objectInterest[Definitions.AQUARIUM_IDX] = interest;
                    objectPath[Definitions.AQUARIUM_IDX] = GetOptimalPathToCell(manager.catObjects[i].cellPosition);
                    break;

                case "FAVORITE JACKET":
                    interest = interest * personality.appealModifiers[Definitions.FAVORITE_JACKET_IDX];
                    objectInterest[Definitions.FAVORITE_JACKET_IDX] = interest;
                    objectPath[Definitions.FAVORITE_JACKET_IDX] = GetOptimalPathToCell(manager.catObjects[i].cellPosition);
                    break;

                case "COMPUTER":
                    interest = interest * personality.appealModifiers[Definitions.COMPUTER_IDX];
                    objectInterest[Definitions.COMPUTER_IDX] = interest;
                    objectPath[Definitions.COMPUTER_IDX] = GetOptimalPathToCell(manager.catObjects[i].cellPosition);
                    break;

                case "JASMINE":
                    interest = interest * personality.appealModifiers[Definitions.JASMINE_IDX];
                    objectInterest[Definitions.JASMINE_IDX] = interest;
                    objectPath[Definitions.JASMINE_IDX] = GetOptimalPathToCell(manager.catObjects[i].cellPosition);
                    break;

                case "AIR FRESHENER":
                    interest = interest * personality.appealModifiers[Definitions.AIR_FRESHENER_IDX];
                    objectInterest[Definitions.AIR_FRESHENER_IDX] = interest;
                    objectPath[Definitions.AIR_FRESHENER_IDX] = GetOptimalPathToCell(manager.catObjects[i].cellPosition);
                    break;

                case "CATNIP":
                    interest = interest * personality.appealModifiers[Definitions.CATNIP_IDX];
                    objectInterest[Definitions.CATNIP_IDX] = interest;
                    objectPath[Definitions.CATNIP_IDX] = GetOptimalPathToCell(manager.catObjects[i].cellPosition);
                    break;

                case "VACUUM CLEANER":
                    interest = interest * personality.appealModifiers[Definitions.VACUUM_CLEANER_IDX];
                    objectInterest[Definitions.VACUUM_CLEANER_IDX] = interest;
                    objectPath[Definitions.VACUUM_CLEANER_IDX] = GetOptimalPathToCell(manager.catObjects[i].cellPosition);
                    break;

                case "BAG OF TREATS":
                    interest = interest * personality.appealModifiers[Definitions.BAG_OF_TREATS_IDX];
                    objectInterest[Definitions.BAG_OF_TREATS_IDX] = interest;
                    objectPath[Definitions.BAG_OF_TREATS_IDX] = GetOptimalPathToCell(manager.catObjects[i].cellPosition);
                    break;
            }
        }

        // Get shortest path
        for (int i = 0; i < objectPath.Length; i++)
        {
            if (objectPath[i].cost < shortestPath)
            {
                shortestPath = objectPath[i].cost;
            }
        }

        // Make proportional factors
        for (int i = 0; i < objectDistanceAtt.Length; i++)
        {
            objectDistanceAtt[i] = shortestPath / objectPath[i].cost;
        }

        // Apply distance attenuation and get highest interest
        for (int i = 0; i < objectInterest.Length; i++)
        {
            objectInterest[i] = objectInterest[i] * objectDistanceAtt[i];

            if (objectInterest[i] > bestObjectiveInterest)
            {
                bestObjectiveInterest = objectInterest[i];
                bestObjective = i;
            }
        }

        // Take memory into account
        if (memory != Definitions.OBJECTIVE_EMPTY)
        {
            if (objectInterest[memory] + (objectInterest[memory] * personality.focus[totd]) > bestObjectiveInterest)
            {
                objective = memory;
            }
            else
            {
                objective = bestObjective;
            }
        }
        else
        {
            objective = bestObjective;
        }

        return objective;
    }

    Path getObjectivePath(int objective)
    {
        CatObject toFind = new CatObject();

        switch (objective)
        {
            case Definitions.ASH_URN_IDX:
                toFind = manager.catObjects.Find(x => x.objName == "ASH URN");
                break;

            case Definitions.AQUARIUM_IDX:
                toFind = manager.catObjects.Find(x => x.objName == "AQUARIUM");
                break;

            case Definitions.FAVORITE_JACKET_IDX:
                toFind = manager.catObjects.Find(x => x.objName == "FAVORITE JACKET");
                break;

            case Definitions.COMPUTER_IDX:
                toFind = manager.catObjects.Find(x => x.objName == "COMPUTER");
                break;

            case Definitions.JASMINE_IDX:
                toFind = manager.catObjects.Find(x => x.objName == "JASMINE");
                break;

            case Definitions.AIR_FRESHENER_IDX:
                toFind = manager.catObjects.Find(x => x.objName == "AIR FRESHENER");
                break;

            case Definitions.CATNIP_IDX:
                toFind = manager.catObjects.Find(x => x.objName == "CATNIP");
                break;

            case Definitions.VACUUM_CLEANER_IDX:
                toFind = manager.catObjects.Find(x => x.objName == "VACUUM CLEANER");
                break;

            case Definitions.BAG_OF_TREATS_IDX:
                toFind = manager.catObjects.Find(x => x.objName == "BAG OF TREATS");
                break;
        }
        
        return GetOptimalPathToCell(toFind.cellPosition);
    }

    int GetNextCell(Path objectivePath)
    {
        int nextCell;
        int roundedStamina;

        if (isAsleep)
        {
            nextCell = cellPosition;
        }
        else
        {
            if (isLoker)
            {
                roundedStamina = Mathf.CeilToInt(personality.stamina[totd] * Definitions.LOKER_INCREASE_FACTOR);
            }
            else
            {
                roundedStamina = Mathf.CeilToInt(personality.stamina[totd]);
            }
            
            nextCell = objectivePath.route[roundedStamina];
        }

        return nextCell;
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
            neighbors = manager.GetAdjacentCellsCapped(currentNode);

            // Eliminate visited neighbors
            for (int i = 0; i < visitedNodes.Count; i++)
            {
                if (neighbors.Contains(visitedNodes[i]))
                {
                    neighbors.Remove(visitedNodes[i]);
                }
            }

            // Eliminate occupied cells which are not the current cell and the destination cell
            for (int i = 0; i < manager.cells.Length; i++)
            {
                if (manager.cells[i].occupied && !(i == cell) && !(i == cellPosition))
                {
                    if (neighbors.Contains(i))
                    {
                        neighbors.Remove(i);
                    }
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

        // Reorder route array
        int j = 0;
        for (int i = temporaryRouteSize - 1; i >= 0; i--)
        {
            optimalPath.route[i] = temporaryRoute[j++];
        }

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
