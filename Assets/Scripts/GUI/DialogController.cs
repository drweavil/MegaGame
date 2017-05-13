using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogController : MonoBehaviour {
	public static DialogController dialogController;
	public static Color32 plusColor = new Color32(118, 196, 118, 255);
	public static Color32 minusColor = new Color32(255, 12, 7, 255);
	public static Color32 statColor = new Color32(204, 224, 255, 255);
	public GameObject currentDialog;


	void Awake(){
		dialogController = this;
	}


	public void CloseCurrentDialog(){
		currentDialog.SetActive (false);
	}
}
