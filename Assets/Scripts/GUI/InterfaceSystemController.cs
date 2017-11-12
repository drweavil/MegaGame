using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceSystemController: MonoBehaviour {
	public static InterfaceSystemController interfaceSystemController;

	public BattleInterfaceController battleInterfaceController;
	public EquipmentController equipmentController;
	public SkillInterface skillInterface;
	public TalentsInterface talentsInterface;

	public GameObject menuPanel;
	public GameObject inventoryActive;
	public GameObject skillsActive;
	public GameObject talentsActive;

	public Canvas guiCanvas;

	void Awake(){
		interfaceSystemController = this;
	}


	public void ActivateEquipmentInterface(){
		equipmentController.ActivateInterface ();
		battleInterfaceController.DeactivateInterface ();
		skillInterface.DeactivateInterface ();
		talentsInterface.DeactivateInterface ();
		menuPanel.SetActive (true);
		inventoryActive.SetActive (true);
		skillsActive.SetActive (false);
		talentsActive.SetActive (false);
	}

	public void ActivateBattleInterface(){
		equipmentController.DeactivateInterface ();
		battleInterfaceController.ActivateInterface ();
		skillInterface.DeactivateInterface ();
		talentsInterface.DeactivateInterface ();
		menuPanel.SetActive (false);
	}

	public void ActivateSkillInterface(){
		equipmentController.DeactivateInterface ();
		battleInterfaceController.DeactivateInterface ();
		skillInterface.ActivateInterface();
		talentsInterface.DeactivateInterface ();
		menuPanel.SetActive (true);
		inventoryActive.SetActive (false);
		skillsActive.SetActive (true);
		talentsActive.SetActive (false);
	}

	public void ActivateTalentInterface(){
		equipmentController.DeactivateInterface ();
		battleInterfaceController.DeactivateInterface ();
		skillInterface.DeactivateInterface();
		talentsInterface.ActivateInterface ();
		menuPanel.SetActive (true);
		inventoryActive.SetActive (false);
		skillsActive.SetActive (false);
		talentsActive.SetActive (true);
	}
}
