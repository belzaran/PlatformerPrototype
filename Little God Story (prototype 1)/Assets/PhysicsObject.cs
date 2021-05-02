using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour
{

    public float minGroundNormalY = .65f;
    public float gravityModifier = 1f;
    protected Vector2 targetVelocity; //for movement with input
    protected bool isGrounded;
    protected Vector2 groundNormal;
    protected Rigidbody2D rb2D;

    protected Vector2 velocity;

    ContactFilter2D contactFilter; // for Cast
    protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16]; // for Cast

    protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);

    protected const float minMoveDistance = 0.001f;
    protected const float shellRadius = 0.01f; // padding to be sure not going into another collider

    void OnEnable(){
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //contactFilter
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        contactFilter.useLayerMask = true;
    }

    // Update is called once per frame
    void Update()
    {
        targetVelocity = Vector2.zero;
        ComputeVelocity();
    }

    protected virtual void ComputeVelocity(){

    }

    void FixedUpdate(){
        velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;      
        velocity.x = targetVelocity.x;

        isGrounded = false;
        Vector2 deltaPosition = velocity* Time.deltaTime;
        Vector2 moveAlongGround = new Vector2 (groundNormal.y, -groundNormal.x);
        Vector2 move = moveAlongGround * deltaPosition.x;
        Movement(move, false);
        move = Vector2.up * deltaPosition.y;
        Movement(move, true);

    }

    void Movement(Vector2 move, bool yMovement){
        float distance = move.magnitude;

        if(distance > minMoveDistance){
            int count = rb2D.Cast (move, contactFilter, hitBuffer, distance + shellRadius);
            hitBufferList.Clear(); // remise à zéro avant de remplir la liste
            for(int i = 0; i < count; i++){
                hitBufferList.Add(hitBuffer[i]);
            }
                for(int i = 0; i < hitBufferList.Count; i++)
                {
                    Vector2 currentNormal = hitBufferList[i].normal;
                    if(currentNormal.y > minGroundNormalY)
                    {
                        isGrounded = true;
                        if(yMovement)
                        {
                            groundNormal = currentNormal;
                            currentNormal.x = 0;
                        }
                    }
                    
                float projection = Vector2.Dot (velocity, currentNormal);
                if(projection < 0)
                {
                    velocity = velocity - projection * currentNormal;
                }
                float modifiedDistance = hitBufferList [i].distance - shellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
                }
            }
            rb2D.position = rb2D.position + move.normalized * distance;
        }
    }

