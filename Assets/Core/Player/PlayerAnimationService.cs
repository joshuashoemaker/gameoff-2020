using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationService : MonoBehaviour
{
    private PlayerController playerController;
    private Animator animator;
    private string currentControllerState;
    // Start is called before the first frame update
    void Start()
    {
        playerController = gameObject.GetComponent<PlayerController>();
        animator = gameObject.GetComponent<Animator>();
        currentControllerState = playerController.currentState;

    }

    // Update is called once per frame
    void Update()
    {
        currentControllerState = playerController.currentState;

        animator.SetBool("Idle", currentControllerState == "Idle");
        animator.SetBool("Walking", currentControllerState == "Walking");
        animator.SetBool("Falling", currentControllerState == "Falling");
    }
}
