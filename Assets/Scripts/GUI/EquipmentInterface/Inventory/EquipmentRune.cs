using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class EquipmentRune{
	public const int smallRune = 1, middleRune = 2, largeRune = 3;
	static List<int> tier_1_icons = new List<int> (new int[]{1, 2, 3, 4, 5, 6});
	static List<int> tier_2_icons = new List<int> (new int[]{7, 8, 9, 10, 11, 12});
	static List<int> tier_3_icons = new List<int> (new int[]{13, 14, 15, 16, 17, 18});
	static List<int> tier_4_icons = new List<int> (new int[]{19, 20, 21, 22, 23, 24});
	public int skinID = 0;
	public int mainStat = 0;
	public float healthPoints = 0;
	public float complexity = 0;
	public float criticalPoints = 0;
	public float physicalArmorPoints = 0;
	public float elementalArmorPoints = 0;
	public float physicalDamagePoints = 0;
	public float elementalDamagePoints = 0;


	public string GetTitle(){
		string titleFirstPart = "";
		if (tier_1_icons.Contains (skinID)) {
			titleFirstPart = LanguageController.jsonFile ["runes"] ["tiers"] ["t1"];
		} else if (tier_2_icons.Contains (skinID)) {
			titleFirstPart = LanguageController.jsonFile ["runes"] ["tiers"] ["t2"];
		} else if (tier_3_icons.Contains (skinID)) {
			titleFirstPart = LanguageController.jsonFile ["runes"] ["tiers"] ["t3"];
		} else if (tier_4_icons.Contains (skinID)) {
			titleFirstPart = LanguageController.jsonFile ["runes"] ["tiers"] ["t4"];
		}

		/*foreach (KeyValuePair<int, float> pair in GetOrderedStatIDs(3).ToList<KeyValuePair<int, float>>()) {
			Debug.Log (pair.Key + " " + pair.Value);
		}*/

		return titleFirstPart + " " + LanguageController.jsonFile ["runes"] ["runeStats"] [mainStat.ToString()];
	}

	public string GetDesctiption(){
		return LanguageController.jsonFile ["runes"] ["commonDescription"];
	}

	public Sprite GetIcon(){
		return SkillPanelController.skillPanelController.GetSkillTexture ("rune_" + skinID.ToString());
	}

	public void SetIcon(){
		float maximumRuneComplexity = PlayerController.maximumComplexity * (EquipmentGenerator.maximumRuneComplexityPercent/100);
		float tier1Complexity = (maximumRuneComplexity / 2)/2;
		float tier2Complexity = maximumRuneComplexity / 2;
		float tier3Complexity = tier2Complexity + tier1Complexity;
		float tier4Complexity = maximumRuneComplexity;

		List<int> finalList = new List<int> ();
		if (complexity <= tier1Complexity) {
			finalList = tier_1_icons;
		} else if (complexity > tier1Complexity || complexity <= tier2Complexity) {
			finalList = tier_2_icons;
		} else if (complexity > tier2Complexity || complexity <= tier3Complexity) {
			finalList = tier_3_icons;
		} else if (complexity > tier3Complexity || complexity <= tier4Complexity) {
			finalList = tier_4_icons;
		}

		skinID = finalList [GetOrderedStatIDs (1).ToList<KeyValuePair<int, float>>()[0].Key - 1];

	}

	public void SetMainStat(){
		mainStat = GetOrderedStatIDs (1).ToList<KeyValuePair<int, float>> () [0].Key;
	}

	public Dictionary<int, float> GetOrderedStatIDs(int statsNumber){
		Dictionary<int, float> stats = new Dictionary<int, float> ();
		stats.Add (Stats.healthStatID, healthPoints);
		stats.Add (Stats.critStatID, criticalPoints);
		stats.Add (Stats.physicalDamageStatID, physicalDamagePoints);
		stats.Add (Stats.elementalDamageStatID, elementalDamagePoints);
		stats.Add (Stats.physicalArmorStatID, physicalArmorPoints);
		stats.Add (Stats.elementalArmorStatID, elementalArmorPoints);


		List<KeyValuePair<int, float>> equipStatsList = new List<KeyValuePair<int, float>> ();
		equipStatsList = stats.OrderBy (x => x.Value).ToList ();
		equipStatsList.Reverse ();
		equipStatsList = equipStatsList.GetRange (0, statsNumber).ToList();
		stats = equipStatsList.ToDictionary(l=> l.Key, l=>l.Value);

		return stats;
	}

	public Dictionary<int, float> GetStats(){
		Dictionary<int, float> stats = new Dictionary<int, float> ();

		if (healthPoints != 0) {
			stats.Add (Stats.healthStatID, healthPoints);
		}
		if (criticalPoints != 0) {
			stats.Add (Stats.critStatID, criticalPoints);
		}
		if (physicalDamagePoints != 0) {
			stats.Add (Stats.physicalDamageStatID, physicalDamagePoints);
		}
		if (elementalDamagePoints != 0) {
			stats.Add (Stats.elementalDamageStatID, elementalDamagePoints);
		}
		if (physicalArmorPoints != 0) {
			stats.Add (Stats.physicalArmorStatID, physicalArmorPoints);
		}
		if (elementalArmorPoints != 0) {
			stats.Add (Stats.elementalArmorStatID, elementalArmorPoints);
		}

		return stats;
	}
}
