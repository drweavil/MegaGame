using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

//[System.Serializable]
public class ComplexityChunk {
	public int zoneChunkId;
	public int complexity = -1;
	//public int branchId;
	public bool inBranch = false;
	public SerializableVector2 coordInMapArray;

	public List<ComplexityBranch> branches = new List<ComplexityBranch> ();
}
