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

    //File pulled from resources
    private LevelFromXML levelFromXML;

	void Start () 
    {
        waves = new List<GameObject>();

        levelFromXML = Utilities.LoadLevelFile("LevelX");
        CreateLevelFromXML();

        AddWaveToSpawners(0);
	}

    private void CreateLevelFromXML() 
    {
        foreach (var wave in levelFromXML.waves)
        {
            GameObject waveObj = Instantiate(wavePrefab) as GameObject;

            foreach (var group in wave.groups)
            {
                GameObject groupObj = Instantiate(groupPrefab) as GameObject;
                
                foreach (var enemy in group.enemies)
                {
                    GameObject enemyObj = Instantiate(enemyPrefab, gameObject.transform.position, gameObject.transform.rotation) as GameObject;
                    //enemyObj.GetComponent<EnemyProperties>().type = enemy.type;
                    enemyObj.GetComponent<EnemyProperties>().HealthPoints = enemy.health;
                    enemyObj.GetComponent<EnemyProperties>().Shield = enemy.hasShield;
                    enemyObj.GetComponent<EnemyProperties>().EnemyActive = false;

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

        foreach (var group in currentWave.GetComponentsInChildren<Transform>())
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
}
