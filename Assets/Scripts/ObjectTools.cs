using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;

public class ObjectCloneTool : MonoBehaviour {

	/*public static object CopyObject(object obj){
		System.Type type = obj.GetType ();
		List<ParameterInfo> objParams = type.GetProperties ();



		object newObj = Activator.CreateInstance (type);


	}*/

	public static object CloneObject(object obj){
		ObjectClone oldObj = new ObjectClone ();
		oldObj.obj = obj;
		ObjectClone newObj = (ObjectClone)oldObj.Clone ();
		return newObj.obj;
	}

	public class ObjectClone : ICloneable{
		public object obj;

		public object Clone(){
			return this.MemberwiseClone ();
		}
	}
}
