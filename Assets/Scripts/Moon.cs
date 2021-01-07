using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class Moon : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float bounceX = 2f;
    [SerializeField] float bounceY = 5f;
    [SerializeField] float baddieKick = 25f;
    //Both
    Rigidbody2D myRigidBody;
    Animator moonAnimator;

    //Half
    CapsuleCollider2D feet;
    CapsuleCollider2D hurtBox;
    CapsuleCollider2D sides;

    //Full
    CapsuleCollider2D rollBall;
    CapsuleCollider2D rollHurtBox;


    LayerMask floor;
    LayerMask pain;
    LayerMask box;
    LayerMask hit;
    
    bool moonIsFull;


    void Start()
    {
        //Both
        myRigidBody = GetComponent<Rigidbody2D>();
        myRigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        moonAnimator = GetComponent<Animator>();

        //Half
        feet = transform.Find("Feet").GetComponent<CapsuleCollider2D>();
        hurtBox = transform.Find("HurtBox").GetComponent<CapsuleCollider2D>();
        sides = transform.Find("Sides").GetComponent<CapsuleCollider2D>();

        //Full
        rollBall = transform.Find("RollBall").GetComponent<CapsuleCollider2D>();
        rollHurtBox = transform.Find("RollHurtBox").GetComponent<CapsuleCollider2D>();


        floor = LayerMask.GetMask("ForeGround");
        pain = LayerMask.GetMask("Pain");
        box = LayerMask.GetMask("Box");
        hit = LayerMask.GetMask("BaddieHit");
        moonIsFull = false;
    }


    void FixedUpdate()
    {
        if (!moonIsFull) 
        {
            Run();
            Jump();
            JumpSprite();
            FlipSprite();
            Bounce();
            ChangeTo(); 

        }
        else 
        {
            Run();
            FlipSprite();
            Bounce();
        }
    }

    public void BaddieHit(float baddieDirection)
    {
        Vector2 deathBonk = new Vector2(baddieDirection * baddieKick, baddieKick);
        myRigidBody.velocity = deathBonk;
        
    }

    public bool ChangeBack()
    {
        if (moonIsFull)
        {
            moonAnimator.SetTrigger("isHalf");
            moonIsFull = false;
            feet.enabled = true;
            sides.enabled = true;
            hurtBox.enabled = true;
            rollBall.enabled = false;
            rollHurtBox.enabled = false;
            transform.rotation = Quaternion.identity;
            myRigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
            return true;
        }
        else { return false; }
    }

    private void Bounce()
    {
        float xSign = Mathf.Sign(myRigidBody.velocity.x);
        Vector2 bounceVelocity = new Vector2(xSign * bounceX, bounceY);

        if ((hurtBox.IsTouchingLayers(pain)) || (rollHurtBox.IsTouchingLayers(pain))) 
        { 
            myRigidBody.velocity = bounceVelocity; 
        } 
    }

    private void JumpSprite()
    {
        bool isJumping = (feet.IsTouchingLayers(floor))||(feet.IsTouchingLayers(box));
        moonAnimator.SetBool("isJumping", !isJumping);
    }

    private void Run()
    {
        float controllerInput = Input.GetAxis("Horizontal");
        Vector2 runVelocity = new Vector2(controllerInput * moveSpeed, myRigidBody.velocity.y);
        myRigidBody.velocity = runVelocity;
        bool isRunning = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        moonAnimator.SetBool("isRunning", isRunning);
    }

    private void FlipSprite()
    {
        bool isRunning = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        if (isRunning)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x),1f);
        }
    }

    private void Jump()
    {
        if (feet.IsTouchingLayers(floor)||feet.IsTouchingLayers(box))
        {
            if (Input.GetKeyDown("space"))
            {
                Vector2 jumpForce = new Vector2(myRigidBody.velocity.x, jumpSpeed);
                myRigidBody.velocity += jumpForce;
            }
            else { return; }
        } 
    }

    public void ChangeTo()
    {
        
        if (hurtBox.IsTouchingLayers(pain)||hurtBox.IsTouchingLayers(hit)) 
        {
            BecomeFull();  
        }
        else { return; }
    }

    public void BecomeFull()
    {
        moonAnimator.SetTrigger("isFull");
        moonIsFull = true;
        feet.enabled = false;
        sides.enabled = false;
        hurtBox.enabled = false;
        rollBall.enabled = true;
        rollHurtBox.enabled = true;
        myRigidBody.constraints = RigidbodyConstraints2D.None;
    }

}
