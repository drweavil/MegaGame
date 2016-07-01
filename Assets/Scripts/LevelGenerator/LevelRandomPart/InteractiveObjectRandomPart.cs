using UnityEngine;
using System.Collections;

[System.Serializable]
public class InteractiveObjectRandomPart{
	public string type;
	public SerializableVector3 coord;
	public bool isRandom = false;
	public int randomValue;
	public int chunkId;
	public SerializableVector2 coordInChunkRudimentInfoForMiniPortalsArray;


	public void SetData(InteractiveObjectRandomPart obj){
		type = obj.type;
		coord = obj.coord;
		isRandom = obj.isRandom;
		randomValue = obj.randomValue;
		chunkId = obj.chunkId;
		coordInChunkRudimentInfoForMiniPortalsArray = obj.coordInChunkRudimentInfoForMiniPortalsArray;
	}
}
