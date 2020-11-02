using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdle : StateMachineBehaviour
{
    PlayerInput playerInput;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerInput = animator.gameObject.GetComponent<PlayerInput>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float horizontalAxis = playerInput.horizontalInput;
        float verticalAxis = playerInput.verticalInput;
        animator.SetFloat("horizontalAxis", horizontalAxis);
        animator.SetFloat("horizontalAxisAbsolute", Mathf.Abs(horizontalAxis));
        animator.SetFloat("verticalAxis", verticalAxis);
        animator.SetBool("isGrounded", playerInput.isGrounded);

        if (playerInput.isJumpButtonDown)
        {
            animator.SetTrigger("Jump");
        }
    }

}
