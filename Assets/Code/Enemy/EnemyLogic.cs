using UnityEngine;
using System.Collections;

[RequireComponent(typeof(EnemyProperties))]
[RequireComponent(typeof(AIPath))]
public class EnemyLogic : EntityLogic
{
	public float ReassignTargetRate = 0.2f;

	private EnemyProperties m_EnemyProperties;
	private GameObject m_Tower;
	private GameObject m_Shield;
	private AIPath m_AIPath;

	void Awake()
	{
		m_EnemyProperties = GetComponent<EnemyProperties>();
		m_Tower = GameObject.FindGameObjectWithTag("Tower");
		m_Shield = transform.FindChild("Shield").gameObject;
		m_AIPath = gameObject.GetComponent<AIPath>();
	}
	// Use this for initialization
	protected override void Start()
	{
		base.Start();
		DebugUtils.Assert(m_Tower != null, "Tower object not found!");
		DebugUtils.Assert(m_Shield != null, "Shield child not found!");
		StartCoroutine("UpdateAIPath");
	}

	IEnumerator UpdateAIPath()
	{
		while (true)
		{
			if (m_EnemyProperties.EnemyActive)
			{
				m_AIPath.target = GetClosestTarget();
			}
			yield return new WaitForSeconds(ReassignTargetRate);
		}
	}

	Transform GetClosestTarget()
	{
		Transform returnValue = m_Tower.transform;
		Vector3 shortestDistance = m_Tower.transform.position - gameObject.transform.position;
		foreach (GameObject target in GameObject.FindGameObjectsWithTag("Agent_Targets"))
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

	IEnumerator CheckForAttack()
	{
		while (true)
		{
			DoAttack();
			yield return new WaitForSeconds(m_EnemyProperties.TimeBetweenAttacks);
		}
	}

	public void Attack()
	{
		StopCoroutine("UpdateAIPath");
		StartCoroutine("CheckForAttack");
	}

	public void Damage(float amount, Elements type)
	{
		if (m_Shield.activeSelf)
		{
			m_Shield.GetComponent<ShieldLogic>().Damage(type);
		}
		else if (m_EnemyProperties.IsVulnerableTo(type))
		{
			Damage(amount);
		}
	}

	public override void Die()
	{
		base.Die();
		EnemyPoolProperties.instance.RemoveEnemy(gameObject);
	}

	void DoAttack()
	{
		DebugUtils.Assert(m_Tower != null, "Tower NULL!");
		DebugUtils.Assert(m_Tower.GetComponent<TowerLogic>() != null, "TowerLogic NULL!");
		DebugUtils.Assert(m_EnemyProperties != null, "EnemyProperties NULL!");
		m_Tower.GetComponent<TowerLogic>().Damage(m_EnemyProperties.DamageAmount);
	}
}
