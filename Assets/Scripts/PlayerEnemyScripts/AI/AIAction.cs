using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIAction : MonoBehaviour {
	public string name;
	public delegate bool Condition(AIControllerParams conditionParams);
	public Condition actionCondition;
	public delegate IEnumerator Action(CharacterAPI AICharacterAPI, AIAction action, AfterCallback callback = default(AfterCallback));
	public Action action;
	//public  AfterCallback nullCallback = () => {};
	public delegate void AfterCallback();
	public AIControllerParams currentParams;

	/*public  void NullCallback(){
	}*/

	/*public bool CheckCondition(Dictionary<string, Object> actionParams){
		return actionCondition(actionParams);
	}*/
}
