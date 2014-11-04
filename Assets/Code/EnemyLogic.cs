using UnityEngine;
using System.Collections;

[RequireComponent(typeof(EnemyProperties))]
public class EnemyLogic : EntityLogic
{
	private EnemyProperties m_EnemyProperties;
	private GameObject m_Tower;
	private GameObject m_Shield;

	// Use this for initialization
	protected override void Start()
	{
		base.Start();
		m_EnemyProperties = GetComponent<EnemyProperties>();
		m_Tower = GameObject.FindGameObjectWithTag("Tower");
		DebugUtils.Assert(m_Tower != null, "Tower object not found!");
		m_Shield = transform.FindChild("Shield").gameObject;
		DebugUtils.Assert(m_Shield != null, "Shield child not found!");
	}

	// Update is called once per frame
	protected override void Update()
	{
		base.Update();
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
		GameObject.Destroy(gameObject);
	}

	void DoAttack()
	{
		m_Tower.GetComponent<TowerLogic>().Damage(m_EnemyProperties.DamageAmount);
	}
}
