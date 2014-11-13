using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyPoolMap : ScriptableObject
{
	[System.Serializable]
	public class EnemyPoolEntry
	{
		public string Name;
		public GameObject EnemyPrefab;
		public int Count;
	}
}

public class EnemyPool
{
	List<GameObject>	List;
	int					ActiveIndex;

	public EnemyPool()
	{
		List = new List<GameObject>();
		ActiveIndex = 0;
	}

	public EnemyPool(int capacity)
	{
		List = new List<GameObject>(capacity);
		ActiveIndex = 0;
	}

	public void Add(GameObject item)
	{
		item.SetActive(false);
		List.Add(item);
	}

	public int Count 
	{ 
		get {return List.Count - ActiveIndex;} 
	}

	public GameObject At(int i)
	{
		if (List.Count <= i)
		{
			Grow();
		}
		return List[i];
	}

	void Grow()
	{
		DebugUtils.Assert(List.Count > 0, "Trying to extend invalid list!");
		int amount = List.Count / 4;
		for (int i = 0; i < amount; ++i)
		{
			Add(List[0]);
		}
	}

	public GameObject RemoveItemFromPool()
	{
		foreach (GameObject item in List)
		{
			if (!item.activeSelf)
			{
				item.SetActive(true);
				ActiveIndex++;
				return item;
			}
		}
		return null;
	}

	public void AddItemToPool(GameObject item)
	{
		item.SetActive(false);
		ActiveIndex--;
	}
}

public class EnemyPoolProperties : MonoBehaviour {

	public EnemyPoolMap.EnemyPoolEntry[] EnemyPoolEntries;
	Dictionary<string, EnemyPool> m_EnemyPoolMap = new Dictionary<string, EnemyPool>();

	private static EnemyPoolProperties _instance;

	public static EnemyPoolProperties instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = GameObject.FindObjectOfType<EnemyPoolProperties>();

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
		}
	}

	// Use this for initialization
	void Start () 
	{
		foreach (var enemy in EnemyPoolEntries)
		{
			m_EnemyPoolMap.Add(enemy.Name, new EnemyPool(enemy.Count));
			for(int i = 0; i < enemy.Count; ++i)
			{
				GameObject newObject = Instantiate(enemy.EnemyPrefab) as GameObject;
				newObject.name = enemy.Name;
				m_EnemyPoolMap[enemy.Name].Add(newObject);
			}
		}
	}
	
	public List<GameObject> GetEnemiesWithName(string name, int amount)
	{
		if (m_EnemyPoolMap.ContainsKey(name))
		{
			List<GameObject> newList = new List<GameObject>();
			for (int i = 0; i < amount; ++i)
			{
				newList.Add(m_EnemyPoolMap[name].RemoveItemFromPool());
			}
			return newList;
		}
		return null;
	}

	public GameObject GetEnemyWithName(string name)
	{
		if (m_EnemyPoolMap.ContainsKey(name))
		{
			return m_EnemyPoolMap[name].RemoveItemFromPool();
		}
		return null;
	}

	public void RemoveEnemy(GameObject enemy)
	{
		m_EnemyPoolMap[enemy.name].AddItemToPool(enemy);
	}
}
