using UnityEngine;
using System.Collections;


[RequireComponent(typeof(EnemyProperties))]
public class EnemyLogic : EntityLogic
{
	public float ReassignTargetRate = 0.2f;
	public bool MovingTowardsTower = true;

	private EnemyProperties m_EnemyProperties;
	private GameObject m_Tower;
	private GameObject m_Shield;
	private EnemyPath m_Path;
	private CharacterController m_Controller; 
	private Vector3 m_TotalFrameMovement;
	private bool m_PathStarted = false;

	void Awake()
	{
		m_EnemyProperties = GetComponent<EnemyProperties>();
		m_Tower = GameObject.FindGameObjectWithTag("Tower");
		m_Shield = transform.FindChild("Shield").gameObject;
		m_Path = gameObject.GetComponent<EnemyPath>();
		m_TotalFrameMovement = new Vector3();
		m_Controller = GetComponent<CharacterController>();
	}

	// Use this for initialization
	protected override void Start()
	{
		base.Start();
		DebugUtils.Assert(m_Tower != null, "Tower object not found!");
		DebugUtils.Assert(m_Shield != null, "Shield child not found!");
		DebugUtils.Assert(m_Controller != null, "No charactercontroller found!");
	}

	protected override void Update()
	{
		base.Update();
		m_Controller.SimpleMove(m_TotalFrameMovement);
		m_TotalFrameMovement = Vector3.zero;
	}

	public void InitializePath()
	{
		if (!m_PathStarted)
		{
			m_Path.enabled = true;
			StartCoroutine(UpdateAIPath());
		}
	}

	IEnumerator UpdateAIPath()
	{
		while (true)
		{
			if (m_EnemyProperties.EnemyActive)
			{
				m_Path.target = GetClosestTarget();
			}
			yield return new WaitForSeconds(ReassignTargetRate);
		}
	}

	Vector3 GetClosestTarget()
	{
		Vector3 returnValue = m_Tower.transform.position;
		Vector3 shortestDistance = m_Tower.transform.position - gameObject.transform.position;
		foreach (GameObject target in GameObject.FindGameObjectsWithTag("Agent_Targets"))
		{
			Vector3 tempDistance;
			tempDistance = target.transform.position - gameObject.transform.position;
			if (tempDistance.sqrMagnitude < shortestDistance.sqrMagnitude)
			{
				returnValue = target.transform.transform.position;
				shortestDistance = tempDistance;
			}
		}
		// this value should be the center position of the flock. make new targets for each enemy to avoid them interfering in eachother's paths
		Vector3 offset = transform.parent.GetComponent<GroupLogic>().Center() - transform.position;
		returnValue += offset;
		return returnValue;
	}

	IEnumerator MoveInToAttack()
	{
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
		StartCoroutine("CheckForAttack");
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
