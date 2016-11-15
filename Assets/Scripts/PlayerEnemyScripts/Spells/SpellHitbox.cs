﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpellHitbox : MonoBehaviour {

	public List<GameObject> objects = new List<GameObject>();
	private List<Collider> objectColliders = new List<Collider> ();
	public Vector3 nullSpellPosition;
	public Vector3 nullSpellScale;
	public Vector3 currentNullSpellPosition;
	public List<Collider> ignoreColliders = new List<Collider>();
	public Collider collider;
	public Vector3 nullColliderSize;
	public Vector3 nullColliderCenter;

	void OnTriggerEnter(Collider col){
		if (objects.FindIndex (o => o == col.gameObject) == -1 && 
			ignoreColliders.FindIndex (o => o == col.gameObject) == -1 &&
			col.gameObject.tag == "Character"
		) {
			objects.Add (col.gameObject);
			objectColliders.Add (col);
		}
	}

	public List<GameObject> GetObjectsWithoutIgnoredColliders(){
		List<GameObject> returningObjects = new List<GameObject> ();
		foreach (Collider col in objectColliders) {
			if (ignoreColliders.FindIndex (o => o == col) == -1) {
				returningObjects.Add (col.gameObject);
			}
		}
		return returningObjects;
	}

	void OnTriggerStay(Collider col){
		if (objects.FindIndex (o => o == col.gameObject) == -1 && 
			ignoreColliders.FindIndex (o => o == col.gameObject) == -1 &&
			col.gameObject.tag == "Character"
		) {
			objects.Add (col.gameObject);
		}
	}

	public void Flip(){
		Vector3 newPosition = transform.position;
		newPosition.x *= -1;
		transform.localPosition = newPosition;

		Vector3 newScale = transform.localScale;
		newScale.x *= -1;
		transform.localScale = newScale;
	}

	public void FlipWorld(bool withCurrentPosition = false){
		Vector3 nullPosition = nullSpellPosition;
		if (withCurrentPosition) {
			nullPosition = currentNullSpellPosition;
		} 

		Vector3 newPosition = nullPosition;
		newPosition.x *= -1;
		newPosition = newPosition + (transform.position - nullPosition);
		transform.position = newPosition;

		Vector3 newScale = transform.localScale;
		newScale.x *= -1;
		transform.localScale = newScale;
	}

	public void Clear (){
		objects.Clear ();
		objectColliders.Clear ();
		ignoreColliders.Clear ();
	}
}
