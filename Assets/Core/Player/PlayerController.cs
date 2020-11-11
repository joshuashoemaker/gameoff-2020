using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]

public class PlayerController : MonoBehaviour

{
    private Rigidbody2D playerRigidBody;
    private CapsuleCollider2D playerCollider;
    private Animator playerAnimator;
    private GameObject ledgeToGrab = null;
    public string currentState;

    public float movementSpeed = 10.0f;
    public float climbSpeed = 10.0f;
    public float jumpForce = 20.0f;

    private bool isInLedgeTrigger;

    private delegate void FState();
    private FState stateMethod;

    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        playerAnimator = GetComponent<Animator>();
        stateMethod = new FState(Idle);
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.tag == "LedgeGrab") 
        {
            ledgeToGrab = collider.gameObject;
            isInLedgeTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "LedgeGrab")
        {
            isInLedgeTrigger = false;
        }
    }

    void Update ()
    {
        stateMethod();
    }

    private float GetHorizontalAxis ()
    {
        return Input.GetAxis("Horizontal");
    }

    private float GetVerticalAxis()
    {
        return Input.GetAxis("Vertical");
    }

    private void SetPlayerDirection ()
    {
        float horizontalVelocity = GetHorizontalAxis();

        if (Mathf.Abs(horizontalVelocity) < 0.2) { return; }
        if (horizontalVelocity >= 0.2) transform.rotation = Quaternion.AngleAxis(15, Vector3.up);
        if (horizontalVelocity <= -0.2) transform.rotation = Quaternion.AngleAxis(165, Vector3.up);
    }

    private bool isGrounded()
    {
        return Mathf.Abs(playerRigidBody.velocity.y) <= 0.001f;
    }

    private bool isUnderObject()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, 0.7f, LayerMask.GetMask("Default"));
        if (hit.collider != null) { return true; }
        else { return false; }
    }

    // States

    private void Idle ()
    {
        currentState = "Idle";
        float absoluteHorizontalAxis = Mathf.Abs(GetHorizontalAxis());
        
        // To Walking
        if (absoluteHorizontalAxis >= 0.2)
        {
            stateMethod = new FState(Walking);
            stateMethod();
        }

        // To Jump
        if (isGrounded() && Input.GetButtonDown("Jump"))
        {
            stateMethod = new FState(Jump);
            stateMethod();
        }

        // To Crouch
        if (GetVerticalAxis() <= -0.2f)
        {
            stateMethod = new FState(CrouchIdle);
            stateMethod();
        }
    }

    private void Walking ()
    {
        currentState = "Walking";
        SetPlayerDirection();
        float horizontalAxis = GetHorizontalAxis();
        transform.position += new Vector3(horizontalAxis, 0, 0) * Time.deltaTime * movementSpeed;

        // To Idle
        if (Mathf.Abs(horizontalAxis) <= 0.2)
        {
            stateMethod = new FState(Idle);
            stateMethod();
        }

        // To Jump
        if (isGrounded() && Input.GetButtonDown("Jump"))
        {
            stateMethod = new FState(Jump);
            stateMethod();
        }
    }

    private void Jump ()
    {
        currentState = "Jump";
        playerRigidBody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        stateMethod = new FState(Falling);
        stateMethod();
    }

    private void Falling ()
    {
        currentState = "Falling";
        float horizontalAxis = GetHorizontalAxis();
        transform.position += new Vector3(horizontalAxis, 0, 0) * Time.deltaTime * movementSpeed;

        // To Grabbing Ledge
        if (isInLedgeTrigger && Input.GetButton("Jump"))
        {
            stateMethod = new FState(GrabbingLedge);
            stateMethod();
        }

        // To Idle
        if (isGrounded())
        {
            stateMethod = new FState(Idle);
            stateMethod();
        }
    }

    private void GrabbingLedge ()
    {
        currentState = "GrabbingLedge";
        // To Falling if No Ledge
        if (ledgeToGrab == null)
        {
            playerRigidBody.gravityScale = 2.4f;
            stateMethod = new FState(Falling);
            stateMethod();
        }

        playerRigidBody.gravityScale = 0.0f;

        Vector2 GrabLocation = ledgeToGrab.GetComponent<LedgeGrabPoint>().GrabObject.transform.position;
        float distanceFromGrabLocation = Vector2.Distance(transform.position, GrabLocation);

        if (distanceFromGrabLocation >= 0.2)
        {
            transform.position = Vector2.MoveTowards(transform.position, GrabLocation, climbSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = new Vector2(GrabLocation.x, GrabLocation.y);
        }

        // To Climb Ledge
        if (Input.GetAxis("Vertical") >= 0.2)
        {
            playerRigidBody.gravityScale = 2.4f;
            stateMethod = new FState(ClimbLedge);
            stateMethod();
        }

        // To Falling
        if (!Input.GetButton("Jump"))
        {
            playerRigidBody.gravityScale = 2.4f;
            stateMethod = new FState(Falling);
            stateMethod();
        }
    }

    private void ClimbLedge ()
    {
        playerAnimator.SetBool("isClimbingLedge", true);
        currentState = "ClimbLedge";
        playerRigidBody.gravityScale = 0;
        playerCollider.isTrigger = true;

        Vector2 PullUpLocation = ledgeToGrab.GetComponent<LedgeGrabPoint>().PullUpObject.transform.position;
        transform.position = Vector2.MoveTowards(transform.position, PullUpLocation, climbSpeed * Time.deltaTime);

        bool finishedClimbing = Vector2.Distance(transform.position, PullUpLocation) <= 0.1f;

        if (finishedClimbing)
        {
            playerAnimator.SetBool("isClimbingLedge", false);
            playerRigidBody.gravityScale = 2.4f;
            playerCollider.isTrigger = false;
            stateMethod = new FState(Idle);
            stateMethod();
        }
    }

    private void CrouchIdle ()
    {
        currentState = "CrouchIdle";
        playerCollider.size = new Vector2(1, 1);
        playerCollider.offset = new Vector2(0, -0.5f);

        float absoluteHorizontalAxis = Mathf.Abs(GetHorizontalAxis());

        // To Walking
        if (absoluteHorizontalAxis >= 0.2 && GetVerticalAxis() >= -0.2 && !isUnderObject())
        {
            playerCollider.size = new Vector2(1, 2);
            playerCollider.offset = new Vector2(0, 0);
            stateMethod = new FState(Walking);
            stateMethod();
        }

        // To Idle
        if (GetVerticalAxis() >= -0.2 && !isUnderObject() && absoluteHorizontalAxis <= 0.2)
        {
            playerCollider.size = new Vector2(1, 2);
            playerCollider.offset = new Vector2(0, 0);
            stateMethod = new FState(Idle);
            stateMethod();
        }

        // To Crouch Walking
        if ((GetVerticalAxis() <= -0.2 || isUnderObject()) && absoluteHorizontalAxis >= 0.2)
        {
            stateMethod = new FState(CrouchWalking);
            stateMethod();
        }
    }

    private void CrouchWalking()
    {
        currentState = "CrouchWalking";
        playerCollider.size = new Vector2(1, 1);
        playerCollider.offset = new Vector2(0, -0.5f);

        SetPlayerDirection();
        float horizontalAxis = GetHorizontalAxis();
        float absoluteHorizontalAxis = Mathf.Abs(horizontalAxis);
        transform.position += new Vector3(horizontalAxis, 0, 0) * Time.deltaTime * movementSpeed;

        // To Idle
        if (GetVerticalAxis() >= -0.2 && !isUnderObject() && absoluteHorizontalAxis <= 0.2)
        {
            playerCollider.size = new Vector2(1, 2);
            playerCollider.offset = new Vector2(0, 0);
            stateMethod = new FState(Idle);
            stateMethod();
        }

        // To Crouch Idle
        if ((GetVerticalAxis() <= -0.2 || isUnderObject()) && absoluteHorizontalAxis <= 0.2)
        {
            stateMethod = new FState(CrouchIdle);
            stateMethod();
        }
    }
}
