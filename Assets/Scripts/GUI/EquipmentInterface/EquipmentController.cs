using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentController : MonoBehaviour {
	public static EquipmentController equipmentController;
	public GameObject equipmentInterface;

	public Text health;
	public Text elementalArmor;
	public Text physicalArmor;
	public Text crit;
	public Text physicalDamage;
	public Text elementalDamage;


	public void ActivateInterface(){
		equipmentInterface.SetActive (true);
		SetStats ();
	}

	public void DeactivateInterface(){
		equipmentInterface.SetActive (false);
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
	}
}
