using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BackpackButtonBuffContent : MonoBehaviour {
	public Text titleText;
	//public Text descriptionText;
	public Text weightText;
	public Text priceText;

	public void SetContent(BackpackItem item){
		Buff buff = (Buff)item.itemContent [0];
		titleText.text = buff.GetTitle ();
		//descriptionText.text = buff.GetDescription ();
		weightText.text = System.Math.Round(item.weight, 2).ToString ();
		priceText.text = System.Math.Round(item.price, 2).ToString ();
	}
}
