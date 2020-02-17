using UnityEngine;
public class HoloEarth : MonoBehaviour
{
    public Transform planet,planetMesh,holder,top;
    public float baseRadius;
    private MeshRenderer mesh;
    public Vector3 axisAllowedR;
    public float scaleMax,scaleMin;

    private void Start()
    {
        mesh = planetMesh.GetComponent<MeshRenderer>();
        
    //    top.LookAt(Players.local.transform,Vector3.up);
    //    top.localEulerAngles = new Vector3(0, top.localEulerAngles.y, 0);
        
        baseRadius = GetRadius();
        
        float newDistance = GetRadius() - baseRadius;
        planet.localPosition = new Vector3(planet.localPosition.x, planet.localPosition.y, newDistance);

        hand1 = XRTracking.GetController(HandSide.Left);
        hand2 = XRTracking.GetController(HandSide.Right);
        
  
    }
    float GetRadius()
    {
	    Vector3 oldangle = mesh.transform.eulerAngles;
	    mesh.transform.eulerAngles = Vector3.zero;
	    
	    float radius = mesh.bounds.extents.z;
	    mesh.transform.eulerAngles = oldangle;
        return radius;
    }
    
    public XRController hand1,hand2;
	private Vector3 startPosition, currentPosition;
	private bool rotationStarted,scaleStarted;
	private Quaternion currentRotation, newRotation, rotationStep;

	private bool triggeredRotation,triggeredScale;
	void Update()
	{
	//	top.LookAt(Players.local.transform,Vector3.up);
	//	top.localEulerAngles = new Vector3(0, top.localEulerAngles.y, 0);
		//planet.LookAt(Players.local.transform,Vector3.up);
		XRController hand = null;
		if (hand1.IfButtonPressed(HandButtons.Trigger))
		{
			hand = hand1;
		}
		
		if(hand2.IfButtonPressed(HandButtons.Trigger))
		{
				hand = hand2;
		}
		
		if (hand!=null)
			{
				if (!rotationStarted)
				{
					UiControls.instance.Hide();
					currentRotation = Quaternion.LookRotation(hand.transform.position - holder.position, Vector3.up);
					currentRotation = Quaternion.Euler(axisAllowedR.x == 1 ? currentRotation.eulerAngles.x : 0,axisAllowedR.y == 1 ? currentRotation.eulerAngles.y : 0,axisAllowedR.z == 1 ? currentRotation.eulerAngles.z : 0);

					newRotation = currentRotation;
					rotationStarted = true;
				}
				else
				{
					
					currentRotation = Quaternion.LookRotation(hand.transform.position - holder.position, Vector3.up);
					currentRotation = Quaternion.Euler(axisAllowedR.x == 1 ? currentRotation.eulerAngles.x : 0,axisAllowedR.y == 1 ? currentRotation.eulerAngles.y : 0,axisAllowedR.z == 1 ? currentRotation.eulerAngles.z : 0);

					rotationStep = currentRotation * Quaternion.Inverse(newRotation);
					rotationStep = rotationStep * rotationStep; //2do; в угол, мультиплаить и обратно
					holder.rotation = rotationStep * holder.rotation;
					newRotation = Quaternion.LookRotation(hand.transform.position - holder.position, Vector3.up);

				//	newRotation = Quaternion.Lerp(newRotation,Quaternion.LookRotation(hand.transform.position - holder.position, Vector3.up),0.9f);
					newRotation = Quaternion.Euler(axisAllowedR.x == 1 ? newRotation.eulerAngles.x : 0,axisAllowedR.y == 1 ? newRotation.eulerAngles.y : 0,axisAllowedR.z == 1 ? newRotation.eulerAngles.z : 0);

					//	newRotation = Quaternion.Euler(0, newRotation.eulerAngles.y, 0);
				}
			}
			else
			{
				if (rotationStarted)
				{
					rotationStarted = false;
				}
			}
		
		
		triggeredScale = hand1.IfButtonPressed(HandButtons.Grip) && hand2.IfButtonPressed(HandButtons.Grip);

		if (triggeredScale)
		{
			if (!scaleStarted)
			{
				UiControls.instance.Hide();
				distanceBase = Vector3.Distance(hand1.transform.position, hand2.transform.position);
				scaleBase = holder.localScale;
				scaleStarted = true;
			}
			else
			{
				distance = Vector3.Distance(hand1.transform.position, hand2.transform.position);
				Vector3 newScale = scaleBase + (distance - distanceBase) * 1 * Vector3.one;
				newScale.x = Mathf.Clamp(newScale.x, scaleMin, scaleMax);
				newScale.y = Mathf.Clamp(newScale.y, scaleMin, scaleMax);
				newScale.z = Mathf.Clamp(newScale.z, scaleMin, scaleMax);
				ScaleAround (holder, holder.transform.position,  newScale);
				newDistance = GetRadius()-baseRadius;
				planet.localPosition = new Vector3(planet.localPosition.x, planet.localPosition.y, newDistance);
			
				//	holder.transform.localScale = newScale;
				//	holder.localScale = new Vector3(planet.localScale.z,planet.localScale.z,holder.localScale.z);
			}
		}
		else
		{
			if (scaleStarted)
			{
				scaleStarted = false;
			}
		}


		UiControls.instance.disabled = scaleStarted || rotationStarted;
	}

	public float newDistance;
	public float distanceBase,distance;
	public Vector3 scaleBase;
	private void FixedUpdate()
	{
		
		if (triggeredScale)
		{
			if (scaleStarted)
			{
			
			}
		}
		else
		{
		
		}
		
		if (triggeredScale)
		{
			

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
