using UnityEngine;
using System.Collections;

[RequireComponent(typeof(EnemyProperties))]
public class EnemyLogic : EntityLogic
{
	private EnemyProperties m_EnemyProperties;
	private GameObject m_Tower;
	private GameObject m_Shield;

	void Awake()
	{
		m_EnemyProperties = GetComponent<EnemyProperties>();
		m_Tower = GameObject.FindGameObjectWithTag("Tower");
		m_Shield = transform.FindChild("Shield").gameObject;
	}
	// Use this for initialization
	protected override void Start()
	{
		base.Start();
		DebugUtils.Assert(m_Tower != null, "Tower object not found!");
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
