using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackpackView : MonoBehaviour {
	public int currentPage = 1;
	public int maximumPage = 1;
	public static BackpackView backpackView;
	public BackPackButton button1;
	public BackPackButton button2;
	public BackPackButton button3;
	public BackPackButton button4;
	public Text pageInfo;


	void Awake(){
		backpackView = this;
	}

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
		if (page <= maximumPage) {
			SetButtons (BackpackController.GetBackpackItemsByPage (page));
			currentPage = page;
		} else {
			SetButtons (BackpackController.GetBackpackItemsByPage (maximumPage));
			currentPage = maximumPage;
		}
		pageInfo.text = GetPageInfo ();
	}

	public static string GetPageInfo(){
		return backpackView.currentPage.ToString () + "/" + backpackView.maximumPage.ToString ();
	}

	public void RedrawBackpack(){
		if (InterfaceSystemController.interfaceSystemController.equipmentController.equipmentInterface.activeInHierarchy) {
			maximumPage = BackpackController.GetPagesCount ();
			SetPage (currentPage);
			//RecomputeWeight ();
			EquipmentController.equipmentController.equipmentBarsController.SetWeight (PlayerController.currentWeight);
		}
	}

	public void RecomputeWeight(){
		PlayerController.currentWeight = 0;
		foreach (BackpackItem item in PlayerController.backPackItems){
			PlayerController.currentWeight += item.weight;
		}
	}

	public void DeactivateButtons(){
		button1.button.SetActive (false);
		button2.button.SetActive (false);
		button3.button.SetActive (false);
		button4.button.SetActive (false);
	}



}
