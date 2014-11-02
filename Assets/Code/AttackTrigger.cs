using UnityEngine;
using System.Collections;

public class AttackTrigger : MonoBehaviour {

	void OnTriggerEnter(Collider other)
	{
		GameObject Enemy = other.transform.gameObject;
		if (Enemy.CompareTag("Enemy"))
		{
			Enemy.GetComponent<EnemyLogic>().Attack();
		}
	}
}
