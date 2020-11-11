using UnityEngine;
using System.Collections;

public class CodyMainMovement : MonoBehaviour {

	public bool canControl = true;
	public CharacterController controller;
	Vector3 moveDirection = Vector3.zero;

	//On Ground Stuff
	public float maxWalkspeed = 2;
	public float maxRunSpeed = 4;
	bool running = false;
	float xSpeed;

	bool facingRight = true;
	public float angle = 90;
	

	//In Air Stuff
	bool grounded = false;
	public float groundRadius;
	public Transform groundCheck;
	public LayerMask whatIsGround;
	public LayerMask whatIsBounce;
	public float jumpForce;
	public float bounceAmount;//uses groundCheck.position
	bool jumped;
	bool bounce;//uses groundCheck.postistion

	//Attacking Stuff
	public bool attacking;
	public bool recharging;
	public float fireEnergy;
	public ParticleSystem flamethrower;
	public GameObject lightining;

	Animator anim;
	public Animator fireAnim;
	public AudioSource fireSFX;
	public AudioSource sFXs;
	public AudioClip strike;

	public AudioClip leftStep;
	public AudioClip rightStep;
	
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		fireAnim = GameObject.Find ("Main Camera/Hud/Fire Book/MagicBook/Fire").GetComponent<Animator> ();
		controller = gameObject.GetComponent<CharacterController> ();
	}
	
	// Update is called once per frame
	void FixedUpdate (){

		if(fireAnim == null){fireAnim = GameObject.Find ("Main Camera/Hud/Fire Book/MagicBook/Fire").GetComponent<Animator> ();}

		transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);
		float move = Input.GetAxis ("Horizontal");
		running = false;

		anim.SetFloat ("Walking", move);
		anim.SetBool ("Ground", grounded);
		anim.SetBool ("Jumped", jumped);
		anim.SetBool ("Running", running);
		anim.SetFloat ("Movement Speed", Mathf.Abs (xSpeed));

		#region Walking/Running
		//-------For Checking if Player is Running or Walking-----------------------------
		if (move > 0.1f && !Input.GetButton("Run") && canControl && !anim.GetCurrentAnimatorStateInfo(0).IsName("Lightining Strike")){
			moveDirection = new Vector3 (move * 7, moveDirection.y, 0);
			angle = 90;
			running = false;
		}
		if (move < -0.1f && !Input.GetButton("Run")&& canControl && !anim.GetCurrentAnimatorStateInfo(0).IsName("Lightining Strike")){
			moveDirection = new Vector3 (move * 7, moveDirection.y, 0);
			running = false;
		}
		if (move > 0.1f && Input.GetButton("Run")&& canControl && !anim.GetCurrentAnimatorStateInfo(0).IsName("Lightining Strike")){
			moveDirection = new Vector3 (move * 14, moveDirection.y, 0);
			anim.SetBool("Running", true);
			running = true;
			angle = 90;
		}
		if (move < -0.1f && Input.GetButton("Run")&& canControl && !anim.GetCurrentAnimatorStateInfo(0).IsName("Lightining Strike")){
			moveDirection = new Vector3 (move * 14, moveDirection.y, 0);
			anim.SetBool("Running", true);
			running = true;
			angle = 270;
		}

		#endregion

		//Jumping & Bouncing
		if(grounded){jumped = false;}

		if(Physics.CheckSphere (groundCheck.position, groundRadius, whatIsBounce)){
			anim.SetBool("Ground", false);
			moveDirection = new Vector3 (move * 7, moveDirection.y, 0);
			jumped = true;
		}

		controller.Move(moveDirection * Time.deltaTime);
		moveDirection.y -= 25 * Time.fixedDeltaTime;
	}

	void Update(){

		float move = Input.GetAxis ("Horizontal");

		grounded = Physics.CheckSphere (groundCheck.position, groundRadius, whatIsGround);
		if(grounded && Input.GetButtonDown("Jump")&& canControl && !attacking){
			anim.SetTrigger("Jump");
			moveDirection = new Vector3 (move * 7, moveDirection.y, 0);
		}
		#region Attacking Stuff
		attacking = false;

		if(Input.GetButton("Attack") && fireEnergy > 0 && !running && !recharging && grounded){attacking = true;}
		if(!attacking && fireEnergy < 100){fireEnergy += Time.deltaTime * 50f; flamethrower.enableEmission = false; fireSFX.enabled = false;}
		if(attacking && fireEnergy > 0){fireEnergy -= Time.deltaTime * 30f; flamethrower.enableEmission = true; fireSFX.enabled = true;}

		if(fireEnergy > 100){fireEnergy = 100;}
		if(fireEnergy < 0){fireEnergy = 0;}

		if(fireEnergy < 1){recharging = true;}
		if(fireEnergy > 75){recharging = false;}
		anim.SetBool ("FireBall Attacking", attacking);

		if(fireEnergy > 25){
			flamethrower.startSpeed = 15;
		}
		if(fireEnergy < 25){
			flamethrower.startSpeed = fireEnergy/2;
		}

		fireAnim.SetFloat("Fire Energy", fireEnergy);
		fireAnim.SetBool ("Recharging", recharging);

		anim.SetBool("Special", false);
		if(grounded && Input.GetButtonDown("Special") && anim.GetCurrentAnimatorStateInfo(0).IsName("Idle")){
			anim.SetBool("Special", true);
		}

		#endregion
	}

	void EnableLightining(){
		lightining.SetActive (true);
		sFXs.PlayOneShot (strike);
	}
	void DisableLightining(){
		lightining.SetActive (false);
	}

	void CantControl(){canControl = false;}
	void CanControl(){canControl = true;}

	void LeftStep(){
		GetComponent<AudioSource>().PlayOneShot (leftStep, 0.1f);
	}
	
	void RightStep(){
		GetComponent<AudioSource>().PlayOneShot (rightStep, 0.1f);
	}

}



