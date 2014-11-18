using UnityEngine;
using System.Collections;


[RequireComponent(typeof(EnemyProperties))]
public class EnemyLogic : EntityLogic
{
	public bool MovingTowardsTower = true;

	private EnemyProperties m_EnemyProperties;
	private GameObject m_Shield;
	private CharacterController m_Controller; 
	private Vector3 m_TotalFrameMovement;
	private GameObject m_Tower;

	void Awake()
	{
		m_EnemyProperties = GetComponent<EnemyProperties>();
		m_Shield = transform.FindChild("Shield").gameObject;
		m_TotalFrameMovement = new Vector3();
		m_Controller = GetComponent<CharacterController>();
		m_Tower = GameObject.FindGameObjectWithTag("Tower");
	}

	// Use this for initialization
	protected override void Start()
	{
		base.Start();
		DebugUtils.Assert(m_Shield != null, "Shield child not found!");
		DebugUtils.Assert(m_Controller != null, "No charactercontroller found!");
	}

	protected override void Update()
	{
		base.Update();
		m_Controller.SimpleMove(m_TotalFrameMovement);
		m_TotalFrameMovement = Vector3.zero;
	}


	IEnumerator MoveInToAttack()
	{/*
		m_Path.enabled = false;
		if (GetComponent<Seeker>() != null)
		{
			GetComponent<Seeker>().enabled = false;
		}
		while (MovingTowardsTower)
		{
			Vector3 dir = m_Tower.transform.position - transform.position;
			Vector3 movement = dir.normalized * m_Path.speed;
			if (movement.sqrMagnitude > dir.sqrMagnitude)
				movement = dir;
			Move(movement);
			yield return null;
		}
		StartCoroutine("CheckForAttack");*/
		yield return null;
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
		if (m_EnemyProperties.EnemyActive)
		{
			Debug.Log("Reached Attack");
			StopCoroutine("UpdateAIPath");
			StartCoroutine("MoveInToAttack");
		}
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
		m_EnemyProperties.EnemySpawned = false;
		EnemyPoolProperties.instance.RemoveEnemy(gameObject);
	}

	void DoAttack()
	{
		DebugUtils.Assert(m_Tower != null, "Tower NULL!");
		DebugUtils.Assert(m_Tower.GetComponent<TowerLogic>() != null, "TowerLogic NULL!");
		DebugUtils.Assert(m_EnemyProperties != null, "EnemyProperties NULL!");
		m_Tower.GetComponent<TowerLogic>().Damage(m_EnemyProperties.DamageAmount);
	}

	public void Move(Vector3 amount)
	{
		m_TotalFrameMovement += amount;
	}
}
