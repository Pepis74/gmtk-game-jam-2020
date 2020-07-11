using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [HideInInspector]
    public bool viable;
    [HideInInspector]
    public int cost;
    Animator anim;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseOver()
    {
        anim.SetBool("start", true);
    }

    void OnMouseExit()
    {
        anim.SetBool("start", false);
    }
}
