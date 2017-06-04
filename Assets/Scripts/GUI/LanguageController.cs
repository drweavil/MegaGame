using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;

public class LanguageController : MonoBehaviour {
	public static LanguageController languageController;
	public Text inventoryMenuButton;
	public Text talentsMenuButton;
	public Text skillsMenuButton;

	public static JSONNode jsonFile;


	void Awake(){
		languageController = this;
		StartCoroutine (StartProcess.StartActionAfterFewFrames(15, () =>{
			if(GameSettings.language == ""){
				if(Application.systemLanguage == SystemLanguage.Russian || 
					Application.systemLanguage == SystemLanguage.Ukrainian || 
					Application.systemLanguage == SystemLanguage.Belarusian
				){
					SetRussian();
				} else{
					SetEnglish();
				}
			}else{
				TextAsset jsonAsset = (TextAsset)Resources.Load("Text/" + GameSettings.language + ".json");
				string jsonString = jsonAsset.text;
				jsonFile = JSON.Parse(jsonString);
			}
		}));
	}

	public static void RenameAll(){
		languageController.inventoryMenuButton.text = jsonFile ["menu"] ["inventoryMenuButton"] ["text"];
		languageController.inventoryMenuButton.fontSize = jsonFile ["menu"] ["inventoryMenuButton"] ["size"];
	}

	public static void SetRussian(){
		GameSettings.language = "ru";
		TextAsset jsonAsset = (TextAsset)Resources.Load ("Text/ru");
		string jsonString = jsonAsset.text;
		jsonFile = JSON.Parse(jsonString);
		RenameAll ();
	}

	public static void SetEnglish(){
		GameSettings.language = "en";
		TextAsset jsonAsset = (TextAsset)Resources.Load ("Text/en.txt");
		string jsonString = jsonAsset.text;
		jsonFile = JSON.Parse(jsonString);
		RenameAll ();
	}
}
