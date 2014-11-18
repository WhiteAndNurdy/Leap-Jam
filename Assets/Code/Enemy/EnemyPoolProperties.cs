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

	public int TotalCount
	{
		get { return List.Count; }
	}

	public GameObject At(int i)
	{
		if (List.Count <= i)
		{
			Grow();
		}
		return List[i];
	}

	public GameObject GetFirstPooled()
	{
		foreach (GameObject item in List)
		{
			if (!item.activeSelf)
				return item;
		}
		return null;
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
		GameObject item = GetFirstPooled();
		item.SetActive(true);
		ActiveIndex++;
		return item;
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
			DontDestroyOnLoad(this.gameObject);

			GameObject container = new GameObject();
			container.name = "Unclaimed Pooled Objects";
			DontDestroyOnLoad(container);
			foreach (var enemy in EnemyPoolEntries)
			{
				m_EnemyPoolMap.Add(enemy.Name, new EnemyPool(enemy.Count));
				GameObject subContainer = new GameObject();
				subContainer.name = enemy.Name + " Container";
				subContainer.transform.parent = container.transform;
				for (int i = 0; i < enemy.Count; ++i)
				{
					GameObject newObject = Instantiate(enemy.EnemyPrefab) as GameObject;
					newObject.name = enemy.Name;
					newObject.transform.parent = subContainer.transform;
					m_EnemyPoolMap[enemy.Name].Add(newObject);
				}
			}
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
	void Start () 
	{
		
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
		if (m_EnemyPoolMap[enemy.name].TotalCount > 0)
			enemy.transform.parent = m_EnemyPoolMap[enemy.name].GetFirstPooled().transform.parent;

		m_EnemyPoolMap[enemy.name].AddItemToPool(enemy);
	}

	public void SetTowerForAllEnemies(GameObject tower)
	{
		foreach (var pair in m_EnemyPoolMap)
		{
			for(int i = 0; i < pair.Value.TotalCount; ++i)
			{
				pair.Value.At(i).GetComponent<EnemyProperties>().SetTowerObject(tower);
				pair.Value.At(i).GetComponent<EnemyLogic>().SetTowerObject(tower);
			}
		}
	}
}
