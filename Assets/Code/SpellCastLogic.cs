using UnityEngine;
using System.Collections;
using Leap;

public class SpellCastLogic : MonoBehaviour {

	public LayerMask RayCastLayerMask;
	public float RayCastRange = 100.0f;

	public GameObject[] SpellPrefabs = new GameObject[(int)Elements.Count];
	public bool InverseControls = false;
	public bool InverseAimDirection = false;

	public bool DebugFireGesture;
	public float DotDirectionMiddleAndRingFinger = -0.8f;

	protected const float GIZMO_SCALE = 5.0f;

	GameObject m_AimIndicator;
	Controller m_LeapController;
	SpellCastLogic m_SpellCastLogic;
	bool m_Aiming = false;
	Elements m_PreviousCastingType = Elements.Count;

	void Awake()
	{
		m_LeapController = new Controller();
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
		StartCoroutine("CheckHandPresence");
		StartCoroutine("DetectFire");
		/*StartCoroutine("DetectAir");
		StartCoroutine("DetectEarth");
		StartCoroutine("DetectWater");*/
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

	IEnumerator CheckHandPresence()
	{
		while (true)
		{
			if (m_LeapController.Frame().Hands.Count == 1 && m_LeapController.Frame().Hands[0].IsLeft)
			{
				m_PreviousCastingType = Elements.Count;
			}
			yield return new WaitForSeconds(0.1f);
		}
	}

	IEnumerator DetectFire()
	{
		while (true)
		{
			if (m_Aiming)
			{
				foreach (Hand hand in m_LeapController.Frame().Hands)
				{
					if (hand.IsValid && InverseControls ? hand.IsLeft : hand.IsRight)
					{
						DebugUtils.Log("Valid hand found", DebugFireGesture);
						Finger indexFinger = null;
						Finger pinkieFinger = null;
						foreach (Finger finger in hand.Fingers)
						{
							if (finger.Type() == Finger.FingerType.TYPE_PINKY)
							{
								pinkieFinger = finger;
								DebugUtils.Log("pinkieFinger detected", DebugFireGesture);
							}
							else if (finger.Type() == Finger.FingerType.TYPE_INDEX)
							{
								indexFinger = finger;
								DebugUtils.Log("indexFinger detected", DebugFireGesture);
							}
							if (pinkieFinger != null && indexFinger != null)
							{
								break;
							}
						}
						if (!pinkieFinger.IsExtended && indexFinger.IsExtended)
						{
							DebugUtils.Log("pinkieFinger not extended, indexFinger extended", DebugFireGesture);
							float distance = Vector3.Dot(pinkieFinger.Bone(Leap.Bone.BoneType.TYPE_DISTAL).Direction.ToUnity(),
													indexFinger.Bone(Leap.Bone.BoneType.TYPE_DISTAL).Direction.ToUnity());
							if (distance < DotDirectionMiddleAndRingFinger)
							{
								DebugUtils.Log("Gesture complete!", DebugFireGesture);
								CastSpell(Elements.Fire);
							}
						}
						else
						{
							m_PreviousCastingType = Elements.Count;
						}
					}
				}
			}
			yield return new WaitForSeconds(0.1f);
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
		if (m_PreviousCastingType != element)
		{
			Instantiate(SpellPrefabs[(int)element], m_AimIndicator.transform.position, Quaternion.identity);
			m_PreviousCastingType = element;
		}
	}
}
