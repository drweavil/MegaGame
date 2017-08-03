using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerButton : MonoBehaviour {
	public GameObject active;


	public void ActivateButton(){
		HammerButtonsSystem.DeactivateAll ();
		active.SetActive (true);
	}
}
