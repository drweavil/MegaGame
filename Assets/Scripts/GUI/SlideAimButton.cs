using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SlideAimButton : MonoBehaviour {


	bool buttonDown = false;
	public PlayerController playerController;

	void Update(){
		//Debug.Log (buttonDown);
	}


	public void ButtonDown(){
		buttonDown = true;
		StartCoroutine (StartSlideAimMove ());
	}

	public void ButtonUp(){
		buttonDown = false;
	}

	//public void StartSlideAim(){
		
	//}

	IEnumerator StartSlideAimMove(){
		for (int i = 0; i < 1; i++) {
			yield return null;
		}

		int currentState = 0;
		CharacterAPI playerAPI = playerController.player.GetComponent<CharacterAPI> ();
		GameObject aim = ObjectsPool.PullObject ("Prefabs/UI/grenadeAim");
		NullPosition nullPosition = aim.GetComponent<NullPosition> ();
		Vector3 nullPositionVector = nullPosition.nullPosition;
		//aim.transform.parent = playerAPI.skills.nullSkillPosition.transform;
		int directionIndex = 1;
		if (playerAPI.transform.localScale.x < 0) {
			directionIndex = -1;
			nullPositionVector = new Vector3 (nullPosition.nullPosition.x * -1, nullPosition.nullPosition.y, nullPosition.nullPosition.z);
		}

		aim.transform.position = playerAPI.skills.nullEffectPosition.position + nullPositionVector;
		aim.transform.localScale = nullPosition.nullScale;
		aim.transform.localRotation = Quaternion.Euler (nullPosition.nullRotation);
		Vector3 playerNullSkillPosition = playerAPI.skills.nullEffectPosition.position;

		float nextFramePosition = 0.12f * directionIndex;

		RaycastHit rayToWallHit;
		float maxDistance = 6f;
		float distance;
		if (Physics.Raycast (playerAPI.skills.transform.position, new Vector3 (directionIndex, 0, 0), out rayToWallHit, maxDistance, 1 << 8/*Ground*/)) {
			//Debug.Log ("asa");
			distance = rayToWallHit.distance;
		} else {
			distance = maxDistance;
		}

		while (buttonDown) {
			//Debug.Log (currentState);

			if (currentState == 0) {
				aim.transform.position = new Vector3 (
					aim.transform.position.x + nextFramePosition,
					playerAPI.skills.nullEffectPosition.position.y + nullPositionVector.y,
					aim.transform.position.z
				);
				//Debug.Log ("lol");
				if (directionIndex == 1) {
					if (aim.transform.position.x >= playerNullSkillPosition.x + distance - 0.25f) {
						currentState = 1;
					}
				} else {
					if (aim.transform.position.x <= playerNullSkillPosition.x - distance + 0.25f) {
						currentState = 1;
					}
				}
				yield return null;
			} else {
				if (currentState == 1) {
					aim.transform.position = new Vector3 (
						aim.transform.position.x - nextFramePosition,
						playerAPI.skills.nullEffectPosition.position.y + nullPositionVector.y,
						aim.transform.position.z
					);
					if (directionIndex == 1) {
						if (aim.transform.position.x <= playerAPI.skills.nullEffectPosition.position.x + nullPositionVector.x) {
							currentState = 2;
						}
					} else {
						if (aim.transform.position.x >= playerAPI.skills.nullEffectPosition.position.x + nullPositionVector.x) {
							currentState = 2;
						}
					}
					yield return null;
				} else {
					aim.transform.position = new Vector3 (
						playerAPI.skills.nullEffectPosition.position.x + nullPositionVector.x, 
						playerAPI.skills.nullEffectPosition.position.y + nullPositionVector.y, 
						playerAPI.skills.nullEffectPosition.position.z + nullPositionVector.z
					);
					yield return null;
				}
			}
		}


		playerAPI.skills.Grenade (aim.transform.position);
		ObjectsPool.PushObject ("Prefabs/UI/grenadeAim", aim);
		yield break;
	}
}
