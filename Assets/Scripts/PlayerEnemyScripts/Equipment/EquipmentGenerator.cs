using UnityEngine;
using System.Collections;


public class EquipmentGenerator : MonoBehaviour {
	public static int GetWeaponDamageByComplexity(int complexity){
		float percent = Random.Range(0.2f, 0.5f); 
		int weaponDamage = (int)(System.Math.Round(((float)complexity * 0.2f)* percent));
		if (weaponDamage == 0) {
			weaponDamage = 1;
		}
		return weaponDamage;
	}
}
