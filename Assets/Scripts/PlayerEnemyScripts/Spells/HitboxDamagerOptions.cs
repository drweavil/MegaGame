using UnityEngine;
using System.Collections;

[System.Serializable]
public class HitboxDamagerOptions {
	public GameObject hitBox;
	public SpellHitbox spellHitbox;
	public float damagePercent;
	public int damageType;
	public string path;
	public bool withStun = false;
	public float stunTime = 0;
	public float efficienty = 100f;
	public int skillID = -1;

	public SpellHitbox.ObjectsAction action;
}
