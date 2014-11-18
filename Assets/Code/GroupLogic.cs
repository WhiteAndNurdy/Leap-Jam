using UnityEngine;
using System.Collections;

public enum AIMovementState
{
	Idle,
	Organizing,
	Moving,
	Attacking
}

public class GroupLogic : MonoBehaviour {

	public AIMovementState MovementState
	{
		get { return m_MovementState; }
		set { m_MovementState = value; }
	}
	public bool Active
	{
		get { return m_Active; }
		set 
		{
			m_Active = value;
			if (value)
			{
				Initialize();
			}
			foreach (Transform child in transform)
			{
				if (child.CompareTag("Enemy"))
					child.GetComponent<EnemyProperties>().EnemyActive = value;
				else
				{
					child.GetComponent<EnemyPath>().enabled = value;
					child.GetComponent<Seeker>().enabled = value;
				}
			}
		}
	}
	public float ReassignTargetRate = 0.2f;


	private bool m_Active;
	private AIMovementState m_MovementState;
	private GameObject m_Tower;
	private GameObject m_GroupLeader;

	void Awake()
	{
		MovementState = AIMovementState.Idle;
		m_Tower = GameObject.FindGameObjectWithTag("Tower");
		m_GroupLeader = transform.FindChild("GroupLeader").gameObject;
	}

	void Start()
	{
		DebugUtils.Assert(m_Tower != null, "Tower object not found!");
		DebugUtils.Assert(m_GroupLeader != null, "Couldn't find group leader!");
	}

	void Initialize()
	{
		MovementState = AIMovementState.Moving;
		StartCoroutine(UpdateAIPath());
	}

	void Update()
	{
		//transform.localPosition = m_GroupLeader.transform.position;
	}

	IEnumerator UpdateAIPath()
	{
		while (true)
		{
			if (Active)
			{
				//m_GroupLeader.transform.position = EnemyCenter();
				m_GroupLeader.GetComponent<EnemyPath>().target = GetClosestTarget();
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
		return returnValue;
	}

	public Vector3 Center()
	{
		return transform.FindChild("GroupLeader").position;
	}

	Vector3 EnemyCenter()
	{
		Vector3 center = new Vector3();
		foreach (Transform child in transform)
		{
			if (child.CompareTag("Enemy"))
				center += child.transform.position;
		}
		center /= transform.childCount;
		return center;
	}

	public Vector3 Velocity()
	{
		Vector3 velocity = new Vector3();
		foreach (Transform child in transform)
		{
			if(child.CompareTag("Enemy"))
				velocity += child.GetComponent<CharacterController>().velocity;
		}
		velocity /= transform.childCount;
		return velocity;
	}

	public void Move(Vector3 amount)
	{
		foreach (Transform child in transform)
		{
			child.GetComponent<EnemyLogic>().Move(amount);
		}
	}
}
