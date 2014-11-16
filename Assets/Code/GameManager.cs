using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System;

public enum GameState
{
	Tutorial,
	Preparation,
	Waves,
	GameOver
}

public class GameManager : MonoBehaviour 
{
	public GameState currentGameState;

	public List<GameObject> waves;
	public GameObject wavePrefab;
	public GameObject groupPrefab;
	public GameObject enemyPrefab;

	public float waveDelay = 10.0f;

	//File pulled from resources
	private LevelFromXML levelFromXML;
	private int currentWave = 0;
	private bool waveEnded = true;

	void Start () 
	{
		waves = new List<GameObject>();

		levelFromXML = Utilities.LoadLevelFile("LevelX");
		CreateLevelFromXML();

		AddWaveToSpawners(currentWave);
		StartCoroutine("CheckIfWaveEnded");
	}

	void Update()
	{
		CheckIfWaveEnded();
		if (waveEnded)
		{
			StartNextWave();
		}
	}

	private void CreateLevelFromXML() 
	{
		foreach (var wave in levelFromXML.waves)
		{
			GameObject waveObj = Instantiate(wavePrefab) as GameObject;
			waveObj.tag = "Wave";

			foreach (var group in wave.groups)
			{
				GameObject groupObj = Instantiate(groupPrefab) as GameObject;
				groupObj.tag = "Group";
				
				foreach (var enemy in group.enemies)
				{
					GameObject enemyObj = EnemyPoolProperties.instance.GetEnemyWithName(enemy.type);
					DebugUtils.Assert(enemyObj != null, "Unknown enemy type: " + enemy.type + " make sure this matches with the pool.");
					//enemyObj.GetComponent<EnemyProperties>().type = enemy.type;
					enemyObj.GetComponent<EnemyProperties>().HealthPoints = enemy.health;
					enemyObj.GetComponent<EnemyProperties>().Shield = enemy.hasShield;
					enemyObj.GetComponent<EnemyProperties>().EnemyActive = false;
					enemyObj.GetComponent<EnemyProperties>().EnemySpawned = false;
					enemyObj.transform.parent = groupObj.transform;
				}
				groupObj.transform.parent = waveObj.transform;
			}

			waves.Add(waveObj);
		}
	}

	private void AddWaveToSpawners(int currentWaveIndex)
	{
		var spawners = GameObject.FindGameObjectsWithTag("Spawner");
		var currentWave = waves[currentWaveIndex];

		List<GameObject> groups = new List<GameObject>();
		foreach (Transform child in currentWave.GetComponentsInChildren<Transform>())
		{
			if (child.tag == "Group")
			{
				groups.Add(child.gameObject);
			}
		}

		foreach (var group in groups)
		{           
			foreach(var spawner in spawners)
			{
				if (!spawner.GetComponent<Spawner>().hasGroupAssigned)
				{
					//once an empty spawner is found, no need to look for another one.
					List<GameObject> enemies = new List<GameObject>();
					foreach(Transform child in group.GetComponentsInChildren<Transform>())
					{
						if (child.tag == "Enemy")
						{
							enemies.Add(child.gameObject);
						}
					}

					spawner.GetComponent<Spawner>().AssignGroup(enemies);
					break;
				}
			}
		}
	}

	private IEnumerator CheckIfWaveEnded()
	{
		while (true)
		{
			var enemies = GameObject.FindGameObjectsWithTag("Enemy");
			waveEnded = false;
			bool enemyActive = false;

			if (enemies != null)
			{
				foreach (var enemy in enemies)
				{
					if (enemy.GetComponent<EnemyProperties>().EnemySpawned)
					{
						enemyActive = true;
						break;
					}
				}
			}

			if (!enemyActive)
			{
				yield return new WaitForSeconds(waveDelay);
				waveEnded = true;
			}
			yield return null;
		}
	}

	private void StartNextWave()
	{
		++currentWave;
		if (currentWave >= waves.Count)
		{
			EndLevel(true);
			return;
		}
		else
		{
			AddWaveToSpawners(currentWave);
		}
	}

	private void EndLevel(bool hasWon)
	{
		//Disable controls
		//Open victory/defeat screen
		//Enable quit/continue buttons
	}
}
