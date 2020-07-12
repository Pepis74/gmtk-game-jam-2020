using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public bool occupied;
    public int posValue;
    public float zOffset;
    public float cost;
    GameManager manager;
    CatObject toFind;
    Animator anim;
    Cat cat;
    bool move;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        cat = FindObjectOfType<Cat>();
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

        else if(cat.cellPosition == posValue)
        {
            cat.transform.GetChild(0).GetComponent<Animator>().SetBool("start", true);
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

        else if (cat.cellPosition == posValue)
        {
            cat.transform.GetChild(0).GetComponent<Animator>().SetBool("start", false);
        }
    }

    void OnMouseDown()
    {
        if(manager.objectToMove.cellPosition == posValue)
        {
            manager.DisableCells();
            manager.buttonSound.Play();
        }

        else if(GetComponentInChildren<SpriteRenderer>().color == manager.blue)
        {
            manager.buttonSound.Play();
            manager.cellParent.gameObject.SetActive(false);

            manager.objectToMove.cellToMoveTo = posValue;
            manager.objectToMove.move = true;
        }
    }
}
