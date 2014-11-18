using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System;

public class Spawner : MonoBehaviour 
{
	public bool hasGroupAssigned = false;
	public float enemySpawnDelay = 1.0f;
	public float formationDelay = 2.0f;
	public float groupSpawnDelay = 10.0f;
	
	private List<GameObject> enemyList;
	private int enemiesSpawned = 0;
	private bool grouping;

	void Awake ()
	{
		enemyList = new List<GameObject>();
		hasGroupAssigned = false;
	}

	void Start () 
	{
	}

	IEnumerator CheckEnemySpawn()
	{
		while (enemiesSpawned < enemyList.Count)
		{
			SpawnEnemy();
			yield return new WaitForSeconds(enemySpawnDelay);
		}
		// all enemies spawned. no longer grouping!
		yield return new WaitForSeconds(formationDelay);
		SetGrouping(false);
		yield return new WaitForSeconds(groupSpawnDelay - formationDelay);
		hasGroupAssigned = false;
	}

	void Update () 
	{

	}

	public void AssignGroup(GameObject group)
	{
		if (hasGroupAssigned)
		{
			return;
		}

		hasGroupAssigned = true;
		group.transform.position = transform.FindChild("GroupPoint").position;
		enemyList.Clear();
		foreach (Transform enemy in group.transform)
		{
			if(enemy.CompareTag("Enemy"))
				enemyList.Add(enemy.gameObject);
		}
		group.GetComponent<GroupLogic>().MovementState = AIMovementState.Organizing;
		enemiesSpawned = 0;
		SetGrouping(true);
		StartCoroutine("CheckEnemySpawn");
	}

	private void SpawnEnemy()
	{
		Vector3 enemyPosition = transform.position;
		enemyPosition.x += UnityEngine.Random.Range(-2, 2);
		enemyPosition.y += enemyList[enemiesSpawned].GetComponent<CharacterController>().height;
		enemyPosition.z += UnityEngine.Random.Range(-2, 2);
		enemyList[enemiesSpawned].transform.position = enemyPosition;
		enemyList[enemiesSpawned].GetComponent<EnemyProperties>().EnemySpawned = true;
		++enemiesSpawned;
	}

	void SetGrouping(bool val)
	{
		grouping = val;
		if(!grouping)
		{
			if (enemyList.Count > 0)
			{
				enemyList[0].transform.parent.GetComponent<GroupLogic>().Active = true; 
			}
		}
	}
}
