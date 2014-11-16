using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Scanner : MonoBehaviour 
{

	private HashSet<GameObject> m_GroupSet;

	private static Scanner _instance;

	public static Scanner instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = GameObject.FindObjectOfType<Scanner>();

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

	// Use this for initialization
	public void Initialize()
	{
		GameObject[] groups = GameObject.FindGameObjectsWithTag("Group");
		foreach (GameObject group in groups)
		{
			m_GroupSet.Add(group);
		}
	}
}
