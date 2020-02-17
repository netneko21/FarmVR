using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class RaycasterFocus : Focus
{
    public LayerMask layers;
    public float distance;
    public XRLineRenderer lineRenderer;
    private bool missed;
    
    private RaycastHit hit;
    private Collider currentHitCollider; 
    private Collider raycastCollider;
    private Collider lastGazed,gazeTarget;
    public InteractiveObject currentIO;
    public float delayInSeconds = 0.5f;
    public float loadingTime;
    public Image circle;
    public bool gazeEnabled;
    Coroutine gazeControl; // Keep a single gaze control coroutine for better performance.
    public InteractiveObject io;
    
    private void Start()
    {
        raycastCollider = GetComponent<Collider>();
        lineRenderer = GetComponent<XRLineRenderer>();
   
        missed = false;
    }

    private void Update()
    {
        Raycast();
    }

    public void Raycast()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hit, distance != 0 ? distance : Mathf.Infinity,layers))
        {
            missed = false;
            if (lineRenderer)
            {
                lineRenderer.SetPosition(0,transform.localPosition);
                lineRenderer.SetPosition(1,transform.InverseTransformPoint(hit.point));
            }
       
            Over();
        }
        else
        {
           // if (missed == false)
           // {
                missed = true;
                if (lineRenderer)
                {
                    lineRenderer.SetPosition(0, transform.localPosition);
                    lineRenderer.SetPosition(1, transform.InverseTransformPoint(transform.position + transform.forward));
                }
                
                Out();
          //  }
        }
    }

    void Over()
    {
        if (hit.collider != currentHitCollider)
        {
            if (currentHitCollider)
            {
                if (currentHitCollider.GetComponent<ChildCollider>())
                {
                    currentHitCollider.GetComponent<ChildCollider>()
                        .OnTriggerExit(raycastCollider); //force event same as on touch
                }
                else
                {
                    currentHitCollider.GetComponent<InteractiveObject>()
                        .TryRemoveFocus(raycastCollider.GetComponent<Focus>());
                }
            }

            currentHitCollider = hit.collider;
            gazeTarget = null;
            
            if (currentHitCollider.GetComponent<ChildCollider>())
            {
                currentHitCollider.GetComponent<ChildCollider>().OnTriggerEnter(raycastCollider);
            }
            else
            {
                currentHitCollider.GetComponent<InteractiveObject>().TryAddFocus(raycastCollider.GetComponent<Focus>());
            }
        }
        else
        {
            if (gazeEnabled)
            {
                if (currentHitCollider)
                {
                    io = currentHitCollider.GetComponentInParent<InteractiveObject>();
                    if (io)
                    {
                        if (gazeTarget == currentHitCollider)
                            {
                                return;
                            }
                        
                        if (currentHitCollider != gazeTarget)
                            {
                                circle.fillAmount = 0f;
                                gazeTarget = currentHitCollider;
                            }

                            if (gazeControl != null)
                            {
                                StopCoroutine(gazeControl);
                            }

                            gazeControl = StartCoroutine(FillCircle(currentHitCollider.transform));
                       
                    }
                    else
                    {
                        if (null != gazeControl)
                        {
                            StopCoroutine(gazeControl);
                        }

                        ResetGazer();
                    }
                }
                else
                {
                    if (null != gazeControl)
                    {
                        StopCoroutine(gazeControl);
                    }

                    ResetGazer();
                }
            }
        }
    }

    public void Out()
    {
        if (currentHitCollider) 
        {
            if (currentHitCollider.GetComponent<ChildCollider>())
            {
                currentHitCollider.GetComponent<ChildCollider>().OnTriggerExit(raycastCollider);//force event same as on touch
            }
            else
            {
                currentHitCollider.GetComponent<InteractiveObject>().TryRemoveFocus(raycastCollider.GetComponent<Focus>());
            }

            currentHitCollider = null;
            if (gazeEnabled)
            {
                if (null != gazeControl)
                {
                    StopCoroutine(gazeControl);
                }

                ResetGazer();
            }
            
        }
    }
    
    private IEnumerator FillCircle(Transform target)
    {
        // When the circle starts to fill, reset the timer.
        float timer = 0f;
        circle.fillAmount = 0f;

        yield return new WaitForSeconds(delayInSeconds);

        while (timer < loadingTime)
        {
            timer += Time.deltaTime;
            circle.fillAmount = timer / loadingTime;
            yield return null;
        }
        
        circle.fillAmount = 0f;
        SendEvent(ItemEvents.onTriggerReleased);
    }

    private void ResetGazer()
    {
        if (circle == null) { return; }
        circle.fillAmount = 0f;
        gazeTarget = null;
    }
}


