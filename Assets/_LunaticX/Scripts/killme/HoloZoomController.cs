using UnityEngine;

public class HoloZoomController : MonoBehaviour
{
	
	public XRController hand1,hand2;
	private Vector3 startPosition, currentPosition,offset;

	
	public Transform holder,origin,pivot;
//	public HandButtons buttons;
	public ShowAreaProperties pp;
		
	public Transform scalePoint;

	private bool zoomStarted;
	private Vector3 scaleBase;
	private float distance,multiplier,distanceBase;
	public float minS, maxS;

	public void Reset()
	{
		holder.localPosition = Vector3.zero;
	}
	
	

	public void Init(ShowAreaProperties _properties)
	{
	hand1 = null;
	hand2 = null;
		Reset();
		pp = _properties;
		if (pp.handS == HandSide.Dual)
		{
			hand1 = XRTracking.GetController(HandSide.Left);
			hand2 = XRTracking.GetController(HandSide.Right);
		}
		else
		{
			hand1 = XRTracking.GetController(pp.handS);
		}
		
	}
	
	private ShowArea showArea;
	public void Awake()
	{
		showArea = GetComponent<ShowArea>();
	}

	private bool triggered;
	void Update()
	{
		if (pp)
		{
			triggered = false;
			if (pp.handS == HandSide.Dual)
			{
				triggered = hand1.IfButtonPressed(pp.buttonsS) && hand2.IfButtonPressed(pp.buttonsS);
			}
			else
			{
				triggered = hand1.IfButtonPressed(pp.buttonsS);
			}
			
			if (triggered)
			{
				if (!zoomStarted)
				{
					distanceBase = Vector3.Distance(hand1.transform.position, hand2.transform.position);
					scaleBase = holder.localScale;
					zoomStarted = true;
				}
				else
				{
					distance = Vector3.Distance(hand1.transform.position, hand2.transform.position);
				
					Vector3 newScale = scaleBase + (distance - distanceBase) * 2 * Vector3.one;
					newScale.x = Mathf.Clamp(newScale.x, minS, maxS);
					newScale.y = Mathf.Clamp(newScale.y, minS, maxS);
					newScale.z = Mathf.Clamp(newScale.z, minS, maxS);
					ScaleAround (holder, scalePoint.transform.position,  newScale);
				}
			}
			else
			{
				if (zoomStarted)
				{
					zoomStarted = false;
				}
			}
		}
	}

	static void ScaleAround(Transform _transform, Vector3 _worldPos, Vector3 _newScale)
	{
		Vector3 localScalePos = _transform.InverseTransformPoint (_worldPos);
		Vector3 scaleVector = _transform.localPosition - localScalePos;
		Vector3 oldScale = _transform.localScale;
		Vector3 scaleRatio = Div (_newScale, oldScale);
		_transform.localScale = _newScale;
		_transform.localPosition = Scale (scaleVector, scaleRatio) + localScalePos;
	}
 
	static Vector3 Scale(Vector3 _a, Vector3 _b)
	{
		return new Vector3 (_a.x * _b.x, _a.y * _b.y, _a.z * _b.z);
	}
 
	static Vector3 Div(Vector3 _a, Vector3 _b)
	{
		return new Vector3 (_b.x == 0f ? 0 : _a.x / _b.x, _b.y == 0f ? 0 : _a.y / _b.y, _b.z == 0f ? 0 : _a.z / _b.z);
	}
}