using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillTalentDialog : MonoBehaviour {
	public Text title;
	public Text current;
	public Text next;

	public ScrollRectUpdater scrollRectUpdater;

	public void SetInfo(int skillID){
		SkillSettings settings = SkillSettingsSet.GetSettings (skillID);
		CurrentSkillState state = PlayerController.skillStates [skillID];

		title.text = settings.GetTitle ();

		current.text = settings.GetTalentDescription ();
		next.text = settings.GetTalentDescription (true);
	}
}
