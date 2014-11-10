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
                    GameObject enemyObj = Instantiate(enemyPrefab) as GameObject;
                    //enemyObj.GetComponent<EnemyProperties>().type = enemy.type;
                    enemyObj.GetComponent<EnemyProperties>().HealthPoints = enemy.health;
                    enemyObj.GetComponent<EnemyProperties>().Shield = enemy.hasShield;

                    enemyObj.transform.parent = groupObj.transform;
                }
                groupObj.transform.parent = waveObj.transform;
            }

            waves.Add(waveObj);
        }
	}
}
