using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class BackPackButtonEquipContent : MonoBehaviour {
	public Text title;
	public GameObject health;
	public Text healthText;
	public GameObject physicalDamage;
	public Text physicalDamageText;
	public GameObject elementalDamage;
	public Text elementalDamageText;
	public GameObject critical;
	public Text criticalText;
	public GameObject physicalArmor;
	public Text physicalArmorText;
	public GameObject elementalArmor;
	public Text elementalArmorText;
	public GameObject weaponDamage;
	public Text weaponDamageText;


	public GameObject gem1Line;
	public GameObject emptyGem1;
	public GameObject gem1;
	public Image gem1Sprite;

	public GameObject gem2Line;
	public GameObject emptyGem2;
	public GameObject gem2;
	public Image gem2Sprite;

	public void SetEquipmentStats(Equipment equip){
		DeactiveAll ();
		title.text = equip.GetTitle ();

		Dictionary<string, float> equipStats = new Dictionary<string, float> ();
		equipStats.Add ("health", equip.healthPoints);
		equipStats.Add ("physDamage", equip.physicalDamagePoints);
		equipStats.Add ("elemDamage", equip.elementalDamagePoints);
		equipStats.Add ("critical", equip.criticalPoints);
		equipStats.Add ("physArmor", equip.physicalArmorPoints);
		equipStats.Add ("elemArmor", equip.elementalArmorPoints);

		List<KeyValuePair<string, float>> euipStatsList = new List<KeyValuePair<string, float>> ();
		euipStatsList = equipStats.OrderBy (x => x.Value).ToList ();

		int statsNumber = 2;
		if (equip.slotID == Equipment.meleeWeapon || 
			equip.slotID == Equipment.fireWeapon ||
			equip.slotID == Equipment.elementalWeapon) {
			statsNumber = 1;
			weaponDamage.SetActive (true);
			weaponDamageText.text = System.Math.Round(equip.weaponDamage, 2).ToString ();
		}

		euipStatsList = euipStatsList.GetRange (euipStatsList.Count - statsNumber, statsNumber).ToList();
		equipStats = euipStatsList.ToDictionary(l=> l.Key, l=>l.Value);

		if (equipStats.ContainsKey ("health")) {
			health.SetActive (true);
			healthText.text = System.Math.Round(Stats.GetPlusHealthByPoints(equip.healthPoints), 0).ToString ();
		}
		if (equipStats.ContainsKey ("physDamage")) {
			physicalDamage.SetActive (true);
			physicalDamageText.text = System.Math.Round(Stats.GetPlusPhysicalDamageByPoints(equip.physicalDamagePoints), 2).ToString ();
		}
		if (equipStats.ContainsKey ("elemDamage")) {
			elementalDamage.SetActive (true);
			elementalDamageText.text = System.Math.Round(Stats.GetPlusElementalDamageByPoints(equip.elementalDamagePoints), 2).ToString ();
		}
		if (equipStats.ContainsKey ("critical")) {
			critical.SetActive (true);
			criticalText.text = System.Math.Round(Stats.GetPlusCriticalByPoints(equip.criticalPoints), 2).ToString ();
		}
		if (equipStats.ContainsKey ("physArmor")) {
			physicalArmor.SetActive (true);
			physicalArmorText.text = System.Math.Round(Stats.GetPlusPhysicalArmorByPoints(equip.physicalArmorPoints), 2).ToString ();
		}
		if (equipStats.ContainsKey ("elemArmor")) {
			elementalArmor.SetActive (true);
			elementalArmorText.text = System.Math.Round(Stats.GetPlusElementalArmorByPoints(equip.elementalArmorPoints), 2).ToString ();
		}
			

		if (equip.rune1Enable) {
			gem1Line.SetActive (true);
			if (equip.rune1 == null) {
				emptyGem1.SetActive (true);
			} else {
				gem1.SetActive (true);
				gem1Sprite.sprite = SkillPanelController.skillPanelController.GetSkillTexture ("rune_"+equip.rune1.skinID);
			}
		}

		if (equip.rune2Enable) {
			gem2Line.SetActive (true);
			if (equip.rune2 == null) {
				emptyGem2.SetActive (true);
			} else {
				gem2.SetActive (true);
				gem2Sprite.sprite = SkillPanelController.skillPanelController.GetSkillTexture ("rune_"+equip.rune2.skinID);
			}
		}
	}


	public void DeactiveAll(){
		health.SetActive (false);
		physicalDamage.SetActive (false);
		elementalDamage.SetActive (false);
		critical.SetActive (false);
		physicalArmor.SetActive (false);
		elementalArmor.SetActive (false);
		weaponDamage.SetActive (false);

		gem1Line.SetActive (false);
		gem1.SetActive (false);
		emptyGem1.SetActive (false);

		gem2Line.SetActive (false);
		gem2.SetActive (false);
		emptyGem2.SetActive (false);
		//health.SetActive (false);
	}
}
