using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Level {
	public List<LevelChunkInfo> levelChunksInfo = new List<LevelChunkInfo>();
	public string textureName;
	//public  List<LevelMesh.LevelMeshDataSerializable> levelBlocks = new List<LevelMesh.LevelMeshDataSerializable>();
	//public  SerializableVector3 playerSpawn;
	public List<PortalInfo> portalsInfo = new List<PortalInfo>();

	public List<RandomPartBlock> randomPartBlocks = new List<RandomPartBlock> ();
	public List<InteractiveObjectRandomPart> randomPartInteractiveObjects = new List<InteractiveObjectRandomPart> ();
	public int levelNumber;

	public int levelArrayWidth;
	public int levelArrayHeight;

	//public LevelChunkRudiment[,] level;
}
