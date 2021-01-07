using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaddieHitCheck : MonoBehaviour
{
   
    private void OnTriggerEnter2D(Collider2D potentialBaddie)
    {
        if (potentialBaddie.gameObject.layer == 16) 
        {
            float baddieDirection = Mathf.Sign(transform.position.x - potentialBaddie.transform.position.x);
            transform.parent.GetComponent<Moon>().BaddieHit(-baddieDirection);
        }
    }
}
