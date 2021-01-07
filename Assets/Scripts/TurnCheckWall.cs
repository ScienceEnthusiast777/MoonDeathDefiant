using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnCheckWall : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D bump)
    {
        transform.parent.GetComponent<Baddie>().Turn();
        Debug.Log("should turn because of collision");
    }
}
