using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceSystemController: MonoBehaviour {
	public static InterfaceSystemController interfaceSystemController;

	public BattleInterfaceController battleInterfaceController;
	public EquipmentController equipmentController;

	void Awake(){
		interfaceSystemController = this;
	}


	public void ActivateEquipmentInterface(){
		equipmentController.ActivateInterface ();
		battleInterfaceController.DeactivateInterface ();
	}

	public void ActivateBattleInterface(){
		equipmentController.DeactivateInterface ();
		battleInterfaceController.ActivateInterface ();
	}
}
