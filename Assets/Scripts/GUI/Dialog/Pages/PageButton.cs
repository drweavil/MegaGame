using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class PageButton : MonoBehaviour {
	public UnityEvent buttonEvent;
	public GameObject activePanel;
	public GameObject deactivePanel;


	public void Activate(){
		DialogPages.DeactivateAllPages ();
		activePanel.SetActive (true);
		deactivePanel.SetActive (false);
	}
}
