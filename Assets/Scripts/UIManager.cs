using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text title;
    public Text descGameplay;
    public Text descFlavor;
    public GameObject description;
    GameManager manager;

    void Start()
    {
        manager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CloseDesc()
    {
        for (int i = 0; i < manager.catObjects.Count; i++)
        {
            manager.catObjects[i].GetComponent<Collider2D>().enabled = false;
        }

        description.SetActive(false);
    }
}
