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


	public static void ChangeEquipBackpackItem(Equipment newEquip, int backpackItemID, bool withRedraw = false){
		int itemIndex = GetBackpackItemIndexInInventoryList (backpackItemID);
		List<BackpackItem> itemsList = PlayerController.backPackItems;
		itemsList[itemIndex].weight -= itemsList[itemIndex].weight;
		itemsList[itemIndex].price -= itemsList[itemIndex].price;

		itemsList[itemIndex].weight += newEquip.weight;
		itemsList[itemIndex].price += newEquip.price;
		List<object> content = new List<object> ();
		content.Add (newEquip);
		itemsList[itemIndex].itemContent = content;
		backpackController.backpackView.RecomputeWeight ();
		if (withRedraw) {
			backpackController.backpackView.RedrawBackpack ();
		}
	}

	public static void AddBackpackItem(BackpackItem item, bool withRedrawBackpackContent = true){
		PlayerController.backPackItems.Reverse ();

		if (item.itemContent [0].GetType () == typeof(Equipment)) {
			PlayerController.backPackItems.Add (item);
		} else if (item.itemContent [0].GetType () == typeof(Buff)) {
			//PlayerController.backPackItems.FindIndex (b => b.itemContent[0]);
			Buff buff = (Buff)item.itemContent [0];
			int itemIndex = PlayerController.backPackItems.FindLastIndex (b => {
				if (b.itemContent [0].GetType () == typeof(Buff)) {
					Buff findingBuff = (Buff)b.itemContent [0];
					if (findingBuff.buffID == buff.buffID) {
						return true;
					} else {
						return false;
					}
				} else {
					return false;
				}
			});

			if (itemIndex == -1) {
				PlayerController.backPackItems.Add (item);
			} else {
				
				List<BackpackItem> additionalItems = PlayerController.backPackItems [itemIndex].CombineItems (item);
				foreach (BackpackItem iterItem in additionalItems) {
					PlayerController.backPackItems.Add (iterItem);
				}
			}
		}else if(item.itemContent [0].GetType () == typeof(InventorySkill)){
			InventorySkillInfo skillInfo = InventorySkills.GetSkillInfoByID (((InventorySkill)item.itemContent [0]).skillID);
			int itemIndex = PlayerController.backPackItems.FindLastIndex (b => {
				if (b.itemContent [0].GetType () == typeof(InventorySkill)) {
					InventorySkill findingSkill = (InventorySkill)b.itemContent [0];
					if (findingSkill.skillID == skillInfo.skillID) {
						return true;
					} else {
						return false;
					}
				} else {
					return false;
				}
			});

			if (itemIndex == -1) {
				PlayerController.backPackItems.Add (item);
			} else {

				List<BackpackItem> additionalItems = PlayerController.backPackItems [itemIndex].CombineItems (item);
				foreach (BackpackItem iterItem in additionalItems) {
					PlayerController.backPackItems.Add (iterItem);
				}
			}
		} else {
			PlayerController.backPackItems.Add (item);	
		}
		PlayerController.backPackItems.Reverse ();

		PlayerController.currentWeight += item.weight;

		if (withRedrawBackpackContent) {
			if (InterfaceSystemController.interfaceSystemController.equipmentController.equipmentInterface.activeInHierarchy) {
				backpackController.backpackView.RedrawBackpack ();	
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
				backpackController.backpackView.RedrawBackpack ();
			}
		}
	}
		

	public static int GetBackpackItemIndexInInventoryList(int backpackItemID){
		return PlayerController.backPackItems.FindIndex (i => i.itemID == backpackItemID);
	}

	public static BackpackItem GetBackpackItemByID(int ID){
		return PlayerController.backPackItems.Find (i => i.itemID == ID);
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
			float floatPage = size / pageItemsCount;
			float roundPage = (float)System.Math.Floor (floatPage);

			if (size % pageItemsCount == 0) {
				value = (int)floatPage;
			} else {
				value = (int)(roundPage + 1);
			}
		}
		return value;
	}





	
}
