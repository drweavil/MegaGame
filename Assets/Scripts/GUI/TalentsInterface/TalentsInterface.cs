using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalentsInterface : MonoBehaviour {
	public static TalentsInterface talentsInterface;
	public GameObject meleeActive;
	public GameObject fireActive;
	public GameObject elementalActive;

	public GameObject meleeTalents;
	public GameObject fireTalents;
	public GameObject elementalTalents;

	public Image expBar;
	public Text talentsAvailableNumber;
	public Text expBarText;

	public List<TalentButton> meleeTalentButtons = new List<TalentButton>();
	public List<TalentButton> fireTalentButtons = new List<TalentButton>();
	public List<TalentButton> elementalTalentButtons = new List<TalentButton>();


	public void Awake(){
		talentsInterface = this;
	}

	public void ActivateInterface (){
		gameObject.SetActive (true);
		if (PlayerController.playerCharacterAPI.stats.specId == Stats.meleeSpec) {
			ActivateMeleeTalents ();
		} else if(PlayerController.playerCharacterAPI.stats.specId == Stats.fireSpec) {
			ActivateFireTalents ();
		} else if(PlayerController.playerCharacterAPI.stats.specId == Stats.elementalSpec) {
			ActivateElementalTalents ();
		}

		DrawTalentBarInfo ();
	}

	public void DeactivateInterface(){
		gameObject.SetActive (false);
	}


	public void ActivateMeleeTalents(){
		meleeActive.SetActive (true);
		fireActive.SetActive (false);
		elementalActive.SetActive (false);

		meleeTalents.SetActive (true);
		fireTalents.SetActive (false);
		elementalTalents.SetActive (false);

		foreach (TalentButton button in meleeTalentButtons) {
			button.UpdateSkillInfo ();
		}
	}

	public void ActivateFireTalents(){
		meleeActive.SetActive (false);
		fireActive.SetActive (true);
		elementalActive.SetActive (false);

		meleeTalents.SetActive (false);
		fireTalents.SetActive (true);
		elementalTalents.SetActive (false);

		foreach (TalentButton button in fireTalentButtons) {
			button.UpdateSkillInfo ();
		}
	}

	public void ActivateElementalTalents(){
		meleeActive.SetActive (false);
		fireActive.SetActive (false);
		elementalActive.SetActive (true);

		meleeTalents.SetActive (false);
		fireTalents.SetActive (false);
		elementalTalents.SetActive (true);

		foreach (TalentButton button in elementalTalentButtons) {
			button.UpdateSkillInfo ();
		}
	}

	public void DrawTalentBarInfo(){
		expBar.fillAmount = (PlayerController.currentExpValue * 1f) / (PlayerController.nextTalentExpValue);
		expBarText.text = System.Math.Round(PlayerController.currentExpValue, 0).ToString () + " / " 
			+ System.Math.Round(PlayerController.nextTalentExpValue, 0).ToString ();
		talentsAvailableNumber.text = PlayerController.talentsAvailableNumber.ToString ();
	}

	public void ReDrawTalentButton(int skillID){
		List<TalentButton> allButtons = new List<TalentButton> ();
		foreach(TalentButton button in meleeTalentButtons){
			allButtons.Add (button);
		}

		foreach(TalentButton button in fireTalentButtons){
			allButtons.Add (button);
		}

		foreach(TalentButton button in elementalTalentButtons){
			allButtons.Add (button);
		}

		TalentButton redrawButton = allButtons.Find (s => s.skillID == skillID);

		redrawButton.UpdateSkillInfo (); 
	}

}
