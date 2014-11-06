using UnityEngine;
using System.Collections;
using Leap;

public class SpellCastLogic : MonoBehaviour {

	public LayerMask RayCastLayerMask;
	public float RayCastRange = 100.0f;

	public GameObject[] SpellPrefabs = new GameObject[(int)Elements.Count];
	public bool InverseControls = false;
	public bool InverseAimDirection = false;

	protected const float GIZMO_SCALE = 5.0f;

	GameObject m_AimIndicator;
	Controller m_LeapController;
	SpellCastLogic m_SpellCastLogic;
	bool m_Aiming = false;

	void OnDrawGizmos()
	{
		// Draws the little Leap Motion Controller in the Editor view.
		Gizmos.matrix = Matrix4x4.Scale(GIZMO_SCALE * Vector3.one);
		Gizmos.DrawIcon(transform.position, "leap_motion.png");
	}

	void Awake()
	{
		m_LeapController = new Controller();
		Controller.PolicyFlag policy_flags = m_LeapController.PolicyFlags;
		policy_flags &= ~Controller.PolicyFlag.POLICY_OPTIMIZE_HMD;
		m_LeapController.SetPolicyFlags(policy_flags);
		m_SpellCastLogic = gameObject.GetComponent<SpellCastLogic>();
		m_AimIndicator = GameObject.FindGameObjectWithTag("AimIndicator");
		
	}

	// Use this for initialization
	void Start () 
	{
		Debug.Log("SanityTest");
		if (m_LeapController == null)
		{
			Debug.LogWarning(
				"Cannot connect to controller. Make sure you have Leap Motion v2.0+ installed");
		}
		else if (!m_LeapController.IsConnected)
		{
			Debug.LogWarning(
				"Leapmotion device not connected!");
		}
		DebugUtils.Assert(m_SpellCastLogic != null, "Unable to find SpellCastLogic component!");
		DebugUtils.Assert(m_AimIndicator != null, "Couldn't find object with tag AimIndicator");
		StartCoroutine("DetectAim");
	}

	IEnumerator DetectAim()
	{
		while (true)
		{
			foreach (Hand hand in m_LeapController.Frame().Hands)
			{
				if (hand.IsValid && InverseControls ? hand.IsRight : hand.IsLeft)
				{
					m_Aiming = true;
					// aim hand detected! 
					Debug.Log("Aiming");
					Aim(LeapVectorToUnity(hand.PalmPosition));
				}
			}
			yield return new WaitForSeconds(0.1f);
			m_Aiming = false;
		}
	}
	
	Vector3 LeapVectorToUnity(Leap.Vector leapVector)
	{
		return Vector3.Scale(leapVector.ToUnityScaled(), transform.localScale) + transform.position;
	}
	// Update is called once per frame
	void Update () 
	{
		if (m_Aiming)
		{
			// count is invalid.. 
			Elements CastingType = GetCastingType();
			if (CastingType != Elements.Count)
			{
				CastSpell(CastingType);
			}
		}
		else
		{
			m_AimIndicator.SetActive(false);
		}
	}

	void Aim(Vector3 palmPosition)
	{
		Ray ray;
		RaycastHit hit;
		ray = new Ray(palmPosition, Camera.main.transform.forward);
		
		if (Physics.Raycast(ray, out hit, RayCastRange, RayCastLayerMask))
		{
			if (hit.collider.name == "FloorPlane")
			{
				Debug.DrawLine(palmPosition, hit.point);
				m_AimIndicator.transform.position = hit.point;
				m_AimIndicator.SetActive(true);
			}
		}
	}

	Elements GetCastingType()
	{
		if (Input.GetKeyDown(KeyCode.Q))
		{
			return Elements.Water;
		}
		else if (Input.GetKeyDown(KeyCode.W))
		{
			return Elements.Fire;
		}
		else if (Input.GetKeyDown(KeyCode.E))
		{
			return Elements.Earth;
		}
		else if (Input.GetKeyDown(KeyCode.R))
		{
			return Elements.Air;
		}
		return Elements.Count;
	}

	void CastSpell(Elements element)
	{
		Instantiate(SpellPrefabs[(int)element], m_AimIndicator.transform.position, Quaternion.identity);
	}
}
