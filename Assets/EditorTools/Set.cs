using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Set{
	public List<ObjectInfo> objects = new List<ObjectInfo>();
	public int objId;
	public int rndPercent;

	public void AddObjects (List<ObjectInfo> objectsAdd){
		foreach(ObjectInfo obj in objectsAdd){
			objects.Add (obj);
		}
	}
}
