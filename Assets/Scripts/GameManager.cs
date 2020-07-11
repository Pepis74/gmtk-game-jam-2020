using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    Transform cellParent;
    [SerializeField]
    Transform objectParent;
    [SerializeField]
    GameObject cell;
    GameObject ins;
    [SerializeField]
    Vector2 oGCellPos;
    public Cell[] cells = new Cell[64];
    [SerializeField]
    GameObject[] objectPrefabs;
    public List<CatObject> catObjects = new List<CatObject>();
    int crescendoA;
    int randomInt;
    int posValue;
    
    void Start()
    {
        #region Instantiate Cells

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
            oGCellPos.x = Mathf.Round(oGCellPos.x * 100) / 100;
            oGCellPos.y -= 0.06f;
            oGCellPos.y = Mathf.Round(oGCellPos.y * 100) / 100;
            ins.transform.localPosition = oGCellPos;
            ins.transform.localScale = Vector3.one;
            cells[i] = ins.GetComponentInChildren<Cell>();
            cells[i].posValue = posValue;
            posValue += 1;
            crescendoA += 1;
        }

        #endregion

        #region Instantiate Valuables

        for(int i = 0; i < objectPrefabs.Length; i++)
        {
            do
            {
                randomInt = Random.Range(31, 63);
            }
            while (cells[randomInt].occupied);

            ins = Instantiate(objectPrefabs[i], Vector2.zero, objectPrefabs[i].transform.rotation);
            ins.transform.parent = objectParent;
            ins.transform.localPosition = new Vector2(Mathf.Round(cells[randomInt].transform.localPosition.x * 100) / 100, (Mathf.Round(cells[randomInt].transform.localPosition.y * 100) / 100) + 0.03f);
            ins.transform.localScale = Vector3.one;
            ins.GetComponent<CatObject>().cellPosition = randomInt;
            cells[randomInt].occupied = true;
            catObjects.Add(ins.GetComponent<CatObject>());
        }

        #endregion
    }
}
