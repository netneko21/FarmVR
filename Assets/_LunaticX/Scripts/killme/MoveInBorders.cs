using UnityEngine;

public class MoveInBorders : MonoBehaviour
{

	public XRController hand1,hand2;
	private Vector3 startPosition, currentPosition,offset;
	public Vector3 bounds;

	private bool movementStarted;
	public Transform holder,origin;
//	public HandButtons buttons;
	public ShowAreaProperties pp;


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
		if (pp.handM == HandSide.Dual)
		{
			hand1 = XRTracking.GetController(HandSide.Left);
			//hand2 = XRTracking.GetController(HandSide.Right);
		}
		else
		{
			hand1 = XRTracking.GetController(pp.handM);
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
			if (pp.handM == HandSide.Dual)
			{
				triggered = hand1.IfButtonPressed(pp.buttonsM) && hand2.IfButtonPressed(pp.buttonsM);
			}
			else
			{
				triggered = hand1.IfButtonPressed(pp.buttonsM);
			}
			
			if (triggered)
			{
				if (!movementStarted)
				{
					offset = hand1.transform.position - holder.transform.position;
					movementStarted = true;
				}
				else
				{
					Vector3 newPosition = hand1.transform.position - offset;//переписать под два степа для ускорения
					newPosition.x = Mathf.Clamp(newPosition.x, -bounds.x+origin.position.x, bounds.x+origin.position.x);
					newPosition.y = Mathf.Clamp(newPosition.y, -bounds.y+origin.position.y, bounds.y+origin.position.y);
					newPosition.z = Mathf.Clamp(newPosition.z, -bounds.z+origin.position.z, bounds.z+origin.position.z);
					holder.transform.position = Vector3.Lerp(holder.transform.position,newPosition,Time.deltaTime*4);
				}
			}
			else
			{
				if (movementStarted)
				{
					movementStarted = false;
				}
			}
		}
	}
}