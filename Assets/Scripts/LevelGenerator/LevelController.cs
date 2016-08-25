using UnityEngine;
//using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Linq;
using System.Threading;
using S = System;

public class LevelController : MonoBehaviour {
	LevelGeneration generator;
	public static GameObject player;
	LevelChunkRudiment[,] level;
	public static LevelChunkRudiment[,] levelChunkRudimentsInfoForMiniPortals;
	public int currentLevel;
	//int nextLevel = currentLevel + 1;
	int test = 0;
	LevelMesh currentMesh;
	GameObject levelMeshPrefab;

	public List<PortalScript> portals = new List<PortalScript> ();
	//info 
	static List<LevelChunkInfo> levelChunksData = new List<LevelChunkInfo>(); 
	List<RandomPartBlock> randomPartBlocks = new List<RandomPartBlock> ();
	List<InteractiveObjectRandomPart> randomPartInteractiveObjects = new List<InteractiveObjectRandomPart> ();

	PortalInfo portalOnOldLevel;
	PortalInfo portalOnNewLevel;

	public GameObject levelEnvironment;
	public GameObject levelInteractiveObjects;
	public ButtonsController buttonsController;

	string levelTextureName = "earth";
	public Texture levelTexture;
	public int levelChunkSize = 20;
	LevelChunkFilesInfo levelChunkFilesInfo;

	Thread creatingRandomPartMeshesThread;
	List<LevelMesh.LevelMeshDataSerializable> newMeshesRandomPart;
	Hashtable interactiveObjects = new Hashtable();

	int randomizeRandomPartThreadCounts = 0;



	// Use this for initialization
	void Start () {
		levelMeshPrefab = (GameObject)Resources.Load ("LevelMesh");
		player = GameObject.Find ("Player");
		generator = GameObject.Find ("LevelController").GetComponent<LevelGeneration>();
		buttonsController = GameObject.Find ("ButtonsController").GetComponent<ButtonsController>();
		//level = generator.CreateLevelPath (10, 10);

		levelChunkFilesInfo = SaveLoadManager.LoadLevelChunkFilesInfo();

		foreach (GameObject obj in Resources.LoadAll<GameObject>("InteractiveObjects")) {
			interactiveObjects.Add (obj.name, obj);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.W)){
			//LevelChunkCreater.GenPrefabs ();

			/*List<LevelChunkRudiment> lol = new List<LevelChunkRudiment> (level);
			Debug.Log (lol.Find(l => l.chunkId == 20).coord.x);*/
			foreach(LevelChunkRudiment lol in level){
				Debug.Log (lol.coord.x.ToString() + "/" + lol.coord.y.ToString());
			}
			//SetLevelChunkRudimentsInfoForMiniPortals ();
		}
	
	}

	public void Test(){
	}


	public void Teleport(int portalId){
		PortalScript portal = portals.Find (p => p.portalId == portalId);
		portalOnOldLevel = new PortalInfo ();
		portalOnOldLevel.SetData (portal);

		if (portal.linkWithLevel == -1) {
			CreateLevel ();
			PortalScript newPortal = portals.Find (p => p.portalId == portalOnNewLevel.portalId);
			newPortal.linkWithPortalId = portalOnOldLevel.portalId;
			newPortal.linkWithLevel = currentLevel - 1;

			SaveLoadManager.SetLinkWithOldPortal (portalOnNewLevel, portalOnOldLevel, currentLevel);
		} else {
			int newPortalId = portal.linkWithPortalId;
			LoadLevel (portalOnOldLevel.linkWithLevel);
			Vector3 playerSpawn = portals.Find (p => p.portalId == newPortalId).gameObject.transform.position;
			StartCoroutine(LevelMesh.CoroutineAddDataProcess.PlayerSpawnWhenAllProcessStoped (player, playerSpawn));
		}
	}

	void LoadLevel(int levelNumber){
		ClearLevel ();
		portals.Clear ();
		levelChunksData.Clear();
		randomPartBlocks.Clear ();
		randomPartInteractiveObjects.Clear ();
		Transform[] objs = GameObject.Find ("LevelInteractiveObjects").GetComponentsInChildren<Transform> ();
		if(objs.Length != 0){
			foreach (Transform obj in objs) {
				if(obj.gameObject.name != "LevelInteractiveObjects"){
					Destroy (obj.gameObject);
				}
			}
		}

		Level loadedLevel =  SaveLoadManager.LoadLevel (levelNumber);
		//level = loadedLevel.level;
		SetLevelChunkRudiments(loadedLevel.levelArrayWidth, loadedLevel.levelArrayHeight);
		SetLevelChunkRudimentsInfoForMiniPortals ();
		foreach (LevelChunkInfo chunk in loadedLevel.levelChunksInfo) {
			//Debug.Log ((chunk.coord.y/(float)chunk.chunkSize).ToString() + "/" + (chunk.coord.x/(float)chunk.chunkSize).ToString());
			levelChunkRudimentsInfoForMiniPortals[(int)(Mathf.Abs(chunk.coord.y)/(float)chunk.chunkSize) +1, (int)(Mathf.Abs(chunk.coord.x)/(float)chunk.chunkSize) + 1].type = chunk.chunkType;
			CreatePart (new Vector3 (chunk.coord.x, chunk.coord.y, chunk.coord.z), 
				chunk.chunkSize,
				levelChunkRudimentsInfoForMiniPortals[(int)(Mathf.Abs(chunk.coord.y)/(float)chunk.chunkSize) +1, (int)(Mathf.Abs(chunk.coord.x)/(float)chunk.chunkSize) +1], 
				chunk.chunkNumber, 
				false, 
				false
			); 
		}


		foreach(PortalInfo portalInfo in loadedLevel.portalsInfo){
			GameObject newPortal = Instantiate ((GameObject)interactiveObjects["portal"]);
			PortalScript newPortalScript = newPortal.GetComponent<PortalScript> ();
			newPortalScript.SetData (portalInfo);
			portals.Add (newPortalScript);
		}

		GenRandomPart (loadedLevel.randomPartBlocks, loadedLevel.randomPartInteractiveObjects);
	}

	public void GenLevel(){
		portals.Clear ();
		randomPartBlocks.Clear ();
		randomPartInteractiveObjects.Clear ();
		Transform[] objs = GameObject.Find ("LevelInteractiveObjects").GetComponentsInChildren<Transform> ();
		if(objs.Length != 0){
			foreach (Transform obj in objs) {
				if (obj.gameObject.name != "LevelInteractiveObjects") {
					Destroy (obj.gameObject);
				}
			}
		}

		level = generator.CreateLevelPath (20, 20);
		SetLevelChunkRudimentsInfoForMiniPortals ();
		levelChunksData.Clear ();

		SetLevelTexture ();
		for (int i = 0; i < generator.height+2; i++) {
			for (int j = 0; j < generator.width+2; j++) {
				if (level [i, j].type != -3) {
					CreatePart (new Vector3 (j * levelChunkSize, i * levelChunkSize * -1, 0), levelChunkSize, level [i, j]);

				}
			}
		}
		PortalScript portal = GetRandomFromList (portals);
		Vector3 portalCoord = portal.gameObject.transform.position;
		portalOnNewLevel = new PortalInfo();
		portalOnNewLevel.SetData (portal);
		SetPortalsDirections ();
		WaitRandomizePartProcesses();
		SaveLevel ();
		GenRandomPart (randomPartBlocks, randomPartInteractiveObjects);

		StartCoroutine(LevelMesh.CoroutineAddDataProcess.PlayerSpawnWhenAllProcessStoped (player, portalCoord));
	}
		

	PortalScript GetRandomFromList(List<PortalScript> list){
		int rand = Random.Range (0, portals.Count);
		return list [rand];
	}
		

	void CreatePart(Vector3 coord, int chunkSize, LevelChunkRudiment chunkRudiment, int chunkNumber = -1, bool createPortals = true, bool withRandomPart = true){
		LevelChunkWithInfo chunkWithInfo = LoadLevelChunk(chunkSize, chunkRudiment, chunkNumber);
		levelChunksData.Add (chunkWithInfo.GetChunkInfo(coord));
		LevelChunk chunk = chunkWithInfo.levelChunk;

		/*************************************RandomPart***********************************************/
		if(withRandomPart){
			ThreadPool.QueueUserWorkItem((object param) => {
				/*ManualResetEvent flag = new ManualResetEvent(false);
				threadEvents.Add(flag);*/
				randomizeRandomPartThreadCounts = randomizeRandomPartThreadCounts + 1;
				S.Random rndc = new S.Random();
				foreach (RandomPartBlock block in chunk.randomlevelBlocks) {
					//S.Random rndc = new S.Random();
					int rnd = rndc.Next (0, 101);
					if (rnd <= block.randomValue) {
						RandomPartBlock blockClone = new RandomPartBlock();
						blockClone.SetData (block);
						blockClone.coord = new SerializableVector3(block.coord.x + coord.x , block.coord.y + coord.y, block.coord.z + coord.z );
						randomPartBlocks.Add (blockClone);
					}
				}

				foreach (InteractiveObjectRandomPart obj in chunk.randomObjects) {
					//S.Random rndc = new S.Random();
					int rnd = rndc.Next (0, 101);
					if (rnd <= obj.randomValue) {
						InteractiveObjectRandomPart objClone = new InteractiveObjectRandomPart ();
						objClone.SetData (obj);
						objClone.coord = new SerializableVector3(obj.coord.x + coord.x, obj.coord.y + coord.y, obj.coord.z + coord.z);
						objClone.chunkId = chunkRudiment.chunkId;
						objClone.coordInChunkRudimentInfoForMiniPortalsArray = chunkRudiment.coord;
						randomPartInteractiveObjects.Add (objClone);
					}
				}

				foreach (FullSet newSet in chunk.sets) {
					//S.Random rndc = new S.Random();
					int rnd = rndc.Next (0, 101);
					if (rnd <= newSet.randomValue) {
						foreach (RandomPartBlock block in newSet.blocks) {
							if (block.isRandom) {
								int blockRnd = rndc.Next (0, 101);
								if (blockRnd <= block.randomValue) {
									RandomPartBlock blockClone = new RandomPartBlock ();
									blockClone.SetData (block);
									blockClone.coord = new SerializableVector3 (block.coord.x + coord.x  , block.coord.y + coord.y, block.coord.z + coord.z);
									randomPartBlocks.Add (blockClone);
								}
							} else {
								RandomPartBlock blockClone = new RandomPartBlock ();
								blockClone.SetData (block);
								blockClone.coord = new SerializableVector3 (block.coord.x + coord.x , block.coord.y + coord.y , block.coord.z + coord.z);
								randomPartBlocks.Add (blockClone);
							}
						}
							
						foreach (InteractiveObjectRandomPart obj in newSet.interactiveObjects) {
							if (obj.isRandom) {
								//S.Random rndc = new S.Random();
								int objRnd = rndc.Next (0, 101);
								if (objRnd <= obj.randomValue) {
									InteractiveObjectRandomPart objClone = new InteractiveObjectRandomPart ();
									objClone.SetData (obj);
									objClone.coord = new SerializableVector3 (obj.coord.x + coord.x , obj.coord.y + coord.y , obj.coord.z + coord.z);
									objClone.chunkId = chunkRudiment.chunkId;
									objClone.coordInChunkRudimentInfoForMiniPortalsArray = chunkRudiment.coord;
									randomPartInteractiveObjects.Add (objClone);
								}
							} else {
								InteractiveObjectRandomPart objClone = new InteractiveObjectRandomPart ();
								objClone.SetData (obj);
								objClone.coord = new SerializableVector3 (obj.coord.x + coord.x, obj.coord.y + coord.y, obj.coord.z + coord.z);
								objClone.chunkId = chunkRudiment.chunkId;
								objClone.coordInChunkRudimentInfoForMiniPortalsArray = chunkRudiment.coord;
								randomPartInteractiveObjects.Add (objClone);
							}
						}
					}
				}
				randomizeRandomPartThreadCounts = randomizeRandomPartThreadCounts - 1;
				//flag.Set();
			}, new object[] {chunk, coord});
		}
		/***************************************************************************************************/
		if (createPortals) {
			//chunk.playerSpawn = new SerializableVector3 (8 + coord.x, -18 + coord.y, 0);
			foreach (InteractiveObjectRandomPart portal in chunk.portals) {
				GameObject newPortal = Instantiate ((GameObject)interactiveObjects["portal"]);

				PortalScript newPortalScript = newPortal.GetComponent<PortalScript> ();
				newPortalScript.portalId = portals.Count + 1;

				SerializableVector3 portalCoords = portal.coord;// newPortalInfo.portalCoord;
				newPortal.transform.position = new Vector3 (portalCoords.x + coord.x, portalCoords.y + coord.y, portalCoords.z + coord.z);
				newPortal.GetComponent<PortalScript> ().teleportButton = buttonsController.teleportButton;

				portals.Add (newPortalScript); 
			}
		}
		LevelMesh currentDecorMesh = Instantiate (levelMeshPrefab).GetComponent<LevelMesh>();
		currentDecorMesh.GetComponent<MeshRenderer> ().sharedMaterial.mainTexture = levelTexture;
		currentDecorMesh.transform.parent = levelEnvironment.transform;
		for (int i = 0; i < chunk.levelDecorBlocks.Count; i++) {
			if((currentDecorMesh.squareVerticesEndValue + chunk.levelDecorBlocks [i].squareVerticesSerializeble.Count) < 12000){
				currentDecorMesh.AddData (chunk.levelDecorBlocks [i], coord);
			}else{
				currentDecorMesh = Instantiate (levelMeshPrefab).GetComponent<LevelMesh> ();
				currentDecorMesh.AddData (chunk.levelDecorBlocks [i], coord);
			}
		}

		currentMesh = Instantiate (levelMeshPrefab).GetComponent<LevelMesh>();
		currentMesh.gameObject.layer = LayerMask.NameToLayer ("Ground");
		currentMesh.GetComponent<MeshCollider> ().material = (PhysicMaterial)Resources.Load ("HaveNotWallFriction");
		currentMesh.GetComponent<MeshRenderer> ().sharedMaterial.mainTexture = levelTexture;
		currentMesh.transform.parent = levelEnvironment.transform;
		for (int i = 0; i < chunk.levelBlocks.Count; i++) {
			if((currentMesh.squareVerticesEndValue + chunk.levelBlocks [i].squareVerticesSerializeble.Count) < 12000){
				currentMesh.AddData (chunk.levelBlocks [i], coord);
			}else{
				currentMesh = Instantiate (levelMeshPrefab).GetComponent<LevelMesh> ();
				currentMesh.AddData (chunk.levelBlocks [i], coord);
			}
		}



		/*********************************FixObjects***************************************/
		foreach(InteractiveObjectRandomPart obj in chunk.fixObjects){
			GameObject newObj = Instantiate ((GameObject)interactiveObjects[obj.type]);
			newObj.transform.position = new Vector3 (obj.coord.x + coord.x, obj.coord.y + coord.y, obj.coord.z + + coord.z);
			newObj.transform.parent = levelInteractiveObjects.transform;
			InteractiveObjectRandomPart objClone = new InteractiveObjectRandomPart ();
			objClone.SetData (obj);
			objClone.chunkId = chunkRudiment.chunkId;
			objClone.coordInChunkRudimentInfoForMiniPortalsArray = chunkRudiment.coord;
			CheckInteraciveObjectByMiniPortal (objClone, newObj);
		}
		/*************************************************************************/
	}


	public void CreateLevel(){
		ClearLevel ();
		GenLevel ();
	}

	void ClearLevel(){
		foreach (GameObject destrMesh in GameObject.FindGameObjectsWithTag("Mesh")){
			Destroy(destrMesh);
		}

		foreach (GameObject destr in GameObject.FindGameObjectsWithTag("Portal")){
			Destroy(destr);
		}

		portals.Clear ();
	}


	void SaveLevel(){
		Level savingLevel = new Level ();
		savingLevel.levelChunksInfo = levelChunksData;
		savingLevel.randomPartBlocks = randomPartBlocks;
		savingLevel.randomPartInteractiveObjects = randomPartInteractiveObjects;
		savingLevel.levelArrayWidth = level.GetLength (0);
		savingLevel.levelArrayHeight = level.Length / savingLevel.levelArrayWidth;
		//savingLevel.level = level;
		//save Portals
		foreach(PortalScript portal in portals){
			PortalInfo newPortalInfo = new PortalInfo ();
			newPortalInfo.SetData (portal);
			savingLevel.portalsInfo.Add (newPortalInfo);
		}
		currentLevel = SaveLoadManager.SaveLevel (savingLevel);
	}

	LevelChunkWithInfo LoadLevelChunk(int size, LevelChunkRudiment chunkRudiment, int number = -1){
		//string path = "jar:file://" + Application.dataPath + "!assets/Resources/LevelChunkPrefabs/" + type.ToString() + "/";
		int chunkNumber;
		if (number == -1) {
			chunkNumber = levelChunkFilesInfo.GetRandomChunkNumber(levelTextureName, levelChunkSize, chunkRudiment.type);
		} else {
			chunkNumber = number;
		}

		GameObject meshDataPrefab = (GameObject)Resources.Load ("LevelChunkPrefabs/" + levelTextureName  + "/" + size.ToString() + "/" + chunkRudiment.type.ToString() + "/" + chunkNumber.ToString());
		LevelChunkWithInfo chunk = new LevelChunkWithInfo();
		chunk.levelChunk = meshDataPrefab.GetComponent<LevelChunk> ();
		chunk.chunkSize = size;
		//chunk.chunkRudiment = chunkRudiment;
		chunk.chunkType = chunkRudiment.type;
		chunk.chunkNumber = chunkNumber;
		return chunk;
	}
				

	public void SetPortalsDirections(){
		List<Vector3> portalPositions = new List<Vector3> ();
		foreach(PortalScript portal in portals){
			portalPositions.Add (portal.gameObject.transform.position);
		}

		portalPositions = portalPositions.OrderBy (p => p.x).ToList();
		Vector3 leftPortal = portalPositions[0];
		Vector3 rightPortal = portalPositions[portalPositions.Count - 1];

		portalPositions = portalPositions.OrderBy (p => p.y).ToList();
		Vector3 upPortal =  portalPositions[portalPositions.Count - 1];
		Vector3 downPortal = portalPositions[0];


		Vector2 leftUpPointA = new Vector2 (leftPortal.x, upPortal.y);
		Vector2 rightUpPointB = new Vector2 (rightPortal.x, upPortal.y);
		Vector2 rightDownPointC = new Vector2 (rightPortal.x, downPortal.y);
		Vector2 leftDownPointD = new Vector2 (leftPortal.x, downPortal.y);

		Vector2 squareCenter = GameMath.IntersectionTwoLines (leftDownPointD, rightUpPointB,  leftUpPointA, rightDownPointC);



		List<PortalScript> leftPortals = new List<PortalScript> ();
		List<PortalScript> rightPortals = new List<PortalScript> ();

		/*************************/



		foreach (PortalScript portal in portals) {
			Vector2 portalPosition2d = new Vector2 (portal.gameObject.transform.position.x, portal.gameObject.transform.position.y);
			if (portalPosition2d.x <= squareCenter.x) {
				leftPortals.Add (portal);
			} else {
				rightPortals.Add (portal);
			}
		}


		if (leftPortals.Count >= 2 && rightPortals.Count >=2) {
			leftPortals = leftPortals.OrderBy (p => p.gameObject.transform.position.y).ToList ();
			leftPortals [0].direction = PortalScript.left;
			leftPortals [leftPortals.Count - 1].direction = PortalScript.up;

			rightPortals = rightPortals.OrderBy (p => p.gameObject.transform.position.y).ToList ();
			rightPortals [0].direction = PortalScript.down;
			rightPortals [rightPortals.Count - 1].direction = PortalScript.right;
		}


		if (leftPortals.Count < 2 && rightPortals.Count >=2) {
			leftPortals = leftPortals.OrderBy (p => p.gameObject.transform.position.y).ToList ();
			leftPortals [0].direction = PortalScript.left;

			rightPortals = rightPortals.OrderBy (p => p.gameObject.transform.position.y).ToList ();
			rightPortals [0].direction = PortalScript.down;

			List<PortalScript> rightCenterPortals = rightPortals.GetRange (1, rightPortals.Count - 2);
			int rand = Random.Range (0, rightCenterPortals.Count);
			rightCenterPortals [rand].direction = PortalScript.right;
			rightPortals [rightPortals.Count - 1].direction = PortalScript.up;
		}

		if (leftPortals.Count >= 2 && rightPortals.Count < 2) {
			rightPortals = rightPortals.OrderBy (p => p.gameObject.transform.position.y).ToList ();
			rightPortals [0].direction = PortalScript.right;



			leftPortals = leftPortals.OrderBy (p => p.gameObject.transform.position.y).ToList ();
			leftPortals [0].direction = PortalScript.down;

			List<PortalScript> leftCenterPortals = leftPortals.GetRange (1, leftPortals.Count - 2);
			int rand = Random.Range (0, leftCenterPortals.Count);
			leftCenterPortals [rand].direction = PortalScript.left;
			leftPortals [leftPortals.Count - 1].direction = PortalScript.up;
		}


		foreach(PortalScript portal in portals){
			if(portal.direction == -1){
				if (portal.gameObject.transform.position.x >= squareCenter.x) {
					if (portal.gameObject.transform.position.y >= squareCenter.y) {
						portal.direction = PortalScript.right;
					} else {
						portal.direction = PortalScript.down;
					}
				} else {
					if (portal.gameObject.transform.position.y >= squareCenter.y) {
						portal.direction = PortalScript.up;
					} else {
						portal.direction = PortalScript.left;
					}
				}
			}			
		}
	}


	void DrawLol(Vector3 coord, string text){
		GameObject mapZone = Instantiate ((GameObject)Resources.Load("PortalCheck"));
		mapZone.transform.position = new Vector3 (coord.x, coord.y, coord.z);
		GameObject textRenderer = mapZone.transform.GetChild (0).gameObject;
		textRenderer.GetComponent<TextMesh> ().text = text.ToString ();
	}

	public void SetLevelTexture(){
		levelTexture = (Texture)Resources.Load ("Textures/"+levelTextureName);
	}

	public void GenRandomPart(List<RandomPartBlock> blocks, List<InteractiveObjectRandomPart> objects){
		creatingRandomPartMeshesThread = new Thread (() => {newMeshesRandomPart = CreateRandomPartMeshes(blocks);});
		creatingRandomPartMeshesThread.Start ();
		StartCoroutine (StartAddDataProcessWhenMeshesCreated());


			
		foreach(InteractiveObjectRandomPart obj in objects){
			GameObject newObj = Instantiate ((GameObject)interactiveObjects[obj.type]);
			newObj.transform.position = new Vector3 (obj.coord.x, obj.coord.y,  obj.coord.z);
			newObj.transform.parent = levelInteractiveObjects.transform;
			CheckInteraciveObjectByMiniPortal (obj, newObj);
		}}

	List<LevelMesh.LevelMeshDataSerializable> CreateRandomPartMeshes(List<RandomPartBlock> blocks){
		List<LevelMesh.LevelMeshDataSerializable> newMeshes = new List<LevelMesh.LevelMeshDataSerializable> ();
		newMeshes.Add (new LevelMesh.LevelMeshDataSerializable ());
		//LevelMesh.LevelMeshDataSerializable newMeshData = new LevelMesh.LevelMeshDataSerializable ();
		foreach(RandomPartBlock block in blocks){
			if(newMeshes [newMeshes.Count - 1].squareVerticesSerializeble.Count >= 5000){
				newMeshes.Add (new LevelMesh.LevelMeshDataSerializable ());
			}
			newMeshes [newMeshes.Count - 1].squareVerticesSerializeble.Add (block.coord);
			newMeshes [newMeshes.Count - 1].squareVerticesSerializeble.Add (new SerializableVector3(block.coord.x + block.width, block.coord.y, 0));
			newMeshes [newMeshes.Count - 1].squareVerticesSerializeble.Add (new SerializableVector3(block.coord.x + block.width, block.coord.y - block.height, 0));
			newMeshes [newMeshes.Count - 1].squareVerticesSerializeble.Add (new SerializableVector3(block.coord.x, block.coord.y - block.height, 0));
			foreach (SerializableVector2 uv in block.uvs) {
				newMeshes [newMeshes.Count - 1].squareUVSerializeble.Add (uv);
			}
			if (!block.isDecor) {
				foreach(SerializableVector3 colVertice in LevelMesh.GetColliderCoords(block.coord)){
					newMeshes [newMeshes.Count - 1].colliderVerticesSerializeble.Add(colVertice);
				}
			}
		}
		return newMeshes;
	}

	IEnumerator StartAddDataProcessWhenMeshesCreated(){
		while (true) {
			if(!creatingRandomPartMeshesThread.IsAlive){
				LevelMesh newMesh = Instantiate(levelMeshPrefab).GetComponent<LevelMesh>();
				newMesh.GetComponent<MeshRenderer> ().sharedMaterial.mainTexture = levelTexture;
				foreach (LevelMesh.LevelMeshDataSerializable data in newMeshesRandomPart) {
					if((newMesh.squareVerticesEndValue + data.squareVerticesSerializeble.Count) < 5000){
						newMesh.AddData (data, new Vector3(0f, 0f, 0f));
					}else{
						newMesh = Instantiate (levelMeshPrefab).GetComponent<LevelMesh> ();
						newMesh.GetComponent<MeshRenderer> ().sharedMaterial.mainTexture = levelTexture;
						newMesh.AddData (data, new Vector3(0f, 0f, 0f));
					}
				}
				yield break;
			}
			yield return null;
		}
	}

	void WaitRandomizePartProcesses (){
		while (randomizeRandomPartThreadCounts != 0) {
		}
	}

	void CheckInteraciveObjectByMiniPortal(InteractiveObjectRandomPart obj, GameObject newObj){
		if(obj.type == "portalU"){
			MiniPortal miniPortalScript = newObj.GetComponent<MiniPortal> ();
			miniPortalScript.type = MiniPortal.up;
			miniPortalScript.chunkId = obj.chunkId;
			miniPortalScript.coordInChunkRudimentInfoForMiniPortalsArray = obj.coordInChunkRudimentInfoForMiniPortalsArray;
		}

		if(obj.type == "portalD"){
			MiniPortal miniPortalScript = newObj.GetComponent<MiniPortal> ();
			miniPortalScript.type = MiniPortal.down;
			miniPortalScript.chunkId = obj.chunkId;
			miniPortalScript.coordInChunkRudimentInfoForMiniPortalsArray = obj.coordInChunkRudimentInfoForMiniPortalsArray;
		}

		if(obj.type == "portalR"){
			MiniPortal miniPortalScript = newObj.GetComponent<MiniPortal> ();
			miniPortalScript.type = MiniPortal.right;
			miniPortalScript.chunkId = obj.chunkId;
			miniPortalScript.coordInChunkRudimentInfoForMiniPortalsArray = obj.coordInChunkRudimentInfoForMiniPortalsArray;
		}

		if(obj.type == "portalL"){
			MiniPortal miniPortalScript = newObj.GetComponent<MiniPortal> ();
			miniPortalScript.type = MiniPortal.left;
			miniPortalScript.chunkId = obj.chunkId;
			miniPortalScript.coordInChunkRudimentInfoForMiniPortalsArray = obj.coordInChunkRudimentInfoForMiniPortalsArray;
		}
	}

	void SetLevelChunkRudiments(int width, int height){
		level = new LevelChunkRudiment[height, width];
		int chunkId = 0;
		for(int i = 0; i < height; i++){
			for(int j = 0; j < width; j++){
				level [i, j] = new LevelChunkRudiment ();
				level [i, j].coord = new SerializableVector2 ((float)j, (float)i);
				level [i, j].chunkId = chunkId;
				chunkId = chunkId + 1;
			}	
		}
	}

	void SetLevelChunkRudimentsInfoForMiniPortals(){
		int width = level.GetLength (0);
		int height = level.Length / width;

		levelChunkRudimentsInfoForMiniPortals = new LevelChunkRudiment[height + 2, width + 2];
		for(int i = 0; i < width; i++){
			levelChunkRudimentsInfoForMiniPortals [0, i] = new LevelChunkRudiment ();
			levelChunkRudimentsInfoForMiniPortals [0, i].chunkId = -1;
		}


		for(int i =1; i< height + 1; i++){
			levelChunkRudimentsInfoForMiniPortals [i, 0] = new LevelChunkRudiment ();
			levelChunkRudimentsInfoForMiniPortals [i, 0].chunkId = -1;
			for(int j =1; j< width + 1; j++){
				levelChunkRudimentsInfoForMiniPortals [i, j] = level [i - 1, j - 1];
			}
			levelChunkRudimentsInfoForMiniPortals [i, width + 1] = new LevelChunkRudiment ();
			levelChunkRudimentsInfoForMiniPortals [i, width + 1].chunkId = -1;
		}



		for(int i = 0; i < width; i++){
			levelChunkRudimentsInfoForMiniPortals [height+1, i] = new LevelChunkRudiment ();
			levelChunkRudimentsInfoForMiniPortals [height+1, i].chunkId = -1;
		}


	}











	private	class LevelChunkWithInfo{
		public LevelChunk levelChunk;
		public int chunkSize;
		//public LevelChunkRudiment chunkRudiment;
		public int chunkType;
		public int chunkNumber;

		public LevelChunkInfo GetChunkInfo(Vector3 coord){
			LevelChunkInfo chunkInfo = new LevelChunkInfo ();
			chunkInfo.chunkSize = chunkSize;
			chunkInfo.chunkNumber = chunkNumber;
			chunkInfo.chunkType = chunkType;
			chunkInfo.coord = new SerializableVector3 (coord.x, coord.y, coord.z);
			return chunkInfo;
		}
	} 
}
