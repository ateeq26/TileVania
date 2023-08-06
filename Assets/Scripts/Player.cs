using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D myRigidBody;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeet;

    float initialGravityScale;
    bool isAlive;

    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] Vector2 deathKick = new Vector2(100f, 100f);

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeet = GetComponent<BoxCollider2D>();
        initialGravityScale = myRigidBody.gravityScale;
        isAlive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isAlive) { return;}

        Run();
        Jump();
        ClimbLadder();
        FlipSprite();
        Die();
    }

    void Run()
    {
        float controlThrow = Input.GetAxis("Horizontal");
        Vector2 playerVelocity = new Vector2(controlThrow * runSpeed, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("Running", playerHasHorizontalSpeed);

    }


    void ClimbLadder()
    {

        if(!myFeet.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            myAnimator.SetBool("Climbing", false);
            myRigidBody.gravityScale = initialGravityScale;    
            return; 
        }


        float controlThrow = Input.GetAxis("Vertical");
        Vector2 playerVelocity = new Vector2(myRigidBody.velocity.x, controlThrow * climbSpeed);
        myRigidBody.velocity = playerVelocity;
        myRigidBody.gravityScale = 0f;

        bool playerHasVerticalSpeed = Mathf.Abs(myRigidBody.velocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("Climbing", playerHasVerticalSpeed);

    }


    void Jump()
    {

        if(!myFeet.IsTouchingLayers(LayerMask.GetMask("Ground", "Ladder"))){return; }

        if (Input.GetButtonDown("Jump"))
        {
            Vector2 jumpVelocity = new Vector2(0, jumpSpeed);
            myRigidBody.velocity += jumpVelocity;
        }
    
    }

    void Die()
    {
        if(myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy","Hazards")) || myFeet.IsTouchingLayers(LayerMask.GetMask("Hazards")) )
        {
            isAlive = false;
            // set animation
            myAnimator.SetTrigger("Dying");
            // give player velocity
            myRigidBody.velocity = deathKick;
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }

    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        if(playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x), 1f);
        }
    }
}
