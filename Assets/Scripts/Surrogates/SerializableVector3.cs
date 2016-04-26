using UnityEngine;
using System.Collections;

[System.Serializable]
public class SerializableVector3  {
	public float x;
	public float y;
	public float z;

	public SerializableVector3(float setX, float setY, float setZ){
		x = setX;
		y = setY;
		z = setZ;
	}
}
