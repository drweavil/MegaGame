using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BackpackItem{
	public int itemID;
	public List<object> itemContent = new List<object>();
	public float weight=0;
	public float price=0;
}
