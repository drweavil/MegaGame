using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class DecorBlock {
	public int rnd;
	public List<SerializableVector3> blockCoords = new List<SerializableVector3>();
	//public List<SerializableVector2> squareUV = new List<SerializableVector2>();
	public int objId;
	public int uvsID;

	public void SetData(List<SerializableVector3> getBlockCoords, int id){
		blockCoords = getBlockCoords;/*new SerializableVector3 (getBlockCoords.x, getBlockCoords.y, getBlockCoords.z);
		foreach (Vector2 coord in uv) {
			squareUV.Add (new SerializableVector2(coord.x, coord.y));
		}*/
		uvsID = id;
	}
}
