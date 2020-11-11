using UnityEngine;
using System.Collections;

public class CodyGeneralVoices : MonoBehaviour {

	public AudioSource source;

	public AudioClip[] hurts;
	public AudioClip[] grunts;

	public AudioClip spitFire;
	public AudioClip likeRadien;
	public AudioClip death;

	int hurtCount;
	int gruntCount;
	int spitfireCount = 4;
	int likeRadienCount = 4;
	

	void hurt(){
		source.GetComponent<AudioSource>().PlayOneShot (hurts [hurtCount], 1f);
		hurtCount++;
		if(hurtCount > hurts.Length - 1){
			hurtCount = 0;
		}
	}

	void die(){
		source.GetComponent<AudioSource>().PlayOneShot (death, 1f);
	}

	void grunt(){
		source.GetComponent<AudioSource>().PlayOneShot (grunts [gruntCount], 1f);
		gruntCount++;
		if(gruntCount > grunts.Length - 1){
			gruntCount = 0;
		}
	}

	void SpitFire(){
		spitfireCount++;
		if(spitfireCount > 4){
			source.GetComponent<AudioSource>().PlayOneShot(spitFire, 1);
			spitfireCount = 0;
		}
	}

	void LikeRadien(){
		likeRadienCount++;
		if(likeRadienCount > 4){
			source.GetComponent<AudioSource>().PlayOneShot(likeRadien, 1f);
			likeRadienCount = 0;
		}
	}
}
