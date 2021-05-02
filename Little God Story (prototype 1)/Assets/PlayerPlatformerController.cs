using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatformerController : PhysicsObject
{
    public float maxSpeed = 7;
    public float moveSpeed = 200;
    public float jumpSpeed = 7;
    public float airControl = 1;

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
            move.x = velocity.x/maxSpeed;
        }
        
        //Debug.Log(move.x);

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

            targetVelocity = move * maxSpeed;
        
    }

}
