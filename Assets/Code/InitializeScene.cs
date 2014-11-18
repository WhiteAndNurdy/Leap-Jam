using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InitializeScene : MonoBehaviour {

	public List<GameObject> waves;
	public GameObject wavePrefab;
	public GameObject groupPrefab;
	public float waveDelay = 10.0f;

	//File pulled from resources
	private LevelFromXML levelFromXML;
	private int currentWave = 0;
	private bool waveEnded = true;

	void Awake ()
	{
		waves = new List<GameObject>();
		EnemyPoolProperties.instance.SetTowerForAllEnemies(GameObject.FindWithTag("Tower"));
	}

	// Use this for initialization
	void Start () 
	{
		levelFromXML = Utilities.LoadLevelFile("LevelX");
		CreateLevelFromXML();
		InitializeScanner();

		AddWaveToSpawners(currentWave);
		StartCoroutine("CheckIfWaveEnded");
	}
	
	// Update is called once per frame
	void Update () 
	{
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
			waveObj.transform.position = Vector3.zero;
			foreach (var group in wave.groups)
			{
				GameObject groupObj = Instantiate(groupPrefab) as GameObject;
				groupObj.tag = "Group";
				groupObj.transform.position = Vector3.zero;

				foreach (var enemy in group.enemies)
				{
					GameObject enemyObj = EnemyPoolProperties.instance.GetEnemyWithName(enemy.type);
					DebugUtils.Assert(enemyObj != null, "Unknown enemy type: " + enemy.type + " make sure this matches with the pool.");
					enemyObj.GetComponent<EnemyProperties>().HealthPoints = enemy.health;
					enemyObj.GetComponent<EnemyProperties>().Shield = enemy.hasShield;
					enemyObj.GetComponent<EnemyProperties>().EnemyActive = false;
					enemyObj.GetComponent<EnemyProperties>().EnemySpawned = false;
					enemyObj.transform.position = Vector3.zero;
					enemyObj.transform.parent = groupObj.transform;
				}
				groupObj.transform.parent = waveObj.transform;
			}
			waves.Add(waveObj);
		}
	}

	private void InitializeScanner()
	{
		Scanner.instance.Initialize();
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
			foreach (var spawner in spawners)
			{
				if (!spawner.GetComponent<Spawner>().hasGroupAssigned)
				{
					spawner.GetComponent<Spawner>().AssignGroup(group);
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
			GameManager.instance.EndLevel(true);
			return;
		}
		else
		{
			AddWaveToSpawners(currentWave);
		}
	}
}
