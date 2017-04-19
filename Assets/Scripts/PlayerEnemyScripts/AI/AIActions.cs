using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class AIActions : MonoBehaviour {
	public static List<AIAction> MeleeBase(){
		List<AIAction> actions = new List<AIAction>();
		//actions.Add (Jump());
		actions.Add (FaceToPlayer ());
		actions.Add (MovementToTarget ());
		actions.Add (ReselectTarget ());
		actions.Add (ExitFromChain ());
		actions.Add (EnterToChain ());
		actions.Add (SelectTarget ());
		actions.Add (Patrol());
		return actions;
	}

	public static List<AIAction> RangeBase(){
		List<AIAction> actions = new List<AIAction>();
		actions.Add (FaceToPlayer ());
		actions.Add (SelectRangeTarget());
		actions.Add (RangeTargetMovement());
		actions.Add (MovementToRangeTarget ());
		actions.Add (ReselectRangeTarget());
		actions.Add (Patrol());
		return actions;
	}
		
	public static List<AIAction> MeleeSkillActions(){
		List<AIAction> actions = new List<AIAction>();
		actions.Add(SlowCast());
		return actions;
	}

	public static List<AIAction> GetActionsType(int enemyID){
		MethodInfo method = typeof(AIActions).GetMethod ("EnemyActionsType_" + enemyID.ToString (), BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
		List<AIAction> actions = (List<AIAction>)method.Invoke (null, null);
		return (List<AIAction>)(actions);
	}

	private static List<AIAction> EnemyActionsType_1(){
		List<AIAction> actions = new List<AIAction> ();
		foreach (AIAction action in MeleeBase()) {
			actions.Add (action);
		}
		actions.Add (Chains());
		actions.Add (MeleeStun());
		actions.Add (Charge());
		actions.Add (FastPunch());
		return actions;
	}

	private static List<AIAction> EnemyActionsType_2(){
		List<AIAction> actions = new List<AIAction> ();
		foreach (AIAction action in MeleeBase()) {
			actions.Add (action);
		}
		actions.Add (ThunderClap());
		actions.Add (BlackHole());
		actions.Add (MeleeWave());
		actions.Add (SlowPunch());
		return actions;
	}

	private static List<AIAction> EnemyActionsType_3(){
		List<AIAction> actions = new List<AIAction> ();
		foreach (AIAction action in RangeBase()) {
			actions.Add (action);
		}
		actions.Add (ToxicGrenadeLaunch());
		actions.Add (ExplosiveMine());
		actions.Add (RocketCircle());
		actions.Add (SlowShot());
		return actions;
	}

	private static List<AIAction> EnemyActionsType_4(){
		List<AIAction> actions = new List<AIAction> ();
		foreach (AIAction action in RangeBase()) {
			actions.Add (action);
		}
		actions.Add (ToxicGrenadeWave());
		actions.Add (LightingBombLaunch());
		actions.Add (GrenadeLaunch());
		actions.Add (FastShot());
		return actions;
	}

	private static List<AIAction> EnemyActionsType_5(){
		List<AIAction> actions = new List<AIAction> ();
		foreach (AIAction action in RangeBase()) {
			actions.Add (action);
		}
		actions.Add (IceStun());
		actions.Add (EarthDisruption());
		actions.Add (ElementalCone());
		actions.Add (MiddleCast());
		return actions;
	}

	private static List<AIAction> EnemyActionsType_6(){
		List<AIAction> actions = new List<AIAction> ();
		foreach (AIAction action in RangeBase()) {
			actions.Add (action);
		}
		actions.Add (IceStun());
		actions.Add (FireWall());
		actions.Add (IceWave());
		actions.Add (SlowCast());
		int random = Random.Range (0, 3);
		if (random == 0) {
			actions.Add (ElementalShield());
		} else {
			if (random == 1) {
				actions.Add (PhysicalShield());
			} else {
				actions.Add (HybridShield());
			}
		}
		return actions;
	}

	private static List<AIAction> EnemyActionsType_7(){
		List<AIAction> actions = new List<AIAction> ();
		foreach (AIAction action in MeleeBase()) {
			actions.Add (action);
		}
		actions.Add (MeleeWaveSlow());
		actions.Add (MeleeWave());
		actions.Add (CirclePunch());
		actions.Add (MiddlePunch());
		return actions;
	}

	private static List<AIAction> EnemyActionsType_8(){
		List<AIAction> actions = new List<AIAction> ();
		foreach (AIAction action in RangeBase()) {
			actions.Add (action);
		}
		actions.Add (FireClap());
		actions.Add (ExplosiveMine());
		actions.Add (GrenadeLaunch());
		actions.Add (MiddleShot());
		return actions;
	}

	private static List<AIAction> EnemyActionsType_9(){
		List<AIAction> actions = new List<AIAction> ();
		foreach (AIAction action in RangeBase()) {
			actions.Add (action);
		}
		actions.Add (FireNova());
		actions.Add (IceWave());
		actions.Add (ElementalCone());
		actions.Add (FastCast());
		return actions;
	}






	static AIAction MovementToTarget(){
		AIAction action = new AIAction ();
		action.name = "MovementToTarget";
		action.actionCondition = (newParams) => {
			float distance;
			if(newParams.inMovement){
				distance = 0;
			}else{
				distance = 0.2f;
			}
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
		action.action = MovementToTargetAction;
		return action;
	}
	static IEnumerator MovementToTargetAction(CharacterAPI newCharacterAPI, AIAction action, AIAction.AfterCallback callback){
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
						action.currentParams.inMovement = true;
					} else {
						newCharacterAPI.movementController.SetMovement (new Vector2 (0, 0));
						actionNotComplete = false;
						action.currentParams.inMovement = false;
					}
					yield return null;
				} else {
					newCharacterAPI.movementController.SetMovement (new Vector2 (0, 0));
					actionNotComplete = false;
					action.currentParams.inMovement = false;
				}
			}else{
				newCharacterAPI.movementController.SetMovement (new Vector2 (0, 0));
				actionNotComplete = false;
				action.currentParams.inMovement = false;
			}
		}
		if (callback != null) {
			callback ();
		}
		newCharacterAPI.aiController.RemoveIgnoreActions (new List<string> { "FaceToPlayer" });
		yield break;
	}



	static AIAction MovementToRangeTarget(){
		AIAction action = new AIAction ();
		action.name = "MovementToRangeTarget";
		action.actionCondition = (newParams) => {
			float distance;
			if(newParams.inMovement){
				distance = 0;
			}else{
				distance = 0.2f;
			}
			float distanceToTarget = 0;
			if(newParams.targetInLine){
				if(newParams.rangeTargetSelected){
					distanceToTarget = System.Math.Abs(newParams.characterAPI.transform.position.x - newParams.rangeTarget.x);
					//Debug.Log(distanceToTarget);
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
		action.action = MovementToRangeTargetAction;
		return action;
	}
	static IEnumerator MovementToRangeTargetAction(CharacterAPI newCharacterAPI, AIAction action, AIAction.AfterCallback callback){
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
				if (action.currentParams.rangeTarget != null) {
					float distanceChangeChance = Random.Range (0, 100f);
					if (!action.currentParams.rangeTargetReselected || (distanceChangeChance <= 0.8f)) {
						action.currentParams.rangeDistance = Random.Range (action.currentParams.minimumRangeDistance, action.currentParams.maximumRangeDistance);
						action.currentParams.rangeTargetSelected = true;
						if (newCharacterAPI.transform.position.x < PlayerController.playerCharacterAPI.transform.position.x) {
							action.currentParams.rangeTarget = new Vector3(
								PlayerController.playerCharacterAPI.transform.position.x - action.currentParams.rangeDistance, 
								PlayerController.playerCharacterAPI.transform.position.y, 
								PlayerController.playerCharacterAPI.transform.position.z);
						} else {
							action.currentParams.rangeTarget = new Vector3(
								PlayerController.playerCharacterAPI.transform.position.x + action.currentParams.rangeDistance, 
								PlayerController.playerCharacterAPI.transform.position.y, 
								PlayerController.playerCharacterAPI.transform.position.z);
						}
						action.currentParams.rangeTargetReselected = true;
					}
					Vector2 direction = action.currentParams.rangeTarget - newCharacterAPI.transform.position;
					float distance = System.Math.Abs (direction.x);
					direction.Normalize ();
					if (direction.x > 0) {
						direction.x = 1;
					}
					if (direction.x < 0) {
						direction.x = -1;
					}
					if (!(action.currentParams.restAgainstLeftWall && action.currentParams.rangeTarget.x < newCharacterAPI.transform.position.x) &&
						!(action.currentParams.restAgainstRightWall && action.currentParams.rangeTarget.x > newCharacterAPI.transform.position.x)) {
						newCharacterAPI.movementController.SetMovement (newCharacterAPI.movementController.GetCurrentSpeed (direction));
						action.currentParams.inMovement = true;
					} else {
						newCharacterAPI.movementController.SetMovement (new Vector2 (0, 0));
						action.currentParams.rangeTargetReselected = false;
						action.currentParams.inMovement = false;
						actionNotComplete = false;
					}
					yield return null;
				} else {
					newCharacterAPI.movementController.SetMovement (new Vector2 (0, 0));
					action.currentParams.rangeTargetReselected = false;
					action.currentParams.inMovement = false;
					actionNotComplete = false;
				}
			}else{
				newCharacterAPI.movementController.SetMovement (new Vector2 (0, 0));
				action.currentParams.rangeTargetReselected = false;
				action.currentParams.inMovement = false;
				actionNotComplete = false;
			}
		}
		if (callback != null) {
			callback ();
		}
		newCharacterAPI.aiController.RemoveIgnoreActions (new List<string> { "FaceToPlayer" });
		yield break;
	}



	static AIAction RangeTargetMovement(){
		AIAction action = new AIAction ();
		action.name = "RangeTargetMovement";
		action.actionCondition = (newParams) => {
			if(newParams.rangeTarget.x < PlayerController.playerCharacterAPI.transform.position.x){
				if(newParams.rangeTarget.x != PlayerController.playerCharacterAPI.transform.position.x - newParams.rangeDistance){
					return true;
				}else{
					return false;
				}
			}else{
				if(newParams.rangeTarget.x != PlayerController.playerCharacterAPI.transform.position.x + newParams.rangeDistance){
					return true;
				}else{
					return false;
				}
			}
		};
		action.action = RangeTargetMovementAction;
		return action;
	}
	static IEnumerator RangeTargetMovementAction(CharacterAPI newCharacterAPI, AIAction action, AIAction.AfterCallback callback){
		if(action.currentParams.rangeTarget.x < PlayerController.playerCharacterAPI.transform.position.x){
			action.currentParams.rangeTarget = new Vector3(
				PlayerController.playerCharacterAPI.transform.position.x - action.currentParams.rangeDistance, 
				PlayerController.playerCharacterAPI.transform.position.y, 
				PlayerController.playerCharacterAPI.transform.position.z
			);
		}else{
			action.currentParams.rangeTarget = new Vector3(
				PlayerController.playerCharacterAPI.transform.position.x + action.currentParams.rangeDistance, 
				PlayerController.playerCharacterAPI.transform.position.y, 
				PlayerController.playerCharacterAPI.transform.position.z
			);
		}
		if (callback != null) {
			callback ();
		}
		yield break;
	}



	static AIAction SelectRangeTarget(){
		AIAction action = new AIAction ();
		action.name = "SelectRangeTarget";
		action.actionCondition = (newParams) => {
			if(newParams.targetInLine){
				if(!newParams.rangeTargetSelected){
					return true;
				}else{
					return false;
				}
			}else{
				return false;
			}
		};
		action.action = SelectRangeTargetAction;
		return action;
	}
	static IEnumerator SelectRangeTargetAction(CharacterAPI newCharacterAPI, AIAction action, AIAction.AfterCallback callback){
		//GameObject targetObject = new GameObject ();
		//targetObject.tag = "RangeTarget";
		action.currentParams.rangeDistance = Random.Range (action.currentParams.minimumRangeDistance, action.currentParams.maximumRangeDistance);
		action.currentParams.rangeTargetSelected = true;
		if (newCharacterAPI.transform.position.x < PlayerController.playerCharacterAPI.transform.position.x) {
			action.currentParams.rangeTarget = new Vector3(
				PlayerController.playerCharacterAPI.transform.position.x - action.currentParams.rangeDistance, 
				PlayerController.playerCharacterAPI.transform.position.y, 
				PlayerController.playerCharacterAPI.transform.position.z);
		} else {
			action.currentParams.rangeTarget = new Vector3(
				PlayerController.playerCharacterAPI.transform.position.x + action.currentParams.rangeDistance, 
				PlayerController.playerCharacterAPI.transform.position.y, 
				PlayerController.playerCharacterAPI.transform.position.z);
		}
		if (callback != null) {
			callback ();
		}
		yield break;
	}

	static AIAction RemoveRangeTarget(){
		AIAction action = new AIAction ();
		action.name = "RemoveRangeTarget";
		action.actionCondition = (newParams) => {
			if(!newParams.targetInLine){
				return true;
			}else{
				return false;
			}
		};
		action.action = RemoveRangeTargetAction;
		return action;
	}
	static IEnumerator RemoveRangeTargetAction(CharacterAPI newCharacterAPI, AIAction action, AIAction.AfterCallback callback){
		action.currentParams.rangeTargetSelected = false;
		if (callback != null) {
			callback ();
		}
		yield break;
	}


	static AIAction ReselectRangeTarget(){
		AIAction action = new AIAction ();
		action.name = "ReselectRangeTarget";
		action.actionCondition = (newParams) => {
			if ((newParams.restAgainstLeftWall || !newParams.leftPrecipice) ||
				(newParams.restAgainstRightWall || !newParams.rightPrecipice)
			) {
				return true;
			} else {
				return false;
			}
		};
		action.action = ReselectRangeTargetAction;
		return action;
	}
	static IEnumerator ReselectRangeTargetAction(CharacterAPI newCharacterAPI, AIAction action, AIAction.AfterCallback callback){
		if ((action.currentParams.restAgainstLeftWall || !action.currentParams.leftPrecipice) &&
			(action.currentParams.restAgainstRightWall || !action.currentParams.rightPrecipice)
		) {
			/*Destroy (newCharacterAPI.aiController.targetSlot.gameObject);
			newCharacterAPI.aiController.targetSlot = PlayerController.playerCharacterAPI.transform;
			action.currentParams.rangeTargetIsPlayer = true;*/
		} else {
			if (action.currentParams.rangeTargetIsPlayer) {
				action.currentParams.rangeTargetIsPlayer = false;
				//GameObject targetObject = new GameObject ();
				//newCharacterAPI.aiController.targetSlot = targetObject.transform;
				action.currentParams.rangeDistance = Random.Range (action.currentParams.minimumRangeDistance, action.currentParams.maximumRangeDistance);
				action.currentParams.rangeTargetSelected = true;
				if (newCharacterAPI.transform.position.x < PlayerController.playerCharacterAPI.transform.position.x) {
					action.currentParams.rangeTarget = new Vector3(
						PlayerController.playerCharacterAPI.transform.position.x - 1f, 
						PlayerController.playerCharacterAPI.transform.position.y, 
						PlayerController.playerCharacterAPI.transform.position.z);
				} else {
					action.currentParams.rangeTarget = new Vector3(
						PlayerController.playerCharacterAPI.transform.position.x + 1f, 
						PlayerController.playerCharacterAPI.transform.position.y, 
						PlayerController.playerCharacterAPI.transform.position.z);
				}
			}


			if ((action.currentParams.restAgainstLeftWall || !action.currentParams.leftPrecipice)) {
				action.currentParams.rangeDistance = Random.Range (2f, 6f);
				action.currentParams.rangeTarget = new Vector3 (
					PlayerController.playerCharacterAPI.transform.position.x + action.currentParams.rangeDistance, 
					PlayerController.playerCharacterAPI.transform.position.y, 
					PlayerController.playerCharacterAPI.transform.position.z
				);
			} else {
				if ((action.currentParams.restAgainstRightWall || !action.currentParams.rightPrecipice)) {
					action.currentParams.rangeDistance = Random.Range (2f, 6f);
					action.currentParams.rangeTarget = new Vector3 (
						PlayerController.playerCharacterAPI.transform.position.x - action.currentParams.rangeDistance, 
						PlayerController.playerCharacterAPI.transform.position.y, 
						PlayerController.playerCharacterAPI.transform.position.z
					);
				}
			}

		}


		if (callback != null) {
			callback ();
		}
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



	static AIAction Jump(){
		AIAction action = new AIAction ();
		action.name = "Jump";
		action.actionCondition = (newParams) => {
			if(newParams.characterAPI.transform.position.y < PlayerController.playerCharacterAPI.transform.position.y){
				if(Random.Range(0, 100f) <= 1f){
					return true;
				}else{
					return false;
				}
			}else{
				return false;
			}
		};
		action.action = JumpAction;
		return action;
	}
	static IEnumerator JumpAction(CharacterAPI newCharacterAPI, AIAction action, AIAction.AfterCallback callback){
		newCharacterAPI.movementController.Jump (new Vector3(0, 250f, 0));
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
		/*GameObject targetGO = new GameObject ();*/
		Vector3 newPatrolTarget; //= targetGO.transform;
		Vector3 spawnCoords = newCharacterAPI.aiController.controllerParams.spawnCoords;
		float patrolRadius = newCharacterAPI.aiController.controllerParams.patrolRadius;
		if (directionTarget == AIController.left) {
			newPatrolTarget = new Vector3 (spawnCoords.x - patrolRadius, spawnCoords.y, spawnCoords.z);
		} else {
			newPatrolTarget = new Vector3 (spawnCoords.x + patrolRadius, spawnCoords.y, spawnCoords.z);
		}
		newCharacterAPI.aiController.controllerParams.patrolTarget = newPatrolTarget;





		while(actionNotComplete){
			if(action.actionCondition(action.currentParams)){
				Vector2 direction = newPatrolTarget - newCharacterAPI.transform.position;
				float distance = System.Math.Abs (direction.x);
				direction.Normalize ();
				if (direction.x > 0) {
					direction.x = 1;
				}
				if (direction.x < 0) {
					direction.x = -1;
				}
				if (!(action.currentParams.restAgainstLeftWall && newPatrolTarget.x < newCharacterAPI.transform.position.x) &&
					!(action.currentParams.restAgainstRightWall && newPatrolTarget.x > newCharacterAPI.transform.position.x) &&
					!(!action.currentParams.leftPrecipice && newPatrolTarget.x < newCharacterAPI.transform.position.x) &&
					!(!action.currentParams.rightPrecipice && newPatrolTarget.x > newCharacterAPI.transform.position.x) &&
					((distance - 0.1f) > 0)
				) {
					newCharacterAPI.movementController.SetMovement (newCharacterAPI.movementController.GetCurrentWalkingSpeed (direction));
				} else {
					if((action.currentParams.restAgainstLeftWall && newPatrolTarget.x < newCharacterAPI.transform.position.x) || 
						(((distance - 0.1f) <= 0) && directionTarget == AIController.left) ||
						(!action.currentParams.leftPrecipice)
					){
						newPatrolTarget = new Vector3 (spawnCoords.x + patrolRadius, spawnCoords.y, spawnCoords.z);
						directionTarget = AIController.right;
					}else{
						newPatrolTarget = new Vector3 (spawnCoords.x - patrolRadius, spawnCoords.y, spawnCoords.z);
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
		//Destroy (targetGO);
		newCharacterAPI.aiController.RemoveIgnoreActions (new List<string> { "FaceToPlayer" });
		yield break;	
	}






	static AIAction FastPunch(){
		AIAction action = new AIAction ();
		action.name = "FastPunch";
		action.actionCondition = (newParams) => {
			if(newParams.targetInLine){
				SkillSettings skillSettings = newParams.characterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.fastPunch);
				//Debug.Log(newParams.characterAPI.stats.SkillOnCD(skillSettings.skillID));
				float chance = Random.Range(0, 100.01f);
				if(chance <= skillSettings.chance){
					if(newParams.distanceToPlayer <= skillSettings.botUseDistance &&
						!newParams.characterAPI.stats.onGlobalCooldown &&
						!newParams.characterAPI.stats.SkillOnCD(skillSettings.skillID) &&
						!newParams.characterAPI.stats.inSilence
					){
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
		action.action = FastPunchAction;
		return action;
	}
	static IEnumerator FastPunchAction(CharacterAPI newCharacterAPI, AIAction action, AIAction.AfterCallback callback){
		SkillSettings skillSettings = newCharacterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.fastPunch);
		newCharacterAPI.stats.StartGCD (skillSettings.globalCooldown);
		newCharacterAPI.stats.StartCD (skillSettings.cooldown, skillSettings.skillID);
		FaceToPlayerAction (newCharacterAPI, action, null);
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
			if(newParams.targetInLine){
				SkillSettings skillSettings = newParams.characterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.middlePunch);
				float chance = Random.Range(0, 100.01f);
				if(chance <= skillSettings.chance){
					if(newParams.distanceToPlayer <= skillSettings.botUseDistance &&
						!newParams.characterAPI.stats.onGlobalCooldown &&
						!newParams.characterAPI.stats.SkillOnCD(skillSettings.skillID) &&
						!newParams.characterAPI.stats.inSilence
					){
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
		action.action = MiddlePunchAction;
		return action;
	}
	static IEnumerator MiddlePunchAction(CharacterAPI newCharacterAPI, AIAction action, AIAction.AfterCallback callback){
		SkillSettings skillSettings = newCharacterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.middlePunch);
		newCharacterAPI.stats.StartGCD (skillSettings.globalCooldown);
		newCharacterAPI.stats.StartCD (skillSettings.cooldown, skillSettings.skillID);
		FaceToPlayerAction (newCharacterAPI, action, null);
		newCharacterAPI.skills.MiddlePunch();
		if (callback != null) {
			callback ();
		}
		yield break;	
	}


	static AIAction SlowPunch(){
		AIAction action = new AIAction ();
		action.name = "SlowPunch";
		action.actionCondition = (newParams) => {
			if(newParams.targetInLine){
				SkillSettings skillSettings = newParams.characterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.slowPunch);
				float chance = Random.Range(0, 100.01f);
				if(chance <= skillSettings.chance){
					if(newParams.distanceToPlayer <= skillSettings.botUseDistance &&
						!newParams.characterAPI.stats.onGlobalCooldown &&
						!newParams.characterAPI.stats.SkillOnCD(skillSettings.skillID) &&
						!newParams.characterAPI.stats.inSilence
					){
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
		action.action = SlowPunchAction;
		return action;
	}
	static IEnumerator SlowPunchAction(CharacterAPI newCharacterAPI, AIAction action, AIAction.AfterCallback callback){
		SkillSettings skillSettings = newCharacterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.slowPunch);
		newCharacterAPI.stats.StartGCD (skillSettings.globalCooldown);
		newCharacterAPI.stats.StartCD (skillSettings.cooldown, skillSettings.skillID);
		FaceToPlayerAction (newCharacterAPI, action, null);
		newCharacterAPI.skills.SlowPunch();
		if (callback != null) {
			callback ();
		}
		yield break;	
	}


	static AIAction MeleeCone(){
		AIAction action = new AIAction ();
		action.name = "MeleeCone";
		action.actionCondition = (newParams) => {
			if(newParams.targetInLine){
				SkillSettings skillSettings = newParams.characterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.meleeCone);
				float chance = Random.Range(0, 100.01f);
				if(chance <= skillSettings.chance){
					if(newParams.distanceToPlayer <= skillSettings.botUseDistance &&
						!newParams.characterAPI.stats.onGlobalCooldown &&
						!newParams.characterAPI.stats.SkillOnCD(skillSettings.skillID) &&
						!newParams.characterAPI.stats.inSilence && 
						(newParams.targetDirection.y <= 0.8f && newParams.targetDirection.y >= -0.8f) &&
						newParams.resourceValue >= skillSettings.resourceRemove
					){
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
		action.action = MeleeConeAction;
		return action;
	}
	static IEnumerator MeleeConeAction(CharacterAPI newCharacterAPI, AIAction action, AIAction.AfterCallback callback){
		SkillSettings skillSettings = newCharacterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.meleeCone);
		newCharacterAPI.stats.StartGCD (skillSettings.globalCooldown);
		newCharacterAPI.stats.StartCD (skillSettings.cooldown, skillSettings.skillID);
		FaceToPlayerAction (newCharacterAPI, action, null);
		newCharacterAPI.skills.MeleeCone();
		if (callback != null) {
			callback ();
		}
		yield break;	
	}


	static AIAction CirclePunch(){
		AIAction action = new AIAction ();
		action.name = "CirclePunch";
		action.actionCondition = (newParams) => {
			if(newParams.targetInLine){
				SkillSettings skillSettings = newParams.characterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.meleeCone);
				float chance = Random.Range(0, 100.01f);
				if(chance <= skillSettings.chance){
					if(newParams.distanceToPlayer <= skillSettings.botUseDistance &&
						!newParams.characterAPI.stats.onGlobalCooldown &&
						!newParams.characterAPI.stats.SkillOnCD(skillSettings.skillID) &&
						!newParams.characterAPI.stats.inSilence && 
						(newParams.targetDirection.y <= 0.8f && newParams.targetDirection.y >= -0.8f) &&
						newParams.resourceValue >= skillSettings.resourceRemove
					){
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
		action.action = CirclePunchAction;
		return action;
	}
	static IEnumerator CirclePunchAction(CharacterAPI newCharacterAPI, AIAction action, AIAction.AfterCallback callback){
		SkillSettings skillSettings = newCharacterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.circlePunch);
		newCharacterAPI.stats.StartGCD (skillSettings.globalCooldown);
		newCharacterAPI.stats.StartCD (skillSettings.cooldown, skillSettings.skillID);
		FaceToPlayerAction (newCharacterAPI, action, null);
		newCharacterAPI.skills.CirclePunch();
		if (callback != null) {
			callback ();
		}
		yield break;	
	}


	static AIAction MeleeWave(){
		AIAction action = new AIAction ();
		action.name = "MeleeWave";
		action.actionCondition = (newParams) => {
			if(newParams.targetInLine){
				SkillSettings skillSettings = newParams.characterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.meleeWave);
				float chance = Random.Range(0, 100.01f);
				if(chance <= skillSettings.chance){
					if(newParams.distanceToPlayer <= skillSettings.botUseDistance &&
						newParams.distanceToPlayer >= skillSettings.minimumDistanceToUse &&
						!newParams.characterAPI.stats.onGlobalCooldown &&
						!newParams.characterAPI.stats.SkillOnCD(skillSettings.skillID) &&
						!newParams.characterAPI.stats.inSilence && 
						(newParams.targetDirection.y <= 0.8f && newParams.targetDirection.y >= -0.8f) &&
						newParams.resourceValue >= skillSettings.resourceRemove
					){
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
		action.action = MeleeWaveAction;
		return action;
	}
	static IEnumerator MeleeWaveAction(CharacterAPI newCharacterAPI, AIAction action, AIAction.AfterCallback callback){
		SkillSettings skillSettings = newCharacterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.meleeWave);
		newCharacterAPI.stats.StartGCD (skillSettings.globalCooldown);
		newCharacterAPI.stats.StartCD (skillSettings.cooldown, skillSettings.skillID);
		FaceToPlayerAction (newCharacterAPI, action, null);
		newCharacterAPI.skills.MeleeWave();
		if (callback != null) {
			callback ();
		}
		yield break;	
	}


	static AIAction BlackHole(){
		AIAction action = new AIAction ();
		action.name = "BlackHole";
		action.actionCondition = (newParams) => {
			if(newParams.targetInLine){
				SkillSettings skillSettings = newParams.characterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.blackHole);
				float chance = Random.Range(0, 100.01f);
				if(chance <= skillSettings.chance){
					if(newParams.distanceToPlayer <= skillSettings.botUseDistance &&
						newParams.distanceToPlayer >= skillSettings.minimumDistanceToUse &&
						!newParams.characterAPI.stats.onGlobalCooldown &&
						!newParams.characterAPI.stats.SkillOnCD(skillSettings.skillID) &&
						!newParams.characterAPI.stats.inSilence && 
						(newParams.targetDirection.y <= 0.8f && newParams.targetDirection.y >= -0.8f) &&
						newParams.resourceValue >= skillSettings.resourceRemove
					){
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
		action.action = BlackHoleAction;
		return action;
	}
	static IEnumerator BlackHoleAction(CharacterAPI newCharacterAPI, AIAction action, AIAction.AfterCallback callback){
		SkillSettings skillSettings = newCharacterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.blackHole);
		newCharacterAPI.stats.StartGCD (skillSettings.globalCooldown);
		newCharacterAPI.stats.StartCD (skillSettings.cooldown, skillSettings.skillID);
		FaceToPlayerAction (newCharacterAPI, action, null);
		newCharacterAPI.skills.BlackHole();
		if (callback != null) {
			callback ();
		}
		yield break;	
	}


	static AIAction Chains(){
		AIAction action = new AIAction ();
		action.name = "Chains";
		action.actionCondition = (newParams) => {
			if(newParams.targetInLine){
				SkillSettings skillSettings = newParams.characterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.chains);
				float chance = Random.Range(0, 100.01f);
				if(chance <= skillSettings.chance){
					if(newParams.distanceToPlayer <= skillSettings.botUseDistance &&
						!newParams.characterAPI.stats.onGlobalCooldown &&
						!newParams.characterAPI.stats.SkillOnCD(skillSettings.skillID) &&
						!newParams.characterAPI.stats.inSilence && 
						(newParams.targetDirection.y <= 0.8f && newParams.targetDirection.y >= -0.8f) &&
						newParams.resourceValue >= skillSettings.resourceRemove
					){
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
		action.action = ChainsAction;
		return action;
	}
	static IEnumerator ChainsAction(CharacterAPI newCharacterAPI, AIAction action, AIAction.AfterCallback callback){
		SkillSettings skillSettings = newCharacterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.chains);
		newCharacterAPI.stats.StartGCD (skillSettings.globalCooldown);
		newCharacterAPI.stats.StartCD (skillSettings.cooldown, skillSettings.skillID);
		FaceToPlayerAction (newCharacterAPI, action, null);
		newCharacterAPI.skills.Chains();
		if (callback != null) {
			callback ();
		}
		yield break;	
	}


	static AIAction MeleeStun(){
		AIAction action = new AIAction ();
		action.name = "MeleeStun";
		action.actionCondition = (newParams) => {
			if(newParams.targetInLine){
				SkillSettings skillSettings = newParams.characterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.meleeStun);
				float chance = Random.Range(0, 100.01f);
				if(chance <= skillSettings.chance){
					if(newParams.distanceToPlayer <= skillSettings.botUseDistance &&
						!newParams.characterAPI.stats.onGlobalCooldown &&
						!newParams.characterAPI.stats.SkillOnCD(skillSettings.skillID) &&
						!newParams.characterAPI.stats.inSilence && 
						(newParams.targetDirection.y <= 0.8f && newParams.targetDirection.y >= -0.8f) &&
						newParams.resourceValue >= skillSettings.resourceRemove
					){
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
		action.action = MeleeStunAction;
		return action;
	}
	static IEnumerator MeleeStunAction(CharacterAPI newCharacterAPI, AIAction action, AIAction.AfterCallback callback){
		SkillSettings skillSettings = newCharacterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.meleeStun);
		newCharacterAPI.stats.StartGCD (skillSettings.globalCooldown);
		newCharacterAPI.stats.StartCD (skillSettings.cooldown, skillSettings.skillID);
		FaceToPlayerAction (newCharacterAPI, action, null);
		newCharacterAPI.skills.MeleeStun();
		if (callback != null) {
			callback ();
		}
		yield break;	
	}



	static AIAction MeleeWaveSlow(){
		AIAction action = new AIAction ();
		action.name = "MeleeWaveSlow";
		action.actionCondition = (newParams) => {
			if(newParams.targetInLine){
				SkillSettings skillSettings = newParams.characterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.meleeWaveSlow);
				float chance = Random.Range(0, 100.01f);
				if(chance <= skillSettings.chance){
					if(newParams.distanceToPlayer <= skillSettings.botUseDistance &&
						newParams.distanceToPlayer >= skillSettings.minimumDistanceToUse &&
						!newParams.characterAPI.stats.onGlobalCooldown &&
						!newParams.characterAPI.stats.SkillOnCD(skillSettings.skillID) &&
						!newParams.characterAPI.stats.inSilence && 
						(newParams.targetDirection.y <= 0.8f && newParams.targetDirection.y >= -0.8f) &&
						newParams.resourceValue >= skillSettings.resourceRemove
					){
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
		action.action = MeleeWaveSlowAction;
		return action;
	}
	static IEnumerator MeleeWaveSlowAction(CharacterAPI newCharacterAPI, AIAction action, AIAction.AfterCallback callback){
		SkillSettings skillSettings = newCharacterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.meleeWaveSlow);
		newCharacterAPI.stats.StartGCD (skillSettings.globalCooldown);
		newCharacterAPI.stats.StartCD (skillSettings.cooldown, skillSettings.skillID);
		FaceToPlayerAction (newCharacterAPI, action, null);
		newCharacterAPI.skills.MeleeWaveSlow();
		if (callback != null) {
			callback ();
		}
		yield break;	
	}



	static AIAction Charge(){
		AIAction action = new AIAction ();
		action.name = "Charge";
		action.actionCondition = (newParams) => {
			if(newParams.targetInLine){
				SkillSettings skillSettings = newParams.characterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.charge);
				float chance = Random.Range(0, 100.01f);
				if(chance <= skillSettings.chance){
					if(newParams.distanceToPlayer <= skillSettings.botUseDistance &&
						newParams.distanceToPlayer >= skillSettings.minimumDistanceToUse &&
						!newParams.characterAPI.stats.onGlobalCooldown &&
						!newParams.characterAPI.stats.SkillOnCD(skillSettings.skillID) &&
						!newParams.characterAPI.stats.inSilence && 
						(newParams.targetDirection.y <= 0.8f && newParams.targetDirection.y >= -0.8f) &&
						newParams.resourceValue >= skillSettings.resourceRemove
					){
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
		action.action = ChargeAction;
		return action;
	}
	static IEnumerator ChargeAction(CharacterAPI newCharacterAPI, AIAction action, AIAction.AfterCallback callback){
		SkillSettings skillSettings = newCharacterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.charge);
		newCharacterAPI.stats.StartGCD (skillSettings.globalCooldown);
		newCharacterAPI.stats.StartCD (skillSettings.cooldown, skillSettings.skillID);
		FaceToPlayerAction (newCharacterAPI, action, null);
		newCharacterAPI.skills.Charge();
		if (callback != null) {
			callback ();
		}
		yield break;	
	}




	static AIAction ThunderClap(){
		AIAction action = new AIAction ();
		action.name = "ThunderClap";
		action.actionCondition = (newParams) => {
			if(newParams.targetInLine){
				SkillSettings skillSettings = newParams.characterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.thunderClap);
				float chance = Random.Range(0, 100.01f);
				if(chance <= skillSettings.chance){
					if(newParams.distanceToPlayer <= skillSettings.botUseDistance &&
						!newParams.characterAPI.stats.onGlobalCooldown &&
						!newParams.characterAPI.stats.SkillOnCD(skillSettings.skillID) &&
						!newParams.characterAPI.stats.inSilence && 
						(newParams.targetDirection.y <= 0.8f && newParams.targetDirection.y >= -0.8f) &&
						newParams.resourceValue >= skillSettings.resourceRemove
					){
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
		action.action = ThunderClapAction;
		return action;
	}
	static IEnumerator ThunderClapAction(CharacterAPI newCharacterAPI, AIAction action, AIAction.AfterCallback callback){
		SkillSettings skillSettings = newCharacterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.thunderClap);
		newCharacterAPI.stats.StartGCD (skillSettings.globalCooldown);
		newCharacterAPI.stats.StartCD (skillSettings.cooldown, skillSettings.skillID);
		FaceToPlayerAction (newCharacterAPI, action, null);
		newCharacterAPI.skills.ThunderClap();
		if (callback != null) {
			callback ();
		}
		yield break;	
	}




	static AIAction SiphonLife(){
		AIAction action = new AIAction ();
		action.name = "SiphonLife";
		action.actionCondition = (newParams) => {
			if(newParams.targetInLine){
				SkillSettings skillSettings = newParams.characterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.siphonLife);
				float chance = Random.Range(0, 100.01f);
				if(chance <= skillSettings.chance){
					if(newParams.distanceToPlayer <= skillSettings.botUseDistance &&
						!newParams.characterAPI.stats.onGlobalCooldown &&
						!newParams.characterAPI.stats.SkillOnCD(skillSettings.skillID) &&
						!newParams.characterAPI.stats.inSilence && 
						(newParams.targetDirection.y <= 0.8f && newParams.targetDirection.y >= -0.8f) &&
						newParams.resourceValue >= skillSettings.resourceRemove
					){
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
		action.action = SiphonLifeAction;
		return action;
	}
	static IEnumerator SiphonLifeAction(CharacterAPI newCharacterAPI, AIAction action, AIAction.AfterCallback callback){
		SkillSettings skillSettings = newCharacterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.siphonLife);
		newCharacterAPI.stats.StartGCD (skillSettings.globalCooldown);
		newCharacterAPI.stats.StartCD (skillSettings.cooldown, skillSettings.skillID);
		FaceToPlayerAction (newCharacterAPI, action, null);
		newCharacterAPI.skills.SiphonLife();
		if (callback != null) {
			callback ();
		}
		yield break;	
	}



	static AIAction FastShot(){
		AIAction action = new AIAction ();
		action.name = "FastShot";
		action.actionCondition = (newParams) => {
			if(newParams.targetInLine){
				SkillSettings skillSettings = newParams.characterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.fastShot);
				//Debug.Log(newParams.characterAPI.stats.SkillOnCD(skillSettings.skillID));
				float chance = Random.Range(0, 100.01f);
				if(chance <= skillSettings.chance){
					if(newParams.distanceToPlayer <= skillSettings.botUseDistance &&
						!newParams.characterAPI.stats.onGlobalCooldown &&
						!newParams.characterAPI.stats.SkillOnCD(skillSettings.skillID) &&
						!newParams.characterAPI.stats.inSilence
					){
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
		action.action = FastShotAction;
		return action;
	}
	static IEnumerator FastShotAction(CharacterAPI newCharacterAPI, AIAction action, AIAction.AfterCallback callback){
		SkillSettings skillSettings = newCharacterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.fastShot);
		newCharacterAPI.stats.StartGCD (skillSettings.globalCooldown);
		newCharacterAPI.stats.StartCD (skillSettings.cooldown, skillSettings.skillID);
		FaceToPlayerAction (newCharacterAPI, action, null);
		newCharacterAPI.skills.FastShot();
		if (callback != null) {
			callback ();
		}
		yield break;	
	}



	static AIAction MiddleShot(){
		AIAction action = new AIAction ();
		action.name = "MiddleShot";
		action.actionCondition = (newParams) => {
			if(newParams.targetInLine){
				SkillSettings skillSettings = newParams.characterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.middleShot);
				//Debug.Log(newParams.characterAPI.stats.SkillOnCD(skillSettings.skillID));
				float chance = Random.Range(0, 100.01f);
				if(chance <= skillSettings.chance){
					if(newParams.distanceToPlayer <= skillSettings.botUseDistance &&
						!newParams.characterAPI.stats.onGlobalCooldown &&
						!newParams.characterAPI.stats.SkillOnCD(skillSettings.skillID) &&
						!newParams.characterAPI.stats.inSilence
					){
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
		action.action = MiddleShotAction;
		return action;
	}
	static IEnumerator MiddleShotAction(CharacterAPI newCharacterAPI, AIAction action, AIAction.AfterCallback callback){
		SkillSettings skillSettings = newCharacterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.middleShot);
		newCharacterAPI.stats.StartGCD (skillSettings.globalCooldown);
		newCharacterAPI.stats.StartCD (skillSettings.cooldown, skillSettings.skillID);
		FaceToPlayerAction (newCharacterAPI, action, null);
		newCharacterAPI.skills.MiddleShot();
		if (callback != null) {
			callback ();
		}
		yield break;	
	}


	static AIAction SlowShot(){
		AIAction action = new AIAction ();
		action.name = "SlowShot";
		action.actionCondition = (newParams) => {
			if(newParams.targetInLine){
				SkillSettings skillSettings = newParams.characterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.slowShot);
				float chance = Random.Range(0, 100.01f);
				if(chance <= skillSettings.chance){
					if(newParams.distanceToPlayer <= skillSettings.botUseDistance &&
						!newParams.characterAPI.stats.onGlobalCooldown &&
						!newParams.characterAPI.stats.SkillOnCD(skillSettings.skillID) &&
						!newParams.characterAPI.stats.inSilence
					){
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
		action.action = SlowShotAction;
		return action;
	}
	static IEnumerator SlowShotAction(CharacterAPI newCharacterAPI, AIAction action, AIAction.AfterCallback callback){
		SkillSettings skillSettings = newCharacterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.slowShot);
		newCharacterAPI.stats.StartGCD (skillSettings.globalCooldown);
		newCharacterAPI.stats.StartCD (skillSettings.cooldown, skillSettings.skillID);
		FaceToPlayerAction (newCharacterAPI, action, null);
		newCharacterAPI.skills.SlowShot();
		if (callback != null) {
			callback ();
		}
		yield break;	
	}



	static AIAction GrenadeLaunch(){
		AIAction action = new AIAction ();
		action.name = "GrenadeLaunch";
		action.actionCondition = (newParams) => {
			if(newParams.targetInLine){
				SkillSettings skillSettings = newParams.characterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.grenadeLaunch);
				float chance = Random.Range(0, 100.01f);
				if(chance <= skillSettings.chance){
					if(newParams.distanceToPlayer <= skillSettings.botUseDistance &&
						!newParams.characterAPI.stats.onGlobalCooldown &&
						!newParams.characterAPI.stats.SkillOnCD(skillSettings.skillID) &&
						!newParams.characterAPI.stats.inSilence &&
						((newParams.characterAPI.stats.fireEnergy + skillSettings.resourceAdd) <= 100)
					){
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
		action.action = GrenadeLaunchAction;
		return action;
	}
	static IEnumerator GrenadeLaunchAction(CharacterAPI newCharacterAPI, AIAction action, AIAction.AfterCallback callback){
		SkillSettings skillSettings = newCharacterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.grenadeLaunch);
		newCharacterAPI.stats.StartGCD (skillSettings.globalCooldown);
		newCharacterAPI.stats.StartCD (skillSettings.cooldown, skillSettings.skillID);
		FaceToPlayerAction (newCharacterAPI, action, null);
		newCharacterAPI.skills.aimCoord = new Vector3 (PlayerController.playerCharacterAPI.transform.position.x, newCharacterAPI.transform.position.y, newCharacterAPI.transform.position.z);
		newCharacterAPI.skills.GrenadeLaunch();
		if (callback != null) {
			callback ();
		}
		yield break;	
	}



	static AIAction GrenadeWave(){
		AIAction action = new AIAction ();
		action.name = "GrenadeWave";
		action.actionCondition = (newParams) => {
			if(newParams.targetInLine){
				SkillSettings skillSettings = newParams.characterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.grenadeWave);
				float chance = Random.Range(0, 100.01f);
				if(chance <= skillSettings.chance){
					if(newParams.distanceToPlayer <= skillSettings.botUseDistance &&
						newParams.distanceToPlayer >= skillSettings.minimumDistanceToUse &&
						!newParams.characterAPI.stats.onGlobalCooldown &&
						!newParams.characterAPI.stats.SkillOnCD(skillSettings.skillID) &&
						!newParams.characterAPI.stats.inSilence && 
						((newParams.characterAPI.stats.fireEnergy + skillSettings.resourceAdd) <= 100)
					){
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
		action.action = GrenadeWaveAction;
		return action;
	}
	static IEnumerator GrenadeWaveAction(CharacterAPI newCharacterAPI, AIAction action, AIAction.AfterCallback callback){
		SkillSettings skillSettings = newCharacterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.grenadeWave);
		newCharacterAPI.stats.StartGCD (skillSettings.globalCooldown);
		newCharacterAPI.stats.StartCD (skillSettings.cooldown, skillSettings.skillID);
		FaceToPlayerAction (newCharacterAPI, action, null);
		newCharacterAPI.skills.GrenadeWave();
		if (callback != null) {
			callback ();
		}
		yield break;	
	}




	static AIAction RocketCircle(){
		AIAction action = new AIAction ();
		action.name = "RocketCircle";
		action.actionCondition = (newParams) => {
			if(newParams.targetInLine){
				SkillSettings skillSettings = newParams.characterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.rocketCircle);
				float chance = Random.Range(0, 100.01f);
				if(chance <= skillSettings.chance){
					if(newParams.distanceToPlayer <= skillSettings.botUseDistance &&
						!newParams.characterAPI.stats.onGlobalCooldown &&
						!newParams.characterAPI.stats.SkillOnCD(skillSettings.skillID) &&
						!newParams.characterAPI.stats.inSilence && 
						((newParams.characterAPI.stats.fireEnergy + skillSettings.resourceAdd) <= 100)
					){
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
		action.action = RocketCircleAction;
		return action;
	}
	static IEnumerator RocketCircleAction(CharacterAPI newCharacterAPI, AIAction action, AIAction.AfterCallback callback){
		SkillSettings skillSettings = newCharacterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.rocketCircle);
		newCharacterAPI.stats.StartGCD (skillSettings.globalCooldown);
		newCharacterAPI.stats.StartCD (skillSettings.cooldown, skillSettings.skillID);
		FaceToPlayerAction (newCharacterAPI, action, null);
		newCharacterAPI.skills.RocketCircle();
		if (callback != null) {
			callback ();
		}
		yield break;	
	}



	static AIAction ExplosiveMine(){
		AIAction action = new AIAction ();
		action.name = "ExplosiveMine";
		action.actionCondition = (newParams) => {
			if(newParams.targetInLine){
				SkillSettings skillSettings = newParams.characterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.explosiveMine);
				float chance = Random.Range(0, 100.01f);
				if(chance <= skillSettings.chance){
					if(newParams.distanceToPlayer <= skillSettings.botUseDistance &&
						!newParams.characterAPI.stats.onGlobalCooldown &&
						!newParams.characterAPI.stats.SkillOnCD(skillSettings.skillID) &&
						!newParams.characterAPI.stats.inSilence &&
						((newParams.characterAPI.stats.fireEnergy + skillSettings.resourceAdd) <= 100)
					){
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
		action.action = ExplosiveMineAction;
		return action;
	}
	static IEnumerator ExplosiveMineAction(CharacterAPI newCharacterAPI, AIAction action, AIAction.AfterCallback callback){
		SkillSettings skillSettings = newCharacterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.explosiveMine);
		newCharacterAPI.stats.StartGCD (skillSettings.globalCooldown);
		newCharacterAPI.stats.StartCD (skillSettings.cooldown, skillSettings.skillID);
		FaceToPlayerAction (newCharacterAPI, action, null);
		newCharacterAPI.skills.aimCoord = new Vector3 (PlayerController.playerCharacterAPI.transform.position.x, newCharacterAPI.transform.position.y, newCharacterAPI.transform.position.z);
		newCharacterAPI.skills.ExplosiveMine();
		if (callback != null) {
			callback ();
		}
		yield break;	
	}




	static AIAction LightingBombLaunch(){
		AIAction action = new AIAction ();
		action.name = "LightingBombLaunch";
		action.actionCondition = (newParams) => {
			if(newParams.targetInLine){
				SkillSettings skillSettings = newParams.characterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.lightingBombLaunch);
				float chance = Random.Range(0, 100.01f);
				if(chance <= skillSettings.chance){
					if(newParams.distanceToPlayer <= skillSettings.botUseDistance &&
						!newParams.characterAPI.stats.onGlobalCooldown &&
						!newParams.characterAPI.stats.SkillOnCD(skillSettings.skillID) &&
						!newParams.characterAPI.stats.inSilence &&
						((newParams.characterAPI.stats.fireEnergy + skillSettings.resourceAdd) <= 100)
					){
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
		action.action = LightingBombLaunchAction;
		return action;
	}
	static IEnumerator LightingBombLaunchAction(CharacterAPI newCharacterAPI, AIAction action, AIAction.AfterCallback callback){
		SkillSettings skillSettings = newCharacterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.lightingBombLaunch);
		newCharacterAPI.stats.StartGCD (skillSettings.globalCooldown);
		newCharacterAPI.stats.StartCD (skillSettings.cooldown, skillSettings.skillID);
		FaceToPlayerAction (newCharacterAPI, action, null);
		newCharacterAPI.skills.aimCoord = new Vector3 (PlayerController.playerCharacterAPI.transform.position.x, newCharacterAPI.transform.position.y, newCharacterAPI.transform.position.z);
		newCharacterAPI.skills.LightingBombLaunch();
		if (callback != null) {
			callback ();
		}
		yield break;	
	}



	static AIAction SnipeShot(){
		AIAction action = new AIAction ();
		action.name = "SnipeShot";
		action.actionCondition = (newParams) => {
			if(newParams.targetInLine){
				SkillSettings skillSettings = newParams.characterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.snipeShot);
				float chance = Random.Range(0, 100.01f);
				if(chance <= skillSettings.chance){
					if(newParams.distanceToPlayer <= skillSettings.botUseDistance &&
						!newParams.characterAPI.stats.onGlobalCooldown &&
						!newParams.characterAPI.stats.SkillOnCD(skillSettings.skillID) &&
						!newParams.characterAPI.stats.inSilence &&
						((newParams.characterAPI.stats.fireEnergy + skillSettings.resourceAdd) <= 100)
					){
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
		action.action = SnipeShotAction;
		return action;
	}
	static IEnumerator SnipeShotAction(CharacterAPI newCharacterAPI, AIAction action, AIAction.AfterCallback callback){
		SkillSettings skillSettings = newCharacterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.snipeShot);
		newCharacterAPI.stats.StartGCD (skillSettings.globalCooldown);
		newCharacterAPI.stats.StartCD (skillSettings.cooldown, skillSettings.skillID);
		FaceToPlayerAction (newCharacterAPI, action, null);
		newCharacterAPI.skills.SnipeShot();
		if (callback != null) {
			callback ();
		}
		yield break;	
	}


	static AIAction ToxicGrenadeWave(){
		AIAction action = new AIAction ();
		action.name = "ToxicGrenadeWave";
		action.actionCondition = (newParams) => {
			if(newParams.targetInLine){
				SkillSettings skillSettings = newParams.characterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.toxicGrenadeWave);
				float chance = Random.Range(0, 100.01f);
				if(chance <= skillSettings.chance){
					if(newParams.distanceToPlayer <= skillSettings.botUseDistance &&
						newParams.distanceToPlayer >= skillSettings.minimumDistanceToUse &&
						!newParams.characterAPI.stats.onGlobalCooldown &&
						!newParams.characterAPI.stats.SkillOnCD(skillSettings.skillID) &&
						!newParams.characterAPI.stats.inSilence && 
						((newParams.characterAPI.stats.fireEnergy + skillSettings.resourceAdd) <= 100)
					){
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
		action.action = ToxicGrenadeWaveAction;
		return action;
	}
	static IEnumerator ToxicGrenadeWaveAction(CharacterAPI newCharacterAPI, AIAction action, AIAction.AfterCallback callback){
		SkillSettings skillSettings = newCharacterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.toxicGrenadeWave);
		newCharacterAPI.stats.StartGCD (skillSettings.globalCooldown);
		newCharacterAPI.stats.StartCD (skillSettings.cooldown, skillSettings.skillID);
		FaceToPlayerAction (newCharacterAPI, action, null);
		newCharacterAPI.skills.ToxicGrenadeWave();
		if (callback != null) {
			callback ();
		}
		yield break;	
	}



	static AIAction RocketJump(){
		AIAction action = new AIAction ();
		action.name = "RocketJump";
		action.actionCondition = (newParams) => {
			if(newParams.targetInLine){
				SkillSettings skillSettings = newParams.characterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.rocketJump);
				float chance = Random.Range(0, 100.01f);
				if(chance <= skillSettings.chance){
					if(newParams.distanceToPlayer <= skillSettings.botUseDistance &&
						!newParams.characterAPI.stats.onGlobalCooldown &&
						!newParams.characterAPI.stats.SkillOnCD(skillSettings.skillID) &&
						!newParams.characterAPI.stats.inSilence &&
						((newParams.characterAPI.stats.fireEnergy + skillSettings.resourceAdd) <= 100)
					){
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
		action.action = RocketJumpAction;
		return action;
	}
	static IEnumerator RocketJumpAction(CharacterAPI newCharacterAPI, AIAction action, AIAction.AfterCallback callback){
		SkillSettings skillSettings = newCharacterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.rocketJump);
		newCharacterAPI.stats.StartGCD (skillSettings.globalCooldown);
		newCharacterAPI.stats.StartCD (skillSettings.cooldown, skillSettings.skillID);
		FaceToPlayerAction (newCharacterAPI, action, null);
		newCharacterAPI.skills.RocketJump();
		if (callback != null) {
			callback ();
		}
		yield break;	
	}


	static AIAction FireClap(){
		AIAction action = new AIAction ();
		action.name = "FireClap";
		action.actionCondition = (newParams) => {
			if(newParams.targetInLine){
				SkillSettings skillSettings = newParams.characterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.fireClap);
				float chance = Random.Range(0, 100.01f);
				if(chance <= skillSettings.chance){
					if(newParams.distanceToPlayer <= skillSettings.botUseDistance &&
						!newParams.characterAPI.stats.onGlobalCooldown &&
						!newParams.characterAPI.stats.SkillOnCD(skillSettings.skillID) &&
						!newParams.characterAPI.stats.inSilence &&
						((newParams.characterAPI.stats.fireEnergy + skillSettings.resourceAdd) <= 100)
					){
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
		action.action = FireClapAction;
		return action;
	}
	static IEnumerator FireClapAction(CharacterAPI newCharacterAPI, AIAction action, AIAction.AfterCallback callback){
		SkillSettings skillSettings = newCharacterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.fireClap);
		newCharacterAPI.stats.StartGCD (skillSettings.globalCooldown);
		newCharacterAPI.stats.StartCD (skillSettings.cooldown, skillSettings.skillID);
		FaceToPlayerAction (newCharacterAPI, action, null);
		newCharacterAPI.skills.FireClap();
		if (callback != null) {
			callback ();
		}
		yield break;	
	}



	static AIAction ToxicGrenadeLaunch(){
		AIAction action = new AIAction ();
		action.name = "ToxicGrenadeLaunch";
		action.actionCondition = (newParams) => {
			if(newParams.targetInLine){
				SkillSettings skillSettings = newParams.characterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.toxicGrenadeLaunch);
				float chance = Random.Range(0, 100.01f);
				if(chance <= skillSettings.chance){
					if(newParams.distanceToPlayer <= skillSettings.botUseDistance &&
						!newParams.characterAPI.stats.onGlobalCooldown &&
						!newParams.characterAPI.stats.SkillOnCD(skillSettings.skillID) &&
						!newParams.characterAPI.stats.inSilence &&
						((newParams.characterAPI.stats.fireEnergy + skillSettings.resourceAdd) <= 100)
					){
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
		action.action = ToxicGrenadeLaunchAction;
		return action;
	}
	static IEnumerator ToxicGrenadeLaunchAction(CharacterAPI newCharacterAPI, AIAction action, AIAction.AfterCallback callback){
		SkillSettings skillSettings = newCharacterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.toxicGrenadeLaunch);
		newCharacterAPI.stats.StartGCD (skillSettings.globalCooldown);
		newCharacterAPI.stats.StartCD (skillSettings.cooldown, skillSettings.skillID);
		FaceToPlayerAction (newCharacterAPI, action, null);
		newCharacterAPI.skills.aimCoord = new Vector3 (PlayerController.playerCharacterAPI.transform.position.x, newCharacterAPI.transform.position.y, newCharacterAPI.transform.position.z);
		newCharacterAPI.skills.ToxicGrenadeLaunch();
		if (callback != null) {
			callback ();
		}
		yield break;	
	}



	static AIAction FastCast(){
		AIAction action = new AIAction ();
		action.name = "FastCast";
		action.actionCondition = (newParams) => {
			if(newParams.targetInLine){
				SkillSettings skillSettings = newParams.characterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.fastCast);
				//Debug.Log(newParams.characterAPI.stats.SkillOnCD(skillSettings.skillID));
				float chance = Random.Range(0, 100.01f);
				if(chance <= skillSettings.chance){
					if(newParams.distanceToPlayer <= skillSettings.botUseDistance &&
						!newParams.characterAPI.stats.onGlobalCooldown &&
						!newParams.characterAPI.stats.SkillOnCD(skillSettings.skillID) &&
						!newParams.characterAPI.stats.inSilence
					){
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
		action.action = FastCastAction;
		return action;
	}
	static IEnumerator FastCastAction(CharacterAPI newCharacterAPI, AIAction action, AIAction.AfterCallback callback){
		SkillSettings skillSettings = newCharacterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.fastCast);
		newCharacterAPI.stats.StartGCD (skillSettings.globalCooldown);
		newCharacterAPI.stats.StartCD (skillSettings.cooldown, skillSettings.skillID);
		FaceToPlayerAction (newCharacterAPI, action, null);
		newCharacterAPI.skills.FastCast();
		if (callback != null) {
			callback ();
		}
		yield break;	
	}




	static AIAction MiddleCast(){
		AIAction action = new AIAction ();
		action.name = "MiddleCast";
		action.actionCondition = (newParams) => {
			if(newParams.targetInLine){
				SkillSettings skillSettings = newParams.characterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.middleCast);
				//Debug.Log(newParams.characterAPI.stats.SkillOnCD(skillSettings.skillID));
				float chance = Random.Range(0, 100.01f);
				if(chance <= skillSettings.chance){
					if(newParams.distanceToPlayer <= skillSettings.botUseDistance &&
						!newParams.characterAPI.stats.onGlobalCooldown &&
						!newParams.characterAPI.stats.SkillOnCD(skillSettings.skillID) &&
						!newParams.characterAPI.stats.inSilence
					){
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
		action.action = MiddleCastAction;
		return action;
	}
	static IEnumerator MiddleCastAction(CharacterAPI newCharacterAPI, AIAction action, AIAction.AfterCallback callback){
		SkillSettings skillSettings = newCharacterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.middleCast);
		newCharacterAPI.stats.StartGCD (skillSettings.globalCooldown);
		newCharacterAPI.stats.StartCD (skillSettings.cooldown, skillSettings.skillID);
		FaceToPlayerAction (newCharacterAPI, action, null);
		newCharacterAPI.skills.MiddleCast();
		if (callback != null) {
			callback ();
		}
		yield break;	
	}




	static AIAction SlowCast(){
		AIAction action = new AIAction ();
		action.name = "SlowCast";
		action.actionCondition = (newParams) => {
			if(newParams.targetInLine){
				SkillSettings skillSettings = newParams.characterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.slowCast);
				//Debug.Log(newParams.characterAPI.stats.SkillOnCD(skillSettings.skillID));
				float chance = Random.Range(0, 100.01f);
				if(chance <= skillSettings.chance){
					if(newParams.distanceToPlayer <= skillSettings.botUseDistance &&
						!newParams.characterAPI.stats.onGlobalCooldown &&
						!newParams.characterAPI.stats.SkillOnCD(skillSettings.skillID) &&
						!newParams.characterAPI.stats.inSilence
					){
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
		action.action = SlowCastAction;
		return action;
	}
	static IEnumerator SlowCastAction(CharacterAPI newCharacterAPI, AIAction action, AIAction.AfterCallback callback){
		SkillSettings skillSettings = newCharacterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.slowCast);
		newCharacterAPI.stats.StartGCD (skillSettings.globalCooldown);
		newCharacterAPI.stats.StartCD (skillSettings.cooldown, skillSettings.skillID);
		FaceToPlayerAction (newCharacterAPI, action, null);
		newCharacterAPI.skills.SlowCast();
		if (callback != null) {
			callback ();
		}
		yield break;	
	}



	static AIAction ElementalCone(){
		AIAction action = new AIAction ();
		action.name = "ElementalCone";
		action.actionCondition = (newParams) => {
			if(newParams.targetInLine){
				SkillSettings skillSettings = newParams.characterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.elementalCone);
				float chance = Random.Range(0, 100.01f);
				if(chance <= skillSettings.chance){
					if(newParams.distanceToPlayer <= skillSettings.botUseDistance &&
						!newParams.characterAPI.stats.onGlobalCooldown &&
						!newParams.characterAPI.stats.SkillOnCD(skillSettings.skillID) &&
						!newParams.characterAPI.stats.inSilence && 
						(newParams.targetDirection.y <= 0.8f && newParams.targetDirection.y >= -0.8f) &&
						newParams.resourceValue >= skillSettings.resourceRemove
					){
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
		action.action = ElementalConeAction;
		return action;
	}
	static IEnumerator ElementalConeAction(CharacterAPI newCharacterAPI, AIAction action, AIAction.AfterCallback callback){
		SkillSettings skillSettings = newCharacterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.elementalCone);
		newCharacterAPI.stats.StartGCD (skillSettings.globalCooldown);
		newCharacterAPI.stats.StartCD (skillSettings.cooldown, skillSettings.skillID);
		FaceToPlayerAction (newCharacterAPI, action, null);
		newCharacterAPI.skills.ElementalCone();
		if (callback != null) {
			callback ();
		}
		yield break;	
	}




	static AIAction FireWall(){
		AIAction action = new AIAction ();
		action.name = "FireWall";
		action.actionCondition = (newParams) => {
			if(newParams.targetInLine){
				SkillSettings skillSettings = newParams.characterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.fireWall);
				//Debug.Log(newParams.characterAPI.stats.SkillOnCD(skillSettings.skillID));
				float chance = Random.Range(0, 100.01f);
				if(chance <= skillSettings.chance){
					if(newParams.distanceToPlayer <= skillSettings.botUseDistance &&
						!newParams.characterAPI.stats.onGlobalCooldown &&
						!newParams.characterAPI.stats.SkillOnCD(skillSettings.skillID) &&
						!newParams.characterAPI.stats.inSilence &&
						newParams.resourceValue >= skillSettings.resourceRemove
					){
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
		action.action = FireWallAction;
		return action;
	}
	static IEnumerator FireWallAction(CharacterAPI newCharacterAPI, AIAction action, AIAction.AfterCallback callback){
		SkillSettings skillSettings = newCharacterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.fireWall);
		newCharacterAPI.stats.StartGCD (skillSettings.globalCooldown);
		newCharacterAPI.stats.StartCD (skillSettings.cooldown, skillSettings.skillID);
		FaceToPlayerAction (newCharacterAPI, action, null);
		newCharacterAPI.skills.FireWall();
		if (callback != null) {
			callback ();
		}
		yield break;	
	}




	static AIAction IceWave(){
		AIAction action = new AIAction ();
		action.name = "IceWave";
		action.actionCondition = (newParams) => {
			if(newParams.targetInLine){
				SkillSettings skillSettings = newParams.characterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.iceWave);
				float chance = Random.Range(0, 100.01f);
				if(chance <= skillSettings.chance){
					if(newParams.distanceToPlayer <= skillSettings.botUseDistance &&
						newParams.distanceToPlayer >= skillSettings.minimumDistanceToUse &&
						!newParams.characterAPI.stats.onGlobalCooldown &&
						!newParams.characterAPI.stats.SkillOnCD(skillSettings.skillID) &&
						!newParams.characterAPI.stats.inSilence && 
						(newParams.targetDirection.y <= 0.8f && newParams.targetDirection.y >= -0.8f) &&
						newParams.resourceValue >= skillSettings.resourceRemove
					){
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
		action.action = IceWaveAction;
		return action;
	}
	static IEnumerator IceWaveAction(CharacterAPI newCharacterAPI, AIAction action, AIAction.AfterCallback callback){
		//Debug.Log ("lol");
		SkillSettings skillSettings = newCharacterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.iceWave);
		newCharacterAPI.stats.StartGCD (skillSettings.globalCooldown);
		newCharacterAPI.stats.StartCD (skillSettings.cooldown, skillSettings.skillID);
		FaceToPlayerAction (newCharacterAPI, action, null);
		newCharacterAPI.skills.IceWave();
		if (callback != null) {
			callback ();
		}
		yield break;	
	}



	static AIAction IceStun(){
		AIAction action = new AIAction ();
		action.name = "IceStun";
		action.actionCondition = (newParams) => {
			if(newParams.targetInLine){
				SkillSettings skillSettings = newParams.characterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.iceStun);
				float chance = Random.Range(0, 100.01f);
				if(chance <= skillSettings.chance){
					if(newParams.distanceToPlayer <= skillSettings.botUseDistance &&
						!newParams.characterAPI.stats.onGlobalCooldown &&
						!newParams.characterAPI.stats.SkillOnCD(skillSettings.skillID) &&
						!newParams.characterAPI.stats.inSilence &&
						newParams.resourceValue >= skillSettings.resourceRemove
					){
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
		action.action = IceStunAction;
		return action;
	}
	static IEnumerator IceStunAction(CharacterAPI newCharacterAPI, AIAction action, AIAction.AfterCallback callback){
		SkillSettings skillSettings = newCharacterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.iceStun);
		newCharacterAPI.stats.StartGCD (skillSettings.globalCooldown);
		newCharacterAPI.stats.StartCD (skillSettings.cooldown, skillSettings.skillID);
		FaceToPlayerAction (newCharacterAPI, action, null);
		newCharacterAPI.skills.IceStun();
		if (callback != null) {
			callback ();
		}
		yield break;	
	}





	static AIAction Blizzard(){
		AIAction action = new AIAction ();
		action.name = "Blizzard";
		action.actionCondition = (newParams) => {
			if(newParams.targetInLine){
				SkillSettings skillSettings = newParams.characterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.blizzard);
				float chance = Random.Range(0, 100.01f);
				if(chance <= skillSettings.chance){
					if(newParams.distanceToPlayer <= skillSettings.botUseDistance &&
						!newParams.characterAPI.stats.onGlobalCooldown &&
						!newParams.characterAPI.stats.SkillOnCD(skillSettings.skillID) &&
						!newParams.characterAPI.stats.inSilence && 
						newParams.resourceValue >= skillSettings.resourceRemove
					){
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
		action.action = BlizzardAction;
		return action;
	}
	static IEnumerator BlizzardAction(CharacterAPI newCharacterAPI, AIAction action, AIAction.AfterCallback callback){
		SkillSettings skillSettings = newCharacterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.blizzard);
		newCharacterAPI.stats.StartGCD (skillSettings.globalCooldown);
		newCharacterAPI.stats.StartCD (skillSettings.cooldown, skillSettings.skillID);
		FaceToPlayerAction (newCharacterAPI, action, null);
		newCharacterAPI.skills.Blizzard();
		if (callback != null) {
			callback ();
		}
		yield break;	
	}




	static AIAction ChainLavaBurst(){
		AIAction action = new AIAction ();
		action.name = "ChainLavaBurst";
		action.actionCondition = (newParams) => {
			if(newParams.targetInLine){
				SkillSettings skillSettings = newParams.characterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.chainLavaBurst);
				float chance = Random.Range(0, 100.01f);
				if(chance <= skillSettings.chance){
					if(newParams.distanceToPlayer <= skillSettings.botUseDistance &&
						!newParams.characterAPI.stats.onGlobalCooldown &&
						!newParams.characterAPI.stats.SkillOnCD(skillSettings.skillID) &&
						!newParams.characterAPI.stats.inSilence && 
						newParams.resourceValue >= skillSettings.resourceRemove
					){
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
		action.action = ChainLavaBurstAction;
		return action;
	}
	static IEnumerator ChainLavaBurstAction(CharacterAPI newCharacterAPI, AIAction action, AIAction.AfterCallback callback){
		SkillSettings skillSettings = newCharacterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.chainLavaBurst);
		newCharacterAPI.stats.StartGCD (skillSettings.globalCooldown);
		newCharacterAPI.stats.StartCD (skillSettings.cooldown, skillSettings.skillID);
		FaceToPlayerAction (newCharacterAPI, action, null);
		newCharacterAPI.skills.ChainLavaBurst();
		if (callback != null) {
			callback ();
		}
		yield break;	
	}




	static AIAction FireNova(){
		AIAction action = new AIAction ();
		action.name = "FireNova";
		action.actionCondition = (newParams) => {
			if(newParams.targetInLine){
				SkillSettings skillSettings = newParams.characterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.fireNova);
				float chance = Random.Range(0, 100.01f);
				if(chance <= skillSettings.chance){
					if(newParams.distanceToPlayer <= skillSettings.botUseDistance &&
						!newParams.characterAPI.stats.onGlobalCooldown &&
						!newParams.characterAPI.stats.SkillOnCD(skillSettings.skillID) &&
						!newParams.characterAPI.stats.inSilence && 
						newParams.resourceValue >= skillSettings.resourceRemove
					){
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
		action.action = FireNovaAction;
		return action;
	}
	static IEnumerator FireNovaAction(CharacterAPI newCharacterAPI, AIAction action, AIAction.AfterCallback callback){
		SkillSettings skillSettings = newCharacterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.fireNova);
		newCharacterAPI.stats.StartGCD (skillSettings.globalCooldown);
		newCharacterAPI.stats.StartCD (skillSettings.cooldown, skillSettings.skillID);
		FaceToPlayerAction (newCharacterAPI, action, null);
		newCharacterAPI.skills.FireNova();
		if (callback != null) {
			callback ();
		}
		yield break;	
	}



	static AIAction PhysicalShield(){
		AIAction action = new AIAction ();
		action.name = "PhysicalShield";
		action.actionCondition = (newParams) => {
			if(newParams.targetInLine){
				SkillSettings skillSettings = newParams.characterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.physicalShield);
				float chance = Random.Range(0, 100.01f);
				if(chance <= skillSettings.chance){
					if((newParams.characterAPI.stats.health <= (newParams.characterAPI.stats.GetMaximumHealth() * 0.6f)) &&
						!newParams.characterAPI.stats.onGlobalCooldown &&
						!newParams.characterAPI.stats.SkillOnCD(skillSettings.skillID) &&
						!newParams.characterAPI.stats.inSilence
					){
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
		action.action = PhysicalShieldAction;
		return action;
	}
	static IEnumerator PhysicalShieldAction(CharacterAPI newCharacterAPI, AIAction action, AIAction.AfterCallback callback){
		SkillSettings skillSettings = newCharacterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.physicalShield);
		newCharacterAPI.stats.StartGCD (skillSettings.globalCooldown);
		newCharacterAPI.stats.StartCD (skillSettings.cooldown, skillSettings.skillID);
		FaceToPlayerAction (newCharacterAPI, action, null);
		newCharacterAPI.skills.PhysicalShield();
		if (callback != null) {
			callback ();
		}
		yield break;	
	}



	static AIAction ElementalShield(){
		AIAction action = new AIAction ();
		action.name = "ElementalShield";
		action.actionCondition = (newParams) => {
			if(newParams.targetInLine){
				SkillSettings skillSettings = newParams.characterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.elementalShield);
				float chance = Random.Range(0, 100.01f);
				if(chance <= skillSettings.chance){
					if((newParams.characterAPI.stats.health <= (newParams.characterAPI.stats.GetMaximumHealth() * 0.6f)) &&
						!newParams.characterAPI.stats.onGlobalCooldown &&
						!newParams.characterAPI.stats.SkillOnCD(skillSettings.skillID) &&
						!newParams.characterAPI.stats.inSilence
					){
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
		action.action = ElementalShieldAction;
		return action;
	}
	static IEnumerator ElementalShieldAction(CharacterAPI newCharacterAPI, AIAction action, AIAction.AfterCallback callback){
		SkillSettings skillSettings = newCharacterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.elementalShield);
		newCharacterAPI.stats.StartGCD (skillSettings.globalCooldown);
		newCharacterAPI.stats.StartCD (skillSettings.cooldown, skillSettings.skillID);
		FaceToPlayerAction (newCharacterAPI, action, null);
		newCharacterAPI.skills.ElementalShield();
		if (callback != null) {
			callback ();
		}
		yield break;	
	}




	static AIAction HybridShield(){
		AIAction action = new AIAction ();
		action.name = "HybridShield";
		action.actionCondition = (newParams) => {
			if(newParams.targetInLine){
				SkillSettings skillSettings = newParams.characterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.hybridShield);
				float chance = Random.Range(0, 100.01f);
				if(chance <= skillSettings.chance){
					if((newParams.characterAPI.stats.health <= (newParams.characterAPI.stats.GetMaximumHealth() * 0.6f)) &&
						!newParams.characterAPI.stats.onGlobalCooldown &&
						!newParams.characterAPI.stats.SkillOnCD(skillSettings.skillID) &&
						!newParams.characterAPI.stats.inSilence
					){
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
		action.action = HybridShieldAction;
		return action;
	}
	static IEnumerator HybridShieldAction(CharacterAPI newCharacterAPI, AIAction action, AIAction.AfterCallback callback){
		SkillSettings skillSettings = newCharacterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.hybridShield);
		newCharacterAPI.stats.StartGCD (skillSettings.globalCooldown);
		newCharacterAPI.stats.StartCD (skillSettings.cooldown, skillSettings.skillID);
		FaceToPlayerAction (newCharacterAPI, action, null);
		newCharacterAPI.skills.HybridShield();
		if (callback != null) {
			callback ();
		}
		yield break;	
	}



	static AIAction EarthDisruption(){
		AIAction action = new AIAction ();
		action.name = "EarthDisruption";
		action.actionCondition = (newParams) => {
			if(newParams.targetInLine){
				SkillSettings skillSettings = newParams.characterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.earthDisruption);
				float chance = Random.Range(0, 100.01f);
				if(chance <= skillSettings.chance){
					if(newParams.distanceToPlayer <= skillSettings.botUseDistance &&
						!newParams.characterAPI.stats.onGlobalCooldown &&
						!newParams.characterAPI.stats.SkillOnCD(skillSettings.skillID) &&
						!newParams.characterAPI.stats.inSilence && 
						newParams.resourceValue >= skillSettings.resourceRemove
					){
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
		action.action = EarthDisruptionAction;
		return action;
	}
	static IEnumerator EarthDisruptionAction(CharacterAPI newCharacterAPI, AIAction action, AIAction.AfterCallback callback){
		SkillSettings skillSettings = newCharacterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.earthDisruption);
		newCharacterAPI.stats.StartGCD (skillSettings.globalCooldown);
		newCharacterAPI.stats.StartCD (skillSettings.cooldown, skillSettings.skillID);
		FaceToPlayerAction (newCharacterAPI, action, null);
		newCharacterAPI.skills.EarthDisruption();
		if (callback != null) {
			callback ();
		}
		yield break;	
	}



	static AIAction IceWall(){
		AIAction action = new AIAction ();
		action.name = "IceWall";
		action.actionCondition = (newParams) => {
			if(newParams.targetInLine){
				SkillSettings skillSettings = newParams.characterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.iceWall);
				float chance = Random.Range(0, 100.01f);
				if(chance <= skillSettings.chance){
					if(newParams.distanceToPlayer <= skillSettings.botUseDistance &&
						!newParams.characterAPI.stats.onGlobalCooldown &&
						!newParams.characterAPI.stats.SkillOnCD(skillSettings.skillID) &&
						!newParams.characterAPI.stats.inSilence && 
						newParams.resourceValue >= skillSettings.resourceRemove
					){
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
		action.action = IceWallAction;
		return action;
	}
	static IEnumerator IceWallAction(CharacterAPI newCharacterAPI, AIAction action, AIAction.AfterCallback callback){
		SkillSettings skillSettings = newCharacterAPI.skills.GetSkillSetting(SkillSettingsSet.SkillID.iceWall);
		newCharacterAPI.stats.StartGCD (skillSettings.globalCooldown);
		newCharacterAPI.stats.StartCD (skillSettings.cooldown, skillSettings.skillID);
		FaceToPlayerAction (newCharacterAPI, action, null);
		newCharacterAPI.skills.IceWall();
		if (callback != null) {
			callback ();
		}
		yield break;	
	}
}
