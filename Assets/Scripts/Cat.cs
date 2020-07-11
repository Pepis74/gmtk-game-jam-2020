using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : MonoBehaviour
{
    // Personality structure
    struct Personality
    {
        public int[] stamina;
    }

    // Personality
    Personality pers = new Personality();
    // Cell where the cat is located
    public int position;
    // Max movement range of the cat. Personality Stat v
    public int stamina;
    // 
    public float sleep_chances;
    public float locker_chances;
    public int[] personality_mods;
    public int[][] totd_mods;
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GeneratePersonality()
    {
        // TODO
        pers.stamina = new int[Definitions.NO_OF_TOTD_SLOTS];
    }

    void UpdateStatsTotd()
    {
        // TODO
    }

    int GetNextCell(int position, int stamina)
    {
        // TODO 
        return 0;
    }


}
