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
	private bool grouping;

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
		// all enemies spawned. no longer grouping!
		SetGrouping(false);
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
		SetGrouping(true);
		StartCoroutine("CheckEnemySpawn");
	}

	private void SpawnEnemy()
	{
		Vector3 enemyPosition = transform.position;
		enemyPosition.y += enemyList[enemiesSpawned].GetComponent<CharacterController>().height;
		enemyList[enemiesSpawned].transform.position = enemyPosition;
		enemyList[enemiesSpawned].GetComponent<EnemyProperties>().EnemySpawned = true;
		StartCoroutine(MoveEnemyToGroupPoint(enemyList[enemiesSpawned]));
		++enemiesSpawned;
	}

	IEnumerator MoveEnemyToGroupPoint(GameObject enemy)
	{
		while (grouping)
		{
			Vector3 dir = transform.FindChild("GroupPoint").position - enemy.transform.position;
			Vector3 movement = dir.normalized * enemy.GetComponent<AIPath>().speed * Time.deltaTime;
			if (movement.magnitude > dir.magnitude)
				movement = dir;
			enemy.GetComponent<CharacterController>().Move(movement);
			yield return null;
		}
	}

	void SetGrouping(bool val)
	{
		grouping = val;
		if(!grouping)
		{
			foreach (var enemy in enemyList)
			{
				enemy.GetComponent<EnemyProperties>().EnemyActive = true;
			}
		}
	}
}
