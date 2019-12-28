using UnityEngine;

public class UiControls : MonoBehaviour
{
    public bool disabled;
    private float timer=2;
    private Marker current;
        public void Show(Marker source)
        {
            if(disabled)return;
            if (current == source) {return;}
            current = source;
            gameObject.SetActive(true);
            finished = false;
            timer = 0;
            targetScale = Vector3.one;
            transform.position = source.circle.position;
            startPosition = source.circle.position;
            targetPosition = basePosition;
        }
        
        public void Hide()
        {
            current = null;
            finished = false;
            timer = 0;
            targetScale = Vector3.one*0.01f;
            targetPosition = startPosition;
        }
        
        public void GoTo()
        {
    
        }

        public static UiControls instance;
        private Vector3 startPosition, basePosition,targetPosition;
        private void Awake()
        {
            Hide();
            instance = this;
        }
        private void Start()
        {
            basePosition = transform.position;
        }

        private bool finished=true;
        private Vector3 targetScale;
        public void Update()
        {
            if (timer < 1)
            {
                timer += Time.deltaTime;
                transform.localScale = Vector3.Lerp(transform.localScale,targetScale,timer);
                transform.position = Vector3.Lerp(transform.position,targetPosition,timer);
            }
            else
            {
                if (!finished)
                {
                    transform.localScale = targetScale;
                             transform.position = targetPosition;
                    finished = true;
                }
            }
          //  if (Input.GetKeyUp(KeyCode.Q))
          //  {
          //      Show();
          //  }
            
           // if (Input.GetKeyUp(KeyCode.E))
           // {
           //     Hide();
           // }
        }
        
        
    }


