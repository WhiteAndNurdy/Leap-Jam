using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System;

public class Spawner : MonoBehaviour 
{
    public bool hasGroupAssigned = false;
    private List<GameObject> enemyList;

    public float spawnDelay = 1.0f;
    private float timeToNextSpawn = 0.0f;
    private int enemiesSpawned = 0;
    //public float spawnAmount;

	void Start () 
    {
        enemyList = new List<GameObject>();
        hasGroupAssigned = false;
	}

	void Update () 
    {
        timeToNextSpawn -= Time.deltaTime;
        if (timeToNextSpawn <= 0 && enemiesSpawned < enemyList.Count)
        {
            SpawnEnemy();
            timeToNextSpawn = spawnDelay;
        }

        //Spawned all enemies.
        if (enemiesSpawned == enemyList.Count - 1 && timeToNextSpawn < -10)
        {
           hasGroupAssigned = false;
        }
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
    }

    private void SpawnEnemy()
    {
        enemyList[enemiesSpawned].transform.position = transform.position;
        enemyList[enemiesSpawned].GetComponent<EnemyProperties>().EnemyActive = true;
        ++enemiesSpawned;
    }
}
