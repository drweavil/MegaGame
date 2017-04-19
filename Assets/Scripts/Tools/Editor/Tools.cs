using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;

public class Tools : MonoBehaviour {


	public static void RenameFiles(string directory, string newDirectory, string fileName){
		//string directory = "C://megaGameWorkDirectory/icons/smot/head/";
		//string renamedDirectory = "C://megaGameWorkDirectory/icons/smot/renamed/head/";
		string[] strs = Directory.GetFiles (directory);
		System.Array.Sort(strs, new Tools.WindowsSortComparer());
		List<FileInfo> files = new List<FileInfo> ();
		foreach(string str in strs){
			FileInfo inf = new FileInfo (str);
			if (inf.Name != "Thumbs.db") {
				files.Add (inf);
			}
		}
		for (int i = 0; i < files.Count; i++) {
			files [i].MoveTo (newDirectory + fileName + (i + 1).ToString() + ".png"); 
		}
	}




	public static void RenameSprites(ref List<Sprite> sprites, string directory, string fileName){
		//string directory = "C://megaGameWorkDirectory/icons/smot/head/";
		//string renamedDirectory = "C://megaGameWorkDirectory/icons/smot/renamed/head/";
		string[] strs = Directory.GetFiles (directory);
		System.Array.Sort(strs, new Tools.WindowsSortComparer());
		List<FileInfo> files = new List<FileInfo> ();
		foreach(string str in strs){
			FileInfo inf = new FileInfo (str);
			if (inf.Name != "Thumbs.db") {
				files.Add (inf);
			}
		}
		List<string> notFound = new List<string> ();
		for (int i = 0; i < files.Count; i++) {
			string[] nameA = files [files.Count - 1 - i].Name.Split ('.');

			int ind = sprites.FindIndex (s => s.name == nameA[0]);
			if (ind != -1) {
				
				sprites [ind].name = fileName + (files.Count - i).ToString ();
			} else {
				notFound.Add (files[files.Count - 1 - i].Name);
			}
			//files [i].MoveTo (newDirectory + fileName + (i + 1).ToString() + ".png"); 
		}

		foreach(string str in notFound){
			Debug.Log (str + " Not Found");
		}
		AssetDatabase.ImportAsset (AssetDatabase.GetAssetPath (sprites[0].texture), ImportAssetOptions.ForceUpdate);
	}


	public static void RenameArmorSprites(string resourcePath, string directory, string oldName, string newName){
		//string directory = "C://megaGameWorkDirectory/icons/smot/head/";
		//string renamedDirectory = "C://megaGameWorkDirectory/icons/smot/renamed/head/";
		string[] strs = Directory.GetFiles (directory);
		System.Array.Sort(strs, new Tools.WindowsSortComparer());
		List<FileInfo> files = new List<FileInfo> ();
		foreach(string str in strs){
			FileInfo inf = new FileInfo (str);
			if (inf.Name != "Thumbs.db") {
				files.Add (inf);
			}
		}

		TextureImporter textureImporter = AssetImporter.GetAtPath (AssetDatabase.GetAssetPath((Texture2D)Resources.Load(resourcePath))) as TextureImporter;
		//Debug.Log (sprites.FindIndex(h => h.name == "head_1"));
		EditorUtility.SetDirty (textureImporter);
		List<SpriteMetaData> newSpriteSheet = new List<SpriteMetaData>(textureImporter.spritesheet);
		List<string> notFound = new List<string> ();
		for (int i = 0; i < files.Count; i++) {
			string finalName = "";
			string[] nameA = files [files.Count - 1 - i].Name.Split ('.');

			string[] nameB = nameA [0].Split ('-');
			if (nameB.Length == 3) {
				finalName = "s" + nameB [1] + "-" +  nameB [2] + oldName;
			} else if (nameB.Length == 2) {
				finalName = nameB [1] + oldName;
			}
			//Debug.Log (finalName);
			int ind = newSpriteSheet.FindIndex (s => s.name == finalName);
			if (ind != -1) {
				//Debug.Log (finalName);
				SpriteMetaData newSpriteData = newSpriteSheet [ind];
				newSpriteData.name = newName + (files.Count - i).ToString ();
				newSpriteSheet [ind] = newSpriteData;
			} else {
				notFound.Add (files[files.Count - 1 - i].Name);
			}
			//Debug.Log (finalName);
			//files [i].MoveTo (newDirectory + fileName + (i + 1).ToString() + ".png"); 
		}

		foreach(string str in notFound){
			//Debug.Log (str + " Not Found");
		}

		textureImporter.spritesheet = newSpriteSheet.ToArray();
		AssetDatabase.SaveAssets ();
		AssetDatabase.ImportAsset (AssetDatabase.GetAssetPath((Texture2D)Resources.Load(resourcePath)), ImportAssetOptions.ForceUpdate);
		Debug.Log ("Rename "+oldName+" Done");
	}

	public static void RenameSprite(string resourcePath, string oldName, string newName){
		TextureImporter textureImporter = AssetImporter.GetAtPath (AssetDatabase.GetAssetPath((Texture2D)Resources.Load(resourcePath))) as TextureImporter;
		EditorUtility.SetDirty (textureImporter);
		List<SpriteMetaData> newSpriteSheet = new List<SpriteMetaData>(textureImporter.spritesheet);
		int ind = newSpriteSheet.FindIndex(s => s.name == oldName);
		if(ind != -1){
			SpriteMetaData newSpriteData = newSpriteSheet[ind];
			newSpriteData.name = newName;
			newSpriteSheet [ind] = newSpriteData;
		}else{
			Debug.Log(oldName + " Not Found");
		}
		textureImporter.spritesheet = newSpriteSheet.ToArray ();
		AssetDatabase.SaveAssets ();
		AssetDatabase.ImportAsset (AssetDatabase.GetAssetPath((Texture2D)Resources.Load(resourcePath)), ImportAssetOptions.ForceUpdate);
	}


	public static void SaveSprites(List<Sprite> sprites, string path){
		
		foreach (Sprite spr in sprites) {
			//Debug.Log (spr.texture.width);
			Texture2D newTexture = new Texture2D((int)spr.textureRect.width, (int)spr.textureRect.height);
			Color[] pixels = spr.texture.GetPixels (
				(int)spr.textureRect.x, 
				(int)spr.textureRect.y, 
				(int)spr.textureRect.width, 
				(int)spr.textureRect.height
			);
			newTexture.SetPixels (pixels);


			byte[] texture = newTexture.EncodeToPNG();
			FileStream file = File.Open (path + "/" + spr.name + ".png", FileMode.Create);
			BinaryWriter binaryWriter = new BinaryWriter (file);
			binaryWriter.Write (texture);
			file.Close ();
		}


	}

	/*public void OnPreprocessTexture(){
		SetPivots (assetPath, "C://megaGameWorkDirectory/weaponSprites/pivots.dct");
	}*/
	public static void SavePivots(List<Sprite> sprites, string path){
		Dictionary<string, SerializableVector2> pivots = new Dictionary<string, SerializableVector2> ();
		BinaryFormatter bf = new BinaryFormatter ();
		foreach (Sprite spr in sprites) {
			pivots.Add (spr.name, new SerializableVector2 (
				- spr.bounds.center.x / spr.bounds.extents.x /  2 + 0.5f , 
				- spr.bounds.center.y / spr.bounds.extents.y /  2 + 0.5f
			));
			Debug.Log (spr.name + ": " + pivots[spr.name].x.ToString() + " " + pivots[spr.name].y.ToString());
		}
		FileStream fs = File.Open (path + "/pivots.dct",  FileMode.Create);
		bf.Serialize (fs, pivots);
		fs.Close ();
	}

		
	public static void SetPivots(string texturePath, string pivotsPath){
		//foreach (Sprite spr in sprites) {
		FileStream fs = File.Open(pivotsPath, FileMode.Open);
		BinaryFormatter bf = new BinaryFormatter ();

		Dictionary<string, SerializableVector2> pivots = (Dictionary<string, SerializableVector2>)bf.Deserialize (fs);
		//TextureImporterSettings sett = new TextureImporterSettings ();
		//sett.spriteAlignment = (int)SpriteAlignment.Custom;
		TextureImporter textureImporter = AssetImporter.GetAtPath (AssetDatabase.GetAssetPath((Texture2D)Resources.Load(texturePath))) as TextureImporter;
		//Debug.Log(textureImporter.spritesheet.Length);
		//textureImporter.SetTextureSettings(sett);
		EditorUtility.SetDirty (textureImporter);
		List<string> notFoundPivots = new List<string>();
		//Debug.Log (pivots.Count);
		List<SpriteMetaData> newSpritesheet = new List<SpriteMetaData>(textureImporter.spritesheet);
		foreach(KeyValuePair<string, SerializableVector2> pivot in pivots){
			//notFoundPivots.Add (pivot.Key);
			//Debug.Log (pivot.Value.x.ToString() + " " + pivot.Value.y.ToString());
			//Debug.Log("lol");
			int spriteIND = newSpritesheet.FindIndex(p => p.name == pivot.Key);
			if (spriteIND != -1) {
				
				SpriteMetaData lol = newSpritesheet [spriteIND];
				lol.alignment = (int)SpriteAlignment.Custom;
				lol.pivot = new Vector2 (pivot.Value.x, pivot.Value.y);
				newSpritesheet [spriteIND] = lol;
				//Debug.Log (newSpritesheet[spriteIND].name + ":" + newSpritesheet[spriteIND].pivot.ToString());
			} else {
				notFoundPivots.Add (pivot.Key);
			}
		}
			
		//List<SpriteMetaData> newSpritesheet = new List<SpriteMetaData> ();
		/*foreach (SpriteMetaData spr in textureImporter.spritesheet) {
			//SpriteMetaData newSpr = spr;
			if (pivots.ContainsKey (spr.name)) {
				notFoundPivots.Remove (spr.name);

				//Debug.Log (newSpr.name +" "+ spr.name);
				newSpr.pivot = new Vector2 (pivots[spr.name].x, pivots[spr.name].y);
			}

			newSpritesheet.Add (newSpr);
		}*/

		SpriteMetaData[] ziz = newSpritesheet.ToArray ();
		//Debug.Log (ziz.Length);

		foreach (SpriteMetaData ad in ziz) {
			Debug.Log (ad.name + ": " + ad.pivot.ToString());
		}

		textureImporter.spritesheet = ziz;


		foreach (string pvt in notFoundPivots) {
			Debug.Log (pvt);
		}

		//AssetDatabase.WriteImportSettingsIfDirty (AssetDatabase.GetAssetPath ((Texture2D)Resources.Load (texturePath)));
		//AssetDatabase.Refresh();
		//TextureImporter newTe = textureImporter;
		string path = AssetDatabase.GetAssetPath ((Texture2D)Resources.Load (texturePath));
		//AssetDatabase.DeleteAsset(path);
		//AssetDatabase.CreateAsset(newTe, path);
		AssetDatabase.SaveAssets ();
		//AssetDatabase.ImportAsset (AssetDatabase.GetAssetPath ((Texture2D)Resources.Load (texturePath)));
		AssetDatabase.ImportAsset (AssetDatabase.GetAssetPath ((Texture2D)Resources.Load (texturePath)), ImportAssetOptions.ForceUpdate);
		//}
		//Debug.Log(AssetDatabase.GetAssetPath ((Texture2D)Resources.Load (texturePath)));


	}



	public class WindowsSortComparer : IComparer<string>{
		[DllImport("shlwapi.dll", CharSet=CharSet.Unicode, ExactSpelling=true)]
		static extern int StrCmpLogicalW (string x, string y);

		public int Compare(string x, string y){
			return StrCmpLogicalW (x, y);
		}
	}
}
