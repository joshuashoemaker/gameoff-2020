using UnityEngine;
using System.Collections;

public class CodyController : MonoBehaviour {

	public bool canMove;
	float jumpAmount = 1000;
	float horizontalInput;
	float verticalInput;
	bool grounded;
	Transform groundCheck;
	float groundRadius = 0.11f;
	public LayerMask whatIsGround;
	
	Animator anim;
	Rigidbody rigidBody;
	public PhysicMaterial[] phxMats;
	Collider collider;


	public float angle = 180;
	
	void Start () {
		anim = GetComponent<Animator> ();
		rigidBody = GetComponent<Rigidbody> ();
		collider = gameObject.GetComponent<Collider>();
		groundCheck = GameObject.Find ("Cody_Player_Main(Clone)" + "/groundCheck").GetComponent<Transform> ();
		
	}
	
	void Update () {
		
		grounded = Physics.CheckSphere (groundCheck.position, groundRadius, whatIsGround);
		transform.rotation = Quaternion.AngleAxis (angle, Vector3.up);
		horizontalInput = Input.GetAxis ("Horizontal");
		verticalInput = Input.GetAxis ("Vertical");

		anim.SetFloat ("HorizontalAbs", Mathf.Abs (horizontalInput));
		anim.SetBool ("Ground", grounded);
		anim.SetBool ("Running", false);
		anim.SetBool ("FlameThrower", Input.GetButton ("Attack"));


			if(horizontalInput < -0.1){
				angle = 270;
				rigidBody.velocity = new Vector3(horizontalInput * 7, rigidBody.velocity.y);

			if(Input.GetButton("Run")){
				anim.SetBool("Running", true);
				rigidBody.velocity = new Vector3(horizontalInput * 14, rigidBody.velocity.y);
				}
			}

			if(horizontalInput > 0.1){ 
			angle = 90;
			rigidBody.velocity = new Vector3(horizontalInput * 7, rigidBody.velocity.y);

			if(Input.GetButton("Run")){
				anim.SetBool("Running", true);
				rigidBody.velocity = new Vector3(horizontalInput * 14, rigidBody.velocity.y);
				}
			}

		if(grounded && Input.GetButtonDown("Jump")){
			rigidBody.AddForce(new Vector2(0, 1000));
		}

		if(grounded){
			collider.material = phxMats[0];
		}
		if(!grounded){
			collider.material = phxMats[1];
		}
	}
}
