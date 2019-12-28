using UnityEngine;

public class TransformFollowMe : MonoBehaviour
{
	[Space (10)]
	public Transform target;
	[Space (10)]
	public bool position = true;
	public bool rotation = true;
	public Vector3 offsetPosition, offsetRotation;

	public void Update ()
	{
		if (target == null)
		{
			return;
		}

		if (position) 
		{
			target.position = transform.position+offsetPosition;
		}

		if (rotation) 
		{
			target.rotation = transform.rotation*Quaternion.Euler(offsetRotation);
		}
	}
}
