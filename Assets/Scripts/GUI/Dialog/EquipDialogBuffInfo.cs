using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipDialogBuffInfo : MonoBehaviour {
	public GameObject equipDialogBuffInfo;
	public GameObject weightPriceLine;
	public Text title;
	public Text description;
	public Text price;
	public Text weight;
	public ScrollRectUpdater scrollRectUpdater;
	public DialogRectResizer dialogRectResizer;

	public void SetBuffInfo(int buffID, BackpackItem item){
		Buff buff = Buffs.GetBuff (buffID);
		title.text = buff.GetTitle ();
		description.text = buff.GetDescription ();


		dialogRectResizer.SetWeightPriceSize ();
		weightPriceLine.SetActive (true);
		price.text = System.Math.Round(item.price, 2).ToString();
		weight.text = System.Math.Round(item.weight, 2).ToString();

		scrollRectUpdater.UpdateRect ();
	}

	public void SetProcessBuffInfo(int buffID){
		Buff buff = Buffs.GetBuff (buffID);
		title.text = buff.GetTitle ();
		description.text = buff.GetProcessDescription ();
		weightPriceLine.SetActive (false);
		dialogRectResizer.SetFullSize ();

		scrollRectUpdater.UpdateRect ();
	}
}
