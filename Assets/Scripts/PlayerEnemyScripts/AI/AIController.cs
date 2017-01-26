using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIController : MonoBehaviour {
	Dictionary<string, AIAction> ignoreActions = new Dictionary<string, AIAction> ();
	List<AIAction> actions = new List<AIAction>();
	public CharacterAPI characterAPI;
	AIControllerParams controllerParams = new AIControllerParams ();
	//Dictionary<string, object> allParams = new Dictionary<string, object>();

	RaycastHit hitWithTarget;


	void Awake(){
		//allParams.Add ("distanceToPlayer", new object());
		//allParams.Add ("player", new object());
		foreach(AIAction action in AIActions.MeleeBase){
			actions.Add (action);
		}
	}
	// Update is called once per frame
	void Update () {
		UpdateParams ();
		foreach (AIAction action in actions) {
			if (action.actionCondition (controllerParams) && !ignoreActions.ContainsKey(action.name)) {
				//Debug.Log ("start");
				ignoreActions.Add (action.name, action);
				StartCoroutine (StartAction(action));
			}
		}
		
	}

	void UpdateParams(){
		

		Vector3 playerPosition = PlayerController.player.transform.position;
		Vector3 currentPosition = characterAPI.transform.position;
		controllerParams.distanceToTarget = Vector3.Distance (currentPosition, playerPosition);



		CharacterAPI meleeSlotTarget = PlayerController.playerCharacterAPI;




		controllerParams.currentPosition = currentPosition;
		Vector3 directionToTarget = playerPosition - currentPosition;
		directionToTarget.Normalize ();
		//Debug.Log (directionToTarget);
		Physics.Raycast (
			currentPosition, 
			directionToTarget,
			out hitWithTarget,
			5f, 
			(1 << LayerMask.NameToLayer ("Player")) | (1 << LayerMask.NameToLayer ("Ground")));
		if (hitWithTarget.collider != null) {
			if (hitWithTarget.collider.gameObject.layer == LayerMask.NameToLayer ("Player")) {
				controllerParams.targetInLine = true;
				Debug.Log ("inLine");
			} else {
				controllerParams.targetInLine = false;
			}
		} else {
			controllerParams.targetInLine = false;
		}
	}

	IEnumerator StartAction(AIAction action){
		AIAction.AfterCallback callback = () => {
			//Debug.Log("End");
			ignoreActions.Remove (action.name);	
		};
		action.currentParams = controllerParams;
		StartCoroutine(action.action (characterAPI, action, callback));
		yield break;
	}
}
