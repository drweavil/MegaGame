using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackpackView : MonoBehaviour {
	public int currentPage = 0;
	public int maximumPage = 0;
	public BackPackButton button1;
	public BackPackButton button2;
	public BackPackButton button3;
	public BackPackButton button4;



	public void SetButtons(List<BackpackItem> items){
		DeactivateButtons ();
		if (items.Count > 0) {
			if (items.Count == 1) {
				button1.SetButton (items [0]);
			}
			if (items.Count == 2) {
				button1.SetButton (items [0]);
				button2.SetButton (items [1]);
			}
			if (items.Count == 3) {
				button1.SetButton (items [0]);
				button2.SetButton (items [1]);
				button3.SetButton (items [2]);
			}
			if (items.Count == 4) {
				button1.SetButton (items [0]);
				button2.SetButton (items [1]);
				button3.SetButton (items [2]);
				button4.SetButton (items [3]);
			}
		}
	}


	public void SetPage(int page){
		SetButtons(BackpackController.GetBackpackItemsByPage (page));
	}

	public void DeactivateButtons(){
		button1.button.SetActive (false);
		button2.button.SetActive (false);
		button3.button.SetActive (false);
		button4.button.SetActive (false);
	}



}
