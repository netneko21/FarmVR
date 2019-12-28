using UnityEngine;

public class TouchFocus : Focus
{
 public LayerMask layers;
    public float distance;
    public XRLineRenderer lineRenderer;
    private bool missed;
    
    private RaycastHit hit;
    private Collider currentHitCollider; 
    private Collider raycastCollider;

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
        Debug.DrawRay (transform.position, transform.forward*distance, Color.red, 3);
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
            {//
                currentHitCollider.GetComponent<ChildCollider>().OnTriggerExit(raycastCollider);//force event same as on touch
            }

            currentHitCollider = hit.collider;
            currentHitCollider.GetComponent<ChildCollider>().OnTriggerEnter(raycastCollider);
        }
    }

    public void Out()
    {
        if (currentHitCollider) 
        {
            currentHitCollider.GetComponent<ChildCollider>().OnTriggerExit(raycastCollider);
            currentHitCollider = null;
        }
    }
}


