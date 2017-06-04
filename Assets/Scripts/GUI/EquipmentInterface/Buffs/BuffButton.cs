using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffButton : MonoBehaviour {
	public GameObject button;
	public int buffID;
	public Image buffIcon;
	public GameObject active;
	public Image buffProcessImage;
	public GameObject buffTime;
	public Text buffTimeText;

	bool touchInButtonZone = false;

	public void TouchInButtonZone(bool value){
		touchInButtonZone = value;
	}

	public void ButtonDown(){
		active.SetActive (true);
	}

	public void ButtonUp(){
		active.SetActive (false);
		if (touchInButtonZone) {
			DialogController.dialogController.currentBuffID = buffID;
			EquipDialog.equipDialogStatic.OpenDialogProcessBuffInfo (buffID);
			DialogController.DeactivateButtons ();
			if (Buffs.GetBuff (buffID).canDeactivate) {
				DialogController.dialogController.buffRemoveButton.SetActive (true);
			}
		}
	}


	public void SetButton(Buff buff){
		button.SetActive (true);
		buffID = buff.buffID;
		buffIcon.sprite = SkillPanelController.skillPanelController.GetSkillTexture ("burst_" + buff.buffID);
		SetTime (buff.buffTime, buff.buffTime);
	}

	public void SetTime(float currentTime, float maximumTime){
		int time = 0;
		string abbreviation = "";
		int timeH = (int)System.Math.Floor(currentTime / 3600f);
		if (timeH >= 1) {
			time = timeH;
			abbreviation = LanguageController.jsonFile ["menu"] ["timeAbbreviations"] ["h"];
		} else {
			int timeM = (int)System.Math.Floor (currentTime/60);
			if (timeM >= 1) {
				time = timeM;
				abbreviation = LanguageController.jsonFile ["menu"] ["timeAbbreviations"] ["m"];
			} else {
				time = (int)System.Math.Round (currentTime, 0);
				abbreviation = LanguageController.jsonFile ["menu"] ["timeAbbreviations"] ["s"];
			}
		}
			
		buffProcessImage.fillAmount = 1 - (1 * currentTime) / maximumTime;
		buffTimeText.text = time.ToString () + abbreviation;
	}

}
