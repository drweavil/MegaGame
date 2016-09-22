using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ObjectsPool : MonoBehaviour {
	static Dictionary<string, List<object>> objects = new Dictionary<string, List<object>>();


	public static void PushObject(string path, GameObject pushedObject){
		pushedObject.SetActive (false);

		List<object> objectsList = new List<object> ();
		if (objects.TryGetValue (path, out objectsList)) {
			objectsList.Add (pushedObject);
		} else {
			List<object> newObjectList = new List<object>();
			newObjectList.Add (pushedObject);
			objects.Add (path, newObjectList);
		}
	}

	public static GameObject PullObject(string path){
		List<object> foundObjectsList = new List<object> ();
		GameObject foundObject;
		if (objects.TryGetValue (path, out foundObjectsList)) {
			if (foundObjectsList.Count == 0) {
				foundObject = Instantiate((GameObject)Resources.Load(path));
				foundObject.SetActive (true);
			} else {
				foundObject = (GameObject)foundObjectsList [0];
				foundObject.SetActive (true);
				foundObjectsList.RemoveAt (0);
			}
		} else {
			foundObject = Instantiate((GameObject)Resources.Load(path));
			foundObject.SetActive (true);
		}
		return foundObject;
	}
}
