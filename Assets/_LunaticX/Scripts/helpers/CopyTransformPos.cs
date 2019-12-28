using UnityEngine;

public class CopyTransformPos : MonoBehaviour
{
	[Space (10)]
	public Transform source;
	[Space (10)]
	public Transform target;
	[Space (10)]
	public bool position = true;
	public bool rotation = true;
	public Vector3 offsetPosition, offsetRotation;
	
	void Awake ()
	{

	}

	public void Update ()
	{
		if (source == null || target == null)
		{
			return;
		}

		if (position) 
		{
			source.position = target.position+offsetPosition;
		}

		if (rotation) 
		{
			source.rotation = target.rotation*Quaternion.Euler(offsetRotation);
		}
	}
}
