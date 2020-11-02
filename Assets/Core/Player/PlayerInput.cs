using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float horizontalInput = 0;
    public float verticalInput = 0;
    public bool isGrabLedgeButtonDown = false;
    public bool isGrounded = false;
    public bool isUnderObject = false;
    public bool isJumpButtonDown = false;
    public bool isInLedgeTrigger;
    public GameObject ledgeToGrab = null;

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

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        isGrabLedgeButtonDown = Input.GetButton("GrabLedge");
        isJumpButtonDown = Input.GetButton("Jump");
        isUnderObject = isUnderObjectCheck();
        isGrounded = isGroundedCheck();
    }

    private bool isGroundedCheck()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.1f, LayerMask.GetMask("Default"));
        if (hit.collider != null) { return true; }
        else { return false; }
    }

    private bool isUnderObjectCheck()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, 0.7f, LayerMask.GetMask("Default"));
        if (hit.collider != null) { return true; }
        else { return false; }
    }
}
