using UnityEngine;
using System.Collections;

public class SpellCastLogic : MonoBehaviour {

	public LayerMask RayCastLayerMask;
	public float RayCastRange = 100.0f;

	public GameObject[] SpellPrefabs = new GameObject[(int)Elements.Count];

	private GameObject m_AimIndicator;
	// Use this for initialization
	void Start () 
	{
		m_AimIndicator = transform.Find("AimIndicator").gameObject;
		DebugUtils.Assert(m_AimIndicator != null, "Couldn't find child AimIndicator");
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (IsAiming())
		{
			Aim();
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

	void Aim()
	{
		Ray ray;
		RaycastHit hit;
		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out hit, RayCastRange, RayCastLayerMask))
		{
			if (hit.collider.name == "FloorPlane")
			{

				m_AimIndicator.transform.position = hit.point;
				m_AimIndicator.SetActive(true);
			}
		}
	}

	bool IsAiming()
	{
		return Input.GetKey(KeyCode.A);
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
