using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


public class AimLineController : MonoBehaviour {
	public static AimLineController aimLineController;

	public Image aimLine;
	public Transform playerAimLineStart;
	public RectTransform aimGuiStart;
	public RaycastHit objectHit;

	public RectTransform aim;

	bool aimLineActive = false;

	bool rotated = false;


	public Vector2 joystickVector;
	SkillSettings skillSettings;

	float distanceWidth;
	Vector2 distanceWorldWithEnd;


	public Transform target;
	public List<Transform> potentialTargets = new List<Transform> ();
	public bool autoTarget = false;
	public int oldFramePotentialTargetsCount = -1;
	public bool targetCountChanged = false;

	public StreamHitbox targetFinder;


	void Awake(){
		aimLineController = this;
	}

	void Update(){
		if (aimLineActive) {

			if (autoTarget) {
				if (target == null) {
					joystickVector = Joystick.joystick.GetDirection ();
				} else {
					joystickVector = target.position - PlayerController.playerCharacterAPI.transform.position;
					joystickVector.Normalize ();
				}

			} else {
				joystickVector = Joystick.joystick.GetDirection ();
			}


			if (joystickVector == new Vector2 (0, 0)) {
				if (PlayerController.playerCharacterAPI.transform.lossyScale.x < 0) {
					joystickVector = new Vector2 (-1, 0);
				} else {
					joystickVector = new Vector2 (1, 0);
				}
			}


				
			Physics.Raycast (playerAimLineStart.position, joystickVector, out objectHit, skillSettings.distance, (1 << LayerMask.NameToLayer("Enemy")) | (1 << LayerMask.NameToLayer("Ground")));


			if (objectHit.collider != null) {
				aim.position = Camera.main.WorldToScreenPoint(objectHit.point);
				aimLine.rectTransform.sizeDelta = new Vector2 (Vector2.Distance(aimGuiStart.localPosition, aim.localPosition), aimLine.rectTransform.sizeDelta.y);
			}else{
				aim.position = Camera.main.WorldToScreenPoint((joystickVector / joystickVector.magnitude) * skillSettings.distance + (Vector2)playerAimLineStart.position);
				aimLine.rectTransform.sizeDelta = new Vector2 (Vector2.Distance(aimGuiStart.localPosition, aim.localPosition), aimLine.rectTransform.sizeDelta.y);
			}


			aimLine.transform.position = aimGuiStart.position;

			if (joystickVector == new Vector2 (0, 0)) {
				float angle = 0;
				if (PlayerController.playerCharacterAPI.transform.lossyScale.x < 0) {
					angle = 180;
				} else {
					angle = 0;
				}
				aimLine.transform.rotation = Quaternion.Euler (new Vector3(0, 0, angle));
			} else {
				float angle = Vector3.Angle(joystickVector, new Vector3(1, 0));
				if (joystickVector.y < 0) {
					angle = 360 - angle;
				}
					
				aimLine.transform.rotation = Quaternion.Euler (new Vector3(0, 0, angle));
			}

			if (PlayerController.playerCharacterAPI.transform.lossyScale.x < 0) {
				if(!rotated){
					rotated = true;
					aimGuiStart.position = Camera.main.WorldToScreenPoint (playerAimLineStart.position);
				}
			} else {
				if(rotated){
					rotated = false;
					aimGuiStart.position = Camera.main.WorldToScreenPoint (playerAimLineStart.position);
				}
			}
		}
	}

	public void StartAimLine(SkillSettings settings){
		
		skillSettings = settings;
		aimLineActive = true;

		if (skillSettings.autoTarget) {

			StartCoroutine (StartTargetFinding ());	
		}

		aim.gameObject.SetActive (true);
		aimLine.gameObject.SetActive (true);
	}

	public void StopAimLine(){
		if (skillSettings.autoTarget) {
			StopTargetFinding ();	
		}
		aimLineActive = false;

		aim.gameObject.SetActive (false);
		aimLine.gameObject.SetActive (false);
	}

	IEnumerator StartTargetFinding(){
		autoTarget = true;
		targetFinder.gameObject.SetActive (true);
		targetFinder.action = (CharacterAPI enemyAPI) => {
			potentialTargets.Add(enemyAPI.transform);
			targetCountChanged = true;
		};

		targetFinder.exitAction = (CharacterAPI enemyAPI) => {
			potentialTargets.Remove(enemyAPI.transform);
			targetCountChanged = true;
		};
		targetFinder.Clear ();
		potentialTargets.Clear ();

		targetFinder.ignoreColliders.Add (PlayerController.playerCharacterAPI.boxCollider);
		targetFinder.ignoreColliders.Add (PlayerController.playerCharacterAPI.sphereCollider);
		targetFinder.selectingLayer = LayerMask.NameToLayer ("Enemy");

		targetFinder.readyToTrigger = true;
		targetFinder.withExitAction = true;
		targetFinder.collider.radius = skillSettings.distance;




		while (autoTarget) {
			if (potentialTargets.Count != 0) {
				if (targetCountChanged) {
					targetCountChanged = false;
					//Debug.Log ("lol");
					target = potentialTargets.OrderBy (t => Vector3.Distance (PlayerController.playerCharacterAPI.transform.position, t.position)).ToList() [0];
				}
			} else {
				target = null;
			}

			yield return null;
		}

		yield break;
	}

	void StopTargetFinding(){
		autoTarget = false;
		targetFinder.Clear ();
		potentialTargets.Clear ();
		targetFinder.gameObject.SetActive (false);
	}


}
