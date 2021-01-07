using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnCheckFloor : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D platform)
    {
        transform.parent.GetComponent<Baddie>().Turn();
        Debug.Log("should turn because of floor");
    }
}
