using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpellBall : MonoBehaviour {

	public float speed;
	public Vector3 direction;
	RaycastHit ballHit;
	//public float damagePercent;
	//public int damageType;
	public string path;
	public CharacterAPI characterAPI;
	public bool ready = false;
	//public int energyPoints;
	public float time;
	public float currentTime = 0;
	public Transform transform;
	public List<Collider> ignoreColliders = new List<Collider> ();
	//public float efficiency = 100f;
	public SpellHitbox.ObjectsAction action;
	public int selectingLayer;





	// Use this for initialization
	void Awake () {
	
	}

	void Update(){
		currentTime += Time.deltaTime;
		if (currentTime >= time) {
			ready = false;
			ObjectsPool.PushObject (path, this.gameObject);	
		}

		transform.position = new Vector3 (
			transform.position.x + direction.x * speed,
			transform.position.y + direction.y * speed,
			transform.position.z + direction.z * speed
		);
	}
	
	// Update is called once per frame
	void OnTriggerEnter (Collider col) {
		if (ready) {
			if (col.gameObject.layer != LayerMask.NameToLayer ("Ground")) {
				if (col.gameObject.tag == "Character" && col.gameObject.layer == selectingLayer) {
					if (ignoreColliders.FindIndex (c => c == col) == -1) {
						CharacterAPI enemyAPI = col.gameObject.GetComponent<CharacterAPI> ();
						action (enemyAPI);
						ready = false;
						ObjectsPool.PushObject (path, this.gameObject);
					}
				}
			} else {
				ready = false;
				ObjectsPool.PushObject (path, this.gameObject);
			}
		}
	}
}
