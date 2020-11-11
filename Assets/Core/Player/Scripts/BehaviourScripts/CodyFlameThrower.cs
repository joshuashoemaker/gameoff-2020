using UnityEngine;
using System.Collections;

public class CodyFlameThrower : StateMachineBehaviour {
	
	public ParticleSystem flamethrower;

	public override void OnStateEnter (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		base.OnStateEnter (animator, stateInfo, layerIndex);
		flamethrower = GameObject.Find (animator.gameObject.name.ToString () + "/FlameThrower").GetComponent<ParticleSystem>();
		flamethrower.enableEmission = true;
	}
	

	public override void OnStateUpdate (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		base.OnStateUpdate (animator, stateInfo, layerIndex);
		if(flamethrower.enableEmission == false){
			flamethrower.enableEmission = true;
		}


	}

	public override void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		base.OnStateExit (animator, stateInfo, layerIndex);
		flamethrower.enableEmission = false;
	}
}
