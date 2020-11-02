using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : StateMachineBehaviour
{

    PlayerStats playerStats;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerStats = animator.gameObject.GetComponent<PlayerStats>();
        animator.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, playerStats.jumpForce), ForceMode2D.Impulse);
        animator.SetBool("isGrounded", false);
    }
}
