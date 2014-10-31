using UnityEngine;
using System.Collections;
using Pathfinding;

public class EnemyMovement : MonoBehaviour
{

	//The max distance from the AI to a waypoint for it to continue to the next waypoint
	public float NextWaypointDistance = 3.0f;

	private EnemyProperties m_Properties;
	private GameObject m_Tower;
	private Seeker m_Seeker;
	private CharacterController m_CharacterController;
	private Path m_Path;
	private int CurrentWaypoint = 0;

	// Use this for initialization
	void Start()
	{
		m_Properties = gameObject.GetComponent<EnemyProperties>();
		DebugUtils.Assert(m_Properties != null, "Enemy properties not found.");
		m_Tower = GameObject.FindWithTag("Tower");
		DebugUtils.Assert(m_Tower != null, "Object with tag \"Tower\" not found");
		m_Seeker = gameObject.GetComponent<Seeker>();
		DebugUtils.Assert(m_Seeker != null, "Seeker component not found!");
		m_CharacterController = gameObject.GetComponent<CharacterController>();
		DebugUtils.Assert(m_CharacterController != null, "CharacterController component not found!");
		m_Seeker.StartPath(transform.position, m_Tower.transform.position, OnPathComplete);
	}

	// Update is called once per frame
	void Update()
	{
		//Vector3 direction = Tower.transform.position - transform.position;
		//transform.position += Properties.Speed * direction * Time.deltaTime;
	}

	// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
	void FixedUpdate()
	{
		Debug.Log("Calling fixed Update");
		if (m_Path == null)
			return;

		if (CurrentWaypoint >= m_Path.vectorPath.Count)
		{
			Debug.Log("End of path reached");
			return;
		}

		// Direction to the next waypoint
		Vector3 dir = (m_Path.vectorPath[CurrentWaypoint] - transform.position).normalized;
		dir *= m_Properties.Speed * Time.fixedDeltaTime;
		m_CharacterController.SimpleMove(dir);

		// Check if we are close enough to the next waypoint
		// If we are, proceed to follow the next waypoint
		if (Vector3.Distance(transform.position, m_Path.vectorPath[CurrentWaypoint]) < NextWaypointDistance)
		{
			CurrentWaypoint++;
		}
	}

	public void OnPathComplete(Path p)
	{
		Debug.Log("Path completed!");
		if (p.error)
		{
			Debug.LogError(p.errorLog);
		}
		else
		{
			m_Path = p;
		}
	}
}
