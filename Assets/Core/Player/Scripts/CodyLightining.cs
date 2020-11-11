using UnityEngine;
using System.Collections;

public class CodyLightining : MonoBehaviour {
	
	public bool canStrike;
	
	public GameObject lightining;
	public AudioSource sFXs;
	Animator anim;
	
	public AudioClip strike;
	
	
	
	void Start () {
		//lightining = GameObject.Find ("codyLightining");
		//sFXs = GameObject.Find (gameObject.name + "/VoicesSFX/SFXs").GetComponent<AudioSource> ();
		anim = gameObject.GetComponent<Animator> ();
	}
	
	
	void Update () {
		
		if(canStrike){
			if(Input.GetButtonDown("Special")){
				if(anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") || anim.GetCurrentAnimatorStateInfo(0).IsName("Slow Walking")){
					anim.SetTrigger("Lightining");
				}
			}
		}
	}
	
	void EnableLightining(){
		float playerRot = gameObject.GetComponent<CodyController>().angle;
		
		if (playerRot == 90){
			Instantiate(lightining, new Vector3(gameObject.transform.position.x + 5f, gameObject.transform.position.y, gameObject.transform.position.z), lightining.transform.rotation);
		}
		if (playerRot == 270){
			Instantiate(lightining, new Vector3(gameObject.transform.position.x - 5f, gameObject.transform.position.y, gameObject.transform.position.z), lightining.transform.rotation);
		}
	}
}
