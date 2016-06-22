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

		string root = Application.dataPath + "/Resources/LevelChunkPrefabs";
		DirectoryInfo directory = new DirectoryInfo (root);
		DirectoryInfo[] directories = directory.GetDirectories();

		foreach(DirectoryInfo textureDir in directories){
			DirectoryInfo[] sizeDirs = new DirectoryInfo (root + "/" + textureDir.Name).GetDirectories();
			foreach(DirectoryInfo sizeDir in sizeDirs){
				DirectoryInfo[] typeDirs = new DirectoryInfo (root + "/" + textureDir.Name + "/" + sizeDir.Name).GetDirectories();
				foreach(DirectoryInfo typeDir in typeDirs){
					foreach(FileInfo number in typeDir.GetFiles()){
						if (number.Name.Split ('.') [1] == "prefab" && number.Name.Split ('.').Length == 2) {
							LevelChunkFilesInfo.ChunkTypeFileInfo type = new LevelChunkFilesInfo.ChunkTypeFileInfo ();
							type.texture = textureDir.Name;
							type.size = int.Parse (sizeDir.Name);
							type.type = int.Parse (typeDir.Name);
							type.number = int.Parse (number.Name.Split ('.') [0]);
							chunkFilesInfo.types.Add (type);
						}
					}
				}
			}
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
