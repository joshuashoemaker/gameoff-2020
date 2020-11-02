using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFalling : StateMachineBehaviour
{
    PlayerInput playerInput;
    PlayerStats playerStats;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerInput = animator.gameObject.GetComponent<PlayerInput>();
        playerStats = animator.gameObject.GetComponent<PlayerStats>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float horizontalAxis = playerInput.horizontalInput;
        bool isGrabbingLedge = playerInput.isGrabLedgeButtonDown && playerInput.isInLedgeTrigger;
 
        animator.gameObject.transform.position += new Vector3(horizontalAxis, 0, 0) * Time.deltaTime * playerStats.movementSpeed;

        animator.SetFloat("horizontalAxisAbsolute", Mathf.Abs(horizontalAxis));
        animator.SetBool("isGrounded", playerInput.isGrounded);
        animator.SetBool("isGrabbingLedge", isGrabbingLedge);
    }
}
