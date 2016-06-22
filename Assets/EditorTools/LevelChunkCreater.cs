using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

public class LevelChunkCreater : MonoBehaviour {

	static void GenLevelPrefab(string textureName, string size, string type, string number){
		string path = "C:/MegaGameLevels/"+ textureName + "/" + size.ToString() + "/" + type.ToString() + "/" + number.ToString() + ".level";
		GameObject chunk = Instantiate ((GameObject)Resources.Load ("LevelChunk"));
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Open (path, FileMode.Open);
		LevelRudiment data = (LevelRudiment)bf.Deserialize (file);
		//Level data = SaveLoadManager.Load (Application.dataPath.ToString () + "/Resources/Levels/levels_2/" + name.ToString () + ".level");
		List<Block> blocks = data.fixBlocks;
		List<DecorBlock> decorBlocks = data.fixDecorBlocks;
		List<InteractiveObject> interactiveObjects = data.fixInteractiveObjects;

		List<FullSet> sets = new List<FullSet> ();

		foreach (Set newSet in  data.sets) {
			FullSet newFullSet = new FullSet ();
			newFullSet.randomValue = newSet.rndPercent;
			foreach (ObjectInfo obj in newSet.objects) {
				if(obj.objectType == "tile"){
					int blockIndex = data.fixBlocks.FindIndex (b => b.objId == obj.objId);
					if (blockIndex != -1) {
						RandomPartBlock newBlock = new RandomPartBlock ();
						newBlock.coord = data.fixBlocks [blockIndex].blockCoords;
						newBlock.width = 1f;
						newBlock.height = 1f;
						newBlock.isDecor = false;
						newBlock.isRandom = false;
						newBlock.uvs = data.fixBlocks [blockIndex].squareUV;
						newFullSet.blocks.Add (newBlock);
						data.fixBlocks.RemoveAt (blockIndex);
					}

					int randomBlockIndex = data.randomBlocks.FindIndex (b => b.objId == obj.objId);
					if (randomBlockIndex != -1) {
						RandomPartBlock newBlock = new RandomPartBlock ();
						newBlock.coord = data.randomBlocks [randomBlockIndex].blockCoords;
						newBlock.width = 1f;
						newBlock.height = 1f;
						newBlock.isDecor = false;
						newBlock.isRandom = true;
						newBlock.randomValue = data.randomBlocks [randomBlockIndex].rnd;
						newBlock.uvs = data.randomBlocks [randomBlockIndex].squareUV;
						newFullSet.blocks.Add (newBlock);
						data.randomBlocks.RemoveAt (randomBlockIndex);
					}
				}

				if(obj.objectType == "decor"){

					int blockIndex = data.fixDecorBlocks.FindIndex (b => b.objId == obj.objId);
					if (blockIndex != -1) {
						RandomPartBlock newBlock = new RandomPartBlock ();
						newBlock.coord = data.fixDecorBlocks [blockIndex].blockCoords[0];//LeftUp point
						newBlock.width = Mathf.Abs(data.fixDecorBlocks [blockIndex].blockCoords[0].x  - data.fixDecorBlocks [blockIndex].blockCoords[1].x);
						newBlock.height = Mathf.Abs(data.fixDecorBlocks [blockIndex].blockCoords[0].y  - data.fixDecorBlocks [blockIndex].blockCoords[3].y);
						newBlock.isDecor = true;
						newBlock.isRandom = false;
						newBlock.uvs = data.fixDecorBlocks [blockIndex].squareUV;
						newFullSet.blocks.Add (newBlock);
						data.fixDecorBlocks.RemoveAt (blockIndex);
					}

					int randomBlockIndex = data.randomDecorBlocks.FindIndex (b => b.objId == obj.objId);
					if (randomBlockIndex != -1) {
						RandomPartBlock newBlock = new RandomPartBlock ();
						newBlock.coord = data.randomDecorBlocks [randomBlockIndex].blockCoords[0];
						newBlock.width = Mathf.Abs(data.randomDecorBlocks [randomBlockIndex].blockCoords[0].x  - data.randomDecorBlocks [randomBlockIndex].blockCoords[1].x);
						newBlock.height = Mathf.Abs(data.randomDecorBlocks [randomBlockIndex].blockCoords[0].y  - data.randomDecorBlocks [randomBlockIndex].blockCoords[3].y);
						newBlock.isDecor = true;
						newBlock.isRandom = true;
						newBlock.randomValue = data.randomDecorBlocks [randomBlockIndex].rnd;
						newBlock.uvs = data.randomDecorBlocks [randomBlockIndex].squareUV;
						newFullSet.blocks.Add (newBlock);
						data.randomDecorBlocks.RemoveAt (randomBlockIndex);
					}
				}

				if(obj.objectType == "interactiveObject"){
					InteractiveObjectRandomPart newObj = new InteractiveObjectRandomPart ();
					int fix = data.fixInteractiveObjects.FindIndex (o => o.objId == obj.objId);
					if(fix != -1){
						newObj.type = data.fixInteractiveObjects [fix].type;
						newObj.isRandom = false;
						newObj.coord = data.fixInteractiveObjects [fix].objCoord;
						newFullSet.interactiveObjects.Add (newObj);
						data.fixInteractiveObjects.RemoveAt (fix);
					}

					int random = data.randomInteractiveObjects.FindIndex (o => o.objId == obj.objId);
					if(random != -1){
						newObj.type = data.randomInteractiveObjects [random].type;
						newObj.coord = data.randomInteractiveObjects [random].objCoord;
						newObj.isRandom = true;
						newObj.randomValue = data.randomInteractiveObjects [random].rnd;
						newFullSet.interactiveObjects.Add (newObj);
						data.randomInteractiveObjects.RemoveAt (random);
					}
				}
			}
			sets.Add (newFullSet);
		}
		chunk.GetComponent<LevelChunk> ().sets = sets;

		/****************************Random***********************************/
		foreach(Block block in data.randomBlocks){
			RandomPartBlock newBlock = new RandomPartBlock ();
			newBlock.coord = block.blockCoords;
			newBlock.height = 1f;
			newBlock.width = 1f;
			newBlock.isDecor = false;
			newBlock.isRandom = true;
			newBlock.randomValue = block.rnd;
			newBlock.uvs = block.squareUV;
			chunk.GetComponent<LevelChunk> ().randomlevelBlocks.Add (newBlock);
		}

		foreach(DecorBlock block in data.randomDecorBlocks){
			RandomPartBlock newBlock = new RandomPartBlock ();
			newBlock.coord = block.blockCoords[0];
			newBlock.width = Mathf.Abs(block.blockCoords[0].x  - block.blockCoords[1].x);
			newBlock.height = Mathf.Abs(block.blockCoords[0].y  - block.blockCoords[3].y);
			newBlock.isDecor = true;
			newBlock.isRandom = true;
			newBlock.randomValue = block.rnd;
			newBlock.uvs = block.squareUV;
			chunk.GetComponent<LevelChunk> ().randomlevelBlocks.Add (newBlock);
		}

		foreach(InteractiveObject obj in data.randomInteractiveObjects){
			InteractiveObjectRandomPart newObj = new InteractiveObjectRandomPart ();
			newObj.coord = obj.objCoord;
			newObj.randomValue = obj.rnd;
			newObj.type = obj.type;
			chunk.GetComponent<LevelChunk> ().randomObjects.Add (newObj);
		}
		/*****************************************************************************/


		/******************************Fix**********************************************/
		//List<LevelMesh.LevelMeshDataSerializable> fixBlockChunks = new List<LevelMesh.LevelMeshDataSerializable>();
		List<LevelMesh.LevelMeshDataSerializable> fixBlocks = new List<LevelMesh.LevelMeshDataSerializable>();
		fixBlocks.Add (new LevelMesh.LevelMeshDataSerializable ());
		foreach(Block block in data.fixBlocks){
			if(fixBlocks[fixBlocks.Count-1].squareVerticesSerializeble.Count >= 11000){
				fixBlocks.Add (new LevelMesh.LevelMeshDataSerializable ());
			}
			foreach (SerializableVector2 coord in block.squareUV) {
				fixBlocks[fixBlocks.Count-1].squareUVSerializeble.Add (coord);
			}
			fixBlocks[fixBlocks.Count-1].squareVerticesSerializeble.Add (block.blockCoords);
			fixBlocks[fixBlocks.Count-1].squareVerticesSerializeble.Add (new SerializableVector3(block.blockCoords.x+1, block.blockCoords.y, block.blockCoords.z));
			fixBlocks[fixBlocks.Count-1].squareVerticesSerializeble.Add (new SerializableVector3(block.blockCoords.x+1, block.blockCoords.y-1, block.blockCoords.z));
			fixBlocks[fixBlocks.Count-1].squareVerticesSerializeble.Add (new SerializableVector3(block.blockCoords.x, block.blockCoords.y-1, block.blockCoords.z));

			foreach (SerializableVector3 coord in LevelMesh.GetColliderCoords(block.blockCoords)) {
				fixBlocks[fixBlocks.Count-1].colliderVerticesSerializeble.Add (coord);
			}
		
		}
		chunk.GetComponent<LevelChunk> ().levelBlocks = fixBlocks;

		List<LevelMesh.LevelMeshDataSerializable> fixDecorBlocks = new List<LevelMesh.LevelMeshDataSerializable>();
		fixDecorBlocks.Add (new LevelMesh.LevelMeshDataSerializable ());
		foreach(DecorBlock block in data.fixDecorBlocks){
			if(fixDecorBlocks[fixDecorBlocks.Count-1].squareVerticesSerializeble.Count >= 11000){
				fixDecorBlocks.Add (new LevelMesh.LevelMeshDataSerializable ());
			}
			foreach(SerializableVector3 coord in block.blockCoords){
				fixDecorBlocks[fixDecorBlocks.Count-1].squareVerticesSerializeble.Add (coord);
			}

			foreach(SerializableVector2 coord in block.squareUV){
				fixDecorBlocks[fixDecorBlocks.Count-1].squareUVSerializeble.Add (coord);
			}
		}
		chunk.GetComponent<LevelChunk> ().levelDecorBlocks = fixDecorBlocks;


		foreach(InteractiveObject obj in data.fixInteractiveObjects){
			InteractiveObjectRandomPart newObj = new InteractiveObjectRandomPart ();
			newObj.coord = obj.objCoord;
			newObj.randomValue = obj.rnd;
			newObj.type = obj.type;
			if (obj.type == "portal") {
				chunk.GetComponent<LevelChunk> ().portals.Add (newObj);
			} else {
				chunk.GetComponent<LevelChunk> ().fixObjects.Add (newObj);
			}
		}
		/********************************************************************************/

		//chunk.GetComponent<LevelChunk> ().SetData (data);
		string directory = "Assets/Resources/LevelChunkPrefabs/" + textureName + "/" + size.ToString() + "/" + type.ToString();
		if (Directory.Exists (directory)) {
			GameObject prefab = PrefabUtility.CreatePrefab (directory +"/" + number.ToString () + ".prefab", chunk);
		} else {
			Directory.CreateDirectory (directory);
			GameObject prefab = PrefabUtility.CreatePrefab (directory +"/" + number.ToString () + ".prefab", chunk);
		}
		Debug.Log (chunk.GetComponent<LevelChunk>().fixObjects.Count);
			//GameObject prefab = PrefabUtility.CreatePrefab("Assets/Resources/LevelChunkPrefabs/" + name.ToString() + ".prefab", chunk);
		//Destroy (chunk);
	}


	public static void GenPrefabs(){
		string root = "C:/MegaGameLevels";
		DirectoryInfo directory = new DirectoryInfo (root);
		DirectoryInfo[] directories = directory.GetDirectories();

		foreach(DirectoryInfo textureDir in directories){
			DirectoryInfo[] sizeDirs = new DirectoryInfo (root + "/" + textureDir.Name).GetDirectories();
			foreach(DirectoryInfo sizeDir in sizeDirs){
				DirectoryInfo[] typeDirs = new DirectoryInfo (root + "/" + textureDir.Name + "/" + sizeDir.Name).GetDirectories();
				foreach(DirectoryInfo typeDir in typeDirs){
					foreach(FileInfo number in typeDir.GetFiles()){
						GenLevelPrefab (textureDir.Name, sizeDir.Name, typeDir.Name, number.Name.Split ('.')[0]);
					}
				}
			}
		}

		SaveLoadManager.SetLevelChunkFilesInfo ();
	}
}
