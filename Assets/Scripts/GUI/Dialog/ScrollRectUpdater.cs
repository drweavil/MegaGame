using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollRectUpdater : MonoBehaviour {
	public RectTransform updatedRect;
	public Scrollbar scroll;


	public void UpdateRect (){
		StartCoroutine (UpdateRectCoroutine()); 
	}

	IEnumerator UpdateRectCoroutine(){
		for (int i = 0; i <= 5; i++) {
			if (i == 1) {
				updatedRect.sizeDelta = new Vector2 (updatedRect.sizeDelta.x, updatedRect.sizeDelta.y + 0.001f); 
				yield return null;
			} else if (i == 2) {
				if (scroll.IsActive ()) {
					scroll.value = 1;
				}
				yield break;
			} else {
				yield return null;
			}
		}
	}
}
