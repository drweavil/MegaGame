using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LumenArea : MonoBehaviour {
	public RectTransform area;
	public GameObject lumen;
	public UnityEvent areaEvent;
	public int backpackItemID;

	public bool deactive = true;
}
