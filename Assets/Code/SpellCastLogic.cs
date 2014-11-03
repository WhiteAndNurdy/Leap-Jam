using UnityEngine;
using System.Collections;

public class SpellCastLogic : MonoBehaviour {

	public LayerMask RayCastLayerMask;
	public float RayCastRange = 100.0f;

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
}
