using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIActions : MonoBehaviour {
	public static bool isEnabled = false;
	private static List<AIAction> meleeBase = new List<AIAction>();
	public static List<AIAction> MeleeBase{
		get{
			if (isEnabled) {
				return meleeBase;
			} else {
				DeclareActions ();
				return meleeBase;
			}
		}
	}


	public static void DeclareActions(){
		AIAction newAction = new AIAction ();
		newAction.name = "movementToPlayer";
		newAction.actionCondition = (newParams) => {
			float randomDistance = Random.Range(0.2f, 0.4f);
			float distance = randomDistance;// 0.4f;
			if(newParams.distanceToTarget >= distance && newParams.targetInLine){
				if(System.Math.Abs(newParams.currentPosition.x - PlayerController.player.transform.position.x) >= distance)	{
					return true;
				}else{
					return false;
				}
			}else{
				return false;
			}/*randomDistance*/;
		};
		newAction.action = MovementToPlayerAction;
		meleeBase.Add (newAction);


		isEnabled = true;
	}



	static IEnumerator MovementToPlayerAction(CharacterAPI newCharacterAPI, AIAction action, AIAction.AfterCallback callback){
		bool actionNotComplete = true; 
		CharacterAPI playerAPI = PlayerController.player.GetComponent<CharacterAPI>();
		while(actionNotComplete){
			Vector2 direction = playerAPI.transform.position - newCharacterAPI.transform.position;
			//Debug.Log (direction.x);
			float distanceToPlayer = Random.Range (0.2f, 0.4f );
			if(action.actionCondition(action.currentParams)/*System.Math.Abs(direction.x) >= distanceToPlayer*/){
				direction.Normalize();
				if (direction.x > 0) {
					direction.x = 1;
				}
				if (direction.x < 0) {
					direction.x = -1;
				}
				newCharacterAPI.movementController.SetMovement(newCharacterAPI.movementController.GetCurrentSpeed(direction));
				yield return null;
			}else{
				newCharacterAPI.movementController.SetMovement (new Vector2 (0, 0));
				actionNotComplete = false;
			}
		}
		if (callback != null) {
			callback ();
		}
		yield break;
	}

	public static IEnumerator MovementToPlayerWithAround(CharacterAPI newCharacterAPI, AIAction action, AIAction.AfterCallback callback){
		bool actionNotComplete = true; 
		CharacterAPI playerAPI = PlayerController.player.GetComponent<CharacterAPI>();
		while(actionNotComplete){
			Vector2 direction = playerAPI.transform.position - newCharacterAPI.transform.position;
			//Debug.Log (direction.x);
			float distanceToPlayer = Random.Range (0.2f, 0.4f );
			if(action.actionCondition(action.currentParams)/*System.Math.Abs(direction.x) >= distanceToPlayer*/){
				direction.Normalize();
				if (direction.x > 0) {
					direction.x = 1;
				}
				if (direction.x < 0) {
					direction.x = -1;
				}
				newCharacterAPI.movementController.SetMovement(newCharacterAPI.movementController.GetCurrentSpeed(direction));
				yield return null;
			}else{
				newCharacterAPI.movementController.SetMovement (new Vector2 (0, 0));
				actionNotComplete = false;
			}
		}
		if (callback != null) {
			callback ();
		}
		yield break;
	}
}
