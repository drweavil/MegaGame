﻿using System.Collections;
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



	void Awake(){
		SetBarRects ();
	}

	void SetBarRects(){
		healthPoolBarRect = healthPoolBar.GetComponent<RectTransform> ();
		maximumHealthPoolWidth = healthPoolBarRect.rect.width;
		healthPoolRectPosition = new Vector2(healthPoolBarRect.rect.position.x, healthPoolBarRect.rect.position.y);
		newHealthPoolRectPosition.height = healthPoolBarRect.rect.height;
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
}
