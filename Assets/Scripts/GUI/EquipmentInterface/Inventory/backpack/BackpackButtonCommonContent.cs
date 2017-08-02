using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackpackButtonCommonContent : MonoBehaviour {
	public GameObject donatPrice;
	public Text donatPriceText;

	public Text title;
	public Text weightText;
	public Text priceText;


	public void SetInfo(BackpackItem item, string titleText){
		donatPrice.SetActive (false);
		if (item.donatePrice != 0) {
			donatPrice.SetActive (true);
			donatPriceText.text = System.Math.Round(item.donatePrice, 2).ToString();
		}

		weightText.text = System.Math.Round(item.weight, 2).ToString();
		priceText.text = System.Math.Round(item.price, 2).ToString();
		title.text = titleText;
	}
}
