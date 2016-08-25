using UnityEngine;
using System.Collections;
using System.Reflection;

[System.Serializable]
public class MapZoneChunk{
	public int zoneChunkId;
	public int zoneChunkType;
	public bool isKeyPoint = false;
	public int complexity = -1;
	//public int[] zoneChunkStatsLevel = new int[2];

	public void SetData(MapZoneChunkWithGenerationInfo zoneChunk){
		foreach (FieldInfo field in zoneChunk.GetType().GetFields()) {
			FieldInfo thisField = this.GetType ().GetField (field.Name);
			if(thisField != null){
				thisField.SetValue(this, field.GetValue(zoneChunk));
			}
		}
	}
}
