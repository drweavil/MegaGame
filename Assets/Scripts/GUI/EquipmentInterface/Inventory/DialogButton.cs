using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogButton : MonoBehaviour {
	public GameObject activateIndicator;


	public void ButtonDown(){
		activateIndicator.SetActive (true);
	}

	public void ButtonUp(){
		if (activateIndicator.activeInHierarchy) {
			activateIndicator.SetActive (false);
		}
	}


	public void CancelButton(){
		DialogController.dialogController.CloseCurrentDialog ();
	}
}
