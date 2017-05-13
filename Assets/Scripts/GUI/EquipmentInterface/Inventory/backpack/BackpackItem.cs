using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackpackItem : MonoBehaviour {
	public int itemID;
	public List<object> itemContent = new List<object>();
	public float weight=0;
	public float price=0;
}
