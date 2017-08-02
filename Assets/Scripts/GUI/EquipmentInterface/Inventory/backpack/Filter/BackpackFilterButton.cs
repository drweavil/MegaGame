using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BackpackFilterButton : MonoBehaviour {
	public GameObject activeFilter;
	public int filterID = 0;
	public UnityEvent backpackFilterButtonEvent; 


	public void ActiveFilter(){
		BackpackFilter.DeactivateAllButtons ();
		activeFilter.SetActive (true);
		BackpackFilter.currentFilter = this;
		backpackFilterButtonEvent.Invoke ();
	}
}
