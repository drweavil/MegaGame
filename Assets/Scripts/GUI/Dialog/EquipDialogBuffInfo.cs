using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipDialogBuffInfo : MonoBehaviour {
	public GameObject equipDialogBuffInfo;
	public Text title;
	public Text description;
	public ScrollRectUpdater scrollRectUpdater;

	public void SetBuffInfo(int buffID){
		Buff buff = Buffs.GetBuff (buffID);
		title.text = buff.GetTitle ();
		description.text = buff.GetDescription ();
		scrollRectUpdater.UpdateRect ();
	}

	public void SetProcessBuffInfo(int buffID){
		Buff buff = Buffs.GetBuff (buffID);
		title.text = buff.GetTitle ();
		description.text = buff.GetProcessDescription ();
		scrollRectUpdater.UpdateRect ();
	}
}
