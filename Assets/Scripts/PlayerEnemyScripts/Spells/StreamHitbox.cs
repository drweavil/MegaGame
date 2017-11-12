using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StreamHitbox : MonoBehaviour {
	public List<GameObject> objects = new List<GameObject>();
	private List<Collider> objectColliders = new List<Collider> ();
	public List<Collider> ignoreColliders = new List<Collider>();
	public Vector3 nullSpellPosition;
	public Vector3 nullSpellScale;
	public Vector3 currentNullSpellPosition;
	//public float crit;
	public bool readyToTrigger = false;
	public bool withExitAction = false;
	public int selectingLayer;
	public SpellHitbox.ObjectsAction action;
	public SpellHitbox.ObjectsAction exitAction;
	public Transform centerTransform;
	public SphereCollider collider;


	void OnTriggerEnter(Collider col){
		if (readyToTrigger) {
			
			if (objects.FindIndex (o => o == col.gameObject) == -1 &&
			   ignoreColliders.FindIndex (o => o == col.gameObject) == -1 &&
			   col.gameObject.tag == "Character" &&
				col.gameObject.layer == selectingLayer
			) {
				
				objects.Add (col.gameObject);
				CharacterAPI enemyTarget = col.gameObject.GetComponent<CharacterAPI> ();
				if(action != null){
					action (enemyTarget);
				}
			}
		}
	}

	void OnTriggerStay(Collider col){
		if (readyToTrigger) {
			if (objects.FindIndex (o => o == col.gameObject) == -1 &&
				ignoreColliders.FindIndex (o => o == col.gameObject) == -1 &&
				col.gameObject.tag == "Character" &&
				col.gameObject.layer == selectingLayer
			) {
				objects.Add (col.gameObject);
				CharacterAPI enemyTarget = col.gameObject.GetComponent<CharacterAPI> ();
				if(action != null){
					action (enemyTarget);
				}
			}
		}
	}

	void OnTriggerExit(Collider col){
		if (readyToTrigger) {
			if (withExitAction) {
				if (objects.FindIndex (o => o == col.gameObject) != -1 &&
				   ignoreColliders.FindIndex (o => o == col.gameObject) == -1 &&
				   col.gameObject.tag == "Character" &&
				   col.gameObject.layer == selectingLayer) {
					objects.Remove (col.gameObject);
					CharacterAPI enemyTarget = col.gameObject.GetComponent<CharacterAPI> ();
					if (action != null) {
						exitAction (enemyTarget);
					}
				}
			}
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
		readyToTrigger = false;
	}

	public void RemoveIgnoreObject(GameObject obj){
		objects.Remove (obj);
	}


}
