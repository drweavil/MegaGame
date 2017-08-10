using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RuneButton : MonoBehaviour {
	public Image runeIcon;
	public GameObject runeHealth;
	public Text runeHealthText;
	public GameObject runeCritical;
	public Text runeCriticalText;
	public GameObject runePhysicalDamage;
	public Text runePhysicalDamageText;
	public GameObject runeElementalDamage;
	public Text runeElementalDamageText;
	public GameObject runePhysicalArmor;
	public Text runePhysicalArmorText;
	public GameObject runeElementalArmor;
	public Text runeElementalArmorText;

	public void SetRune(EquipmentRune rune){
		runeIcon.sprite = rune.GetIcon ();

		Dictionary <int, float> stats = rune.GetStats ();
		DeactivateStats ();

		if (stats.ContainsKey (Stats.healthStatID)) {
			runeHealth.SetActive (true);
			runeHealthText.text = System.Math.Round (Stats.GetPlusHealthByPoints(stats[Stats.healthStatID]), 0).ToString();
		}

		if (stats.ContainsKey (Stats.critStatID)) {
			runeCritical.SetActive (true);
			runeCriticalText.text = System.Math.Round (Stats.GetPlusCriticalByPoints(stats[Stats.critStatID]), 2).ToString();
		}

		if (stats.ContainsKey (Stats.physicalDamageStatID)) {
			runePhysicalDamage.SetActive (true);
			runePhysicalDamageText.text = System.Math.Round (Stats.GetPlusPhysicalDamageByPoints(stats[Stats.physicalDamageStatID]), 2).ToString();
		}

		if (stats.ContainsKey (Stats.elementalDamageStatID)) {
			runeElementalDamage.SetActive (true);
			runeElementalDamageText.text = System.Math.Round (Stats.GetPlusElementalDamageByPoints(stats[Stats.elementalDamageStatID]), 2).ToString();
		}

		if (stats.ContainsKey (Stats.physicalArmorStatID)) {
			runePhysicalArmor.SetActive (true);
			runePhysicalArmorText.text = System.Math.Round (Stats.GetPlusPhysicalArmorByPoints(stats[Stats.physicalArmorStatID]), 2).ToString();
		}

		if (stats.ContainsKey (Stats.elementalArmorStatID)) {
			runeElementalArmor.SetActive (true);
			runeElementalArmorText.text = System.Math.Round (Stats.GetPlusElementalArmorByPoints(stats[Stats.elementalArmorStatID]), 2).ToString();
		}
	}

	public void DeactivateStats(){
		runeHealth.SetActive (false);
		runePhysicalDamage.SetActive (false);
		runeElementalDamage.SetActive (false);
		runeCritical.SetActive (false);
		runePhysicalArmor.SetActive (false);
		runeElementalArmor.SetActive (false);
	}
}
