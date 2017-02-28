using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillSettings{
	public int skillID = 0;
	public Vector3 skillRectPosition;
	public float skillRectX = 0;
	public float skillRectY = 0;
	public float cooldown = 0;
	public float globalCooldown = 0;
	public int resourceRemove = 0;
	public int resourceAdd = 0;
	public float distance = 0;
	public float damagePercent = 0;
	public float slowTime = 0;
	public float slowSpeed = 0;
	public float stunTime = 0;
	public float restorePercent = 0;
	public float existingTime = 0;

}
