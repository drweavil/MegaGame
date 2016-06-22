using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class LevelChunkFilesInfo {
	public List<LevelChunkFilesInfo.ChunkTypeFileInfo> types = new List<LevelChunkFilesInfo.ChunkTypeFileInfo>();

	public int GetRandomChunkNumber(string texture, int size, int type){
		//Debug.Log (texture);
		List<LevelChunkFilesInfo.ChunkTypeFileInfo> tmpTypes = types.FindAll(
			t => ((t.type == type) && 
				(t.texture == texture) &&
				(t.size == size))
		);
		//Debug.Log (type);
		int rand = Random.Range (0, tmpTypes.Count);

		//Debug.Log (types.Count);
		return tmpTypes[rand].number;
	}

	[System.Serializable]
	public class ChunkTypeFileInfo{
		public int type;
		public int size;
		public int number;
		public string texture;
		//public List<int> numbers = new List<int>();
	}
}
