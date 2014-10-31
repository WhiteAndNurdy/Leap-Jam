using UnityEngine;
using System.Collections;
using Pathfinding;

[RequireComponent(typeof(AIPath))]
public class EnemyProperties : MonoBehaviour {

	public float HealthPoints;
	public bool Shield;
	public float DamageAmount;
	public float ReassignTargetRate = 0.2f;

	private AIPath m_AIPath;
	private GameObject m_Tower;
	private float m_ElapsedTimeTargetRate = 0.0f;

	// Use this for initialization
	void Start () {
		m_Tower = GameObject.FindGameObjectWithTag("Tower");
		DebugUtils.Assert(m_Tower != null, "No object with tag \"Tower\" was found");
		m_AIPath = gameObject.GetComponent<AIPath>();
		m_AIPath.target = GetClosestTarget();
	}
	
	// Update is called once per frame
	void Update () {
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
		foreach(Transform child in m_Tower.transform)
		{
			Vector3 tempDistance;
			tempDistance = child.position - gameObject.transform.position;
			if (tempDistance.sqrMagnitude < shortestDistance.sqrMagnitude)
			{
				returnValue = child.transform;
				shortestDistance = tempDistance;
			}
		}
		return returnValue;
	}
}
