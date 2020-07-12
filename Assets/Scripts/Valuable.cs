using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Valuable : CatObject
{
    [SerializeField]
    GameObject destructionPar;

    public void CatInteraction()
    {
        manager.valuablesLeft -= 1;
        manager.cells[cellPosition].occupied = false;
        manager.catObjects.Remove(this);
        Instantiate(destructionPar, transform.position, destructionPar.transform.rotation);

        if (manager.valuablesLeft == 0)
        {
            manager.GameOver();
        }

        Destroy(gameObject);
    }
}
