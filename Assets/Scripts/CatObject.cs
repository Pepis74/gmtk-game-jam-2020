﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatObject : MonoBehaviour
{
    public string objName;
    [TextArea(3, 10)]
    public string descGameplay;
    [TextArea(3, 10)]
    public string descFlavor;
    [SerializeField]
    int movementType;
    public int internalMovementType;
    public int cellPosition;
    public int cellToMoveTo;
    public int attractiveness;
    public float yOffset;
    public float xOffset;
    public GameManager manager;
    Rigidbody2D rig;
    UIManager uI;
    Animator anim;
    public bool move;
    bool mouseOver;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        manager = FindObjectOfType<GameManager>();
        uI = FindObjectOfType<UIManager>();
        rig = GetComponent<Rigidbody2D>();
        internalMovementType = movementType;
        CloneMovementType();
    }

    void Update()
    {
        #region Right Click

        if(mouseOver && Input.GetKeyDown(KeyCode.Mouse1))
        {
            manager.buttonSound.Play();
            uI.title.text = objName;
            uI.descGameplay.text = descGameplay;
            uI.descFlavor.text = descFlavor;

            for (int i = 0; i < manager.catObjects.Count; i++)
            {
                manager.catObjects[i].GetComponent<Collider2D>().enabled = false;
            }

            uI.description.SetActive(true);
        }

        #endregion

        #region Move

        if (move)
        {
            transform.localPosition = Vector3.MoveTowards(new Vector3(transform.localPosition.x, transform.localPosition.y, manager.cells[cellPosition].zOffset), new Vector3(manager.cells[cellToMoveTo].transform.localPosition.x + xOffset, manager.cells[cellToMoveTo].transform.localPosition.y + yOffset, manager.cells[cellPosition].zOffset), Definitions.CATOBJ_SPD * Time.deltaTime);
            transform.localPosition = new Vector3(Mathf.Round(transform.localPosition.x * 1000) / 1000, Mathf.Round(transform.localPosition.y * 1000) / 1000, transform.localPosition.z);

            if(transform.localPosition == new Vector3(manager.cells[cellToMoveTo].transform.localPosition.x + xOffset, manager.cells[cellToMoveTo].transform.localPosition.y + yOffset, transform.localPosition.z))
            {
                move = false;
                manager.cells[cellPosition].occupied = false;
                cellPosition = cellToMoveTo;
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, manager.cells[cellPosition].zOffset);
                manager.cells[cellPosition].occupied = true;
                manager.EndTurn();
            }
        }

        #endregion
    }

    void OnMouseOver()
    {
        mouseOver = true;

        if(!manager.cellParent.gameObject.activeSelf)
        {
            anim.SetBool("start", true);
        }    
    }

    void OnMouseExit()
    {
        mouseOver = false;

        if (!manager.cellParent.gameObject.activeSelf)
        {
            anim.SetBool("start", false);
        }
    }

    void OnMouseDown()
    {
        manager.EnableCells(this, movementType);
        manager.buttonSound.Play();
    }

    public void CloneMovementType()
    {
        int tempDistance;
        int lowestDistance = Definitions.NODE_DIST_INFINITY;
        int closestValuable = Definitions.OBJECTIVE_EMPTY;
        Valuable[] valuables;

        if (internalMovementType == 5)
        {
            valuables = FindObjectsOfType<Valuable>();

            for (int i = 0; i < valuables.Length; i++)
            {
                tempDistance = manager.GetDistanceBetweenCells(cellPosition, valuables[i].cellPosition);
                if (tempDistance < lowestDistance && tempDistance != 0)
                {
                    lowestDistance = tempDistance;
                    closestValuable = i;
                }
            }

            if (closestValuable == Definitions.OBJECTIVE_EMPTY)
            {
                movementType = 4;
            }
            else
            {
                movementType = valuables[closestValuable].movementType;
            }
        }
    }
}
