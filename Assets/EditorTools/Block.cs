using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Block{
	public SerializableVector3 blockCoords;
	public int rnd;
	public List<SerializableVector2> squareUV = new List<SerializableVector2>();
	public int objId;

	public void SetData(SerializableVector3 getBlockCoords, List<SerializableVector2> uv){
		blockCoords = getBlockCoords;/*new SerializableVector3 (getBlockCoords.x, getBlockCoords.y, getBlockCoords.z);
		foreach (Vector2 coord in uv) {
			squareUV.Add (new SerializableVector2(coord.x, coord.y));
		}*/
		squareUV = uv;
	}
}
