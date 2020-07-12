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
    public GameObject[] toActivate;
    GameManager manager;

    void Start()
    {
        manager = FindObjectOfType<GameManager>();
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
}
