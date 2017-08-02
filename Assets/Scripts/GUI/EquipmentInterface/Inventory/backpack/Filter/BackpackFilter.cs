using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackpackFilter : MonoBehaviour {
	public List<BackpackFilterButton> filterButtons;
	public static BackpackFilter backpackFilter;
	public List<BackpackItem> filteredItems = new List<BackpackItem> ();
	public static int currentFilterID = 0;
	public static BackpackFilterButton currentFilter;
	public BackpackFilterButton noFilterButton;

	public const int noFilter = 0, armorFilter = 1, weaponFilter = 2,
	accessoriesFilter = 3, runesFilter = 4, consumablesFilter = 5;

	void Awake(){
		backpackFilter = this;
	}


	public static void RefreshFilter(){
		currentFilter = backpackFilter.noFilterButton;
		backpackFilter.ActivateNoFilter ();
		backpackFilter.noFilterButton.ActiveFilter();
		BackpackView.backpackView.currentPage = 1;
	}

	public static void ReFindItems(){
		currentFilter.backpackFilterButtonEvent.Invoke();
	}


	public static void DeactivateAllButtons(){
		foreach (BackpackFilterButton button in backpackFilter.filterButtons) {
			button.activeFilter.SetActive (false);
		}
	}


	public void ActivateNoFilter(){
		currentFilterID = currentFilter.filterID;
		filteredItems = new List<BackpackItem> ();
		BackpackView.backpackView.currentPage = 1;
		BackpackView.backpackView.RedrawBackpack ();
	}

	public void ActivateArmorFilter(){
		currentFilterID = currentFilter.filterID;
		filteredItems = PlayerController.backPackItems.FindAll (i => {
			if(i.itemContent[0].GetType() == typeof(Equipment)){
				Equipment equip = i.itemContent[0] as Equipment;
				if(equip.slotID == Equipment.head || 
					equip.slotID == Equipment.chest ||
					equip.slotID == Equipment.legs){
					return true;
				}else{
					return false;
				}
			}else{
				return false;
			}
		});
		BackpackView.backpackView.currentPage = 1;
		BackpackView.backpackView.RedrawBackpack ();
	}

	public void ActivateWeaponFilter(){
		currentFilterID = currentFilter.filterID;
		filteredItems = PlayerController.backPackItems.FindAll (i => {
			if(i.itemContent[0].GetType() == typeof(Equipment)){
				Equipment equip = i.itemContent[0] as Equipment;
				if(equip.slotID == Equipment.meleeWeapon || 
					equip.slotID == Equipment.fireWeapon ||
					equip.slotID == Equipment.elementalWeapon){
					return true;
				}else{
					return false;
				}
			}else{
				return false;
			}
		});
		BackpackView.backpackView.currentPage = 1;
		BackpackView.backpackView.RedrawBackpack ();
	}

	public void ActivateAccessoriesFilter(){
		currentFilterID = currentFilter.filterID;
		filteredItems = PlayerController.backPackItems.FindAll (i => {
			if(i.itemContent[0].GetType() == typeof(Equipment)){
				Equipment equip = i.itemContent[0] as Equipment;
				if(equip.slotID == Equipment.neck || 
					equip.slotID == Equipment.finger ||
					equip.slotID == Equipment.trinket){
					return true;
				}else{
					return false;
				}
			}else{
				return false;
			}
		});
		BackpackView.backpackView.currentPage = 1;
		BackpackView.backpackView.RedrawBackpack ();
	}

	public void ActivateRunesFilter(){
		currentFilterID = currentFilter.filterID;
		filteredItems = PlayerController.backPackItems.FindAll (i => i.itemContent[0].GetType() == typeof(EquipmentRune));
		BackpackView.backpackView.currentPage = 1;
		BackpackView.backpackView.RedrawBackpack ();
	}

	public void ActivateConsumablesFilter(){
		currentFilterID = currentFilter.filterID;
		filteredItems = PlayerController.backPackItems.FindAll (i => i.itemContent[0].GetType() == typeof(Buff) ||
			i.itemContent[0].GetType() == typeof(InventorySkill) ||
			i.itemContent[0].GetType() == typeof(Consumable)
		);
		BackpackView.backpackView.currentPage = 1;
		BackpackView.backpackView.RedrawBackpack ();
	}
}
