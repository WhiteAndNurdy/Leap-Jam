using UnityEngine;
using System.Collections;

[RequireComponent(typeof(EnemyProperties))]
public class EnemyAttackScript : MonoBehaviour {

	private bool m_Attacking;
	private EnemyProperties m_EnemyProperties;
	private float m_ElapsedTimeSinceAttack;
	private GameObject m_Tower;

	// Use this for initialization
	void Start () {
		m_EnemyProperties = GetComponent<EnemyProperties>();
		m_Tower = GameObject.FindGameObjectWithTag("Tower");
		DebugUtils.Assert(m_Tower != null, "Tower object not found!");
	}
	
	// Update is called once per frame
	void Update () {
		if (m_Attacking)
		{
			m_ElapsedTimeSinceAttack += Time.deltaTime;
			if (m_ElapsedTimeSinceAttack >= m_EnemyProperties.TimeBetweenAttacks)
			{
				m_ElapsedTimeSinceAttack -= m_EnemyProperties.TimeBetweenAttacks;
				DoAttack();
			}
		}
	}

	public void Attack()
	{
		m_Attacking = true;
	}

	void DoAttack()
	{
		m_Tower.GetComponent<TowerLogic>().Damage(m_EnemyProperties.DamageAmount);
	}
}
