using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System;

public class Spawner : MonoBehaviour 
{
	public bool hasGroupAssigned = false;
	public float enemySpawnDelay = 1.0f;
	public float groupSpawnDelay = 10.0f;
	
	private List<GameObject> enemyList;
	private int enemiesSpawned = 0;

	//public float spawnAmount;

	void Start () 
	{
		enemyList = new List<GameObject>();
		hasGroupAssigned = false;
	}

	IEnumerator CheckEnemySpawn()
	{
		while (enemiesSpawned < enemyList.Count)
		{
			SpawnEnemy();
			yield return new WaitForSeconds(enemySpawnDelay);
		}
		foreach (GameObject enemy in enemyList)
		{
			enemy.GetComponent<EnemyProperties>().EnemyActive = true;
		}
		yield return new WaitForSeconds(groupSpawnDelay);
		hasGroupAssigned = false;
	}

	void Update () 
	{

	}

	public void AssignGroup(List<GameObject> enemies)
	{
		if (hasGroupAssigned)
		{
			return;
		}

		hasGroupAssigned = true;
		enemyList.Clear();
		foreach (var enemy in enemies)
		{
			enemyList.Add(enemy);
		}
		enemiesSpawned = 0;
		StartCoroutine("CheckEnemySpawn");
	}

	private void SpawnEnemy()
	{
		Vector3 enemyPosition = transform.position;
		enemyPosition.y += enemyList[enemiesSpawned].GetComponent<CharacterController>().height;
		enemyList[enemiesSpawned].transform.position = enemyPosition;
		//enemyList[enemiesSpawned].GetComponent<EnemyProperties>().EnemyActive = true;
		++enemiesSpawned;
	}
}
