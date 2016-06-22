using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class FullSet{
	public int randomValue;
	public List<RandomPartBlock> blocks = new List<RandomPartBlock>();
	public List<InteractiveObjectRandomPart> interactiveObjects = new List<InteractiveObjectRandomPart>();
}
