using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EquipmentRune{
	public const int smallRune = 1, middleRune = 2, largeRune = 3;
	public int skinID = 0;
	public float healthPoints = 0;
	public float complexity = 0;
	public float criticalPoints = 0;
	public float physicalArmorPoints = 0;
	public float elementalArmorPoints = 0;
	public float physicalDamagePoints = 0;
	public float elementalDamagePoints = 0;


	public string GetTitle(){
		return "Rune title";
	}

	public string GetDesctiption(){
		return "Rune desctiption";
	}

	public Sprite GetIcon(){
		return SkillPanelController.skillPanelController.GetSkillTexture ("rune_" + skinID.ToString());
	}

	public Dictionary<string, float> GetStats(){
		Dictionary<string, float> stats = new Dictionary<string, float> ();

		if (healthPoints == 0) {
			stats.Add ("health", healthPoints);
		}
		if (criticalPoints == 0) {
			stats.Add ("critical", criticalPoints);
		}
		if (physicalDamagePoints == 0) {
			stats.Add ("physicalDamage", physicalDamagePoints);
		}
		if (elementalDamagePoints == 0) {
			stats.Add ("elementalDamage", elementalDamagePoints);
		}
		if (physicalArmorPoints == 0) {
			stats.Add ("physicalArmor", physicalArmorPoints);
		}
		if (elementalArmorPoints == 0) {
			stats.Add ("elementalArmor", elementalArmorPoints);
		}

		return stats;
	}
}
