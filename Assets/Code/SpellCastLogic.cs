using UnityEngine;
using System;
using System.Collections;
using Leap;

public class SpellCastLogic : MonoBehaviour
{

	public LayerMask RayCastLayerMask;
	public float RayCastRange = 100.0f;

	public GameObject[] SpellPrefabs = new GameObject[(int)Elements.Count];
	public bool InverseControls = false;
	public bool InverseAimDirection = false;

	public bool DebugFireGesture;
	public float FirePalmDotValue = 0.8f;
	public float FireMmDistanceChange = 50.0f;

	public bool DebugAirGesture;
	public float AirPalmDotValue = 0.6f;
	public float AirMmDistanceChange = 50.0f;

	public bool DebugWaterGesture;
	public float WaterPalmDotValue = 0.6f;
	public float WaterMmDistanceChange = 50.0f;

	public bool DebugEarthGesture;
	public float EarthGrabValue = 0.6f;
	public float EarthPalmDotValue = 0.6f;
	public float EarthMmDistanceChange = 50.0f;


	protected const float GIZMO_SCALE = 5.0f;

	GameObject m_AimIndicator;
	Controller m_LeapController;
	SpellCastLogic m_SpellCastLogic;
	bool m_Aiming = false;
	Elements m_CurrentCastingType = Elements.Count;
	Elements m_PreviousCastingType = Elements.Count;
	float m_AimScale = 1.0f;

	Vector m_FireGestureStartPosition;
	Vector m_AirGestureStartPosition;
	Vector m_WaterGestureStartPosition;
	Vector m_EarthGestureStartPosition;
	

	void Awake()
	{
		m_LeapController = new Controller();
		m_SpellCastLogic = gameObject.GetComponent<SpellCastLogic>();
		m_AimIndicator = GameObject.FindGameObjectWithTag("AimIndicator");
	}

	// Use this for initialization
	void Start()
	{
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
		StartCoroutine("CheckHandPresence");
	}

	void SetAim(Hand hand)
	{
		m_Aiming = true;
		Aim(LeapVectorToUnity(hand.PalmPosition));
	}

	void ResetSpells()
	{
		SetSpell(Elements.Count);
		m_PreviousCastingType = Elements.Count;
	}

	IEnumerator CheckHandPresence()
	{
		while (true)
		{
			m_Aiming = false;
			if (m_LeapController.Frame().Hands.Count == 1 && m_LeapController.Frame().Hands[0].IsLeft)
			{
				// second hand not found, reset spells
				ResetSpells();
			}
			foreach (Hand hand in m_LeapController.Frame().Hands)
			{
				if (hand.IsValid && InverseControls ? hand.IsRight : hand.IsLeft)
				{
					m_AimScale = 1 - hand.GrabStrength;
					SetAim(hand);
				}
				else if (hand.IsValid && InverseControls ? hand.IsLeft : hand.IsRight)
				{
					bool gestureFound = false;
					gestureFound = DetectFire(hand);
					if (!gestureFound)
					{
						gestureFound = DetectAir(hand);
					}
					if (!gestureFound)
					{
						gestureFound = DetectWater(hand);
					}
					if (!gestureFound)
					{
						gestureFound = DetectEarth(hand);
					}
					if (!gestureFound)
					{
						ResetSpells();
					}
				}
			}
			yield return new WaitForSeconds(0.1f);
		}
	}

	bool DetectFire(Hand hand)
	{
		if (hand.PalmNormal.Dot(Vector.Up) > FirePalmDotValue)
		{
			DebugUtils.Log("Hand facing upwards", DebugFireGesture);
			if (m_FireGestureStartPosition != null)
			{
				Vector linearHandMovement = hand.PalmPosition - m_FireGestureStartPosition;
				//Check if y - movement has changed since the gesture was detected.
				if (linearHandMovement.y > FireMmDistanceChange)
				{
					DebugUtils.Log("Fire gesture Complete", DebugFireGesture);
					SetSpell(Elements.Fire);
					return true;
				}
				else
				{
					return false;
				}
			}
			else
			{
				m_FireGestureStartPosition = hand.PalmPosition;
				return false;
			}
		}
		m_FireGestureStartPosition = null;
		return false;
	}

	bool DetectAir(Hand hand)
	{
		bool fingersExtended = true;
		foreach (Finger finger in hand.Fingers)
		{
			if (!finger.IsExtended)
			{
				fingersExtended = false;
				break;
			}
		}
		if (fingersExtended)
		{
			DebugUtils.Log("All Fingers extended", DebugAirGesture);
			if (hand.PalmNormal.Dot(Vector.Forward) > AirPalmDotValue)
			{
				DebugUtils.Log("Hand facing forwards", DebugAirGesture);
				if (m_AirGestureStartPosition != null)
				{
					Vector linearHandMovement = hand.PalmPosition - m_AirGestureStartPosition;
					//Check if z - movement has changed since the gesture was detected.
					if (Math.Abs(linearHandMovement.z) > AirMmDistanceChange)
					{
						DebugUtils.Log("Air gesture Complete", DebugAirGesture);
						SetSpell(Elements.Air);
						return true;
					}
				}
				else
				{
					m_AirGestureStartPosition = hand.PalmPosition;
					return false;
				}
			}
		}
		m_AirGestureStartPosition = null;
		return false;
	}

	bool DetectWater(Hand hand)
	{
		bool fingersExtended = true;
		foreach (Finger finger in hand.Fingers)
		{
			if (!finger.IsExtended)
			{
				fingersExtended = false;
				break;
			}
		}
		if (fingersExtended)
		{
			DebugUtils.Log("All Fingers extended", DebugWaterGesture);
			if (hand.PalmNormal.Dot(Vector.Down) > WaterPalmDotValue)
			{
				DebugUtils.Log("Hand facing downwards", DebugWaterGesture);
				if (m_WaterGestureStartPosition != null)
				{
					Vector linearHandMovement = hand.PalmPosition - m_WaterGestureStartPosition;
					//Check if x - movement has changed since the gesture was detected.
					if (Math.Abs(linearHandMovement.x) > WaterMmDistanceChange)
					{
						DebugUtils.Log("Water gesture Complete", DebugWaterGesture);
						SetSpell(Elements.Water);
						return true;
					}
					else
					{
						return false;
					}
				}
				else
				{
					m_WaterGestureStartPosition = hand.PalmPosition;
					return false;
				}
				
			}
		}
		m_WaterGestureStartPosition = null;
		return false;
	}

	bool DetectEarth(Hand hand)
	{
		if (hand.GrabStrength > EarthGrabValue && hand.PalmNormal.Dot(Vector.Down) > EarthPalmDotValue)
		{
			DebugUtils.Log("Hand Grabbing and facing down", DebugEarthGesture);
			if (m_EarthGestureStartPosition != null)
			{
				Vector linearHandMovement = m_EarthGestureStartPosition - hand.PalmPosition;
				//Check if x - movement has changed since the gesture was detected.
				if (linearHandMovement.y > EarthMmDistanceChange)
				{
					DebugUtils.Log("Earth gesture Complete", DebugEarthGesture);
					SetSpell(Elements.Earth);
					return true;
				}
				else
				{
					return false;
				}
			}
			else
			{
				m_EarthGestureStartPosition = hand.PalmPosition;
				return false;
			}
		}
		m_EarthGestureStartPosition = null;
		return false;
	}

	Vector3 LeapVectorToUnity(Leap.Vector leapVector)
	{
		return Vector3.Scale(leapVector.ToUnityScaled(), transform.localScale) + transform.position;
	}

	// Update is called once per frame
	void Update()
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
		return m_CurrentCastingType;
	}

	void SetSpell(Elements element)
	{
		if (m_PreviousCastingType != element)
			m_CurrentCastingType = element;
	}

	void CastSpell(Elements element)
	{
		if (element != Elements.Count && m_PreviousCastingType != element)
		{
			GameObject spellPrefab = Instantiate(SpellPrefabs[(int)element], m_AimIndicator.transform.position, Quaternion.identity) as GameObject;
			spellPrefab.transform.localScale = Vector3.Scale(spellPrefab.transform.localScale, GetAimScale(spellPrefab));
			m_PreviousCastingType = element;
		}
	}

	Vector3 GetAimScale(GameObject spellPrefab)
	{
		SpellLogic logic = spellPrefab.GetComponent<SpellLogic>();
		DebugUtils.Assert(logic != null, "Spell Logic component not found in prefab");
		float convertedScale = (m_AimScale * (1 - logic.MinimumScale)) + logic.MinimumScale;
		return new Vector3(convertedScale, convertedScale, convertedScale);
	}
}