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
	public float currentPrice;


	float unitPrice = 2;

	public void SetParams(int minimum, int maximum, bool withPrice = false, float newUnitPrice = 0){
		priceLine.SetActive (false);
		minimumValueText.text = minimum.ToString ();
		maximumValueText.text = maximum.ToString ();
		currentValue = minimum;
		slider.minValue = minimum;
		slider.maxValue = maximum;
		if (withPrice) {
			priceLine.SetActive (true);
			unitPrice = newUnitPrice;
			currentPrice = minimum * newUnitPrice;
		}
		ChangeValue ();
	}

	public void ChangeValue(){
		currentValue = (int)slider.value;
		currentPrice = (float)System.Math.Round(unitPrice * currentValue, 2);

		valueText.text = currentValue.ToString ();
		priceText.text = currentPrice.ToString ();
	}
}
