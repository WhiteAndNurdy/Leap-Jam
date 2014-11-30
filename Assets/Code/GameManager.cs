using UnityEngine;
using System.Linq;
using System.Collections;
using System;

public enum GameState
{
	Tutorial,
	Preparation,
	Wave,
	GameOver
}

public class GameManager : MonoBehaviour 
{
	public GameState currentGameState;

#if UNITY_EDITOR
	public bool skipTutorial = false;
	public bool resetPlayerPrefs = false;
#endif

	private static GameManager _instance;

	public static GameManager instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = GameObject.FindObjectOfType<GameManager>();

				//Tell unity not to destroy this object when loading a new scene!
				DontDestroyOnLoad(_instance.gameObject);
			}

			return _instance;
		}
	}

	void Awake()
	{
		if (_instance == null)
		{
			//If I am the first instance, make me the Singleton
			_instance = this;
			DontDestroyOnLoad(this);
		}
		else
		{
			//If a Singleton already exists and you find
			//another reference in scene, destroy it!
			if (this != _instance)
				Destroy(this.gameObject);
			return;
		}
		
	}

	void Start () 
	{
#if UNITY_EDITOR
		if (resetPlayerPrefs)
		{
			PlayerPrefs.DeleteAll();
		}
#endif
	}

	public void StartTutorial()
	{
#if UNITY_EDITOR
		if (skipTutorial)
		{
			StartPreparation();
			return;
		}
#endif
		currentGameState = GameState.Tutorial;
		Application.LoadLevel("Tutorial");
	}

	public void SplashScreenFadeInComplete(string name)
	{
		
	}

	public void SplashScreenFadeOutComplete(string name)
	{
		if (String.Compare(name, "startSplash") == 0)
		{
			StartCoroutine("StartSplashComplete");
		}
	}

	IEnumerator StartSplashComplete()
	{
		if (PlayerPrefs.GetInt("CompletedTutorial") == 0)
		{
			StartTutorial();
		}
		else
		{
			StartPreparation();
		}
		return null;
	}

	public void StartPreparation()
	{
		currentGameState = GameState.Preparation;
		LoadingScreen.show();
		Application.LoadLevel("Level01");
	}

	void Update()
	{

	}

	public void EndLevel(bool hasWon)
	{
		//Disable controls
		//Open victory/defeat screen
		//Enable quit/continue buttons
	}
}
