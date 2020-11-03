using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalking : StateMachineBehaviour
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
        animator.SetFloat("horizontalAxis", horizontalAxis);
        animator.SetFloat("horizontalAxisAbsolute", Mathf.Abs(horizontalAxis));

        if (playerInput.isJumpButtonDown)
        {
            animator.SetTrigger("Jump");
        }
        animator.gameObject.transform.position += new Vector3(horizontalAxis, 0, 0) * Time.deltaTime * playerStats.movementSpeed;

        SetPlayerDirection(horizontalAxis, animator.gameObject.transform);
    }

    private void SetPlayerDirection(float horizontalAxis, Transform playerTransform)
    {
        // if (Mathf.Abs(horizontalAxis) < 0.2) { return; }
        // if (horizontalAxis >= 0.2) playerTransform.localScale = new Vector3(1, playerTransform.localScale.y, playerTransform.localScale.z);
        // if (horizontalAxis <= -0.2) playerTransform.localScale = new Vector3(-1, playerTransform.localScale.y, playerTransform.localScale.z);
    }
}
