using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baddie : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float attackSpeed = 5f;
    
    Rigidbody2D baddieRigidbody;
    Animator baddieAnimator;
    CapsuleCollider2D feet; 
    Moon moon;
    float moonRelativeYPos;
    float moonRelativeXPos;
    Vector2 standingStill;
    Vector2 resting;

    LayerMask floor;
    LayerMask box;


    public enum State { Walking, Standing, Attacking }
    State baddieState;



    private void Start()
    {
        baddieRigidbody = GetComponent<Rigidbody2D>();
        baddieAnimator = GetComponent<Animator>();

        feet = transform.Find("Feet").GetComponent<CapsuleCollider2D>();
            
        moon = FindObjectOfType<Moon>();
        standingStill = new Vector2(0, 0);
        resting = new Vector2(0,-5);

        floor = LayerMask.GetMask("ForeGround");
        box = LayerMask.GetMask("Box");

        baddieState = State.Walking;

    }

    private void FixedUpdate()
    {
        switch (baddieState)
        {
            case State.Walking:

                if(baddieRigidbody.velocity == standingStill) { Turn(); }
                Walk();
                Spot();
                break;

            case State.Standing:

                Spot();
                StandAndStare();
                break;

            case State.Attacking:

                StartCoroutine(Attack());
                break;

            default:
                baddieState = State.Walking;
                break;
        }
    }

    private void Update()
    {
        //Debug.Log($"the baddie velocity is {baddieRigidbody.velocity}");
    }

    private void Walk()
    {
        baddieAnimator.SetTrigger("isWalking");
        if (transform.localScale.x >= 0)
        {
            baddieRigidbody.velocity = new Vector2(moveSpeed, 0f);
        }
        else
        {
            baddieRigidbody.velocity = new Vector2(-moveSpeed, 0f);
        }
    }

    public void Turn()
    {
        if(!(baddieState == State.Attacking))
        {
            if (feet.IsTouchingLayers(floor) || feet.IsTouchingLayers(box))
            {
                //transform.localScale = new Vector2(-(Mathf.Sign(baddieRigidbody.velocity.x)), 1f);
                transform.localScale = new Vector2(-(Mathf.Sign(transform.localScale.x)), 1f);
            }
        }  
    }
        
    private void Spot()
    {
        moonRelativeYPos = moon.transform.position.y - transform.position.y;
        if (Mathf.Abs(moonRelativeYPos) < 0.5 && feet.IsTouchingLayers(floor))    
        {
            baddieState = State.Standing;
        }
        else {baddieState = State.Walking; }
        
    }

    private void StandAndStare()
    {
        moonRelativeXPos = transform.position.x - moon.transform.position.x;
        baddieRigidbody.velocity = standingStill;
        baddieAnimator.SetTrigger("isStanding");
        transform.localScale = new Vector2(-(Mathf.Sign(moonRelativeXPos)), 1f);
    }

    private IEnumerator Attack()
    {
        if (feet.IsTouchingLayers(floor))
        {
            baddieAnimator.SetTrigger("isAttacking");
            if (transform.localScale.x >= 0)
            {
                baddieRigidbody.velocity = new Vector2(attackSpeed, 0f);
            }
            else
            {
                baddieRigidbody.velocity = new Vector2(-attackSpeed, 0f);
            }

            yield return new WaitForSeconds(2);
            baddieRigidbody.velocity += resting;
            baddieState = State.Walking;
        }
        
    }

    public void FOVSpotted()
    {
        baddieState = State.Attacking; 
    }

}
