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
			if (other.gameObject.GetComponent<EnemyProperties>().IsVulnerableTo(SpellType))
			{
				other.gameObject.GetComponent<EnemyLogic>().Damage(DamageAmount);
			}
		}
	}


}
