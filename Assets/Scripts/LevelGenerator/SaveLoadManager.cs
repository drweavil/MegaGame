using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Linq;

public static class SaveLoadManager {

	public static int SaveLevel(Level level){
		CreateDirectoryIfNotExist (Application.persistentDataPath + "/Resources/Levels");
		List<int> levelNames = new List<int> ();
		DirectoryInfo directory = new DirectoryInfo (Application.persistentDataPath + "/Resources/Levels");
		FileInfo[] directoryFiles = directory.GetFiles ("*.level");

		foreach (FileInfo f in directoryFiles) {
			string[] fileName = f.Name.Split ('.');
			levelNames.Add (int.Parse(fileName[0]));
			//Debug.Log (fileName[0]);
		}
		//Debug.Log (Mathf.Max(levelNames.ToArray()));
		int maxLevel = Mathf.Max(levelNames.ToArray());
		level.levelNumber = maxLevel + 1;

		FileStream file = File.Create(Application.persistentDataPath + "/Resources/Levels/" + (maxLevel+1).ToString() + ".level");
		BinaryFormatter bf = new BinaryFormatter ();
		bf.Serialize (file, level);
		file.Close ();
		return maxLevel + 1;
	}

	public static void SetLinkWithOldPortal(PortalInfo newPortalInfo, PortalInfo oldPortalInfo, int currentLevel){
		Level oldLevel = LoadLevel (currentLevel - 1);
		PortalInfo oldPortal = oldLevel.portalsInfo.Find (p => p.portalId == oldPortalInfo.portalId);
		oldPortal.linkWithLevel = currentLevel;
		oldPortal.linkWithPortalId = newPortalInfo.portalId;

		Level newLevel = LoadLevel (currentLevel);
		PortalInfo newPortal = newLevel.portalsInfo.Find (p => p.portalId == newPortalInfo.portalId);
		newPortal.linkWithLevel = currentLevel-1;
		newPortal.linkWithPortalId = oldPortalInfo.portalId;


		BinaryFormatter bf = new BinaryFormatter ();

		//old
		string pathOld = Application.persistentDataPath + "/Resources/Levels/" + (currentLevel - 1).ToString() + ".level";
		FileStream fileOld = File.Open (pathOld, FileMode.Open);
		bf.Serialize (fileOld, oldLevel);
		fileOld.Close ();

		//new
		string pathNew = Application.persistentDataPath + "/Resources/Levels/" + currentLevel.ToString() + ".level";
		FileStream fileNew = File.Open (pathNew, FileMode.Open);
		bf.Serialize (fileNew, newLevel);
		fileNew.Close ();
	}



	public static Level LoadLevel(int level){
		FileStream file;
		Level levelLoaded = new Level();
		string path = Application.persistentDataPath + "/Resources/Levels/" + level.ToString() + ".level";
		BinaryFormatter bf = new BinaryFormatter ();
		if (File.Exists (path)) {
			//meshData.levelBlocks.Clear ();
			file = File.Open (path, FileMode.Open);
			levelLoaded = (Level)bf.Deserialize (file);
			file.Close ();

			//BlockController.bc.CreateMeshesByData (meshData);

		}
		return levelLoaded;
	}

	public static void SetLevelChunkFilesInfo(){
		BinaryFormatter bf = new BinaryFormatter ();
		LevelChunkFilesInfo chunkFilesInfo = new LevelChunkFilesInfo ();

		string[] directoryes = Directory.GetDirectories (Application.dataPath + "/Resources/LevelChunkPrefabs/");


		foreach (string dir in directoryes) {
			string[] dirPath = dir.Split ('/');
			string dirName = dirPath [dirPath.Length - 1];

			LevelChunkFilesInfo.ChunkTypeFileInfo typesFileInfo = new LevelChunkFilesInfo.ChunkTypeFileInfo ();
			typesFileInfo.typeId = int.Parse (dirName);

			string path = Application.dataPath + "/Resources/LevelChunkPrefabs/" + dirName + "/";
			DirectoryInfo directory = new DirectoryInfo (path);
			FileInfo[] info = directory.GetFiles ("*.prefab");

			foreach(FileInfo inf in info){
				string chunkName = inf.Name.Split ('.')[0];
				//Debug.Log (int.Parse(chunkName));
				typesFileInfo.numbers.Add(int.Parse(chunkName));
			}
			chunkFilesInfo.types.Add(typesFileInfo);
		}

		FileStream file = File.Create(Application.dataPath + "/Resources/LevelChunkFilesInfo.txt");
		bf.Serialize (file, chunkFilesInfo);
		file.Close ();
	}

	public static LevelChunkFilesInfo  LoadLevelChunkFilesInfo(){
		BinaryFormatter bf = new BinaryFormatter ();
		TextAsset levelChunkFilesInfoAsset = (TextAsset)Resources.Load ("LevelChunkFilesInfo");
		Stream levelChunkFilesInfoStream = new MemoryStream (levelChunkFilesInfoAsset.bytes);
		return (LevelChunkFilesInfo)bf.Deserialize (levelChunkFilesInfoStream);
	}



	public static void CreateDirectoryIfNotExist (string path){
		if (!Directory.Exists (path)) {
			Directory.CreateDirectory (path);
		}
	}
}
