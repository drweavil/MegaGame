using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class MapZoneComplexityGenerator : MonoBehaviour {

	private const int left = 0, right = 1, down = 2, up = 3;
	public MapZoneChunkWithGenerationInfo[,] map;
	List<MapZoneChunkWithGenerationInfo> mapList = new List<MapZoneChunkWithGenerationInfo> ();
	List<ComplexityBranch> branches = new List<ComplexityBranch>();
	List<ComplexityBranch> branchesWithOtherPath = new List<ComplexityBranch>();
	List<ComplexityBranch> finalBranches = new List<ComplexityBranch> ();
	int complexityRange = 20;
	int complexityStart = 0;



	int upBranch = 0;
	int downBranch = 0;
	int leftBranch = 0;
	int rightBranch = 0;
	int branchesMaximum = 5;
	// Use this for initialization


	void Update() {
		if (Input.GetKeyDown (KeyCode.A)) {
			MapZoneChunkWithGenerationInfo lol = new MapZoneChunkWithGenerationInfo ();
			//MapZoneChunk lol2 = new MapZoneChunk ();
			/*lol.isKeyPoint = false;
			lol.zoneChunkId = 5;
			lol.zoneChunkType = 10;

			List<MapZoneChunkWithGenerationInfo> lol2 = new List<MapZoneChunkWithGenerationInfo> ();

			lol2.Add (lol);
			Test (lol2).zoneChunkId = 6;

			Debug.Log (lol.zoneChunkId);*/





			/*Debug.Log (lol.zoneChunkId);
			MapZoneChunkWithGenerationInfo lol3 = new MapZoneChunkWithGenerationInfo ();
			lol3.SetData (lol);
			Debug.Log (lol3.zoneChunkId);*/
			//Debug.Log (lol.GetType().GetFields());
			/*foreach (FieldInfo prop in  lol.GetType().GetFields()) {
				Debug.Log (lol2.GetType ().GetField (prop.Name));
			}*/
		}
	}


	public  MapZoneChunk[,] SetComplexity(MapZoneChunk[,] mapWithoutComplexity){
		MapZoneChunk[,] mapWithComplexity = new MapZoneChunk[1, 1];


		int width = mapWithoutComplexity.GetLength (0);
		int height = mapWithoutComplexity.Length / width;
		map = new MapZoneChunkWithGenerationInfo[height, width];

		for (int i = 0; i < height; i++) {
			for (int j = 0; j < width; j++) {
				map [i, j] = new MapZoneChunkWithGenerationInfo ();
				map [i, j].SetData (mapWithoutComplexity [i, j]);
				map [i, j].coordInArray = new SerializableVector2 ((float)j, (float)i);
				if (mapWithoutComplexity [i, j].zoneChunkId == -1) {
					map [i, j].pointType = -1;
				}else{
					map [i, j].pointType = 0;
				}
			}
		}

		int side = Random.Range (0, 4);
		if (side == up) {
			int rndWidth = Random.Range (1, width - 1);
			int rndHeight = 1;
			//if (map [rndHeight, rndWidth].pointType != -1) {
				ComplexityBranch firstDownBranch = new ComplexityBranch ();
				branches.Add (firstDownBranch);
				SetValueDown (new int[2]{ rndHeight, rndWidth }, firstDownBranch);
			//}
		}

		if (side == down) {
			int rndWidth = Random.Range (1, width - 1);
			int rndHeight = height - 2;
			//if (map [rndHeight + 1, rndWidth].pointType != -1) {
				ComplexityBranch firstUpBranch = new ComplexityBranch ();
				branches.Add (firstUpBranch);
				SetValueUp (new int[2]{ rndHeight, rndWidth }, firstUpBranch);
			//}
		}


		if (side == left) {
			int rndWidth = 1;
			int rndHeight = Random.Range (1, height - 1);
			//if (map [rndHeight + 1, rndWidth].pointType != -1) {
				ComplexityBranch firstRightBranch = new ComplexityBranch ();
				branches.Add (firstRightBranch);
				SetValueRight (new int[2]{ rndHeight, rndWidth }, firstRightBranch);
			//}
		}


		if (side == right) {
			int rndWidth = width - 2;
			int rndHeight = Random.Range (1, height - 1);
			//if (map [rndHeight + 1, rndWidth].pointType != -1) {
				ComplexityBranch firstLeftBranch = new ComplexityBranch ();
				branches.Add (firstLeftBranch);
				SetValueLeft (new int[2]{ rndHeight, rndWidth }, firstLeftBranch);
			//}
		}
			
		SetBranchesWithoutComplexity ();
		SetComplexityToBranches ();

		List<ComplexityChunk> lol = new List<ComplexityChunk> ();
		List<ComplexityChunk> lol2 = new List<ComplexityChunk> ();
		foreach(ComplexityBranch branch in branches){
			//Debug.Log (branch.branchFrom.ToString());
			foreach (ComplexityChunk chunk in branch.complexities) {
				lol.Add (chunk);
				//Debug.Log (branch.branchFrom.ToString() + "/" + chunk.zoneChunkId);
			}
		}
		foreach(ComplexityBranch branch in branchesWithOtherPath){
			//Debug.Log (branch.branchFrom.ToString());
			foreach (ComplexityChunk chunk in branch.complexities) {
				lol.Add (chunk);
				lol2.Add (chunk);
				//Debug.Log (branch.branchFrom.ToString() + "/" + chunk.zoneChunkId);
			}
		}
		Debug.Log (lol.Count.ToString() + "/<=Chunks WithoutComplexity =>/" + lol2.Count);

		//Debug.Log (branchesWithOtherPath.FindAll(b => b.complexities.Count == 0).Count);

		return GetMapZoneChunkArray();
	}


	void SetBranchesWithoutComplexity(){
		List<ComplexityChunk> zonesWithoutComplexity = new List<ComplexityChunk>();

		List<ComplexityChunk> chunksWithComplexity = new List<ComplexityChunk>();
		foreach (ComplexityBranch branch in branches) {
			foreach (ComplexityChunk chunk in branch.complexities) {
				chunksWithComplexity.Add (chunk);
			}
		}
		foreach(MapZoneChunkWithGenerationInfo chunk in map){
			//Debug.Log (chunk.complexity);
			if (/*chunk.complexity == -1 &&*/ 
				zonesWithoutComplexity.FindIndex (c => c.zoneChunkId == chunk.zoneChunkId) == -1 && 
				chunk.zoneChunkId != -1 && 
				chunksWithComplexity.FindIndex(c => c.zoneChunkId == chunk.zoneChunkId) == -1
			) {
				ComplexityChunk newChunk = new ComplexityChunk();
				newChunk.zoneChunkId = chunk.zoneChunkId;
				newChunk.coordInMapArray = chunk.coordInArray;
				zonesWithoutComplexity.Add (newChunk);
			}
			
		}

		mapList.Clear ();
		foreach (MapZoneChunkWithGenerationInfo zone in map) {
			mapList.Add (zone);
		}
		Debug.Log ("withoutComplexity/" + zonesWithoutComplexity.Count.ToString() + "/withComplexity/" + chunksWithComplexity.Count.ToString());


		//List<int> inBranchZones = new List<int> ();
		//Debug.Log (zonesWithoutComplexity.Count.ToString());
		foreach (ComplexityChunk chunk in zonesWithoutComplexity) {
			//Debug.Log ("lol");
			if (!chunk.inBranch) {
				//Debug.Log ("lol");
				CreateComplexityBranch (chunk, zonesWithoutComplexity, chunksWithComplexity);
			}
		}
	}

	void CreateComplexityBranch(ComplexityChunk chunk, List<ComplexityChunk> zonesWithoutComplexity, List<ComplexityChunk> chunksWithComplexity){
		ComplexityBranch newBranch = new ComplexityBranch ();	
		newBranch.complexities.Insert (0, chunk);
		zonesWithoutComplexity.Find (z => z.zoneChunkId == chunk.zoneChunkId).inBranch = true;

		List<ComplexityChunk> complexityWithOtherPathArray = new List<ComplexityChunk> ();
		ComplexityChunk currentChunk = chunk;

		foreach (ComplexityBranch branch in branchesWithOtherPath) {
			foreach(ComplexityChunk chunkInArr in branch.complexities){
				complexityWithOtherPathArray.Add (chunkInArr);
			}
		}

		//int[] currentZonePoint = new int[]{ (int)chunk.coordInMapArray.x, (int)chunk.coordInMapArray.y };
	
		bool branchNotComplete = true;

		//int i = 0;
		while(branchNotComplete ){
			/*complexityWithOtherPathArray.Clear ();
			foreach (ComplexityBranch branch in branchesWithOtherPath) {
				foreach(ComplexityChunk chunkInArr in branch.complexities){
					complexityWithOtherPathArray.Add (chunkInArr);
				}
			}*/


			//i += 1;
		
			ComplexityChunk nextZone = GetNextZone (currentChunk.zoneChunkId, newBranch);
			//Debug.Log (nextZone.zoneChunkId);
			int zoneWithOtherPathIndex = complexityWithOtherPathArray.FindIndex (z => z.zoneChunkId == nextZone.zoneChunkId);
			int zoneFirstPathIndex = chunksWithComplexity.FindIndex (c => c.zoneChunkId == nextZone.zoneChunkId);
			if (nextZone.zoneChunkId != -1) {
				if (zoneFirstPathIndex == -1 && zoneWithOtherPathIndex == -1) {
					//currentZonePoint = new int[]{ (int)nextZone.coordInMapArray.x, (int)nextZone.coordInMapArray.y };
					newBranch.complexities.Insert (0, nextZone);
					currentChunk = nextZone;
					int zoneIndex = zonesWithoutComplexity.FindIndex (z => z.zoneChunkId == nextZone.zoneChunkId);
					currentChunk = nextZone;
					//Debug.Log (zoneIndex);
					//if (zoneIndex != -1) {
					zonesWithoutComplexity [zoneIndex].inBranch = true;
					//}
				} else {
					//Debug.Log ("lol");
					newBranch.branchFrom = nextZone.zoneChunkId;
					branchNotComplete = false;
				}
			} else {
				currentChunk = chunk;
				foreach (ComplexityChunk inBranchChunk in newBranch.complexities) {
					zonesWithoutComplexity.Find (z => z.zoneChunkId == inBranchChunk.zoneChunkId).inBranch = false;
					//zonesWithoutComplexity [zoneIndex].inBranch = false;
				}
				newBranch.complexities.Clear ();
				newBranch.complexities.Insert (0, chunk);
			}
		}

		branchesWithOtherPath.Add (newBranch);
	}

	ComplexityChunk GetNextZone(int zoneNumber, ComplexityBranch branch){
		List<MapZoneChunkWithGenerationInfo> crossZones = new List<MapZoneChunkWithGenerationInfo> ();

		crossZones = mapList.FindAll (z => z.zoneChunkId != -1 && z.zoneChunkId == zoneNumber);
		crossZones = crossZones.FindAll (p => p.zoneChunkId != map[(int)p.coordInArray.y - 1, (int)p.coordInArray.x].zoneChunkId ||
			p.zoneChunkId != map[(int)p.coordInArray.y, (int)p.coordInArray.x + 1].zoneChunkId ||
			p.zoneChunkId != map[(int)p.coordInArray.y + 1, (int)p.coordInArray.x].zoneChunkId ||
			p.zoneChunkId != map[(int)p.coordInArray.y, (int)p.coordInArray.x - 1].zoneChunkId
		);


		List<MapZoneChunkWithGenerationInfo> crossZonesNeighbors = new List<MapZoneChunkWithGenerationInfo> ();
		//Debug.Log (crossZones.Count);

		foreach (MapZoneChunkWithGenerationInfo zone in crossZones) {
			if(zone.zoneChunkId != -1){
				if (map [(int)zone.coordInArray.y - 1, (int)zone.coordInArray.x].zoneChunkId != zone.zoneChunkId) {
					crossZonesNeighbors.Add (map [(int)zone.coordInArray.y - 1, (int)zone.coordInArray.x]);
				}
				if (map [(int)zone.coordInArray.y, (int)zone.coordInArray.x + 1].zoneChunkId != zone.zoneChunkId) {
					crossZonesNeighbors.Add (map [(int)zone.coordInArray.y, (int)zone.coordInArray.x + 1]);
				}
				if (map [(int)zone.coordInArray.y + 1, (int)zone.coordInArray.x].zoneChunkId != zone.zoneChunkId) {
					crossZonesNeighbors.Add (map [(int)zone.coordInArray.y + 1, (int)zone.coordInArray.x]);
				}
				if (map [(int)zone.coordInArray.y, (int)zone.coordInArray.x - 1].zoneChunkId != zone.zoneChunkId) {
					crossZonesNeighbors.Add (map [(int)zone.coordInArray.y, (int)zone.coordInArray.x - 1]);
				}
			}
		}
		//Debug.Log (crossZonesNeighbors.Count);
		List<MapZoneChunkWithGenerationInfo> nextZones = new List<MapZoneChunkWithGenerationInfo> ();
		foreach (MapZoneChunkWithGenerationInfo zone in crossZonesNeighbors) {
			int zoneIndex = branch.complexities.FindIndex (c => c.zoneChunkId == zone.zoneChunkId);
			if(zone.zoneChunkId != -1 && 
				nextZones.FindIndex(p => p.zoneChunkId == zone.zoneChunkId) == -1 &&
				zoneIndex == -1
			){
				nextZones.Add (zone);
			}
		}

		//Debug.Log (nextZones.Count);
		ComplexityChunk chunk = new ComplexityChunk ();
		if (nextZones.Count == 0) {
			chunk.zoneChunkId = -1; 
			return chunk;
		} else {	
			MapZoneChunkWithGenerationInfo mapZone = nextZones [Random.Range (0, nextZones.Count)];
			chunk.zoneChunkId = mapZone.zoneChunkId;
			chunk.coordInMapArray = mapZone.coordInArray;
			chunk.complexity = mapZone.complexity;
			return chunk;
		}

	}


	/*ComplexityChunk GetNextZone(int[] currentZonePoint, ComplexityBranch branch){
		List<int> directions = new List<int>(new int[]{0, 1, 2, 3});
		ComplexityChunk nextZone = new ComplexityChunk ();

		if (directions.Count != 0) {
			int choosenDirection = directions [Random.Range (0, 4)];
			int[] currentPoint = new int[]{ currentZonePoint [0], currentZonePoint [1] };
			int[] nextPoint = LevelGeneration.GetNextPoint (currentPoint, choosenDirection);

			
			bool nextZoneNotFound = true;
			int i = 0;
			while (nextZoneNotFound && i < 1000) {
				i += 1;
				int zoneIndex = branch.complexities.FindIndex (c => c.zoneChunkId == map [nextPoint [0], nextPoint [1]].zoneChunkId);
				//Debug.Log (nextPoint[0].ToString() + "/" + nextPoint[1].ToString());
				//Debug.Log(zoneIndex);
				if (map [nextPoint [0], nextPoint [1]].zoneChunkId != -1 && zoneIndex == -1) {
					if (map [nextPoint [0], nextPoint [1]].zoneChunkId != map [currentPoint [0], currentPoint [1]].zoneChunkId) {
						nextZone.zoneChunkId = map [nextPoint [0], nextPoint [1]].zoneChunkId;
						nextZone.coordInMapArray = map [nextPoint [0], nextPoint [1]].coordInArray;

						nextZone.complexity = map [nextPoint [0], nextPoint [1]].complexity;
						nextZoneNotFound = false;
					} else {
						currentPoint = new int[]{ nextPoint [0], nextPoint [1] };
						nextPoint = LevelGeneration.GetNextPoint (currentPoint, choosenDirection);
						//Debug.Log (nextPoint[0].ToString() + "/" + nextPoint[1]);
					}
				} else {
					//Debug.Log (directions.Count);
					directions.Remove (choosenDirection);

					if (directions.Count != 0) {
						choosenDirection = directions [Random.Range (0, directions.Count)];
						currentPoint = new int[]{ currentZonePoint [0], currentZonePoint [1] };
						nextPoint = LevelGeneration.GetNextPoint (currentPoint, choosenDirection);
					}
				}
			}
		}

		//Debug.Log (nextZoneNotFound);
		//Debug.Log (nextZone.coordInMapArray.x.ToString() + "/" + nextZone.coordInMapArray.y.ToString());

		return nextZone;
	}*/

	MapZoneChunk[,] GetMapZoneChunkArray(){
		int width = map.GetLength (0);
		int height = map.Length / width;
		MapZoneChunk[,] mapWithComplexity = new MapZoneChunk[height, width];


		List<ComplexityChunk> chunks = new List<ComplexityChunk> ();
		foreach(ComplexityBranch branch in branches){
			foreach (ComplexityChunk chunk in branch.complexities) {
				chunks.Add (chunk);
			}
		}
		foreach(ComplexityBranch branch in branchesWithOtherPath){
			foreach (ComplexityChunk chunk in branch.complexities) {
				chunk.inBranch = true;
				chunks.Add (chunk);
			}
		}

		/*foreach (ComplexityChunk chunk in chunks) {
			Debug.Log (chunk.complexity);
		}*/


		for (int i = 0; i < height; i++) {
			for (int j = 0; j < width; j++) {
				mapWithComplexity [i, j] = new MapZoneChunk();
				mapWithComplexity [i, j].SetData (map [i, j]);
				ComplexityChunk chunk = chunks.Find (c => c.zoneChunkId == mapWithComplexity [i, j].zoneChunkId);
				/*chunk.zoneChunkId = -1;
				foreach(ComplexityBranch branch in finalBranches){
					chunk = branch.FindComplexityChunk (mapWithComplexity [i, j].zoneChunkId);
					if (chunk.zoneChunkId != -1) {
						break;
					}
				}*/
				if (chunk != null) {
					if (chunk.inBranch) {
						mapWithComplexity [i, j].zoneChunkType = -1;
					}
					mapWithComplexity [i, j].complexity = chunk.complexity;
				}
			}
		}
		return mapWithComplexity;
	}


	void SetComplexityToBranches(){
		foreach (ComplexityBranch branch in branches.FindAll (b => b.branchFrom == -1)) {
			int startComplexity = complexityStart;
			foreach (ComplexityChunk chunk in branch.complexities) {
				chunk.complexity = startComplexity;
				startComplexity = startComplexity + complexityRange;
			}
		}

		foreach (ComplexityBranch branch in branches) {
			if (branch.branchFrom != -1) {
				int startComplexity = FindZoneComplexity (branch.branchFrom) + complexityRange;
				foreach (ComplexityChunk chunk in branch.complexities) {
					chunk.complexity = startComplexity;
					startComplexity = startComplexity + complexityRange;
				}
			}
		}

		bool branchesNotInBranchExist = true;
		int i = 0;
		//while (branchesNotInBranchExist && i < 1000) {
			foreach (ComplexityBranch branch in branchesWithOtherPath) {
				if (branch.branchFrom != -1) {
					int foundComplexity = FindZoneComplexity (branch.branchFrom, true);
					//Debug.Log (foundComplexity);
					if (foundComplexity != -1) {
						//Debug.Log ("lol");
						int startComplexity = foundComplexity + complexityRange;
						foreach (ComplexityChunk chunk in branch.complexities) {
							//Debug.Log (startComplexity);
							chunk.complexity = startComplexity;
							startComplexity = startComplexity + complexityRange;
						}
					}
				}
			}

			if (branchesWithOtherPath.FindIndex (b => b.complexities.FindIndex (c => c.complexity == -1) == -1) == -1) {
				branchesNotInBranchExist = false;
			}
		//}


		/*List<ComplexityBranch> allBranches = new List<ComplexityBranch> ();
		allBranches.AddRange (branches);
		allBranches.AddRange (branchesWithOtherPath);
		//Debug.Log (allBranches.Count);


		foreach (ComplexityBranch branch in allBranches) {
			if(branch.branchFrom == -1){
				branch.inBranch = true;
				finalBranches.Add (branch);
			}
		}


		bool branchesNotInBranchExist = true;
		int i = 0;
		while (branchesNotInBranchExist && i< 200) {
			i++;
			foreach (ComplexityBranch branch in finalBranches) {
				//Debug.Log ("lol");
				foreach (ComplexityBranch addedBranch in allBranches) {
					if (addedBranch.branchFrom != -1) {
						addedBranch.inBranch = branch.AddBranch (addedBranch);
						//Debug.Log (branch.AddBranch (addedBranch));
					}

				}
			}

			if (allBranches.FindIndex (b => b.inBranch == false) == -1) {
				branchesNotInBranchExist = false;
			}
		}

		foreach (ComplexityBranch branch in finalBranches) {
			branch.SetComplexity (complexityStart, complexityRange);
		}*/
	}

	int FindZoneComplexity(int zoneNumber, bool isOtherPath = false){
		int complexity = -1;

		//if (isOtherPath) {
			foreach (ComplexityBranch branch in branchesWithOtherPath) {
				ComplexityChunk chunk = branch.complexities.Find (c => c.zoneChunkId == zoneNumber);
				if (chunk != null) {
					//Debug.Log (chunk.complexity);
					complexity = chunk.complexity;
					break;
				}
			}
		//} else {
			foreach (ComplexityBranch branch in branches) {
				ComplexityChunk chunk = branch.complexities.Find (c => c.zoneChunkId == zoneNumber);
				if (chunk != null) {
					complexity = chunk.complexity;
					break;
				}
			}
		//}
		return complexity;
	}



	void SetValueDown(int[] point, ComplexityBranch branch){
		//1
		if (map [point [0], point [1] - 1].pointType == 0 &&
			map [point [0], point [1] + 1].pointType == 0 &&
			map [point [0]+1, point [1]].pointType == 0 
		) {
			/*int rand = Random.Range (1, 101);
			if(rand > 80 && downBranch < branchesMaximum){
				downBranch = downBranch + 1;
				int leftOrRightOrBoth = Random.Range (0, 3);
				switch (leftOrRightOrBoth) {
				case 0: 
					ComplexityBranch leftBranch = new ComplexityBranch ();
					leftBranch.branchFrom = map [point [0], point [1] - 1].zoneChunkId;
					branches.Add (leftBranch);

					AddComplexityChunkToBranch (point, branch);
					SetValueLeft (LevelGeneration.GetNextPoint (point, left), leftBranch);
					SetValueDown (LevelGeneration.GetNextPoint (point, down), branch);
					return;
					break;
				case 1: 
					ComplexityBranch rightBranch = new ComplexityBranch ();
					rightBranch.branchFrom = map [point [0], point [1] - 1].zoneChunkId;
					branches.Add (rightBranch);

					AddComplexityChunkToBranch (point, branch);
					SetValueRight (LevelGeneration.GetNextPoint (point, right), rightBranch);
					SetValueDown (LevelGeneration.GetNextPoint (point, down), branch);
					return;
					break;
				case 2: 
					ComplexityBranch rightBranch1 = new ComplexityBranch ();
					rightBranch1.branchFrom = map [point [0], point [1] - 1].zoneChunkId;
					branches.Add (rightBranch1);

					ComplexityBranch leftBranch1 = new ComplexityBranch ();
					leftBranch1.branchFrom = map [point [0], point [1] - 1].zoneChunkId;
					branches.Add (leftBranch1);

					AddComplexityChunkToBranch (point, branch);
					SetValueRight (LevelGeneration.GetNextPoint (point, right), rightBranch1);
					SetValueLeft (LevelGeneration.GetNextPoint (point, left), leftBranch1);
					SetValueDown (LevelGeneration.GetNextPoint (point, down), branch);
					return;
					break;
				}
			}else{*/
				int direction = LevelGeneration.PathDirectionLeftRightDown (33, 33, 33);
				int[] nextPoint = LevelGeneration.GetNextPoint (point, direction);
				AddComplexityChunkToBranch (point, branch);
				SetValueDown (nextPoint, branch);
				return;
			//}

		}

		//2
		if (map [point [0], point [1] - 1].pointType != 0 &&
			map [point [0], point [1] + 1].pointType == 0 &&
			map [point [0] + 1, point [1]].pointType == 0 
		) {
			int direction = LevelGeneration.PathDirectionRightDown (33, 33);
			int[] nextPoint = LevelGeneration.GetNextPoint (point, direction);
			AddComplexityChunkToBranch (point, branch);
			SetValueDown (nextPoint, branch);
			return;
		}


		//3
		if (map [point [0], point [1] - 1].pointType == 0 &&
			map [point [0], point [1] + 1].pointType != 0 &&
			map [point [0] + 1, point [1]].pointType == 0 
		) {
			int direction = LevelGeneration.PathDirectionLeftDown (33, 33);
			int[] nextPoint = LevelGeneration.GetNextPoint (point, direction);
			AddComplexityChunkToBranch (point, branch);
			SetValueDown (nextPoint, branch);
			return;
		}

		//4
		if (map [point [0], point [1] - 1].pointType != 0 &&
			map [point [0], point [1] + 1].pointType != 0 &&
			map [point [0] + 1, point [1]].pointType == 0 
		) {
			int direction = down;
			int[] nextPoint = LevelGeneration.GetNextPoint (point, direction);
			AddComplexityChunkToBranch (point, branch);
			SetValueDown (nextPoint, branch);
			return;
		}

		//5

		if (map [point [0], point [1] - 1].pointType == 0 &&
			map [point [0], point [1] + 1].pointType != 0 &&
			map [point [0] + 1, point [1]].pointType != 0 
		) {
			int direction = LevelGeneration.PathDirectionLeftDown (33, 33);
			int[] nextPoint = LevelGeneration.GetNextPoint (point, direction);
			AddComplexityChunkToBranch (point, branch);
			if (direction != down) {
				SetValueDown (nextPoint, branch);
			}
			return;
		}

		//6
		if (map [point [0], point [1] - 1].pointType != 0 &&
			map [point [0], point [1] + 1].pointType == 0 &&
			map [point [0] + 1, point [1]].pointType != 0 
		) {
			int direction = LevelGeneration.PathDirectionRightDown (33, 33);
			int[] nextPoint = LevelGeneration.GetNextPoint (point, direction);
			AddComplexityChunkToBranch (point, branch);
			if (direction != down) {
				SetValueDown (nextPoint, branch);
			}
			return;
		}

		//7
		if (map [point [0], point [1] - 1].pointType != 0 &&
			map [point [0], point [1] + 1].pointType != 0 &&
			map [point [0] + 1, point [1]].pointType != 0 
		) {
			int direction = down;
			int[] nextPoint = LevelGeneration.GetNextPoint (point, direction);
			AddComplexityChunkToBranch (point, branch);
			return;
		}

		//8
		if (map [point [0], point [1] - 1].pointType == 0 &&
			map [point [0], point [1] + 1].pointType == 0 &&
			map [point [0] + 1, point [1]].pointType != 0 
		) {
			int direction = LevelGeneration.PathDirectionLeftRightDown(33,33,33);
			int[] nextPoint = LevelGeneration.GetNextPoint (point, direction);
			AddComplexityChunkToBranch (point, branch);
			if (direction != down) {
				SetValueDown (nextPoint, branch);
			}
			return;
		}

	}








	void SetValueLeft(int[] point, ComplexityBranch branch){
		//1
		if (map [point [0], point [1] - 1].pointType == 0 &&
			map [point [0]+1, point [1]].pointType == 0 &&
			map [point [0]-1, point [1]].pointType == 0 ) {
			int rand = Random.Range (1, 101);
			/*if(rand > 80 && leftBranch < branchesMaximum){
				leftBranch = leftBranch + 1;
				int upOrdownOrBoth = Random.Range (0, 3);
				switch (upOrdownOrBoth) {
				case 0: 
					ComplexityBranch upBranch = new ComplexityBranch ();
					upBranch.branchFrom = map [point [0], point [1] - 1].zoneChunkId;
					branches.Add (upBranch);

					AddComplexityChunkToBranch (point, branch);
					SetValueUp (LevelGeneration.GetNextPoint (point, up), upBranch);
					SetValueLeft (LevelGeneration.GetNextPoint (point, left), branch);
					return;
					break;
				case 1: 
					ComplexityBranch downBranch = new ComplexityBranch ();
					downBranch.branchFrom = map [point [0], point [1] - 1].zoneChunkId;
					branches.Add (downBranch);

					AddComplexityChunkToBranch (point, branch);
					SetValueDown (LevelGeneration.GetNextPoint (point, down), downBranch);
					SetValueLeft (LevelGeneration.GetNextPoint (point, left), branch);
					return;
					break;
				case 2: 
					ComplexityBranch upBranch1 = new ComplexityBranch ();
					upBranch1.branchFrom = map [point [0], point [1] - 1].zoneChunkId;
					branches.Add (upBranch1);

					ComplexityBranch downBranch1 = new ComplexityBranch ();
					downBranch1.branchFrom = map [point [0], point [1] - 1].zoneChunkId;
					branches.Add (downBranch1);

					AddComplexityChunkToBranch (point, branch);
					SetValueUp (LevelGeneration.GetNextPoint (point, up), upBranch1);
					SetValueDown (LevelGeneration.GetNextPoint (point, down), downBranch1);
					SetValueLeft (LevelGeneration.GetNextPoint (point, left), branch);
					return;
					break;
				}
			}else{*/
				int direction = LevelGeneration.PathDirectionLeftUpDown (33, 33, 33);
				int[] nextPoint = LevelGeneration.GetNextPoint (point, direction);
				AddComplexityChunkToBranch (point, branch);
				SetValueLeft (nextPoint, branch);
				return;
			//}

		}

		//2
		if (map [point [0], point [1] - 1].pointType == 0 &&
			map [point [0]+1, point [1]].pointType == 0 &&
			map [point [0]-1, point [1]].pointType != 0) {
			int direction = LevelGeneration.PathDirectionLeftDown (33, 33);
			int[] nextPoint = LevelGeneration.GetNextPoint (point, direction);
			AddComplexityChunkToBranch (point, branch);
			SetValueLeft (nextPoint, branch);
			return;
		}

		//3
		if (map [point [0], point [1] - 1].pointType == 0 &&
			map [point [0]+1, point [1]].pointType != 0 &&
			map [point [0]-1, point [1]].pointType == 0 ) {
			int direction = LevelGeneration.PathDirectionLeftUp (33, 33);
			int[] nextPoint = LevelGeneration.GetNextPoint (point, direction);
			AddComplexityChunkToBranch (point, branch);
			SetValueLeft (nextPoint, branch);
			return;
		}

		//4
		if (map [point [0], point [1] - 1].pointType == 0 &&
			map [point [0]+1, point [1]].pointType != 0 &&
			map [point [0]-1, point [1]].pointType != 0 ) {
			int[] nextPoint = LevelGeneration.GetNextPoint (point, left);
			AddComplexityChunkToBranch (point, branch);
			SetValueLeft (nextPoint, branch);
			return;
		}

		//5
		if (map [point [0], point [1] - 1].pointType != 0 &&
			map [point [0]+1, point [1]].pointType == 0 &&
			map [point [0]-1, point [1]].pointType != 0 ) {
			int direction = LevelGeneration.PathDirectionLeftDown (33, 33);
			int[] nextPoint = LevelGeneration.GetNextPoint (point, direction);
			AddComplexityChunkToBranch (point, branch);
			if (direction != left) {
				SetValueLeft (nextPoint, branch);
			}
			return;
		}

		//6

		if (map [point [0], point [1] - 1].pointType != 0 &&
			map [point [0]+1, point [1]].pointType != 0 &&
			map [point [0]-1, point [1]].pointType == 0 ) {
			int direction = LevelGeneration.PathDirectionLeftUp (33, 33);
			int[] nextPoint = LevelGeneration.GetNextPoint (point, direction);
			AddComplexityChunkToBranch (point, branch);
			if (direction != left) {
				SetValueLeft (nextPoint, branch);
			}
			return;
		}

		//7
		if (map [point [0], point [1] - 1].pointType != 0 &&
			map [point [0]+1, point [1]].pointType != 0 &&
			map [point [0]-1, point [1]].pointType != 0 ) {
			int direction = left;//LevelGeneration.PathDirectionLeftUpDown (33, 33, 33);
			int[] nextPoint = LevelGeneration.GetNextPoint (point, direction);
			AddComplexityChunkToBranch (point, branch);
			return;
		}

		//8
		if (map [point [0], point [1] - 1].pointType != 0 &&
			map [point [0]+1, point [1]].pointType == 0 &&
			map [point [0]-1, point [1]].pointType == 0 ) {
			int direction = LevelGeneration.PathDirectionLeftUpDown (33, 33, 33);
			int[] nextPoint = LevelGeneration.GetNextPoint (point, direction);
			AddComplexityChunkToBranch (point, branch);
			if (direction != left) {
				SetValueLeft (nextPoint, branch);
			}
			return;
		}
		return;
	}





	void SetValueRight(int[] point, ComplexityBranch branch){
		//1
		if (map [point [0], point [1] + 1].pointType == 0 &&
			map [point [0]+1, point [1]].pointType == 0 &&
			map [point [0]-1, point [1]].pointType == 0 ) {
			int rand = Random.Range (1, 101);
			/*if(rand > 80 && rightBranch < branchesMaximum){
				rightBranch = rightBranch + 1;
				int upOrdownOrBoth = Random.Range (0, 3);
				switch (upOrdownOrBoth) {
				case 0: 
					ComplexityBranch upBranch = new ComplexityBranch ();
					upBranch.branchFrom = map [point [0], point [1] - 1].zoneChunkId;
					branches.Add (upBranch);

					AddComplexityChunkToBranch (point, branch);
					SetValueUp (LevelGeneration.GetNextPoint (point, up), upBranch);
					SetValueRight (LevelGeneration.GetNextPoint (point, right), branch);
					return;
					break;
				case 1: 
					ComplexityBranch downBranch = new ComplexityBranch ();
					downBranch.branchFrom = map [point [0], point [1] - 1].zoneChunkId;
					branches.Add (downBranch);

					AddComplexityChunkToBranch (point, branch);
					SetValueDown (LevelGeneration.GetNextPoint (point, down), downBranch);
					SetValueRight (LevelGeneration.GetNextPoint (point, right), branch);
					return;
					break;
				case 2: 
					ComplexityBranch upBranch1 = new ComplexityBranch ();
					upBranch1.branchFrom = map [point [0], point [1] - 1].zoneChunkId;
					branches.Add (upBranch1);

					ComplexityBranch downBranch1 = new ComplexityBranch ();
					downBranch1.branchFrom = map [point [0], point [1] - 1].zoneChunkId;
					branches.Add (downBranch1);

					AddComplexityChunkToBranch (point, branch);
					SetValueUp (LevelGeneration.GetNextPoint (point, up), upBranch1);
					SetValueDown (LevelGeneration.GetNextPoint (point, down), downBranch1);
					SetValueRight (LevelGeneration.GetNextPoint (point, right), branch);
					return;
					break;
				}
			}else{*/
				int direction = LevelGeneration.PathDirectionRightUpDown (33, 33, 33);
				int[] nextPoint = LevelGeneration.GetNextPoint (point, direction);
				AddComplexityChunkToBranch (point, branch);
				SetValueRight (nextPoint, branch);
				return;
			//}
		}

		//2
		if (map [point [0], point [1] + 1].pointType == 0 &&
			map [point [0] + 1, point [1]].pointType == 0 &&
			map [point [0] - 1, point [1]].pointType != 0) {
			int direction = LevelGeneration.PathDirectionRightDown (33, 33);
			int[] nextPoint = LevelGeneration.GetNextPoint (point, direction);
			AddComplexityChunkToBranch (point, branch);
			SetValueRight (nextPoint, branch);
			return;
		}


		//3
		if (map [point [0], point [1] + 1].pointType == 0 &&
			map [point [0] + 1, point [1]].pointType != 0 &&
			map [point [0] - 1, point [1]].pointType == 0) {
			int direction = LevelGeneration.PathDirectionRightUp (33, 33);
			int[] nextPoint = LevelGeneration.GetNextPoint (point, direction);
			AddComplexityChunkToBranch (point, branch);
			SetValueRight (nextPoint, branch);
			return;
		}

		//4
		if (map [point [0], point [1] + 1].pointType == 0 &&
			map [point [0] + 1, point [1]].pointType != 0 &&
			map [point [0] - 1, point [1]].pointType != 0) {
			int direction = right;
			int[] nextPoint = LevelGeneration.GetNextPoint (point, direction);
			AddComplexityChunkToBranch (point, branch);
			SetValueRight (nextPoint, branch);
			return;
		}

		//5

		if (map [point [0], point [1] + 1].pointType != 0 &&
			map [point [0] + 1, point [1]].pointType == 0 &&
			map [point [0] - 1, point [1]].pointType != 0) {
			int direction = LevelGeneration.PathDirectionRightDown (33, 33);
			int[] nextPoint = LevelGeneration.GetNextPoint (point, direction);
			AddComplexityChunkToBranch (point, branch);
			if (direction != right) {
				SetValueRight (nextPoint, branch);
			} 
			return;
		}

		//6
		if (map [point [0], point [1] + 1].pointType != 0 &&
			map [point [0] + 1, point [1]].pointType != 0 &&
			map [point [0] - 1, point [1]].pointType == 0) {
			int direction = LevelGeneration.PathDirectionRightUp (33, 33);
			int[] nextPoint = LevelGeneration.GetNextPoint (point, direction);
			AddComplexityChunkToBranch (point, branch);
			if (direction != right) {
				SetValueRight (nextPoint, branch);
			} 
			return;
		}

		//7
		if (map [point [0], point [1] + 1].pointType != 0 &&
			map [point [0] + 1, point [1]].pointType != 0 &&
			map [point [0] - 1, point [1]].pointType != 0) {
			int direction = right;
			int[] nextPoint = LevelGeneration.GetNextPoint (point, direction);
			AddComplexityChunkToBranch (point, branch);
			if (direction != right) {
				SetValueRight (nextPoint, branch);
			} 
			return;
		}

		//8
		if (map [point [0], point [1] + 1].pointType != 0 &&
			map [point [0] + 1, point [1]].pointType == 0 &&
			map [point [0] - 1, point [1]].pointType == 0) {
			int direction = LevelGeneration.PathDirectionRightUpDown(33,33,33);
			int[] nextPoint = LevelGeneration.GetNextPoint (point, direction);
			AddComplexityChunkToBranch (point, branch);
			if (direction != right) {
				SetValueRight (nextPoint, branch);
			} 
			return;
		}
	}




	void SetValueUp(int[] point, ComplexityBranch branch){
		//1
		if (map [point [0], point [1] + 1].pointType == 0 &&
			map [point [0], point [1] - 1].pointType == 0 &&
			map [point [0]-1, point [1]].pointType == 0 ) {
			/*int rand = Random.Range (1, 101);
			if(rand > 80 && upBranch < branchesMaximum){
				upBranch = upBranch + 1;
				int leftOrRightOrBoth = Random.Range (0, 3);
				switch (leftOrRightOrBoth) {
				case 0: 
					ComplexityBranch leftBranch = new ComplexityBranch ();
					leftBranch.branchFrom = map [point [0], point [1] - 1].zoneChunkId;
					branches.Add (leftBranch);

					AddComplexityChunkToBranch (point, branch);
					SetValueLeft (LevelGeneration.GetNextPoint (point, left), leftBranch);
					SetValueUp (LevelGeneration.GetNextPoint (point, up), branch);
					return;
					break;
				case 1: 
					ComplexityBranch rightBranch = new ComplexityBranch ();
					rightBranch.branchFrom = map [point [0], point [1] - 1].zoneChunkId;
					branches.Add (rightBranch);

					AddComplexityChunkToBranch (point, branch);
					SetValueRight (LevelGeneration.GetNextPoint (point, right), rightBranch);
					SetValueUp (LevelGeneration.GetNextPoint (point, up), branch);
					return;
					break;
				case 2: 
					ComplexityBranch rightBranch1 = new ComplexityBranch ();
					rightBranch1.branchFrom = map [point [0], point [1] - 1].zoneChunkId;
					branches.Add (rightBranch1);

					ComplexityBranch leftBranch1 = new ComplexityBranch ();
					leftBranch1.branchFrom = map [point [0], point [1] - 1].zoneChunkId;
					branches.Add (leftBranch1);

					AddComplexityChunkToBranch (point, branch);
					SetValueRight (LevelGeneration.GetNextPoint (point, right), rightBranch1);
					SetValueLeft (LevelGeneration.GetNextPoint (point, left), leftBranch1);
					SetValueUp (LevelGeneration.GetNextPoint (point, up), branch);
					return;
					break;
				}
			}else{*/
				int direction = LevelGeneration.PathDirectionLeftUpRight (33, 33, 33);
				int[] nextPoint = LevelGeneration.GetNextPoint (point, direction);
				AddComplexityChunkToBranch (point, branch);
				SetValueUp (nextPoint, branch);
				return;
			//}
		}

		//2
		if (map [point [0], point [1] + 1].pointType == 0 &&
			map [point [0] - 1, point [1]].pointType == 0 &&
			map [point [0] , point [1] - 1].pointType != 0) {
			int direction = LevelGeneration.PathDirectionUpRight (33, 33);
			int[] nextPoint = LevelGeneration.GetNextPoint (point, direction);
			AddComplexityChunkToBranch (point, branch);
			SetValueUp (nextPoint, branch);
			return;
		}


		//3
		if (map [point [0], point [1] - 1].pointType == 0 &&
			map [point [0] - 1, point [1]].pointType == 0 &&
			map [point [0], point [1] + 1].pointType != 0) {
			int direction = LevelGeneration.PathDirectionUpLeft (33, 33);
			int[] nextPoint = LevelGeneration.GetNextPoint (point, direction);
			AddComplexityChunkToBranch (point, branch);
			SetValueUp (nextPoint, branch);
			return;
		}

		//4
		if (map [point [0], point [1] - 1].pointType != 0 &&
			map [point [0] - 1, point [1]].pointType == 0 &&
			map [point [0], point [1] + 1].pointType != 0) {
			int direction = up;
			int[] nextPoint = LevelGeneration.GetNextPoint (point, direction);
			AddComplexityChunkToBranch (point, branch);
			SetValueUp (nextPoint, branch);
			return;
		}

		//5

		if (map [point [0], point [1] - 1].pointType != 0 &&
			map [point [0] - 1, point [1]].pointType != 0 &&
			map [point [0], point [1] + 1].pointType == 0) {
			int direction = LevelGeneration.PathDirectionUpRight (33, 33);
			int[] nextPoint = LevelGeneration.GetNextPoint (point, direction);
			AddComplexityChunkToBranch (point, branch);
			if (direction != up) {
				SetValueUp (nextPoint, branch);
			} 
			return;
		}

		//6
		if (map [point [0], point [1] - 1].pointType == 0 &&
			map [point [0] - 1, point [1]].pointType != 0 &&
			map [point [0], point [1] + 1].pointType != 0) {
			int direction = LevelGeneration.PathDirectionUpLeft (33, 33);
			int[] nextPoint = LevelGeneration.GetNextPoint (point, direction);
			AddComplexityChunkToBranch (point, branch);
			if (direction != up) {
				SetValueUp (nextPoint, branch);
			} 
			return;
		}

		//7
		if (map [point [0], point [1] - 1].pointType != 0 &&
			map [point [0] - 1, point [1]].pointType != 0 &&
			map [point [0], point [1] + 1].pointType != 0) {
			int direction = up;
			int[] nextPoint = LevelGeneration.GetNextPoint (point, direction);
			AddComplexityChunkToBranch (point, branch);
			if (direction != up) {
				SetValueUp (nextPoint, branch);
			} 
			return;
		}

		//8
		if (map [point [0], point [1] - 1].pointType == 0 &&
			map [point [0] - 1, point [1]].pointType != 0 &&
			map [point [0], point [1] + 1].pointType == 0) {
			int direction = LevelGeneration.PathDirectionLeftUpRight(33,33,33);
			int[] nextPoint = LevelGeneration.GetNextPoint (point, direction);
			AddComplexityChunkToBranch (point, branch);
			if (direction != up) {
				SetValueUp (nextPoint, branch);
			} 
			return;
		}
	}


	bool BranchesNotContainsNumber(int number){
		bool isContainsNumber = true;
		foreach (ComplexityBranch branch in branches) {
			foreach(ComplexityChunk chunk in branch.complexities){
				if (chunk.zoneChunkId == number) {
					isContainsNumber = false;
				}
			}
		}
		return isContainsNumber;
	}
		
	void AddComplexityChunkToBranch(int[] point, ComplexityBranch branch){
		if (BranchesNotContainsNumber (map [point [0], point [1]].zoneChunkId)) {
			map [point [0], point [1]].pointType = 1;
			ComplexityChunk newComplexityChunk = new ComplexityChunk ();
			newComplexityChunk.zoneChunkId = map [point [0], point [1]].zoneChunkId;
			branch.complexities.Add (newComplexityChunk);
		}
	}
}
