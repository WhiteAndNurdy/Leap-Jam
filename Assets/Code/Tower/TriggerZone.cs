using UnityEngine;
using System.Collections;

public class TriggerZone : MonoBehaviour {

	void OnTriggerEnter(Collider other)
	{
		GameObject Enemy = other.transform.gameObject;
		if (Enemy.CompareTag("Enemy") && Enemy.GetComponent<EnemyProperties>().EnemyActive)
		{
			// reached destination, stop moving
			Enemy.GetComponent<EnemyLogic>().MovingTowardsTower = false;
		}
	}
}
