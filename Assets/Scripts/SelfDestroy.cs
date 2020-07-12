using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    public float seconds;

    void Start()
    {
        StartCoroutine(SelfDestroyCo());
    }

    IEnumerator SelfDestroyCo()
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }
}
