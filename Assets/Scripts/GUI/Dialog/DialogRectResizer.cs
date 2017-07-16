using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DialogRectResizer : MonoBehaviour {
	public RectTransform changingRect;
	public RectTransform fullSize;
	public RectTransform weightPriceLineSize;


	public void SetFullSize(){
		changingRect.sizeDelta = fullSize.sizeDelta;
		changingRect.localPosition = fullSize.localPosition;
	}

	public void SetWeightPriceSize(){
		changingRect.sizeDelta = weightPriceLineSize.sizeDelta;
		changingRect.localPosition = weightPriceLineSize.localPosition;
	}
}
