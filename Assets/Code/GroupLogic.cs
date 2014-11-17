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

	private AIMovementState m_MovementState;

	void Awake()
	{
		MovementState = AIMovementState.Idle;
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
}
