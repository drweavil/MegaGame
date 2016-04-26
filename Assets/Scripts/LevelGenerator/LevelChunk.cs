using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelChunk : MonoBehaviour {
	public  List<LevelMesh.LevelMeshDataSerializable> levelBlocks = new List<LevelMesh.LevelMeshDataSerializable>();
	public  SerializableVector3 playerSpawn;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/*public void SetData(Level data){
		levelBlocks = data.levelBlocks;
		playerSpawn = data.playerSpawn;
	}*/
}
