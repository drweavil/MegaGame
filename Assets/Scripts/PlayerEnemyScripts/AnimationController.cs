﻿using UnityEngine;
using System.Collections;

public class AnimationController : MonoBehaviour {

	Animator animator;
	// Use this for initialization
	/*void Start () {

	}*/

	void Awake(){
		animator = GetComponent<Animator> ();
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.Z)) {
			GetComponent<Animator> ().SetBool ("IsRun", true);
		} else {
			GetComponent<Animator> ().SetBool ("IsRun", false);
		}

		if (Input.GetKey (KeyCode.X)) {
			GetComponent<Animator> ().SetBool ("IsAttackGun90", true);
		} else {
			GetComponent<Animator> ().SetBool ("IsAttackGun90", false);
		}
	}
}
