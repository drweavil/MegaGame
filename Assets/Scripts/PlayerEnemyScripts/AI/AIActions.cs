using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class AIActions : MonoBehaviour {
	public static List<AIAction> MeleeBase(){
		List<AIAction> actions = new List<AIAction>();
		actions.Add (MovementToPlayer ());
		actions.Add (FaceToPlayer ());
		actions.Add (ReselectTarget ());
		actions.Add (ExitFromChain ());
		actions.Add (EnterToChain ());
		actions.Add (SelectTarget ());
		actions.Add (Patrol());
		return actions;
	}
		
	public static List<AIAction> MeleeSkillActions(){
		List<AIAction> actions = new List<AIAction>();
		actions.Add (FastPunch());
		//actions.Add(MiddlePunch());
		return actions;
	}

	public static AIAction GetAction(string actionName){
		MethodInfo method = typeof(SkillSettings).GetMethod ("Settings_" + actionName.ToString (), BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
		AIAction type = (AIAction)method.Invoke (null, null);
		return (AIAction)(type);
	}





	static AIAction MovementToPlayer(){
		AIAction action = new AIAction ();
		action.name = "MovementToPlayer";
		action.actionCondition = (newParams) => {
			float distance =  0f;
			float distanceToTarget = 0;
			if(newParams.targetInLine){
				if(newParams.target != null){
					distanceToTarget = System.Math.Abs(newParams.characterAPI.transform.position.x - newParams.target.position.x);
					if((distanceToTarget > distance) && ((distanceToTarget - 0.1f) > 0))	{
						return true;
					}else{
						return false;
					}
				}else{
					return false;
				}
			}else{
				return false;
			}
		};
		action.action = MovementToPlayerAction;
		return action;
	}
	static IEnumerator MovementToPlayerAction(CharacterAPI newCharacterAPI, AIAction action, AIAction.AfterCallback callback){
		newCharacterAPI.aiController.AddIgnoreActions (new List<string> { "FaceToPlayer" });
		bool actionNotComplete = true; 
		CharacterAPI playerAPI = PlayerController.player.GetComponent<CharacterAPI>();
		int framesDalay = Random.Range (0, 10);
		for (int i = 0; i < framesDalay; i++) {
			if (action.actionCondition (action.currentParams)) {
				yield return null;
			} else {
				newCharacterAPI.movementController.SetMovement (new Vector2 (0, 0));
				actionNotComplete = false;
			}
		}

		while(actionNotComplete){
			if(action.actionCondition(action.currentParams)){
				if (newCharacterAPI.aiController.targetSlot != null) {
					Vector2 direction = newCharacterAPI.aiController.targetSlot.position - newCharacterAPI.transform.position;
					float distance = System.Math.Abs (direction.x);
					direction.Normalize ();
					if (direction.x > 0) {
						direction.x = 1;
					}
					if (direction.x < 0) {
						direction.x = -1;
					}
					if (!(action.currentParams.restAgainstLeftWall && action.currentParams.target.position.x < newCharacterAPI.transform.position.x) &&
					    !(action.currentParams.restAgainstRightWall && action.currentParams.target.position.x > newCharacterAPI.transform.position.x)) {
						newCharacterAPI.movementController.SetMovement (newCharacterAPI.movementController.GetCurrentSpeed (direction));
					} else {
						newCharacterAPI.movementController.SetMovement (new Vector2 (0, 0));
						actionNotComplete = false;
					}
					yield return null;
				} else {
					newCharacterAPI.movementController.SetMovement (new Vector2 (0, 0));
					actionNotComplete = false;
				}
			}else{
				newCharacterAPI.movementController.SetMovement (new Vector2 (0, 0));
				actionNotComplete = false;
			}
		}
		if (callback != null) {
			callback ();
		}
		newCharacterAPI.aiController.RemoveIgnoreActions (new List<string> { "FaceToPlayer" });
		yield break;
	}




	static AIAction FaceToPlayer(){
		AIAction action = new AIAction ();
		action.name = "FaceToPlayer";
		action.actionCondition = (newParams) => {
			if(newParams.targetInLine && !newParams.characterAPI.stats.withoutControl){
				if((newParams.currentPosition.x < PlayerController.playerCharacterAPI.transform.position.x) && (newParams.characterAPI.transform.localScale.x < 0))	{
					return true;
				}else{
					if((newParams.currentPosition.x > PlayerController.playerCharacterAPI.transform.position.x) && (newParams.characterAPI.transform.localScale.x > 0)){
						return true;
					}else{
						return false;
					}
				}
			}else{
				return false;
			}
		};
		action.action = FaceToPlayerAction;
		return action;
	}
	static IEnumerator FaceToPlayerAction(CharacterAPI newCharacterAPI, AIAction action, AIAction.AfterCallback callback){
		if (newCharacterAPI.transform.position.x <= PlayerController.playerCharacterAPI.transform.position.x) {
			if (newCharacterAPI.transform.localScale.x < 0) {
				newCharacterAPI.movementController.Flip ();
			}
		} else {
			if (newCharacterAPI.transform.localScale.x > 0) {
				newCharacterAPI.movementController.Flip ();
			}
		}
		if (callback != null) {
			callback ();
		}
		yield break;
	}




	static AIAction ReselectTarget(){
		AIAction action = new AIAction ();
		action.name = "ReselectTarget";
		action.actionCondition = (newParams) => {
			if(!newParams.targetInLine && newParams.targetSelected && newParams.inChain){
				return true;
			}else{
				return false;
			}
		};
		action.action = ReselectTargetAction;
		return action;
	}
	static IEnumerator ReselectTargetAction(CharacterAPI newCharacterAPI, AIAction action, AIAction.AfterCallback callback){
		newCharacterAPI.aiController.ReselectTarget ();
		if (callback != null) {
			callback ();
		}
		yield break;	
	}




	static AIAction ExitFromChain(){
		AIAction action = new AIAction ();
		action.name = "ExitFromChain";
		action.actionCondition = (newParams) => {
			if((newParams.characterAPI.stats.withoutControl || !newParams.characterAPI.stats.IsMaximumSpeed() || newParams.characterAPI.stats.isDeath) && newParams.inChain){
				return true;
			}else{
				return false;
			}
		};
		action.action = ExitFromChainAction;
		return action;
	}
	static IEnumerator ExitFromChainAction(CharacterAPI newCharacterAPI, AIAction action, AIAction.AfterCallback callback){
		newCharacterAPI.aiController.controllerParams.inChain = false;
		newCharacterAPI.aiController.ReselectTarget ();
		newCharacterAPI.aiController.SetToZeroSLotsAndTargets();
		newCharacterAPI.aiController.controllerParams.targetSelected = true;
		if (newCharacterAPI.transform.position.x <= PlayerController.playerCharacterAPI.transform.position.x) {
			newCharacterAPI.aiController.targetSlot = PlayerController.playerCharacterAPI.movementController.leftSlot;
			newCharacterAPI.aiController.controllerParams.targetAPI = PlayerController.playerCharacterAPI;
		} else {
			newCharacterAPI.aiController.targetSlot= PlayerController.playerCharacterAPI.movementController.rightSlot;
			newCharacterAPI.aiController.controllerParams.targetAPI = PlayerController.playerCharacterAPI;
		}
		if (callback != null) {
			callback ();
		}
		yield break;	
	}




	static AIAction EnterToChain(){
		AIAction action = new AIAction ();
		action.name = "EnterToChain";
		action.actionCondition = (newParams) => {
			if(!newParams.characterAPI.stats.withoutControl && newParams.characterAPI.stats.IsMaximumSpeed() && !newParams.inChain){
				return true;
			}else{
				return false;
			}
		};
		action.action = EnterToChainAction;
		return action;
	}
	static IEnumerator EnterToChainAction(CharacterAPI newCharacterAPI, AIAction action, AIAction.AfterCallback callback){
		newCharacterAPI.aiController.SetToZeroSLotsAndTargets();
		newCharacterAPI.aiController.controllerParams.inChain = true;
		if (callback != null) {
			callback ();
		}
		yield break;	
	}




	static AIAction SelectTarget(){
		AIAction action = new AIAction ();
		action.name = "SelectTarget";
		action.actionCondition = (newParams) => {
			if(!newParams.targetSelected && newParams.targetInLine && newParams.inChain){
				return true;
			}else{
				return false;
			}
		};
		action.action = SelectTargetAction;
		return action;
	}
	static IEnumerator SelectTargetAction(CharacterAPI newCharacterAPI, AIAction action, AIAction.AfterCallback callback){
		newCharacterAPI.aiController.SelectTarget();
		if (callback != null) {
			callback ();
		}
		yield break;	
	}




	static AIAction Patrol(){
		AIAction action = new AIAction ();
		action.name = "Patrol";
		action.actionCondition = (newParams) => {
			if(!newParams.targetInLine){
				return true;
			}else{
				return false;
			}
		};
		action.action = PatrolAction;
		return action;
	}
	static IEnumerator PatrolAction(CharacterAPI newCharacterAPI, AIAction action, AIAction.AfterCallback callback){
		newCharacterAPI.aiController.AddIgnoreActions (new List<string> { "FaceToPlayer" });
		bool actionNotComplete = true; 
		CharacterAPI playerAPI = PlayerController.player.GetComponent<CharacterAPI>();
		int framesDalay = Random.Range (0, 100);
		for (int i = 0; i < framesDalay; i++) {
			if (action.actionCondition (action.currentParams)) {
				yield return null;
			} else {
				newCharacterAPI.movementController.SetMovement (new Vector2 (0, 0));
				actionNotComplete = false;
			}
		}

		int directionTarget = Random.Range (0, 2);
		GameObject targetGO = new GameObject ();
		Transform newPatrolTarget = targetGO.transform;
		Vector3 spawnCoords = newCharacterAPI.aiController.controllerParams.spawnCoords;
		newCharacterAPI.aiController.controllerParams.patrolTarget = newPatrolTarget;
		float patrolRadius = newCharacterAPI.aiController.controllerParams.patrolRadius;
		if (directionTarget == AIController.left) {
			newPatrolTarget.position = new Vector3 (spawnCoords.x - patrolRadius, spawnCoords.y, spawnCoords.z);
		} else {
			newPatrolTarget.position = new Vector3 (spawnCoords.x + patrolRadius, spawnCoords.y, spawnCoords.z);
		}





		while(actionNotComplete){
			if(action.actionCondition(action.currentParams)){
				Vector2 direction = newPatrolTarget.position - newCharacterAPI.transform.position;
				float distance = System.Math.Abs (direction.x);
				direction.Normalize ();
				if (direction.x > 0) {
					direction.x = 1;
				}
				if (direction.x < 0) {
					direction.x = -1;
				}
				if (!(action.currentParams.restAgainstLeftWall && newPatrolTarget.position.x < newCharacterAPI.transform.position.x) &&
					!(action.currentParams.restAgainstRightWall && newPatrolTarget.position.x > newCharacterAPI.transform.position.x) &&
					!(!action.currentParams.leftPrecipice && newPatrolTarget.position.x < newCharacterAPI.transform.position.x) &&
					!(!action.currentParams.rightPrecipice && newPatrolTarget.position.x > newCharacterAPI.transform.position.x) &&
					((distance - 0.1f) > 0)
				) {
					newCharacterAPI.movementController.SetMovement (newCharacterAPI.movementController.GetCurrentWalkingSpeed (direction));
				} else {
					if((action.currentParams.restAgainstLeftWall && newPatrolTarget.position.x < newCharacterAPI.transform.position.x) || 
						(((distance - 0.1f) <= 0) && directionTarget == AIController.left) ||
						(!action.currentParams.leftPrecipice)
					){
						newPatrolTarget.position = new Vector3 (spawnCoords.x + patrolRadius, spawnCoords.y, spawnCoords.z);
						directionTarget = AIController.right;
					}else{
						newPatrolTarget.position = new Vector3 (spawnCoords.x - patrolRadius, spawnCoords.y, spawnCoords.z);
						directionTarget = AIController.left;
					}
					newCharacterAPI.movementController.SetMovement (new Vector2 (0, 0));
					//actionNotComplete = false;
				}
				yield return null;
			}else{
				newCharacterAPI.movementController.SetMovement (new Vector2 (0, 0));
				actionNotComplete = false;
			}
		}
		if (callback != null) {
			callback ();
		}
		Destroy (targetGO);
		newCharacterAPI.aiController.RemoveIgnoreActions (new List<string> { "FaceToPlayer" });
		yield break;	
	}




	static AIAction FastPunch(){
		AIAction action = new AIAction ();
		action.name = "FastPunch";
		action.actionCondition = (newParams) => {
			SkillSettings skillSettings = newParams.characterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.fastPunch);
			float chance = Random.Range(0, 100.01f);
			if(chance <= 10f){
				if(newParams.distanceToPlayer <= skillSettings.distance &&
					!newParams.characterAPI.stats.onGlobalCooldown &&
					!newParams.characterAPI.stats.inSilence
				){
					return true;
				}else{
					return false;
				}
			}else{
				return false;
			}
		};
		action.action = FastPunchAction;
		return action;
	}
	static IEnumerator FastPunchAction(CharacterAPI newCharacterAPI, AIAction action, AIAction.AfterCallback callback){
		newCharacterAPI.stats.StartGCD (newCharacterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.fastPunch).globalCooldown);
		newCharacterAPI.skills.FastPunch();
		if (callback != null) {
			callback ();
		}
		yield break;	
	}



	static AIAction MiddlePunch(){
		AIAction action = new AIAction ();
		action.name = "MiddlePunch";
		action.actionCondition = (newParams) => {
			SkillSettings skillSettings = newParams.characterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.middlePunch);
			float chance = Random.Range(0, 100.01f);
			if(chance <= 5f){
				if(newParams.distanceToPlayer <= skillSettings.distance &&
					!newParams.characterAPI.stats.onGlobalCooldown &&
					!newParams.characterAPI.stats.inSilence
				){
					return true;
				}else{
					return false;
				}
			}else{
				return false;
			}
		};
		action.action = MiddlePunchAction;
		return action;
	}
	static IEnumerator MiddlePunchAction(CharacterAPI newCharacterAPI, AIAction action, AIAction.AfterCallback callback){
		newCharacterAPI.stats.StartGCD (newCharacterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.middlePunch).globalCooldown);
		newCharacterAPI.skills.MiddlePunch();
		if (callback != null) {
			callback ();
		}
		yield break;	
	}
}
