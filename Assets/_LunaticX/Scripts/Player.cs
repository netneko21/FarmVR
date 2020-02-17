using System.Collections.Generic;

using UnityEngine;

[DisallowMultipleComponent]
public class Player : MonoBehaviour
{
	[SerializeField] private List <LinkedTransform> XRLinkedTransforms = new List <LinkedTransform> ();
	void Awake ()
	{

	}
	
	[SerializeField] private Hand[] hands;
	public Transform avatarHead;
	void Start()
	{
		XRTracking.instance.RecenterOnTarget(avatarHead);
		Renderer[] rends = avatarHead.GetComponentsInChildren <Renderer> (true);
		foreach (var v in rends) v.enabled = false;
		
		XRTracking.ClearLinks();
		foreach (LinkedTransform linkedTransform in XRLinkedTransforms)
		{
			XRTracking.AddLinkedTransform(linkedTransform.transform,linkedTransform.node);
		}

		((XRController)XRTracking.leftController).SetHand(hands[(int)HandSide.Left]);
		((XRController)XRTracking.rightController).SetHand(hands[(int)HandSide.Right]);
	}
	
	public Hand GetHands(HandSide _side)
	{
		return hands[(int)_side];
	}
	
	//	_anchor.localPosition = Vector3.Lerp(_anchor.localPosition,InputTracking.GetLocalPosition(_trackedDevice),Time.deltaTime*10);
		//_anchor.localRotation = Quaternion.Lerp(_anchor.localRotation,InputTracking.GetLocalRotation(_trackedDevice),Time.deltaTime*10);
        
	//	if (Players.local != null)
	//	{
	//		Players.local.UpdateTrackedTransforms(_trackedDevice, _anchor);
	//	}
	 

	
	public void Update ()
	{
		if (Input.GetKeyUp(KeyCode.Space))
		{
			XRTracking.instance.RecenterOnTarget(avatarHead);
		}
	}

	public Transform fingertip;

	public Transform leftHandT, rightHandT;
	public float height = 1.5f,heightDiff = 1.5f;

	public void LateUpdate ()
	{/*
		if (isLocal)
		{
			transform.position = XRTracking.instance.headAnchor.transform.position - Vector3.up*heightDiff;
			transform.rotation = Quaternion.Euler(0,XRTracking.instance.headAnchor.transform.eulerAngles.y,0);
			
			leftHandT.position = XRTracking.instance.leftAnchor.transform.position;
			leftHandT.rotation = XRTracking.instance.leftAnchor.transform.rotation*Quaternion.Euler(0,0,90);

			rightHandT.position = XRTracking.instance.rightAnchor.transform.position;
			rightHandT.rotation = XRTracking.instance.rightAnchor.transform.rotation*Quaternion.Euler(0,0,-90);;
			
		}*/
	}

	
	void OnDestroy ()
	{
		Debug.Log("OnDestroy player");
	}
}
