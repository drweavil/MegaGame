using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ThunderClapHitbox : MonoBehaviour {
	public SpellHitbox spellHitbox;
	public float damagePercent;
	public CharacterAPI characterAPI;
	public float efficiency = 100f;
	string path = "Prefabs/SkillPrefabs/ThunderClapHitBoxCenter";


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
					int randomVectorValue = Random.Range (100, 150);
					//enemyTarget.stats.inJump = true;
					if (obj.transform.position.x >= this.gameObject.transform.position.x) {
						enemyTarget.movementController.AddForceWithoutAnimation (new Vector3 (randomVectorValue, 250, 0));
						enemyTarget.stats.FullStun (3f);
					} else {
						enemyTarget.movementController.AddForceWithoutAnimation (new Vector3 (randomVectorValue * -1, 250, 0));
						enemyTarget.stats.FullStun (3f);
					} 
					//Debug.Log (characterAPI.skills.SkillDamage (2.98718f, Stats.physicalDamageType));
					enemyTarget.stats.MakeDamage (characterAPI.skills.SkillDamage (2.98718f, Stats.physicalDamageType, efficiency), Stats.physicalDamageType, true);
				}
			}
			//yield return null;
		}

		spellHitbox.Clear ();
		ObjectsPool.PushObject (path, this.gameObject);
		yield break;
	}



}
