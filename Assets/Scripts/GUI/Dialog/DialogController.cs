using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogController : MonoBehaviour {
	public static DialogController dialogController;
	public static Color32 plusColor = new Color32(118, 196, 118, 255);
	public static Color32 minusColor = new Color32(255, 12, 7, 255);
	public static Color32 statColor = new Color32(204, 224, 255, 255);
	public GameObject currentDialog;
	public int equipButtonIdInDialog;

	public BackpackItem currenBackpackItemInDialog;
	public int currentBuffID;

	public GameObject backpackAreaEquipButton;
	public GameObject backpackAreaDeleteItemButton;
	public GameObject equipmentSlotToBackpackButton;
	public GameObject buffRemoveButton;


	void Awake(){
		dialogController = this;
	}


	public void CloseCurrentDialog(){
		currentDialog.SetActive (false);
	}

	public static void DeactivateButtons(){
		dialogController.backpackAreaEquipButton.SetActive (false);
		dialogController.backpackAreaDeleteItemButton.SetActive (false);
		dialogController.equipmentSlotToBackpackButton.SetActive (false);
		dialogController.buffRemoveButton.SetActive (false);
	}


	public void BackpackAreaEquipButton(){
		Equipment equip = (Equipment)currenBackpackItemInDialog.itemContent [0];
		Equipment playerEquip = PlayerController.GetEquipmentBySlotID(equip.slotID);
		if (playerEquip.isNullEquip) {
			PlayerController.SetEquip (equip);
			BackpackController.RemoveBackpackItemByID (currenBackpackItemInDialog.itemID, true);
		} else {
			PlayerController.SetEquip (equip);
			BackpackController.ChangeEquipBackpackItem (playerEquip, currenBackpackItemInDialog.itemID, true);
		}
		EquipmentController.equipmentController.RedrawEquipAndStats ();

		CloseCurrentDialog ();
	}

	public void BackpackAreaDeleteItemButton(){
		BackpackController.RemoveBackpackItemByID (currenBackpackItemInDialog.itemID, true);
		CloseCurrentDialog ();
	}

	public void EquipmentSlotToBackpackButton(){
		Equipment tmp = new Equipment ();
		if (equipButtonIdInDialog == Equipment.head) {
			tmp = PlayerController.head;
			PlayerController.SetEquip(PlayerController.GetNullEquipment (Equipment.head));
		} else if (equipButtonIdInDialog == Equipment.chest) {
			tmp = PlayerController.chest;
			PlayerController.SetEquip(PlayerController.GetNullEquipment (Equipment.chest));
		} else if (equipButtonIdInDialog == Equipment.legs) {
			tmp = PlayerController.legs;
			PlayerController.SetEquip(PlayerController.GetNullEquipment (Equipment.legs));
		} else if (equipButtonIdInDialog == Equipment.neck) {
			tmp = PlayerController.neck;
			PlayerController.SetEquip(PlayerController.GetNullEquipment (Equipment.neck));
		} else if (equipButtonIdInDialog == Equipment.finger) {
			tmp = PlayerController.finger;
			PlayerController.SetEquip(PlayerController.GetNullEquipment (Equipment.finger));
		} else if (equipButtonIdInDialog == Equipment.trinket) {
			tmp = PlayerController.trinket;
			PlayerController.SetEquip(PlayerController.GetNullEquipment (Equipment.trinket));
		} else if (equipButtonIdInDialog == Equipment.meleeWeapon) {
			tmp = PlayerController.meleeWeapon;
			PlayerController.SetEquip(PlayerController.GetNullEquipment (Equipment.meleeWeapon));
		} else if (equipButtonIdInDialog == Equipment.fireWeapon) {
			tmp = PlayerController.fireWeapon;
			PlayerController.SetEquip(PlayerController.GetNullEquipment (Equipment.fireWeapon));
		} else if (equipButtonIdInDialog == Equipment.elementalWeapon) {
			tmp = PlayerController.elementalWeapon;
			PlayerController.SetEquip(PlayerController.GetNullEquipment (Equipment.elementalWeapon));
		}

		BackpackItem item = new BackpackItem ();
		item.weight = tmp.weight;
		item.price = tmp.price;
		item.itemContent.Add (tmp);
		item.itemID = PlayerController.GetBackpackItemID ();
		BackpackController.AddBackpackItem (item);

		EquipmentController.equipmentController.RedrawEquipAndStats ();
		BackpackController.backpackController.backpackView.RedrawBackpack ();
		CloseCurrentDialog ();
	}


	public void BuffRemoveButton(){
		BuffsController.StopBuff (currentBuffID);
		CloseCurrentDialog ();
	}
}
