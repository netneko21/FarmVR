using UnityEngine;

public class FreeCamera : MonoBehaviour
{
    public bool allowRotation = true;
    public float cursorSensitivity = 0.025f;

    private void Awake()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            Destroy(this);
        }
     }

    private void Update()
    {

       // allowRotation = Input.GetMouseButton(2);

       // if (allowRotation)
       // {
            Vector3 eulerAngles = transform.eulerAngles;
            eulerAngles.x += -Input.GetAxis("Mouse Y") * 359f * cursorSensitivity;
            eulerAngles.y += Input.GetAxis("Mouse X") * 359f * cursorSensitivity;
            transform.eulerAngles = eulerAngles;
       // }

    }
}
