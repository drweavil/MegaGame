using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class LevelChunkFilesInfo {
	public List<ChunkTypeFileInfo> types = new List<ChunkTypeFileInfo>();

	public int GetRandomChunkNumberByType(int typeId){
		ChunkTypeFileInfo type = types.Find(t => t.typeId == typeId);
		int rand = Random.Range (0, type.numbers.Count);
		return type.numbers[rand];
	}

	[System.Serializable]
	public class ChunkTypeFileInfo{
		public int typeId;
		public List<int> numbers = new List<int>();
	}
}
