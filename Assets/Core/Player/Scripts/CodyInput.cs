using UnityEngine;
using System.Collections;

public class CodyInput : MonoBehaviour {

	float horizontalInput;
	float verticalInput;
	bool grounded;
	Transform groundCheck;
	float groundRadius = 0.21f;
	public LayerMask whatIsGround;
	string playerName;

	public Animator anim;
	
	void Start () {
		anim = GetComponent<Animator> ();
		playerName = gameObject.name;
		groundCheck = GameObject.Find (playerName + "/groundCheck").GetComponent<Transform> ();
	
	}

	void Update () {

		grounded = Physics.CheckSphere (groundCheck.position, groundRadius, whatIsGround);
		horizontalInput = Input.GetAxis ("Horizontal");
		verticalInput = Input.GetAxis ("Vertical");

		anim.SetFloat ("Horizontal", horizontalInput);
		anim.SetFloat ("Vertical", verticalInput);
		anim.SetFloat ("HorizontalAbs", Mathf.Abs (horizontalInput));
		anim.SetBool ("Grounded", grounded);
	
	}
}
