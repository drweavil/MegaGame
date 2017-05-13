using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipSlotButton : MonoBehaviour {
	public int slotID;
	public Image buttonImage;


	public void SlotButtonDown(){
		
		Equipment buttonEquipment = GetButtonEquipment ();

		EquipDialog.equipDialogStatic.OpenDialogInfo (buttonEquipment, PlayerController.GetNullEquipment (buttonEquipment.slotID));
		/*Debug.Log (Stats.GetPlusHealthByPoints(buttonEquipment.healthPoints));
		Debug.Log (Stats.GetPlusPhysicalArmorByPoints(buttonEquipment.physicalArmorPoints));
		Debug.Log (Stats.GetPlusElementalArmorByPoints(buttonEquipment.elementalArmorPoints));
		Debug.Log (Stats.GetPlusCriticalByPoints(buttonEquipment.criticalPoints));
		Debug.Log (Stats.GetPlusPhysicalDamageByPoints(buttonEquipment.physicalDamagePoints));
		Debug.Log (Stats.GetPlusElementalDamageByPoints(buttonEquipment.elementalDamagePoints));
		Debug.Log (buttonEquipment.weaponDamage);*/
	}


	public Equipment GetButtonEquipment(){
		Equipment buttonEquip = new Equipment();
		if (slotID == Equipment.head) {
			buttonEquip = PlayerController.head;
		} else if (slotID == Equipment.chest) {
			buttonEquip = PlayerController.chest;
		} else if (slotID == Equipment.legs) {
			buttonEquip = PlayerController.legs;
		} else if (slotID == Equipment.trinket) {
			buttonEquip = PlayerController.trinket;
		} else if (slotID == Equipment.finger) {
			buttonEquip = PlayerController.finger;
		} else if (slotID == Equipment.neck) {
			buttonEquip = PlayerController.neck;
		} else if (slotID == Equipment.meleeWeapon) {
			buttonEquip = PlayerController.meleeWeapon;
		} else if (slotID == Equipment.fireWeapon) {
			buttonEquip = PlayerController.fireWeapon;
		} else if (slotID == Equipment.elementalWeapon) {
			buttonEquip = PlayerController.elementalWeapon;
		} 
		return buttonEquip;
	}

	public void ChangeImage(Sprite newImage){
		buttonImage.sprite = newImage;
	}
}
