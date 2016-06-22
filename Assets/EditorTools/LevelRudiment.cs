using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class LevelRudiment{
	public string textureName;
	public List<Set> sets = new List<Set>();


	public List<Block> fixBlocks = new List<Block>();
	public List<Block> randomBlocks = new List<Block>();

	public List<DecorBlock> fixDecorBlocks = new List<DecorBlock>();
	public List<DecorBlock> randomDecorBlocks = new List<DecorBlock>();

	public List<InteractiveObject> fixInteractiveObjects = new List<InteractiveObject> ();
	public List<InteractiveObject> randomInteractiveObjects = new List<InteractiveObject> ();

	public int objIdsCount = 0;
	public int setsCount = 0;
	public List<ObjectInfo> objects = new List<ObjectInfo>();

	public void AddBlock(SerializableVector3 coords, List<SerializableVector2> uvs, bool isRandom = false, int rndPercent = 50){
		Block block = new Block ();
		block.SetData (coords, uvs);
		block.objId = objIdsCount;
		if (isRandom) {
			block.rnd = rndPercent;
			randomBlocks.Add (block);
		} else {
			fixBlocks.Add (block);
		}
		ObjectInfo obj = new ObjectInfo ();
		obj.objectType = "tile";
		obj.objId = objIdsCount;
		obj.objRectCoord = new SerializableVector2 (coords.x, coords.y - 1);
		obj.objRectHeight = 1f;
		obj.objRectWidth = 1f;
		objects.Add (obj);
		objIdsCount = objIdsCount + 1;
	}

	public void AddDecorBlock(List<SerializableVector3> coords, List<SerializableVector2> uvs, ObjectInfo obj, bool isRandom = false, int rndPercent = 50){
		DecorBlock block = new DecorBlock ();
		block.SetData (coords, uvs);
		block.objId = objIdsCount;
		if (isRandom) {
			block.rnd = rndPercent;
			randomDecorBlocks.Add (block);
		} else {
			fixDecorBlocks.Add (block);
		}
		obj.objId = objIdsCount;
		obj.objectType = "decor";
		objects.Add (obj);
		objIdsCount = objIdsCount + 1;
	}

	public InteractiveObject AddInteractiveObject(SerializableVector3 coords, string type, ObjectInfo obj, bool isRandom = false, int rndPercent = 50){
		InteractiveObject newObj = new InteractiveObject ();
		newObj.objId = objIdsCount;
		newObj.objCoord = coords;
		newObj.type = type;
		if (isRandom) {
			newObj.rnd = rndPercent;
			randomInteractiveObjects.Add (newObj);
		} else {
			fixInteractiveObjects.Add (newObj);
		}

		obj.objId = objIdsCount;
		obj.objectType = "interactiveObject";
		objects.Add (obj);
		objIdsCount = objIdsCount + 1;
		return newObj;
	}

	public Set AddSet (List<ObjectInfo> selectedObjects, int rndPercent){
		Set newSet = new Set ();
		newSet.rndPercent = rndPercent;
		newSet.objId = objIdsCount;
		objIdsCount = objIdsCount + 1;
		foreach(ObjectInfo obj in selectedObjects){
			newSet.objects.Add (obj);
		}
		sets.Add (newSet);
		return newSet;
	}

	public void RemoveSet(int id){
		int index = sets.FindIndex (s => s.objId == id);
		if(index != -1){
			sets.RemoveAt (index);
		}
	}

	public Set FindSetById(int id){
		return sets.Find (s => s.objId == id);
	}

	public void RemoveObjectById (int id){
		int objectIndex = objects.FindIndex (o => o.objId == id);
		if (objectIndex != -1) {
			objects.RemoveAt (objectIndex);

			int fixBlocksIndex = fixBlocks.FindIndex (b => b.objId == id);
			if (fixBlocksIndex != -1) {
				fixBlocks.RemoveAt (fixBlocksIndex);
			}

			int randomBlocksIndex = randomBlocks.FindIndex (b => b.objId == id);
			if (randomBlocksIndex  != -1) {
				randomBlocks.RemoveAt (randomBlocksIndex );
			}

			int fixDecorBlockIndex = fixDecorBlocks.FindIndex (b => b.objId == id);
			if (fixDecorBlockIndex != -1) {
				fixDecorBlocks.RemoveAt (fixDecorBlockIndex);
			}

			int randomDecorBlockIndex = randomDecorBlocks.FindIndex (b => b.objId == id);
			if (randomDecorBlockIndex != -1) {
				randomDecorBlocks.RemoveAt (randomDecorBlockIndex);
			}

			int fixInteractiveObjectIndex = fixInteractiveObjects.FindIndex (b => b.objId == id);
			if (fixInteractiveObjectIndex != -1) {
				fixInteractiveObjects.RemoveAt (fixInteractiveObjectIndex);
			}

			int randomInteractiveObjectIndex = randomInteractiveObjects.FindIndex (b => b.objId == id);
			if (randomInteractiveObjectIndex != -1) {
				randomInteractiveObjects.RemoveAt (randomInteractiveObjectIndex);
			}
		}
	}


}
