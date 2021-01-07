using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
     transform.parent.GetComponent<Baddie>().FOVSpotted();
    }
}
