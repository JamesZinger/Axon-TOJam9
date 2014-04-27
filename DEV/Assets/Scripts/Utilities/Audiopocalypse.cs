using UnityEngine;
using System.Collections;

public class Audiopocalypse : MonoBehaviour {
	public AudioClip[] clipList;
	public enum Sounds{
		Death = 0,
		Jump,
		Menu_Click,
		Menu_Select,
		Pickup_Card,
		Pickup_Distractor,
		Pickup_Furniture,
		Pickup_Meatball,
		Pickup_Money,
		Slide,
		Win,
		Use_Meatball
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void PlayClip(Audiopocalypse.Sounds s){
		audio.clip = clipList[(int)s];
		audio.Play();
	}
}
