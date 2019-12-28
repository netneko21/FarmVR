using UnityEngine;

public class RotateAround : MonoBehaviour
{
	public XRController hand1,hand2;
	private Vector3 startPosition, currentPosition;
	private bool rotationStarted;
	private Quaternion currentRotation, newRotation, rotationStep;
	public Transform holder;
//	public HandButtons buttons;
	public ShowAreaProperties pp;


	private void Reset ()
	{
		hand1 = null;
		hand2 = null;
		holder.transform.localRotation = Quaternion.identity;
	}

	public void Init(ShowAreaProperties _properties)
	{
		Reset();
		pp = _properties;
		if (pp.handR == HandSide.Dual)
		{
			hand1 = XRTracking.GetController(HandSide.Left);
			//hand2 = XRTracking.GetController(HandSide.Right);
		}
		else
		{
			hand1 = XRTracking.GetController(pp.handR);
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
			if (pp.handR == HandSide.Dual)
			{
				triggered = hand1.IfButtonPressed(pp.buttonsR) && hand2.IfButtonPressed(pp.buttonsR);
			}
			else
			{
				triggered = hand1.IfButtonPressed(pp.buttonsR);
			}


			if (triggered)
			{
				if (!rotationStarted)
				{
					currentRotation = Quaternion.LookRotation(hand1.transform.position - holder.position, Vector3.up);

					currentRotation = Quaternion.Euler(pp.axisAllowedR.x == 1 ? currentRotation.eulerAngles.x : 0,
						pp.axisAllowedR.y == 1 ? currentRotation.eulerAngles.y : 0,
						pp.axisAllowedR.z == 1 ? currentRotation.eulerAngles.z : 0);

					newRotation = currentRotation;
					rotationStarted = true;
				}
				else
				{
					currentRotation = Quaternion.LookRotation(hand1.transform.position - holder.position, Vector3.up);
					currentRotation = Quaternion.Euler(pp.axisAllowedR.x == 1 ? currentRotation.eulerAngles.x : 0,
						pp.axisAllowedR.y == 1 ? currentRotation.eulerAngles.y : 0,
						pp.axisAllowedR.z == 1 ? currentRotation.eulerAngles.z : 0);

					rotationStep = currentRotation * Quaternion.Inverse(newRotation);
					rotationStep = rotationStep * rotationStep; //2do; в угол, мультиплаить и обратно
					holder.rotation = rotationStep * holder.rotation;
					
					newRotation = Quaternion.Lerp(newRotation,Quaternion.LookRotation(hand1.transform.position - holder.position, Vector3.up),0.9f);

					//newRotation = Quaternion.LookRotation(hand1.transform.position - holder.position, Vector3.up);
					newRotation = Quaternion.Euler(pp.axisAllowedR.x == 1 ? newRotation.eulerAngles.x : 0,
						pp.axisAllowedR.y == 1 ? newRotation.eulerAngles.y : 0,
						pp.axisAllowedR.z == 1 ? newRotation.eulerAngles.z : 0);

					newRotation = Quaternion.Euler(0, newRotation.eulerAngles.y, 0);

				}
			}
			else
			{
				if (rotationStarted)
				{
					rotationStarted = false;
				}
			}
		}
	}
}