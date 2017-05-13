using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackPackButton : MonoBehaviour {
	public int inventoryObjectId;
	public Image buttonIcon;
	public GameObject button;
	public GameObject stackFrame;
	public GameObject withoutStackFrame;
	public Text stackText;
	public GameObject backLight;
	public GameObject active;
	public BackPackButtonEquipContent equipContent;


	public void SetButton(BackpackItem item){
		button.SetActive (true);
		stackFrame.SetActive (false);
		if (item.itemContent.Count > 1) {
			stackFrame.SetActive (true);
			stackText.text = item.itemContent.Count.ToString ();
		}

		inventoryObjectId = item.itemID;

		if (item.itemContent [0].GetType() == typeof(Equipment)) {
			Equipment equip = (Equipment)item.itemContent [0];
			if (equip.slotID == Equipment.head) {
				buttonIcon.sprite = EquipmentController.equipmentController.GetArmorIcon ("head_" + equip.skinID);
			} else if (equip.slotID == Equipment.chest) {
				buttonIcon.sprite = EquipmentController.equipmentController.GetArmorIcon ("chest_" + equip.skinID);
			} else if (equip.slotID == Equipment.legs) {
				buttonIcon.sprite = EquipmentController.equipmentController.GetArmorIcon ("legs_" + equip.skinID);
			} else if (equip.slotID == Equipment.trinket) {
				buttonIcon.sprite = EquipmentController.equipmentController.GetAccessoryIcon ("trinket_" + equip.skinID);
			} else if (equip.slotID == Equipment.finger) {
				buttonIcon.sprite = EquipmentController.equipmentController.GetAccessoryIcon ("finger_" + equip.skinID);
			} else if (equip.slotID == Equipment.neck) {
				buttonIcon.sprite = EquipmentController.equipmentController.GetAccessoryIcon ("neck_" + equip.skinID);
			} else if (equip.slotID == Equipment.meleeWeapon) {
				buttonIcon.sprite = EquipmentController.equipmentController.GetWeaponIcon ("m_w_" + equip.skinID);
			} else if (equip.slotID == Equipment.fireWeapon) {
				buttonIcon.sprite = EquipmentController.equipmentController.GetWeaponIcon ("r_w_" + equip.skinID);
			} else if (equip.slotID == Equipment.elementalWeapon) {
				buttonIcon.sprite = EquipmentController.equipmentController.GetWeaponIcon ("e_w_" + equip.skinID);
			}
			equipContent.SetEquipmentStats (equip);
		}
	}


}
