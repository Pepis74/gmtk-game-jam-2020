using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    Transform cellParent;
    [SerializeField]
    GameObject cell;
    GameObject ins;
    [SerializeField]
    Vector2 oGCellPos;
    Cell[] cells = new Cell[64];   
    int crescendoA;
    
    void Start()
    {
        oGCellPos = new Vector2(oGCellPos.x + 0.09f, oGCellPos.y + 0.06f);

        for (int i = 0; i < 64; i++)
        {
            if(crescendoA == 8)
            {
                crescendoA = 0;
                oGCellPos = new Vector2(oGCellPos.x + 0.09f * 9, oGCellPos.y + 0.06f * 7);
            }

            ins = Instantiate(cell, Vector2.zero, cell.transform.rotation);
            ins.transform.parent = cellParent;
            oGCellPos.x -= 0.09f;
            oGCellPos.y -= 0.06f;
            ins.transform.localPosition = oGCellPos;
            ins.transform.localScale = Vector3.one;
            cells[i] = ins.GetComponentInChildren<Cell>();
            crescendoA += 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
