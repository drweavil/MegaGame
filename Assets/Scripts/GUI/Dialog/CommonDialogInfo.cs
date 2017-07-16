using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommonDialogInfo : MonoBehaviour {
	public GameObject weightPriceLine;
	public GameObject price;
	public Text priceText;
	public GameObject weight;
	public Text weightText;
	public GameObject donatePrice;
	public Text donatePriceText; 
	public ScrollRectUpdater scrollRectUpdater;
	public DialogRectResizer dialogRectResizer;

	public Text title;
	public Text description;



	public void SetItemInfo(BackpackItem item, bool withWeightPrice = true){
		donatePrice.SetActive (false);
		if (withWeightPrice) {
			weightPriceLine.SetActive (true);
			dialogRectResizer.SetWeightPriceSize ();
		} else {
			dialogRectResizer.SetFullSize ();
			weightPriceLine.SetActive (false);
		}

		priceText.text = System.Math.Round (item.price, 2).ToString();
		weightText.text = System.Math.Round (item.weight, 2).ToString();

		if (item.donatePrice != 0) {
			donatePrice.SetActive (true);
			donatePriceText.text = System.Math.Round(item.donatePrice, 2).ToString();
		}
	}
}
