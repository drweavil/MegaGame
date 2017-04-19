using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AIController : MonoBehaviour {
	public Dictionary<string, AIAction> ignoreActions = new Dictionary<string, AIAction> ();
	Dictionary<string, AIAction> actions = new Dictionary<string, AIAction>();
	public CharacterAPI characterAPI;
	public AIControllerParams controllerParams = new AIControllerParams ();
	public Transform targetSlot;
	public const int left = 0, right = 1;

	RaycastHit hitWithTarget;


	void Awake(){
		/*foreach(AIAction action in AIActions.MeleeBase()){
			actions.Add (action.name, action);
		}
		foreach(AIAction action in AIActions.MeleeSkillActions()){
			actions.Add (action.name, action);
		}*/
		controllerParams.characterAPI = characterAPI;
		controllerParams.spawnCoords = characterAPI.transform.position;
		controllerParams.patrolRadius = 100f;
		//Debug.Log ("alive");

		int random = Random.Range (0, 9);
		List<int> types = new List<int>(new int[] {1, 2, 3, 4, 5, 6, 7, 8, 9});
		characterAPI.SetEnemyType (600, types[random]);
	}
	// Update is called once per frame
	void Update () {
		UpdateParams ();
		foreach (KeyValuePair<string, AIAction> action in actions) {
			if (action.Value.actionCondition (controllerParams) && !ignoreActions.ContainsKey (action.Value.name)) {
				if (characterAPI.stats.isDeath) {
					if (action.Value.name == "ExitFromChain") {
						ignoreActions.Add (action.Value.name, action.Value);
						StartCoroutine (StartAction (action.Value));
					}
				} else {
					ignoreActions.Add (action.Value.name, action.Value);
					StartCoroutine (StartAction (action.Value));
				}
			}
		}
		//Debug.Log(controllerParams.rangeDistance);
	}

	void UpdateParams(){
		Vector3 playerPosition = PlayerController.player.transform.position;
		Vector3 currentPosition = characterAPI.transform.position;
		controllerParams.distanceToPlayer = Vector3.Distance (currentPosition, playerPosition);

		controllerParams.restAgainstRightWall = Physics.CheckSphere(characterAPI.movementController.rightSlot.position, 0.1f, (1 << LayerMask.NameToLayer("Ground")));
		controllerParams.restAgainstLeftWall = Physics.CheckSphere(characterAPI.movementController.leftSlot.position, 0.1f, (1 << LayerMask.NameToLayer("Ground")));
		controllerParams.rightPrecipice = Physics.CheckSphere(characterAPI.movementController.rightPrecipiceCheck.position, 0.1f, (1 << LayerMask.NameToLayer("Ground")));
		controllerParams.leftPrecipice = Physics.CheckSphere(characterAPI.movementController.leftPrecipiceCheck.position, 0.1f, (1 << LayerMask.NameToLayer("Ground")));

		controllerParams.target = targetSlot;

		controllerParams.currentPosition = currentPosition;
		Vector3 directionToTarget = playerPosition - currentPosition;
		directionToTarget.Normalize ();
		controllerParams.targetDirection = directionToTarget;

		//Debug.Log (directionToTarget.y);
		if (characterAPI.stats.specId == Stats.meleeSpec) {
			controllerParams.resourceValue = characterAPI.stats.meleeEnergy;
		} else {
			if (characterAPI.stats.specId == Stats.fireSpec) {
				controllerParams.resourceValue = characterAPI.stats.fireEnergy;
				characterAPI.movementController.movement.y = directionToTarget.y;
				characterAPI.animator.SetFloat ("DirectionY", directionToTarget.y);
			} else {
				controllerParams.resourceValue = characterAPI.stats.magicEnergy;
				characterAPI.movementController.movement.y = directionToTarget.y;
				characterAPI.animator.SetFloat ("DirectionY", directionToTarget.y);
			}
		}
		//characterAPI.movementController.movement.y = directionToTarget.y;
		//characterAPI.animator.SetFloat("DirectionY", directionToTarget.y);
		Physics.Raycast (
			currentPosition, 
			directionToTarget,
			out hitWithTarget,
			10f, 
			(1 << LayerMask.NameToLayer ("Player")) | (1 << LayerMask.NameToLayer ("Ground")));
		if (hitWithTarget.collider != null) {
			if (hitWithTarget.collider.gameObject.layer == LayerMask.NameToLayer ("Player")) {
				controllerParams.targetInLine = true;
			} else {
				if (controllerParams.distanceToPlayer <= 0.5f) {
					controllerParams.targetInLine = true;
				} else {
					controllerParams.targetInLine = false;
				}
			}
		} else {
			if (controllerParams.distanceToPlayer <= 0.5f) {
				controllerParams.targetInLine = true;
			} else {
				controllerParams.targetInLine = false;
			}
		}
	}

	public void SetActionsByEnemyID(int id){
		foreach (AIAction action in AIActions.GetActionsType(id)) {
			actions.Add (action.name, action);
		}
	}


	public void SelectTarget(){
		controllerParams.targetSelected = true;
		if ((PlayerController.playerCharacterAPI.movementController.leftCharacterSlot == null) &&
		    (PlayerController.playerCharacterAPI.movementController.rightCharacterSlot == null)) {
			if (characterAPI.transform.position.x <= PlayerController.playerCharacterAPI.transform.position.x) {
				PlayerController.playerCharacterAPI.movementController.leftCharacterSlot = characterAPI;
				targetSlot = PlayerController.playerCharacterAPI.movementController.leftSlot;	
				controllerParams.targetAPI = PlayerController.playerCharacterAPI;
				PlayerController.leftEnemyChain.Add (characterAPI);

			} else {
				PlayerController.playerCharacterAPI.movementController.rightCharacterSlot = characterAPI;
				targetSlot = PlayerController.playerCharacterAPI.movementController.rightSlot;
				controllerParams.targetAPI = PlayerController.playerCharacterAPI;
				PlayerController.rightEnemyChain.Add (characterAPI);
			}
		} else {
			if (PlayerController.playerCharacterAPI.movementController.leftCharacterSlot == null) {
				PlayerController.playerCharacterAPI.movementController.leftCharacterSlot = characterAPI;
				targetSlot = PlayerController.playerCharacterAPI.movementController.leftSlot;	
				controllerParams.targetAPI = PlayerController.playerCharacterAPI;
				PlayerController.leftEnemyChain.Add (characterAPI);
			} else {
				if (PlayerController.playerCharacterAPI.movementController.rightCharacterSlot == null) {
					PlayerController.playerCharacterAPI.movementController.rightCharacterSlot = characterAPI;
					targetSlot = PlayerController.playerCharacterAPI.movementController.rightSlot;
					controllerParams.targetAPI = PlayerController.playerCharacterAPI;
					PlayerController.rightEnemyChain.Add (characterAPI);
				} else {
					float random = Random.Range (0, 100.00001f);
					int direction;
					if (random <= 50f) {
						direction = AIController.left;
					} else {
						direction = AIController.right;
					}
					CharacterAPI newTarget = GetLastTarget (PlayerController.playerCharacterAPI, direction);
					if (direction == AIController.left) {
						newTarget.movementController.leftCharacterSlot = characterAPI;
						targetSlot = newTarget.movementController.leftSlot;
						PlayerController.leftEnemyChain.Add (characterAPI);
					} else {
						newTarget.movementController.rightCharacterSlot = characterAPI;
						targetSlot = newTarget.movementController.rightSlot;
						PlayerController.rightEnemyChain.Add (characterAPI);
					}
					controllerParams.targetAPI = newTarget;

				}
			}
		}

	}

	CharacterAPI GetLastTarget(CharacterAPI target, int direction){
		CharacterAPI lastTarget = null;
		if (direction ==  AIController.left) {
			if (target.movementController.leftCharacterSlot == null) {
				lastTarget = target;
			} else {
				lastTarget = GetLastTarget (target.movementController.leftCharacterSlot, AIController.left);
			}
		}
		if (direction == AIController.right) {
			if (target.movementController.rightCharacterSlot == null) {
				lastTarget = target;
			} else {
				lastTarget = GetLastTarget (target.movementController.rightCharacterSlot, AIController.right);
			}
		}
		return lastTarget;
	}

	IEnumerator StartAction(AIAction action){
		AIAction.AfterCallback callback = () => {
			ignoreActions.Remove (action.name);	
		};
		action.currentParams = controllerParams;
		StartCoroutine(action.action (characterAPI, action, callback));
		yield break;
	}

	public void AddIgnoreActions(List<string> actionNames){
		foreach (string aName in actionNames) {
			if(actions.ContainsKey(aName) && !ignoreActions.ContainsKey(aName)){
				ignoreActions.Add(aName, actions[aName]);
			}
		}
	}

	public void RemoveIgnoreActions(List<string> actionNames){
		foreach (string aName in actionNames) {
			if(ignoreActions.ContainsKey(aName)){
				ignoreActions.Remove(aName);
			}
		}
	}


	public void ReselectTarget(){
		if (controllerParams.targetSelected) {
			if (targetSlot.gameObject != null) {
				if (targetSlot.gameObject.name == "LeftCharacterSlot") {
					PlayerController.leftEnemyChain.Remove (characterAPI);
					if (characterAPI.movementController.leftCharacterSlot != null) {
						ChainTargetAndSlot (controllerParams.targetAPI, characterAPI.movementController.leftCharacterSlot, left);
					} else {
						if (controllerParams.targetAPI.gameObject.layer == LayerMask.NameToLayer ("Player")) {
							if (PlayerController.playerCharacterAPI.movementController.rightCharacterSlot != null) {
								CharacterAPI lastTarget = GetLastTarget (PlayerController.playerCharacterAPI, right);
								if (lastTarget.aiController.controllerParams.targetAPI.gameObject.layer != LayerMask.NameToLayer ("Player")) {
									lastTarget.aiController.controllerParams.targetAPI.movementController.rightCharacterSlot = null;
								}else{
									PlayerController.playerCharacterAPI.movementController.rightCharacterSlot = null;
								}
								PlayerController.rightEnemyChain.RemoveAt(PlayerController.rightEnemyChain.Count - 1);
								PlayerController.leftEnemyChain.Add (lastTarget);
								ChainTargetAndSlot (controllerParams.targetAPI, lastTarget, left);	
							} else {
								PlayerController.playerCharacterAPI.movementController.leftCharacterSlot = null;
							}
						} else {
							controllerParams.targetAPI.movementController.leftCharacterSlot = null;
						}
					}
					SetToZeroSLotsAndTargets ();
				} else {
					if (targetSlot.gameObject.name == "RightCharacterSlot") {
						PlayerController.rightEnemyChain.Remove (characterAPI);
						if (characterAPI.movementController.rightCharacterSlot != null) {
							ChainTargetAndSlot (controllerParams.targetAPI, characterAPI.movementController.rightCharacterSlot, right);
						} else {
							if (controllerParams.targetAPI.gameObject.layer == LayerMask.NameToLayer ("Player")) {
								if (PlayerController.playerCharacterAPI.movementController.leftCharacterSlot != null) {
									CharacterAPI lastTarget = GetLastTarget (PlayerController.playerCharacterAPI, left);
									if (lastTarget.aiController.controllerParams.targetAPI.gameObject.layer != LayerMask.NameToLayer ("Player")) {
										lastTarget.aiController.controllerParams.targetAPI.movementController.leftCharacterSlot = null;
									} else {
										PlayerController.playerCharacterAPI.movementController.leftCharacterSlot = null;
									}
									PlayerController.leftEnemyChain.RemoveAt(PlayerController.leftEnemyChain.Count - 1);
									PlayerController.rightEnemyChain.Add (lastTarget);
									ChainTargetAndSlot (controllerParams.targetAPI, lastTarget, right);	
								} else {
									PlayerController.playerCharacterAPI.movementController.rightCharacterSlot = null;
								}
							}else {
								controllerParams.targetAPI.movementController.rightCharacterSlot = null;
							}
						}
						SetToZeroSLotsAndTargets ();
					}
				}
			}
			controllerParams.targetSelected = false;
		}
	}


	void ChainTargetAndSlot(CharacterAPI newTarget, CharacterAPI slot, int direction){
		if (direction == AIController.right) {
			newTarget.movementController.rightCharacterSlot = slot;
			slot.aiController.controllerParams.targetAPI = newTarget;
			slot.aiController.targetSlot = newTarget.movementController.rightSlot;
		} else {
			newTarget.movementController.leftCharacterSlot = slot;
			slot.aiController.controllerParams.targetAPI = newTarget;
			slot.aiController.targetSlot = newTarget.movementController.leftSlot;
		}

	}
		

	public void SetToZeroSLotsAndTargets(){
		targetSlot = null;
		controllerParams.targetAPI = null;
		characterAPI.movementController.leftCharacterSlot = null;
		characterAPI.movementController.rightCharacterSlot = null;
		controllerParams.targetSelected = false;
	}
}
