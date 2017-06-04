using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentBarsController : MonoBehaviour {
	private RectTransform healthPoolBarRect;
	private Rect newHealthPoolRectPosition = new Rect ();
	private Vector2 healthPoolRectPosition;
	private float maximumHealthPoolWidth;

	public GameObject healthPoolBar;
	public Text healthPoolText;


	private RectTransform weightBarRect;
	private Rect newWeightRectPosition = new Rect ();
	private Vector2 weightRectPosition;
	private float maximumWeightWidth;

	public GameObject weightBar;
	public Text weightText;



	void Awake(){
		SetBarRects ();
	}

	void SetBarRects(){
		healthPoolBarRect = healthPoolBar.GetComponent<RectTransform> ();
		maximumHealthPoolWidth = healthPoolBarRect.rect.width;
		healthPoolRectPosition = new Vector2(healthPoolBarRect.rect.position.x, healthPoolBarRect.rect.position.y);
		newHealthPoolRectPosition.height = healthPoolBarRect.rect.height;

		weightBarRect = weightBar.GetComponent<RectTransform> ();
		maximumWeightWidth = weightBarRect.rect.width;
		weightRectPosition = new Vector2(weightBarRect.rect.position.x, weightBarRect.rect.position.y);
		newWeightRectPosition.height = weightBarRect.rect.height;
	}


	public void SetHealthInjectionPool(float currentHealthPoolFloat){
		int currentHealthPool = (int)(System.Math.Round(currentHealthPoolFloat, 0));
		healthPoolText.text = currentHealthPool.ToString() + " / " + PlayerController.GetMaximumHealthInjectionPool().ToString();
		float oneHealthPoolWidthPoint = PlayerController.GetMaximumHealthInjectionPool() / maximumHealthPoolWidth;
		healthPoolBarRect.sizeDelta = new Vector2(currentHealthPool / oneHealthPoolWidthPoint, healthPoolBarRect.rect.height);
		newHealthPoolRectPosition.position = healthPoolRectPosition;
		newHealthPoolRectPosition.width = healthPoolBarRect.rect.width;
		healthPoolBarRect.localPosition = newHealthPoolRectPosition.center;
	}

	public void SetWeight(float currentWeight){
		currentWeight = (float)System.Math.Round (currentWeight, 2);
		weightText.text = currentWeight.ToString() + " / " + PlayerController.maximumWeight;
		if (currentWeight >= PlayerController.maximumWeight) {
			currentWeight = PlayerController.maximumWeight;
		}
		float oneWeightWidthPoint = PlayerController.maximumWeight / maximumWeightWidth;
		weightBarRect.sizeDelta = new Vector2(currentWeight / oneWeightWidthPoint, weightBarRect.rect.height);
		newWeightRectPosition.position = weightRectPosition;
		newWeightRectPosition.width = weightBarRect.rect.width;
		weightBarRect.localPosition = newWeightRectPosition.center;
	}
}
