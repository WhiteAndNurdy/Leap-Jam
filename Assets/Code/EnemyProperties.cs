using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;

[RequireComponent(typeof(AIPath))]
public class EnemyProperties : EntityProperties {

	public bool Shield;
	public float DamageAmount;
	public float ReassignTargetRate = 0.2f;
	public float TimeBetweenAttacks;
	public Elements[] VulnerableTo = new Elements[1];

	private AIPath m_AIPath;
	private GameObject m_Tower;
	private float m_ElapsedTimeTargetRate = 0.0f;
	private HashSet<Elements> VulnerableToUnique = new HashSet<Elements>();


	// Use this for initialization
	protected override void Start () {
		base.Start();
		m_Tower = GameObject.FindGameObjectWithTag("Tower");
		DebugUtils.Assert(m_Tower != null, "No object with tag \"Tower\" was found");
		m_AIPath = gameObject.GetComponent<AIPath>();
		m_AIPath.target = GetClosestTarget();
		foreach (Elements element in VulnerableTo)
		{
			VulnerableToUnique.Add(element);
		}
		// dereference array that should not be used.
		VulnerableTo = null;
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
		m_ElapsedTimeTargetRate += Time.deltaTime;
		if (m_ElapsedTimeTargetRate >= ReassignTargetRate)
		{
			m_ElapsedTimeTargetRate -= ReassignTargetRate;
			m_AIPath.target = GetClosestTarget();
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
