using UnityEngine;
using System.Collections;

public class HitboxDamagerOptions : MonoBehaviour {
	public GameObject hitBox;
	public SpellHitbox spellHitbox;
	public float damagePercent;
	public int damageType;
	public string path;
	public bool withStun = false;
	public float stunTime = 0;
	public float efficienty = 100f;
}
