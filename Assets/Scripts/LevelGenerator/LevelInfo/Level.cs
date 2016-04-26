using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Level {
	public List<LevelChunkInfo> levelChunksInfo = new List<LevelChunkInfo>();
	//public  List<LevelMesh.LevelMeshDataSerializable> levelBlocks = new List<LevelMesh.LevelMeshDataSerializable>();
	//public  SerializableVector3 playerSpawn;
	public List<PortalInfo> portalsInfo = new List<PortalInfo>();
	public int levelNumber;
}
