using UnityEngine;
using System.Collections;

[System.Serializable]
public class InteractiveObjectRandomPart{
	public string type;
	public SerializableVector3 coord;
	public bool isRandom = false;
	public int randomValue;
}
