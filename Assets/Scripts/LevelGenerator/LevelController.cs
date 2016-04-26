using UnityEngine;
//using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class LevelController : MonoBehaviour {
	LevelGeneration generator;
	GameObject player;
	public int currentLevel;
	//int nextLevel = currentLevel + 1;
	int test = 0;
	LevelMesh currentMesh;
	public List<PortalScript> portals = new List<PortalScript> ();
	//info 
	static List<LevelChunkInfo> levelChunksData = new List<LevelChunkInfo>(); 

	PortalInfo portalOnOldLevel;
	PortalInfo portalOnNewLevel;

	int[,] level;
	public GameObject levelEnvironment;
	public ButtonsController buttonsController;


	LevelChunkFilesInfo levelChunkFilesInfo;


	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Player");
		generator = GameObject.Find ("LevelController").GetComponent<LevelGeneration>();
		buttonsController = GameObject.Find ("ButtonsController").GetComponent<ButtonsController>();
		//level = generator.CreateLevelPath (10, 10);

		levelChunkFilesInfo = SaveLoadManager.LoadLevelChunkFilesInfo();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Q)){
			//Profiler.BeginSample ("lol");
			//SaveLoadManager.Load (Application.dataPath.ToString() +"/Resources/Levels/levels_2/"+ 0.ToString() +".level");
			//GameObject mesh = Instantiate ((GameObject)Resources.Load ("LevelChunkPrefabs/-1"));
			//Destroy (mesh);
			//GenLevel ();
			//LoadLevelChunk(0);
			//SaveLoadManager.SaveLevel();
			//Profiler.EndSample ();
			/*for (int i = 0; i < 16; i++) {
				LevelToPrefab (i);
			}*/
			Debug.Log (Application.streamingAssetsPath);
			//LoadLevel (16);

		}
	
	}

	public void Test(){
		SaveLoadManager.SetLevelChunkFilesInfo ();
		//Debug.Log(levelChunkFilesInfo.types[0].numbers);
		//SaveLoadManager.CreateDirectoryIfNotExist(Application.dataPath + "/olol/kold/adf");
		/*Debug.Log(Application.persistentDataPath);
		Directory.CreateDirectory (Application.persistentDataPath + "/zuz");

		Debug.Log (Directory.Exists (Application.persistentDataPath + "/zuz"));*/

		/*string[] directory = Directory.GetFiles (Application.persistentDataPath + "/Resources/Levels");
		foreach(string dir in directory){
			Debug.Log (dir);
		}*/
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
			//loadedLevel.portalsInfo.Find (p => p.portalId == portalOnOldLevel.linkWithPortalId);
			StartCoroutine(LevelMesh.CoroutineAddDataProcess.PlayerSpawnWhenAllProcessStoped (player, playerSpawn));
		}
		//portalOnNewLevel = new PortalInfo().SetData(portal);
	}

	void LoadLevel(int level){
		ClearLevel ();
		portals.Clear ();
		levelChunksData.Clear();

		Level loadedLevel =  SaveLoadManager.LoadLevel (level);
		foreach (LevelChunkInfo chunk in loadedLevel.levelChunksInfo) {
			CreatePart (new Vector3 (chunk.coord.x, chunk.coord.y, chunk.coord.z), 
						chunk.chunkType, 
						chunk.chunkNumber, false); 
		}



		//LoadPortals
		foreach(PortalInfo portalInfo in loadedLevel.portalsInfo){
			GameObject newPortal = Instantiate ((GameObject)Resources.Load ("Prefabs/portal"));
			PortalScript newPortalScript = newPortal.GetComponent<PortalScript> ();
			newPortalScript.SetData (portalInfo);
			portals.Add (newPortalScript);
			//newPortal.gameObject.transform.position = new Vector3(portal.portalCoord.x, portal.portalCoord.y, portal.portalCoord.z)
		}
		//return loadedLevel;
	}

	public void GenLevel(){
		portals.Clear ();
		level = generator.CreateLevelPath (20, 20);
		levelChunksData.Clear ();

		//test
		//level = new int[1,3] {{13,1,12}};
		for (int i = 0; i < generator.height+2; i++) {
			for (int j = 0; j < generator.width+2; j++) {
				if (level [i, j] != -3) {
					CreatePart (new Vector3 (j * 20, i * 20 * -1, 0), level [i, j]);
					/*CreatePart (new Vector3 (0, 0, 0), level [i, j]);*/
				}
			}
		}
		PortalScript portal = GetRandomFromList (portals);
		Vector3 portalCoord = portal.gameObject.transform.position;
		portalOnNewLevel = new PortalInfo();
		portalOnNewLevel.SetData (portal);
		SaveLevel ();
		//GenPortals ();
		StartCoroutine(LevelMesh.CoroutineAddDataProcess.PlayerSpawnWhenAllProcessStoped (player, portalCoord));
	}

	/*void GenPortals(){
		int teleportId = 0;
		foreach (PortalInfo portal in portals) {
			GameObject newPortal =  Instantiate ((GameObject)Resources.Load ("Prefabs/portal"));
			SerializableVector3 portalCoords = portal.portalCoord;
			newPortal.transform.position = new Vector3 (portalCoords.x, portalCoords.y, portalCoords.z);
			newPortal.GetComponent<PortalScript> ().teleportButton = buttonsController.teleportButton;
		}
	}*/

	PortalScript GetRandomFromList(List<PortalScript> list){
		int rand = Random.Range (0, portals.Count);
		return list [rand];
	}
		

	void CreatePart(Vector3 coord, int chunkType, int chunkNumber = -1, bool createPortals = true){
		LevelChunkWithInfo chunkWithInfo = LoadLevelChunk(chunkType, chunkNumber);
		levelChunksData.Add (chunkWithInfo.GetChunkInfo(coord));
		LevelChunk meshData = chunkWithInfo.levelChunk;

		if ((chunkType == 12 || chunkType == 13 || chunkType == 14 || chunkType == 15) && createPortals) {
			meshData.playerSpawn = new SerializableVector3 (8 + coord.x, -18 + coord.y, 0);
			PortalInfo newPortalInfo = new PortalInfo ();
			newPortalInfo.portalId = portals.Count + 1;
			newPortalInfo.portalCoord = meshData.playerSpawn;


			GameObject newPortal =  Instantiate ((GameObject)Resources.Load ("Prefabs/portal"));

			PortalScript newPortalScript =  newPortal.GetComponent<PortalScript>();
			newPortalScript.portalId = portals.Count + 1;

			SerializableVector3 portalCoords = newPortalInfo.portalCoord;
			newPortal.transform.position = new Vector3 (portalCoords.x, portalCoords.y, portalCoords.z);
			newPortal.GetComponent<PortalScript> ().teleportButton = buttonsController.teleportButton;

			portals.Add (newPortalScript); 
			//portalsData.Add(newPortalInfo)
		}
		currentMesh = Instantiate ((GameObject)Resources.Load ("LevelMesh")).GetComponent<LevelMesh>();
		currentMesh.gameObject.layer = LayerMask.NameToLayer ("Ground");
		currentMesh.GetComponent<MeshCollider> ().material = (PhysicMaterial)Resources.Load ("HaveNotWallFriction");
		currentMesh.transform.parent = levelEnvironment.transform;
		for (int i = 0; i < meshData.levelBlocks.Count; i++) {
			currentMesh.AddData (meshData.levelBlocks [i], coord);
		}
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
		//save Portals
		foreach(PortalScript portal in portals){
			PortalInfo newPortalInfo = new PortalInfo ();
			newPortalInfo.SetData (portal);
			savingLevel.portalsInfo.Add (newPortalInfo);
		}
		currentLevel = SaveLoadManager.SaveLevel (savingLevel);
	}

	LevelChunkWithInfo LoadLevelChunk(int type, int number = -1){
		//string path = "jar:file://" + Application.dataPath + "!assets/Resources/LevelChunkPrefabs/" + type.ToString() + "/";
		int chunkNumber;
		if (number == -1) {
			/*DirectoryInfo directory = new DirectoryInfo (path);
			FileInfo[] info = directory.GetFiles ("*.prefab");
			int rand = Random.Range (0, info.Length);
			string[] fileName = info [rand].Name.Split ('.');
			chunkNumber = int.Parse(fileName [0]);*/
			chunkNumber = levelChunkFilesInfo.GetRandomChunkNumberByType (type);
		} else {
			chunkNumber = number;
		}

		//int chunkNumber = 0;

		GameObject meshDataPrefab = (GameObject)Resources.Load ("LevelChunkPrefabs/" + type.ToString() + "/" + chunkNumber.ToString());
		LevelChunkWithInfo chunk = new LevelChunkWithInfo();
		chunk.levelChunk = meshDataPrefab.GetComponent<LevelChunk> ();
		chunk.chunkType = type;
		chunk.chunkNumber = chunkNumber;
		return chunk;
	}


	private	class LevelChunkWithInfo{
		public LevelChunk levelChunk;
		public int chunkType;
		public int chunkNumber;

		public LevelChunkInfo GetChunkInfo(Vector3 coord){
			LevelChunkInfo chunkInfo = new LevelChunkInfo ();
			chunkInfo.chunkNumber = chunkNumber;
			chunkInfo.chunkType = chunkType;
			chunkInfo.coord = new SerializableVector3 (coord.x, coord.y, coord.z);
			return chunkInfo;
		}
	} 
				

	/*void LevelToPrefab(int name){
		GameObject chunk = Instantiate ((GameObject)Resources.Load ("LevelChunk"));
		Level data = SaveLoadManager.Load (Application.dataPath.ToString () + "/Resources/Levels/levels_2/" + name.ToString () + ".level");
		chunk.GetComponent<LevelChunk> ().SetData (data);
		GameObject prefab = PrefabUtility.CreatePrefab("Assets/Resources/LevelChunkPrefabs/" + name.ToString() + ".prefab", chunk);
		Destroy (chunk);
	}*/
}
