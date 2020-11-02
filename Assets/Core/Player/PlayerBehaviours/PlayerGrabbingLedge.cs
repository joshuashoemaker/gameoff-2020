using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrabbingLedge : StateMachineBehaviour
{
    PlayerInput playerInput;
    PlayerStats playerStats;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerInput = animator.gameObject.GetComponent<PlayerInput>();
        playerStats = animator.gameObject.GetComponent<PlayerStats>();
        animator.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0.0f;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Transform playerTransform = animator.gameObject.transform;
        Vector2 GrabLocation = playerInput.ledgeToGrab.GetComponent<LedgeGrabPoint>().GrabObject.transform.position;
        float distanceFromGrabLocation = Vector2.Distance(playerTransform.position, GrabLocation);
        if (distanceFromGrabLocation >= 0.4)
        {
            playerTransform.position = Vector2.MoveTowards(playerTransform.position, GrabLocation, playerStats.climbSpeed * Time.deltaTime);
        }
        else
        {
            playerTransform.position = new Vector2(GrabLocation.x, GrabLocation.y);
        }

        bool isGrabbingLedge = playerInput.isGrabLedgeButtonDown;
        animator.SetBool("isGrabbingLedge", isGrabbingLedge);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.GetComponent<Rigidbody2D>().gravityScale = 2.4f;
    }
}
