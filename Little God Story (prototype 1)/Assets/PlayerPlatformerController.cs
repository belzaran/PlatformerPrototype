using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatformerController : PhysicsObject
{
    public float runSpeed = 7;
    public float jumpSpeed = 7;
    public float airControl = 0.5f;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
   
    
    void Awake(){
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

        void Start()
    {
        
    }

    protected override void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;
        if(isGrounded){
            move.x = Input.GetAxis("Horizontal");
        }
        else{
            move.x = velocity.x/runSpeed + (Input.GetAxis("Horizontal"))*airControl;
            Debug.Log("move.x :" + move.x);
    
            if(move.x > 1){
                move.x = 1;               
            }
            else if (move.x < -1){
                move.x = -1;
            }
        }
        

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = jumpSpeed;

        }
        else if(Input.GetButtonUp("Jump"))
        {
            if(velocity.y > 0){
                velocity.y = velocity.y * 0.5f;
            }
            
        }

        if(!isGrounded){
            //move.x = move.x * airControl;
            
        }

            targetVelocity = move * runSpeed;
        
    }

}
