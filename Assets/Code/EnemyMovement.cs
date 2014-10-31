using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{

	private EnemyProperties Properties;
	private GameObject Tower;
	// Use this for initialization
	void Start()
	{
		Properties = gameObject.GetComponent<EnemyProperties>();
		DebugUtils.Assert(Properties != null);
		Tower = GameObject.FindWithTag("Tower");
		DebugUtils.Assert(Tower != null);
	}

	// Update is called once per frame
	void Update()
	{
		Vector3 direction = Tower.transform.position - transform.position;
		transform.position += Properties.Speed * direction * Time.deltaTime;
	}
}
