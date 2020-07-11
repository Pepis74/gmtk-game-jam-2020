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
    GameManager manager;
    Animator anim;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        manager = FindObjectOfType<GameManager>();
    }

    void OnMouseOver()
    {
        if(!manager.cellParent.gameObject.activeSelf)
        {
            anim.SetBool("start", true);
        }    
    }

    void OnMouseExit()
    {
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
