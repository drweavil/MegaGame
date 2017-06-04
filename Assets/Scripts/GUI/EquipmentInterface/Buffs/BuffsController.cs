using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffsController : MonoBehaviour {
	public static BuffsController buffsController;
	public Dictionary<int, Buff> buffs = new Dictionary<int, Buff>();
	public Dictionary<int, Timer> buffTimers = new Dictionary<int, Timer> ();
	//delegate void BuffAction();

	void Awake(){
		buffsController = this;
	}

	public static void StartBuff (Buff buff){
		if (buffsController.buffs.ContainsKey (buff.buffID)) {
			float newTime = buffsController.buffTimers [buff.buffID].ResidualTime () + buff.originBuffTime;
			if(newTime <= buffsController.buffs[buff.buffID].maximumTime){
				buffsController.buffs[buff.buffID].buffTime = newTime;
				buffsController.buffTimers[buff.buffID].SetTimer(newTime);
			}else{
				buffsController.buffs[buff.buffID].buffTime = buff.maximumTime;
				buffsController.buffTimers[buff.buffID].SetTimer(buff.maximumTime);
			}
		}else{
			buffsController.StartBuffProcess (buff);
		}
	}


	public static void StopBuff(int buffID){
		buffsController.buffTimers [buffID].SetTimer (0);
	}


	void StartBuffProcess(Buff buff){
		StartCoroutine (StartBuffCoroutine (buff));
	}
	IEnumerator StartBuffCoroutine(Buff buff){
		//BuffAction startAction = () => {};
		//BuffAction stopAction = () => {};
		Timer buffTimer = new Timer ();
		buffTimer.SetTimer (buff.buffTime);
		buffTimers.Add (buff.buffID, buffTimer);

		Dictionary<string, Buffs.BuffAction> actions = Buffs.GetBuffActions (buff.buffID);



		//startAction ();
		actions["startAction"].Invoke();
		BuffsView.AddBuff (buff);
		if (InterfaceSystemController.interfaceSystemController.equipmentController.equipmentInterface.activeInHierarchy) {
			InterfaceSystemController.interfaceSystemController.equipmentController.RedrawEquipAndStats ();
		}

		int frames = 0;
		while (!buffTimer.TimeIsOver ()) {
			if (InterfaceSystemController.interfaceSystemController.equipmentController.equipmentInterface.activeInHierarchy) {
				BuffsView.ChangeBuffTime (buffTimer.ResidualTime (), buffsController.buffs[buff.buffID].buffTime, buff.buffID);
			}
			yield return null;
		}


		//stopAction ();
		actions["stopAction"].Invoke();
		BuffsView.RemoveBuff (buff);
		buffTimers.Remove (buff.buffID);
		if (InterfaceSystemController.interfaceSystemController.equipmentController.equipmentInterface.activeInHierarchy) {
			InterfaceSystemController.interfaceSystemController.equipmentController.RedrawEquipAndStats ();
		}
			
		yield break;

	}
}
