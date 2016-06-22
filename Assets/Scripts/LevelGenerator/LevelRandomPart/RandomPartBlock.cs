using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class RandomPartBlock{
	public List<SerializableVector2> uvs = new List<SerializableVector2>();
	public SerializableVector3 coord;
	public float width;
	public float height;
	public bool isDecor = false;
	public bool isRandom = false;
	public int randomValue;
}
