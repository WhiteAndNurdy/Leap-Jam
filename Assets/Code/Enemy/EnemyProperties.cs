using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;

[RequireComponent(typeof(AIPath))]
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
	public float ReassignTargetRate = 0.2f;
	public float TimeBetweenAttacks;
	public Elements[] VulnerableTo = new Elements[1];

	private AIPath m_AIPath;
	private GameObject m_Tower;
	private HashSet<Elements> VulnerableToUnique = new HashSet<Elements>();
	private bool m_EnemyActive;

	void Awake()
	{
		m_Tower = GameObject.FindGameObjectWithTag("Tower");
		m_AIPath = gameObject.GetComponent<AIPath>();
	}

	// Use this for initialization
	protected override void Start() 
	{
		base.Start();
		DebugUtils.Assert(m_Tower != null, "No object with tag \"Tower\" was found");
		m_AIPath.target = GetClosestTarget();
		foreach (Elements element in VulnerableTo)
		{
			VulnerableToUnique.Add(element);
		}
		// dereference array that should not be used.
		VulnerableTo = null;
		// force the ai components to the correct state.
		StartCoroutine("UpdateAIPath");
	}
	
	IEnumerator UpdateAIPath()
	{
		while (EnemyActive)
		{
			m_AIPath.target = GetClosestTarget();
			yield return new WaitForSeconds(ReassignTargetRate);
		}
	}

	Transform GetClosestTarget()
	{
		Transform returnValue = m_Tower.transform;
		Vector3 shortestDistance = m_Tower.transform.position - gameObject.transform.position;
		foreach(GameObject target in GameObject.FindGameObjectsWithTag("Agent_Targets"))
		{
			Vector3 tempDistance;
			tempDistance = target.transform.position - gameObject.transform.position;
			if (tempDistance.sqrMagnitude < shortestDistance.sqrMagnitude)
			{
				returnValue = target.transform.transform;
				shortestDistance = tempDistance;
			}
		}
		return returnValue;
	}

	public bool IsVulnerableTo(Elements type)
	{
		return VulnerableToUnique.Contains(type);
	}
}
