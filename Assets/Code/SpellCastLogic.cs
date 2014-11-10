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
	public float FireFingerDotValue = -0.8f;

	public bool DebugAirGesture;
	public float AirPalmDotValue = 0.6f;

	public bool DebugWaterGesture;
	public float WaterPalmDotValue = 0.6f;
	public float WaterMmDistanceChange = 100.0f;

	public bool DebugEarthGesture;
	public float EarthGrabValue = 0.6f;


	protected const float GIZMO_SCALE = 5.0f;

	GameObject m_AimIndicator;
	Controller m_LeapController;
	SpellCastLogic m_SpellCastLogic;
	bool m_Aiming = false;
	Elements m_CurrentCastingType = Elements.Count;
	Elements m_PreviousCastingType = Elements.Count;
	Vector m_WaterGestureStartPosition;
	

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

	void SetAim(Vector palmPosition)
	{
		m_Aiming = true;
		Aim(LeapVectorToUnity(palmPosition));
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
					SetAim(hand.PalmPosition);
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
			if (distance < FireFingerDotValue)
			{
				DebugUtils.Log("Fire gesture complete!", DebugFireGesture);
				SetSpell(Elements.Fire);
				return true;
			}
		}
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
				DebugUtils.Log("Air gesture Complete", DebugAirGesture);
				SetSpell(Elements.Air);
				return true;
			}
		}
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
				}
				else
				{
					m_WaterGestureStartPosition = hand.PalmPosition;
					return false;
				}
				
			}
		}
		// Set the x position to an invalid number
		m_WaterGestureStartPosition = null;
		return false;
	}

	bool DetectEarth(Hand hand)
	{
		if(hand.GrabStrength > EarthGrabValue)
		{
			DebugUtils.Log("Earth gesture Complete", DebugEarthGesture);
			SetSpell(Elements.Earth);
			return true;
		}
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
			Instantiate(SpellPrefabs[(int)element], m_AimIndicator.transform.position, Quaternion.identity);
			m_PreviousCastingType = element;
		}
	}
}