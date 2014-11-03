using UnityEngine;
using System.Collections;

public class SpellDamage : MonoBehaviour {

	public Elements SpellType;
	public float DamageAmount;

	// Use this for initialization
	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Enemy")
		{
			other.gameObject.GetComponent<EnemyLogic>().Damage(DamageAmount, SpellType);
		}
	}


}
