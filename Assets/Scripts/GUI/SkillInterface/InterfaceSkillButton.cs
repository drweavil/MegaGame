using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InterfaceSkillButton : MonoBehaviour {
	public GameObject skillDeactiveObject;
	public bool skillDeactive = false;

	//public int skillID;
	public Text skillTitle;
	public Text skillDescription;
	public Image icon;
	public CurrentSkillState buttonState; 


	public void SetButton(CurrentSkillState state){
		buttonState = state;
		SkillSettings skillSettings = SkillSettingsSet.GetSettings (state.skillID);
		skillTitle.text = skillSettings.GetTitle ();
		skillDescription.text = skillSettings.GetDescription ();	
		//CurrentSkillState state = PlayerController.skillStates.Find (s => s.skillID == state.skillID);

		if (state.skillActive) {
			skillDeactiveObject.SetActive (false);
			skillDeactive = false;
		} else {
			skillDeactiveObject.SetActive (true);
			skillDeactive = true;
		}


		icon.sprite = SkillPanelController.skillPanelController.GetSkillTexture ("skill-" + state.skillID.ToString());
	}

	public void OpenDialog(){
		if (!skillDeactive) {
			EquipDialog.equipDialogStatic.OpenSkillDialog (buttonState.skillID);
		}
	}




	public void DraggingButtonDown(){
		if (!skillDeactive) {
			SkillInterface.skillInterface.draggingIcon.gameObject.SetActive (true);
			SkillInterface.skillInterface.draggingIcon.sprite = SkillPanelController.skillPanelController.GetSkillTexture ("skill-" + buttonState.skillID.ToString ());
			SkillInterface.skillInterface.draggingSkillID = buttonState.skillID;
			SkillInterface.skillInterface.ActivateChangersLumenArea ();
			SkillInterface.skillInterface.draggingIcon.gameObject.transform.position = Input.mousePosition;
		}
	}

	public void DraggingButtonDrag(){
		if (!skillDeactive) {
			SkillInterface.skillInterface.draggingIcon.gameObject.transform.position = Input.mousePosition;
		}
	}

	public void DraggingButtonUp(){
		if (!skillDeactive) {
			SkillInterface.skillInterface.draggingIcon.gameObject.SetActive (false);
			SkillInterface.skillInterface.DeactivateChangersLumenArea ();

			foreach (SkillChanger changer in SkillInterface.skillInterface.skillChangers) {
				if (RectTransformUtility.RectangleContainsScreenPoint (changer.lumenArea.area, Input.mousePosition)) {
					changer.lumenArea.areaEvent.Invoke ();
				}
			}
		}
	}
}
