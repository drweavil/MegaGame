using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentController : MonoBehaviour {
	public static EquipmentController equipmentController;
	public GameObject equipmentInterface;
	public BackpackController backpackController;

	public List<Sprite> armorIcons = new List<Sprite> ();
	public List<Sprite> weaponIcons = new List<Sprite> ();
	public List<Sprite> accessoryIcons = new List<Sprite> ();

	public Text health;
	public Text elementalArmor;
	public Text physicalArmor;
	public Text crit;
	public Text physicalDamage;
	public Text elementalDamage;

	public Text meleeWeaponDamage;
	public Text fireWeaponDamage;
	public Text elementalWeaponDamage;

	public Text gold;
	public Text donatGold;
	public Text lifes;

	public EquipSlotButton headButton;
	public EquipSlotButton chestButton;
	public EquipSlotButton legsButton;
	public EquipSlotButton trinketButton;
	public EquipSlotButton fingerButton;
	public EquipSlotButton neckButton;
	public EquipSlotButton meleeWeaponButton;
	public EquipSlotButton fireWeaponButton;
	public EquipSlotButton elementalWeaponButton;


	public EquipmentBarsController equipmentBarsController;





	void Awake(){
		equipmentController = this;
		armorIcons = new List<Sprite> (Resources.LoadAll<Sprite>("Textures/armorIcons"));
		weaponIcons = new List<Sprite> (Resources.LoadAll<Sprite>("Textures/weaponIcons"));
		accessoryIcons = new List<Sprite> (Resources.LoadAll<Sprite>("Textures/accessoryIcons"));
	}



	public void ActivateInterface(){
		

		equipmentInterface.SetActive (true);
		SetStats ();
		SetResources ();
		SetEquipButtonImages ();

		backpackController.backpackView.currentPage = 1;
		//backpackController.backpackView.RedrawBackpack ();
		BuffsView.RedrawBuffs ();

		BackpackFilter.RefreshFilter ();

	}

	public void DeactivateInterface(){
		equipmentInterface.SetActive (false);
	}

	public void RedrawEquipAndStats(){
		SetStats ();
		SetEquipButtonImages ();
	}


	public void SetHealth(int number){
		health.text = number.ToString ();
	}

	public void SetElementalArmor(float number){
		elementalArmor.text = System.Math.Round (number, 2) + "%";
	}

	public void SetPhysicalArmor(float number){
		physicalArmor.text = System.Math.Round (number, 2) + "%";
	}

	public void SetCritical(float number){
		crit.text = System.Math.Round (number, 2) + "%";
	}

	public void SetPhysicalDamage(float number){
		physicalDamage.text = System.Math.Round (number, 2) + "%";
	}

	public void SetElementalDamage(float number){
		elementalDamage.text = System.Math.Round (number, 2) + "%";
	}

	public void SetStats(){
		SetHealth ((int)System.Math.Round(PlayerController.playerCharacterAPI.stats.GetMaximumHealth(), 0));
		SetPhysicalArmor (PlayerController.playerCharacterAPI.stats.armor);
		SetElementalArmor (PlayerController.playerCharacterAPI.stats.elementalArmor);
		SetCritical (PlayerController.playerCharacterAPI.stats.critical);
		SetPhysicalDamage (PlayerController.playerCharacterAPI.stats.physicalDamage);
		SetElementalDamage (PlayerController.playerCharacterAPI.stats.elementalDamage);

		meleeWeaponDamage.text = System.Math.Round (PlayerController.playerCharacterAPI.stats.weaponDamage, 2).ToString();
		fireWeaponDamage.text = System.Math.Round (PlayerController.playerCharacterAPI.stats.fireWeaponDamage, 2).ToString();
		elementalWeaponDamage.text = System.Math.Round (PlayerController.playerCharacterAPI.stats.elementalWeaponDamage, 2).ToString();
	}

	public void SetResources(){
		equipmentBarsController.SetHealthInjectionPool (PlayerController.currentHealthInjectionPool);
		gold.text = PlayerController.gold.ToString();
		donatGold.text = PlayerController.donatGold.ToString();
		lifes.text = PlayerController.lifes.ToString();
	}


	public void SetEquipButtonImages(){
		headButton.ChangeImage (GetArmorIcon("head_"+ headButton.GetButtonEquipment().skinID));
		chestButton.ChangeImage (GetArmorIcon("chest_"+ chestButton.GetButtonEquipment().skinID));
		legsButton.ChangeImage (GetArmorIcon("legs_"+ legsButton.GetButtonEquipment().skinID));
		trinketButton.ChangeImage (GetAccessoryIcon("trinket_"+ trinketButton.GetButtonEquipment().skinID));
		fingerButton.ChangeImage (GetAccessoryIcon("finger_"+ fingerButton.GetButtonEquipment().skinID));
		neckButton.ChangeImage (GetAccessoryIcon("neck_"+ neckButton.GetButtonEquipment().skinID));
		meleeWeaponButton.ChangeImage (GetWeaponIcon("m_w_"+ meleeWeaponButton.GetButtonEquipment().skinID));
		fireWeaponButton.ChangeImage (GetWeaponIcon("r_w_"+ fireWeaponButton.GetButtonEquipment().skinID));
		elementalWeaponButton.ChangeImage (GetWeaponIcon("e_w_"+ elementalWeaponButton.GetButtonEquipment().skinID));
	}





	public Sprite GetArmorIcon(string name){
		return armorIcons.Find (a => a.name == name);
	}

	public Sprite GetWeaponIcon(string name){
		return weaponIcons.Find (a => a.name == name);
	}

	public Sprite GetAccessoryIcon(string name){
		return accessoryIcons.Find (a => a.name == name);
	}
}
