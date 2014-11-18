using UnityEngine;
using System.Collections;

public enum AIMovementState
{
	Idle,
	Organizing,
	Moving
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
			GetComponent<EnemyPath>().enabled = value;
			GetComponent<Seeker>().enabled = value;
			foreach (Transform child in transform)
			{
				child.GetComponent<EnemyProperties>().EnemyActive = value;
			}
		}
	}
	public float ReassignTargetRate = 0.2f;


	private bool m_Active;
	private EnemyPath m_Path;
	private AIMovementState m_MovementState;
	private GameObject m_Tower;

	void Awake()
	{
		MovementState = AIMovementState.Idle;
		m_Path = GetComponent<EnemyPath>();
		m_Tower = GameObject.FindGameObjectWithTag("Tower");
	}

	void Start()
	{
		DebugUtils.Assert(m_Tower != null, "Tower object not found!");
	}

	void Initialize()
	{
		MovementState = AIMovementState.Moving;
		StartCoroutine(UpdateAIPath());
	}

	void Update()
	{
	}

	IEnumerator UpdateAIPath()
	{
		while (true)
		{
			if (Active)
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
		return returnValue;
	}

	public Vector3 Center()
	{
		Vector3 center = new Vector3();
		foreach (Transform child in transform)
		{
			center += child.position;
		}
		center /= transform.childCount;
		return center;
	}

	public Vector3 Velocity()
	{
		Vector3 velocity = new Vector3();
		foreach (Transform child in transform)
		{
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
