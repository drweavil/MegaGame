using UnityEngine;
using System.Collections;
using System.Reflection;

[System.Serializable]
public class MapZoneChunkWithGenerationInfo : MapZoneChunk {
	public int pointType;
	public SerializableVector2 coordInArray;

	public void SetData(MapZoneChunk zoneChunk){
		foreach (FieldInfo field in zoneChunk.GetType().GetFields()) {
			FieldInfo thisField = this.GetType ().GetField (field.Name);
			if(thisField != null){
				thisField.SetValue(this, field.GetValue(zoneChunk));
			}
		}
	}
}
