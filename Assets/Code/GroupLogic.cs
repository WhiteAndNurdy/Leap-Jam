using UnityEngine;
using System.Collections;

public enum AIMovementState
{
	Idle,
	Organizing,
	Moving
}

public class GroupLogic : MonoBehaviour {

	public Vector3 Center = new Vector3();

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
	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
