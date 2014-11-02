using UnityEngine;
using System.Collections;

[RequireComponent(typeof(EnemyProperties))]
public class EnemyLogic : EntityLogic
{

	private bool m_Attacking;
	private EnemyProperties m_EnemyProperties;
	private float m_ElapsedTimeSinceAttack;
	private GameObject m_Tower;

	// Use this for initialization
	protected override void Start()
	{
		base.Start();
		m_EnemyProperties = GetComponent<EnemyProperties>();
		m_Tower = GameObject.FindGameObjectWithTag("Tower");
		DebugUtils.Assert(m_Tower != null, "Tower object not found!");
	}

	// Update is called once per frame
	protected override void Update()
	{
		base.Update();
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

	// make sure we only call Damage when we are using the correct type of damage..
	// see also : VulnerableTo
	public override void Damage(float amount)
	{
		base.Damage(amount);
	}

	public override void Die()
	{
		base.Die();
		Debug.Log("Enemy Died!");
	}

	void DoAttack()
	{
		m_Tower.GetComponent<TowerLogic>().Damage(m_EnemyProperties.DamageAmount);
	}
}
