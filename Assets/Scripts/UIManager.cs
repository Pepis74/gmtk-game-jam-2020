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
    [SerializeField]
    GameObject phone;
    [SerializeField]
    GameObject endScreen;
    public GameObject[] toActivate;
    public Text stars;
    GameManager manager;
    bool[] inventoryAvailable;

    void Start()
    {
        manager = FindObjectOfType<GameManager>();

        /*for (int i = 0; i < inventoryAvailable.Length; i++)
        {
            inventoryAvailable[i] = true;
        }*/
    }

    public void StartGame()
    {
        phone.SetActive(false);

        for (int i = 0; i < toActivate.Length; i++)
        {
            toActivate[i].SetActive(true);
        }

        for (int i = 0; i < manager.catObjects.Count; i++)
        {
            manager.catObjects[i].GetComponent<Collider2D>().enabled = true;
        }
    }

    public void CloseDesc()
    {
        for (int i = 0; i < manager.catObjects.Count; i++)
        {
            manager.catObjects[i].GetComponent<Collider2D>().enabled = true;
        }

        description.SetActive(false);
    }

    public void EndGame(int valuablesLeft)
    {
        for (int i = 0; i < toActivate.Length; i++)
        {
            toActivate[i].SetActive(false);
        }

        stars.text = "POINTS: " + valuablesLeft + "/5";

        endScreen.SetActive(true);
    }
}
