using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelChunk : MonoBehaviour {
	public  List<LevelMesh.LevelMeshDataSerializable> levelBlocks;
	public  List<LevelMesh.LevelMeshDataSerializable> levelDecorBlocks;
	public List<InteractiveObjectRandomPart> fixObjects = new List<InteractiveObjectRandomPart> ();
	public List<FullSet> sets = new List<FullSet> ();

	public List<RandomPartBlock> randomlevelBlocks = new List<RandomPartBlock>();
	public List<InteractiveObjectRandomPart> randomObjects = new List<InteractiveObjectRandomPart> ();
	public List<InteractiveObjectRandomPart> portals = new List<InteractiveObjectRandomPart> ();
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
