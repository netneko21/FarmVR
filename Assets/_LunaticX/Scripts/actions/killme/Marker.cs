using UnityEngine;

public class Marker : MonoBehaviour
    {
        public Transform[] squares;
        public Transform circle;
        public Transform uiRef;
        public Transform planet,rotator;
        public Vector3 bounds;
        public Vector3 origin;
        public UiControls ui;
        public virtual void Awake()
        {
            foreach (Transform square in squares)
            {
              //  square.transform.localEulerAngles = new Vector3(square.transform.localEulerAngles.x,Random.Range(0,360),square.transform.localEulerAngles.z);
            }
        }

        public void Update()
        {
            float s = 1;//rotator.localScale.x;
            circle.transform.Rotate(Vector3.up, Time.deltaTime * 10);
            if (circle.transform.position.x < origin.x-s*bounds.x*0.45f || circle.transform.position.x > origin.x+s*bounds.x*0.45f
                ||circle.transform.position.y < origin.y-s*bounds.y*0.45f || circle.transform.position.y > origin.y+s*bounds.y*0.45f
                ||circle.transform.position.z < origin.z-s*bounds.z*0.45f || circle.transform.position.z > origin.z+s*bounds.z*0.45f
                )
            {
                ToggleMe(false);
            }
            else
            {
                ToggleMe(true);
            }
        }

        public void ToggleMe(bool _enabled)
        {
            foreach (Transform square in squares)
            {
                square.gameObject.SetActive(_enabled);
            }
            circle.gameObject.SetActive(_enabled);
        }

        public void ActivateMe()
        {
            ui.Show(this);
        }
    }


