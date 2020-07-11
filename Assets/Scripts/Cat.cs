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
    }

    // Personality
    private Personality personality = new Personality();
    // Cell where the cat is located
    public int cellPosition;

    void Start()
    {
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
        int[] unvisitedNodes = new int[Definitions.NO_OF_BOARD_CELLS];
        int[] visitedNodes = new int[Definitions.NO_OF_BOARD_CELLS];
        int[] tentativeDistance = new int[Definitions.NO_OF_BOARD_CELLS];
        int timeout;

        // Initialize arrays
        for (int i = 0; i < Definitions.NO_OF_BOARD_CELLS; i++)
        {
            unvisitedNodes[i] = i;
            visitedNodes[i] = Definitions.NODE_NOT_IN_LIST;
            tentativeDistance[i] = Definitions.NODE_DIST_INFINITY;
        }
        
        // Set tentative distance to current node to 0
        tentativeDistance[this.cellPosition] = 0;

        //while ()
        //{

        //}
        
        return optimalPath;
    }

}
