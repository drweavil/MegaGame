using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SlideDialog : MonoBehaviour {
	public GameObject priceLine;
	public Text priceText;
	public Text valueText;
	public Text minimumValueText;
	public Text maximumValueText;
	public Slider slider;

	public int currentValue;
	public int currentPrice;


	int unitPrice = 2;



	public void ChangeValue(){
		currentValue = (int)slider.value;
		currentPrice = unitPrice * currentValue;

		valueText.text = currentValue.ToString ();
		priceText.text = currentPrice.ToString ();
	}
}
