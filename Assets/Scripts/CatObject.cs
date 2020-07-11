using System.Collections;
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
    public int cellPosition;
    public int cellToMoveTo;
    GameManager manager;
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
    }

    void Update()
    {
        #region Right Click

        if(mouseOver && Input.GetKeyDown(KeyCode.Mouse1))
        {
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
            transform.localPosition = Vector3.MoveTowards(new Vector3(transform.localPosition.x, transform.localPosition.y, 0), new Vector3(manager.cells[cellToMoveTo].transform.localPosition.x, manager.cells[cellToMoveTo].transform.localPosition.y + 0.03f, 0), Definitions.CATOBJ_SPD * Time.deltaTime);

            if(transform.localPosition == manager.cells[cellToMoveTo].transform.localPosition)
            {
                move = false;
                manager.cells[cellPosition].occupied = false;
                cellPosition = cellToMoveTo;
                manager.cells[cellPosition].occupied = true;
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
    }
}
