using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovement : MonoBehaviour
{
    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;
    Animator playerAnimation;
    float gravityScaleAtStart;
    bool isAlive = true;


    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 1f;
    [SerializeField] Vector2 deathKick = new Vector2(10f, 0f);
    [SerializeField] GameObject Bullet;
    [SerializeField] Transform gun;
    

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        playerAnimation = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        gravityScaleAtStart = myRigidbody.gravityScale;
        myFeetCollider = GetComponent<BoxCollider2D>();
    }


    void Update()
    {
        if (!isAlive) { return; }
        Run();
        FlipSprite();
        Climb();
        Die();
        
    }
    
    void OnFire(InputValue value)
    {
        if (!isAlive) { return; }
        
        if (value.isPressed)
        {
            playerAnimation.SetTrigger("Shoot");
            Instantiate(Bullet, gun.position, transform.rotation);
        }
       
    }
 

    void OnMove(InputValue value)
    {
        if (!isAlive) { return; }
        moveInput = value.Get<Vector2>();
        Debug.Log(moveInput);

    }
   

    void OnJump(InputValue value)
    {
        if (!isAlive) { return; }
        if (value.isPressed && myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))      
        {
            myRigidbody.velocity += new Vector2(0f, jumpSpeed);
            
        }
        if (value.isPressed && myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")))      
        {
            myRigidbody.velocity += new Vector2(0f, jumpSpeed);
            
        }
        
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity;

        bool PlayerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        playerAnimation.SetBool("isRunning", PlayerHasHorizontalSpeed);

    }

    void FlipSprite()
    {
        bool PlayerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;

        if (PlayerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x), 1f);
        }
    
    }
   void Climb() 
    {
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            myRigidbody.gravityScale = gravityScaleAtStart;
            
            return; 
        }
        
        Vector2 climbVelocity = new Vector2(myRigidbody.velocity.x, moveInput.y * climbSpeed);
        myRigidbody.velocity = climbVelocity;
        myRigidbody.gravityScale = 0f;
       
        bool PlayerIsClimbing = MathF.Abs(myRigidbody.velocity.y) > Mathf.Epsilon;
        playerAnimation.SetBool("isClimbing", PlayerIsClimbing);
    }

    void Die()
    {
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies","Hazards")))
        {
            isAlive = false;
            playerAnimation.SetTrigger("Death");
            myRigidbody.velocity = deathKick;
        }
    }

    

}
