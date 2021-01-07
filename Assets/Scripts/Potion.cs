using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var moon = FindObjectOfType<Moon>();
        if (moon.ChangeBack()){ Destroy(gameObject); }   
    }
}
