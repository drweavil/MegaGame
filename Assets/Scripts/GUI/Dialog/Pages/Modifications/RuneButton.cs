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

		Dictionary <string, float> stats = rune.GetStats ();
		DeactivateStats ();

		if (stats.ContainsKey ("health")) {
			runeHealth.SetActive (true);
			runeHealthText.text = System.Math.Round (Stats.GetPlusHealthByPoints(stats["health"]), 0).ToString();
		}

		if (stats.ContainsKey ("critical")) {
			runeCritical.SetActive (true);
			runeCriticalText.text = System.Math.Round (Stats.GetPlusCriticalByPoints(stats["critical"]), 2).ToString();
		}

		if (stats.ContainsKey ("physicalDamage")) {
			runePhysicalDamage.SetActive (true);
			runePhysicalDamageText.text = System.Math.Round (Stats.GetPlusPhysicalDamageByPoints(stats["physicalDamage"]), 2).ToString();
		}

		if (stats.ContainsKey ("elementalDamage")) {
			runeElementalDamage.SetActive (true);
			runeElementalDamageText.text = System.Math.Round (Stats.GetPlusElementalDamageByPoints(stats["elementalDamage"]), 2).ToString();
		}

		if (stats.ContainsKey ("physicalArmor")) {
			runePhysicalArmor.SetActive (true);
			runePhysicalArmorText.text = System.Math.Round (Stats.GetPlusPhysicalArmorByPoints(stats["physicalArmor"]), 2).ToString();
		}

		if (stats.ContainsKey ("elementalArmor")) {
			runeElementalArmor.SetActive (true);
			runeElementalArmorText.text = System.Math.Round (Stats.GetPlusElementalArmorByPoints(stats["elementalArmor"]), 2).ToString();
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
