using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public bool occupied;
    public int posValue;
    public float zOffset;
    GameManager manager;
    CatObject toFind;
    Animator anim;
    bool move;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        manager = FindObjectOfType<GameManager>();
    }

    void OnMouseOver()
    {
        anim.SetBool("start", true);
        toFind = manager.catObjects.Find(x => x.cellPosition == posValue);

        if(toFind != null)
        {
            toFind.GetComponentInChildren<Animator>().SetBool("start", true);
        }
    }

    void OnMouseExit()
    {
        anim.SetBool("start", false);
        anim.SetBool("jump", false);
        toFind = manager.catObjects.Find(x => x.cellPosition == posValue);

        if (toFind != null)
        {
            toFind.GetComponentInChildren<Animator>().SetBool("start", false);
        }
    }

    void OnMouseDown()
    {
        if(manager.objectToMove.cellPosition == posValue)
        {
            manager.DisableCells();
        }

        else if(GetComponentInChildren<SpriteRenderer>().color == manager.blue)
        {
            manager.cellParent.gameObject.SetActive(false);

            manager.objectToMove.cellToMoveTo = posValue;
            manager.objectToMove.move = true;
        }
    }
}
