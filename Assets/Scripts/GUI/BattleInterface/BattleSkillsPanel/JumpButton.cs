using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpButton : MonoBehaviour {
	public GameObject active;

	public void ButtonDown(){
		active.SetActive (true);
	}

	public void ButtonUp(){
		PlayerController.playerCharacterAPI.movementController.Jump (MovementController.jumpSpeed);
		active.SetActive (false);
	}
}
