using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Definitions
{
    // Define constant values here

    // Generic constants
    public const int NO_OF_VALUABLE_OBJS = 5;
    public const int NO_OF_EFFECT_OBJS = 4;
    public const int NO_OF_OBJECTS = NO_OF_VALUABLE_OBJS + NO_OF_EFFECT_OBJS;
    public const int NO_OF_TOTD_SLOTS = 4;

    // Board related constants
    public const int BOARD_SIZE = 8;
    public const int NO_OF_BOARD_CELLS = BOARD_SIZE ^ 2;
    public const int NO_OF_ADJACENT_CELLS = 8;

    // Cat related constants
    public const int NODE_NOT_IN_LIST = 30000;
    public const int NODE_DIST_INFINITY = 30001;
    public const int MAX_DIJKSTRA_ITERATIONS = 30000;

    // Cat personality constants
    public const int NO_OF_FUR_COLORS = 1;
    public const float MAX_STAMINA_BASE = 1.0f;
    public const float MIN_STAMINA_BASE = 0.8f;
    public const float MAX_STAMINA_MOD = 1.2f;
    public const float MIN_STAMINA_MOD = 0.8f;
    public const float MAX_SLEEP_BASE = 1.0f;
    public const float MIN_SLEEP_BASE = 0.8f;
    public const float MAX_SLEEP_MOD = 1.2f;
    public const float MIN_SLEEP_MOD = 0.8f;
    public const float MAX_LOKER_BASE = 1.0f;
    public const float MIN_LOKER_BASE = 0.8f;
    public const float MAX_LOKER_MOD = 1.2f;
    public const float MIN_LOKER_MOD = 0.8f;
    public const float MAX_APPEAL_MOD = 1.2f;
    public const float MIN_APPEAL_MOD = 0.8f;

    // Object indexes
    public const int ASH_URN_IDX = 0;
    public const int AQUARIUM_IDX = 1;
    public const int FAVORITE_JACKET_IDX = 2;
    public const int COMPUTER_IDX = 3;
    public const int JASMINE_IDX = 4;
    public const int AIR_FRESHENER_IDX = 5;
    public const int CATNIP_IDX = 6;
    public const int VACUUM_CLEANER_IDX = 7;
    public const int BAG_OF_TREATS_IDX = 8;

    // Texts
    public const string SELECT_TXT = "SELECT AN OBJECT TO MOVE OR PLACE A NEW ONE.";
    public const string CHOOSE_TXT = "CHOOSE A LOCATION. PRESS AGAIN ON THE OBJECT TO CANCEL.";
    public const string PLACE_TXT = "PLACE THE OBJECT YOU'VE CHOSEN.";
    public const string CAT_TXT = "IT'S THE CAT'S TURN.";

    // Speeds
    public const float CATOBJ_SPD = 0.75f;
}
