using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillPanelChanger : MonoBehaviour {
	public int currentPanel = 1;
	public Text indicator;
	public GameObject upButton;
	public GameObject upButtonActive;
	public GameObject downButton;
	public GameObject downButtonActive;



	public void UpButtonDown(){
		upButtonActive.SetActive (true);
	}

	public void UpButtonUp(){
		upButtonActive.SetActive (false);
		/*if (currentPanel == 4) {
			downButton.SetActive (true);
		}
		if (currentPanel == 2) {
			upButton.SetActive (false);
		}*/
		if (currentPanel == 1) {
			currentPanel = 5;
		}
		currentPanel -= 1;
		indicator.text = currentPanel.ToString ();
		SkillPanelController.skillPanelController.SetPanel (currentPanel);
		SkillPanelController.skillPanelController.currentPanelId = currentPanel;
	}


	public void DownButtonDown(){
		downButtonActive.SetActive (true);
	}

	public void DownButtonUp(){
		downButtonActive.SetActive (false);
		/*if (currentPanel == 1) {
			upButton.SetActive (true);
		}
		if (currentPanel == 3) {
			downButton.SetActive (false);
		}*/

		if (currentPanel == 4) {
			currentPanel = 0;
		}

		currentPanel += 1;
		indicator.text = currentPanel.ToString ();
		SkillPanelController.skillPanelController.SetPanel (currentPanel);
		SkillPanelController.skillPanelController.currentPanelId = currentPanel;
	}

	public void SetPanel(int id){
		currentPanel = id;
		indicator.text = id.ToString ();

		/*if (id == 1) {
			upButton.SetActive (false);
			downButton.SetActive (true);
		} else if (id == 4) {
			upButton.SetActive (true);
			downButton.SetActive (false);
		} else {
			upButton.SetActive (true);
			downButton.SetActive (true);
		}*/
	}

}
