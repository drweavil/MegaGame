using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StreamHitbox : MonoBehaviour {
	public List<GameObject> objects = new List<GameObject>();
	private List<Collider> objectColliders = new List<Collider> ();
	public List<Collider> ignoreColliders = new List<Collider>();
	public Vector3 nullSpellPosition;
	public Vector3 nullSpellScale;
	public float damagePercent;
	public int damageType;
	//public float crit;
	public CharacterAPI characterAPI;
	public bool readyToTrigger = false;


	void OnTriggerEnter(Collider col){
		if (readyToTrigger) {
			if (objects.FindIndex (o => o == col.gameObject) == -1 &&
			   ignoreColliders.FindIndex (o => o == col.gameObject) == -1 &&
			   col.gameObject.tag == "Character") {
				objects.Add (col.gameObject);
				CharacterAPI enemyTarget = col.gameObject.GetComponent<CharacterAPI> ();
				enemyTarget.stats.MakeDamage (characterAPI.skills.SkillDamage (damagePercent, damageType), Stats.physicalDamageType, true);
				Debug.Log ("lol");
			}
		}
	}

	public void Clear (){
		objects.Clear ();
		objectColliders.Clear ();
		ignoreColliders.Clear ();
		readyToTrigger = false;
	}


}
