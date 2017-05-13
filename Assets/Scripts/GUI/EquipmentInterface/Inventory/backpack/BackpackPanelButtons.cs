using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackpackPanelButtons : MonoBehaviour {
	public GameObject leftButtonActive;
	public GameObject rightButtonActive;


	public void LeftButtonDown(){
		leftButtonActive.SetActive (true);
	}

	public void LeftButtonUp(){
		leftButtonActive.SetActive (false);

		BackpackController.backpackController.backpackView.currentPage -= 1;
		if (BackpackController.backpackController.backpackView.currentPage == 0) {
			BackpackController.backpackController.backpackView.currentPage = BackpackController.backpackController.backpackView.maximumPage;
		}
		BackpackController.backpackController.backpackView.SetPage (BackpackController.backpackController.backpackView.currentPage);

	}

	public void RightButtonDown(){
		rightButtonActive.SetActive (true);
	}

	public void RightButtonUp(){
		rightButtonActive.SetActive (false);
		BackpackController.backpackController.backpackView.currentPage += 1;
		if (BackpackController.backpackController.backpackView.currentPage == BackpackController.backpackController.backpackView.maximumPage + 1) {
			BackpackController.backpackController.backpackView.currentPage = 1;
		}
		BackpackController.backpackController.backpackView.SetPage(BackpackController.backpackController.backpackView.currentPage);
	}
}
