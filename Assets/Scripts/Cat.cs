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

    int[] GetAdjacentCells(int cell)
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
