using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapZoneGenerator : MonoBehaviour {

	static int  height = 100;
	static int  width = 100;
	int pointCount =0;
	Hashtable colors = new Hashtable ();
	List<int[]> points = new List<int[]>();
	public MapZoneChunk[,] map = new MapZoneChunk[height+2, width+2];
	// Use this for initialization
	void Start () {
		for (int i = 0; i < height+2; i++) {
			for (int j = 0; j < width+2; j++) {
				map[i, j] = new MapZoneChunk();
				map [i, j].zoneChunkId = 0;
			}
		}
		
	
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.E)) {
			RefreshArray ();

			for(int i = 0; i < 50; i++){
				points.Add(GetRandomPoint ());
			}

			GenerateZonesByPoints (points);

			SetColors ();
			for (int i = 0; i < height+2; i++) {
				for (int j = 0; j < width+2; j++) {
					Draw (new Vector3(j, -i, 0), map[i,j].zoneChunkId);
				}
			}
		}
	
	}
	//**********test**************************************
	void SetColors(){
		foreach (int[] point in points) {
			colors.Add (map [point [0], point [1]].zoneChunkId, new Color (Random.value, Random.value, Random.value, 1.0f));
			//colors.Add (map [point [0], point [1]] - 10, new Color (Random.value, Random.value, Random.value, 1.0f));
		}
		colors.Add (-1, new Color (Random.value, Random.value, Random.value, 1.0f));
		colors.Add (0, new Color (Random.value, Random.value, Random.value, 1.0f));
	}

    void Draw(Vector3 coord, int type){
		GameObject mapZone = Instantiate ((GameObject)Resources.Load("TestZone"));
		mapZone.transform.position = new Vector3 (mapZone.transform.position.x + coord.x,
			mapZone.transform.position.y + coord.y,
			mapZone.transform.position.z + coord.z);
		if (colors.ContainsKey (type)) {
			mapZone.GetComponent<SpriteRenderer> ().material.color = (Color)colors [type];
		}
		GameObject textRenderer = mapZone.transform.GetChild (0).gameObject;
		textRenderer.GetComponent<TextMesh> ().text = type.ToString ();
	}
	//****************************************************************

	void RefreshArray(){
		for (int i = 0; i < height+2; i++) {
			for (int j = 0; j < width+2; j++) {
				if(j == 0 || j == (width+1) || i == 0 || i == (height+1)){
					map [i, j] = new MapZoneChunk();
					map [i, j].zoneChunkId = -1;
				}else{
					map [i, j] = new MapZoneChunk();
					map [i, j].zoneChunkId = 0;
				}
			}
		}
	}

	int[] GetRandomPoint(){
		int x = Random.Range (1, width+1);
		int y = Random.Range (1, height+1);
		int[] coord = new int[] {0, 0};
		if (map [y, x].zoneChunkId == 0) {
			pointCount++;
			map [y, x].zoneChunkId = pointCount;
			coord = new int[] { y, x };
		} else {
			GetRandomPoint ();
		}
		return coord;
	}

	void GenMap(){
		
	}


	void GenerateZonesByPoints(List<int[]> points){
		List<Zone> zones = new List<Zone>();
		foreach (int[] point in points) {
			Zone zone = new Zone ();
			zone.SetSettings (ref map, new int[] { point [0],  point [1] }, map[point[0], point[1]].zoneChunkId);
			zones.Add (zone);
		}

		//int zonesCount = zones.Count;
		//int lol = 2000;
		while ( zones.Count != 0/*lol != 0*/) {
			List<int> indexes = new List<int>();
			foreach (Zone zone in zones) {
				if (zone.ZoneCanGrow()) {
					zone.Grow ();
				}
				//Debug.Log (zone.ZoneCanGrow());
			}
				zones.RemoveAll(z => !z.ZoneCanGrow());
		}
	}

	private class Zone{
		MapZoneChunk[,] map;
		int[] pointCoord;
		public int zoneNumber;

		const int up = 0;
		const int down = 1;
		const int left = 2;
		const int right = 3;

		int growUp = 0;
		int growDown = 0;
		int growLeft = 0;
		int growRight = 0;

		bool canGrowUp = true;
		bool canGrowDown = true;
		bool canGrowLeft = true;
		bool canGrowRight = true;

		public void SetSettings(ref MapZoneChunk[,] getMap, int[] getPointCoord, int getZoneNumber){
			map = getMap;
			pointCoord = getPointCoord;
			zoneNumber = getZoneNumber;
		}

		public bool ZoneCanGrow(){
			bool value = false;
			if (canGrowUp || canGrowDown || canGrowLeft || canGrowRight) {
				value = true;
			} else {
				value = false;
			}
			return value;
		}

		public void Grow(){
			int growingUp = Random.Range (0, 2);
			if (growingUp == 0) {
				GrowUp ();
			}

			int growingDown = Random.Range (0, 2);
			if (growingDown == 0) {
				GrowDown ();
			}

			int growingLeft = Random.Range (0, 2);
			if (growingLeft == 0) {
				GrowLeft ();
			}

			int growingRight = Random.Range (0, 2);
			if (growingRight == 0) {
				GrowRight ();
			}
		}

		bool isCanGrow(List<int[]> points){
			List<bool> checkedPoints = new List<bool> ();
			foreach (int[] point in points) {
				if (map [point [0], point [1]].zoneChunkId == 0) {
					checkedPoints.Add (true);
				} else {
					checkedPoints.Add (false);
				}
			}
			return CheckArrayBool(checkedPoints);
		}

		public static bool CheckArrayBool(List<bool> array){
			List<bool> newArray = new List<bool> ();
			bool finalValue = false;
			if(array.Count >= 3){
				for (int i = 0; i < array.Count - 1; i++) {
					if (array [i] || array [i + 1]) {
						newArray.Add (true);
					} else {
						newArray.Add (false);
					}
				}

				if (newArray.Count == 2) {
					if (newArray [0] || newArray [1]) {
						finalValue = true;
					} else {
						finalValue = false;
					}
				} else {
					finalValue = CheckArrayBool (newArray);
				}
			}

			if(array.Count == 2){
				if (array [0] || array [1]) {
					finalValue = true;
				} else {
					finalValue = false;
				}
			}

			if(array.Count == 1){
				finalValue = array [0];
			}

			return finalValue;
		}

		public void GrowLeft(){
			List<int[]> checkedPoints = new List<int[]> ();
			int[] centerPoint = new int[2] {pointCoord[0], pointCoord[1] - growLeft - 1};
			checkedPoints.Add (centerPoint);

			for (int i = 1; i < growDown+1; i++) {
				checkedPoints.Add (new int[] { centerPoint [0] + i, centerPoint [1] });
			}

			for (int i = 1; i < growUp+1; i++) {
				checkedPoints.Add(new int[] {centerPoint[0] - i, centerPoint[1]});
			}

			if (isCanGrow (checkedPoints)) {
				foreach (int[] point in checkedPoints) {
					if (map [point [0], point [1]].zoneChunkId == 0 && map [point [0], point [1]].zoneChunkId != -1) {
						map [point [0], point [1]].zoneChunkId = zoneNumber;
					}	
				}
				growLeft++;
				//canGrowLeft = false;
			} else {
				canGrowLeft = false;
			}

		}



		public void GrowRight(){
			List<int[]> checkedPoints = new List<int[]> ();
			int[] centerPoint = new int[2] {pointCoord[0], pointCoord[1] + growRight + 1};
			checkedPoints.Add (centerPoint);

			for (int i = 1; i < growDown+1; i++) {
				checkedPoints.Add (new int[] { centerPoint [0] + i, centerPoint [1] });
			}

			for (int i = 1; i < growUp+1; i++) {
				checkedPoints.Add(new int[] {centerPoint[0] - i, centerPoint[1]});
			}
			if (isCanGrow (checkedPoints)) {
				foreach(int[] point in checkedPoints){
					if(map[point[0], point[1]].zoneChunkId == 0 && map[point[0], point[1]].zoneChunkId != -1){
						map[point[0], point[1]].zoneChunkId = zoneNumber;
					}	
				}
				growRight++;
				//canGrowRight = false;
			} else {
				canGrowRight = false;
			}
		}

		public void GrowUp(){
			List<int[]> checkedPoints = new List<int[]> ();
			int[] centerPoint = new int[2] {pointCoord[0] - growUp - 1, pointCoord[1]};
			checkedPoints.Add (centerPoint);

			for (int i = 1; i < growRight+1; i++) {
				checkedPoints.Add (new int[] { centerPoint [0], centerPoint [1] + i });
			}

			for (int i = 1; i < growLeft+1; i++) {
				checkedPoints.Add(new int[] {centerPoint[0], centerPoint[1] - i});
			}

			if (isCanGrow (checkedPoints)) {
				foreach(int[] point in checkedPoints){
					if(map[point[0], point[1]].zoneChunkId == 0 && map[point[0], point[1]].zoneChunkId != -1){
						map[point[0], point[1]].zoneChunkId = zoneNumber;
					}	
				}
				growUp++;
				//canGrowUp = false;
			} else {
				canGrowUp = false;
			}
		}

		public void GrowDown(){
			List<int[]> checkedPoints = new List<int[]> ();
			int[] centerPoint = new int[2] {pointCoord[0] + growDown + 1, pointCoord[1]};
			checkedPoints.Add (centerPoint);

			for (int i = 1; i < growRight+1; i++) {
				checkedPoints.Add (new int[] { centerPoint [0], centerPoint [1] + i });
			}

			for (int i = 1; i < growLeft+1; i++) {
				checkedPoints.Add(new int[] {centerPoint[0], centerPoint[1] - i});
			}

			if (isCanGrow (checkedPoints)) {
				foreach(int[] point in checkedPoints){
					if(map[point[0], point[1]].zoneChunkId == 0 && map[point[0], point[1]].zoneChunkId != -1){
						map[point[0], point[1]].zoneChunkId = zoneNumber;
					}	
				}
				growDown++;
				//canGrowDown = false;
			} else {
				canGrowDown = false;
			}
		}
	}
}
