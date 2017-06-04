using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackpackPanelButtons : MonoBehaviour {
	public GameObject leftButtonActive;
	public GameObject rightButtonActive;
	public bool leftButtonNotTaped = true;
	public bool rightButtonNotTaped = false;



	public void LeftButtonDown(){
		leftButtonActive.SetActive (true);
	}

	public void LeftButtonUp(bool fromChanger = false){
		if (!fromChanger) {
			leftButtonActive.SetActive (false);
		}

		if (leftButtonNotTaped || fromChanger) {
			BackpackController.backpackController.backpackView.currentPage -= 1;
			if (BackpackController.backpackController.backpackView.currentPage == 0) {
				BackpackController.backpackController.backpackView.currentPage = BackpackController.backpackController.backpackView.maximumPage;
			}
			BackpackController.backpackController.backpackView.SetPage (BackpackController.backpackController.backpackView.currentPage);
		

		}
	}

	public void RightButtonDown(){
		rightButtonActive.SetActive (true);
	}

	public void RightButtonUp(bool fromChanger = false){
		if (!fromChanger) {
			rightButtonActive.SetActive (false);
		}
		if (rightButtonNotTaped || fromChanger) {
			BackpackController.backpackController.backpackView.currentPage += 1;
			if (BackpackController.backpackController.backpackView.currentPage == BackpackController.backpackController.backpackView.maximumPage + 1) {
				BackpackController.backpackController.backpackView.currentPage = 1;
			}
			BackpackController.backpackController.backpackView.SetPage (BackpackController.backpackController.backpackView.currentPage);
		}
	}

	public void LeftButtonNotTaped(bool value){
		if (!value) {
			leftButtonNotTaped = false;
		}else{
			StartCoroutine (StartProcess.StartActionAfterFewFrames(2, () => {
				leftButtonNotTaped = true;
			}));
		}
	}

	public void RightButtonNotTaped(bool value){
		if (!value) {
			rightButtonNotTaped = false;
		}else{
			StartCoroutine (StartProcess.StartActionAfterFewFrames(2, () => {
				rightButtonNotTaped = true;
			}));
		}
	}
}
