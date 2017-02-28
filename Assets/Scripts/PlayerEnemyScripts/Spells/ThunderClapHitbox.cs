using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ThunderClapHitbox : MonoBehaviour {
	public SpellHitbox spellHitbox;
	public CharacterAPI characterAPI;
	string path = "Prefabs/SkillPrefabs/ThunderClapHitBoxCenter";
	public SpellHitbox.ObjectsAction action;


	public IEnumerator MakeDamageAndThrow(){
		int frameIndex = 0;
		while (frameIndex < 4) {
			if (spellHitbox.objects.Count == 0) {
				frameIndex += 1;
				yield return null;
			} else {
				frameIndex = 4;
				List<GameObject> objects = spellHitbox.GetObjectsWithoutIgnoredColliders ();
				//Debug.Log (objects.Count);
				foreach (GameObject obj in objects) {
					//Debug.Log (obj.name);
					CharacterAPI enemyTarget = obj.GetComponent<CharacterAPI> ();
					int randomVectorValue = Random.Range (50, 75);
					//enemyTarget.stats.inJump = true;
					if (obj.transform.position.x >= this.gameObject.transform.position.x) {
						enemyTarget.movementController.NullSpeedWhenGround ();
						enemyTarget.movementController.AddForceWithoutAnimation (new Vector3 (randomVectorValue, 250, 0));
					} else {
						enemyTarget.movementController.NullSpeedWhenGround ();
						enemyTarget.movementController.AddForceWithoutAnimation (new Vector3 (randomVectorValue * -1, 250, 0));
					} 
					if (action != null) {
						action (enemyTarget);
					}
				}
			}
			//yield return null;
		}

		spellHitbox.Clear ();
		ObjectsPool.PushObject (path, this.gameObject);
		yield break;
	}



}
