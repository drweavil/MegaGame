using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackpackController : MonoBehaviour {
	public static BackpackController backpackController;
	public BackpackView backpackView;
	static int pageItemsCount = 4;
	void Awake(){
		backpackController = this;
	}



	public static void AddBackpackItem(BackpackItem item, bool withRedrawBackpackContent = true){
		PlayerController.backPackItems.Reverse ();
		PlayerController.backPackItems.Add (item);
		PlayerController.backPackItems.Reverse ();

		PlayerController.currentWeight += item.weight;

		if (withRedrawBackpackContent) {
			if (InterfaceSystemController.interfaceSystemController.equipmentController.equipmentInterface.activeInHierarchy) {
				backpackController.backpackView.SetButtons (GetBackpackItemsByPage(backpackController.backpackView.currentPage));	
			}
		}
	}

	public static void RemoveBackpackItemByID(int itemID, bool withRedrawBackpackContent = true){
		int index = PlayerController.backPackItems.FindIndex (i => i.itemID == itemID);
		if (index != -1) {
			PlayerController.currentWeight -= PlayerController.backPackItems [index].weight;
			PlayerController.backPackItems.RemoveAt (index);
		}

		if (withRedrawBackpackContent) {
			if (InterfaceSystemController.interfaceSystemController.equipmentController.equipmentInterface.activeInHierarchy) {
				if (backpackController.backpackView.currentPage < GetPagesCount ()) {
					backpackController.backpackView.SetButtons (GetBackpackItemsByPage(backpackController.backpackView.currentPage));
				} else {
					backpackController.backpackView.SetButtons (GetBackpackItemsByPage(backpackController.backpackView.currentPage));
				}
			}
		}
	}


	public static List<BackpackItem> GetBackpackItemsByPage(int page){
		//List<BackpackItem> items = new List<BackpackItem> ();
		int size = PlayerController.backPackItems.Count;
		int index = (page * pageItemsCount) - pageItemsCount;
		int count = 0;
		int deltaPageSize = size - page * pageItemsCount;
		if (deltaPageSize >= 0) {
			count = 4;
		}
		if (deltaPageSize < 0) {
			count = pageItemsCount + deltaPageSize;
		}
 
		return PlayerController.backPackItems.GetRange (index, count);
	}

	public static int GetPagesCount(){
		int value = 0;
		int size = PlayerController.backPackItems.Count;
		if (size <= pageItemsCount) {
			value = 1;
		}

		if (size > pageItemsCount) {
			/*float floatPage = size/pageItemsCount;
			float roundFloatPage = (float)System.Math.Round (floatPage, 0);
			float deltaSize = floatPage - roundFloatPage;
			if (deltaSize == 0) {
				value = (int)floatPage;
			}
			if (deltaSize < 0) {
				value = (int)roundFloatPage;
			}
			if (deltaSize > 0) {
				value = (int)roundFloatPage + 1;
			}*/
			float floatPage = size / pageItemsCount;
			float roundPage = (float)System.Math.Floor (floatPage);

			if (floatPage % pageItemsCount == 0) {
				value = (int)floatPage;
			} else {
				value = (int)(roundPage + 1);
			}
		}
		return value;
	}





	
}
