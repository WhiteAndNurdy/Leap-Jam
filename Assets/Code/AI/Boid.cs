using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Boid : MonoBehaviour 
{
	private static Boid _instance;

	public static Boid instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = GameObject.FindObjectOfType<Boid>();

				//Tell unity not to destroy this object when loading a new scene!
				DontDestroyOnLoad(_instance.gameObject);
			}

			return _instance;
		}
	}

	void Awake()
	{
		if (_instance == null)
		{
			//If I am the first instance, make me the Singleton
			_instance = this;
			DontDestroyOnLoad(this);
		}
		else
		{
			//If a Singleton already exists and you find
			//another reference in scene, destroy it!
			if (this != _instance)
				Destroy(this.gameObject);
			return;
		}
	}

	
	void Start () 
	{

	}

	void Update()
	{
		MoveAllBoidsToNewPositions();
	}

	void MoveAllBoidsToNewPositions()
	{
		foreach (GameObject group in Scanner.instance.GroupSet)
		{
			Vector3 v1, v2, v3;
			CalculateGroupCenter(group, out v1, out v3);
			foreach (Transform child in group.transform)
			{
				Vector3 newV3 = v3;
				newV3 -= child.GetComponent<CharacterController>().velocity;
				newV3.Normalize();
				DebugUtils.Assert(child.CompareTag("Enemy"), "Boids tried to group an object that is not an enemy!");
				v2 = AvoidCollision(child);

				Vector3 dir = (v1 + v2 + newV3) - child.position;
				Vector3 movement = dir.normalized * child.GetComponent<AIPath>().speed;
				if (movement.sqrMagnitude > dir.sqrMagnitude)
					movement = dir;
				child.GetComponent<EnemyLogic>().Move(movement);
			}
		}
	}

	void CalculateGroupCenter(GameObject group, out Vector3 center, out Vector3 velocity)
	{
		center = new Vector3();
		velocity = new Vector3();
		foreach (Transform child in group.transform)
		{
			center += child.position;
			velocity += child.GetComponent<CharacterController>().velocity;
		}
		center /= group.transform.childCount;
		velocity /= group.transform.childCount;
	}

	Vector3 AvoidCollision(Transform boid)
	{
		Vector3 displacement = new Vector3();

		foreach (Transform otherBoid in boid.transform.parent)
		{
			if (boid != otherBoid)
			{
				if (Vector3.Magnitude(otherBoid.position - boid.position) < boid.GetComponent<CharacterController>().radius)
				{
					displacement -= (otherBoid.position - boid.position);
				}
			}
		}
		return displacement;
	}
}
