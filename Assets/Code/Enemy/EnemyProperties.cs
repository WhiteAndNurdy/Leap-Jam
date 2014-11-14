using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;

public class EnemyProperties : EntityProperties {

	public bool Shield;
	public bool EnemyActive
	{
		get { return m_EnemyActive; }
		set
		{
			m_EnemyActive = value;
			gameObject.GetComponent<AIPath>().enabled = value;
			gameObject.GetComponent<Seeker>().enabled = value;
		}
	}
	public float DamageAmount;
	public float TimeBetweenAttacks;
	public Elements[] VulnerableTo = new Elements[1];

	private GameObject m_Tower;
	private HashSet<Elements> VulnerableToUnique = new HashSet<Elements>();
	private bool m_EnemyActive;

	void Awake()
	{
		m_Tower = GameObject.FindGameObjectWithTag("Tower");
	}

	// Use this for initialization
	protected override void Start() 
	{
		base.Start();
		DebugUtils.Assert(m_Tower != null, "No object with tag \"Tower\" was found");
		foreach (Elements element in VulnerableTo)
		{
			VulnerableToUnique.Add(element);
		}
	}

	public bool IsVulnerableTo(Elements type)
	{
		return VulnerableToUnique.Contains(type);
	}
}
