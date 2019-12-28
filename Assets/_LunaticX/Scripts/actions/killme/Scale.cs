using UnityEngine;

namespace ActionsIO
{
    [ComponentIdentifierAttribute(action = ItemActions.Scale)]
    public class Scale : ActionIO
    {
        private float timer;
        Vector3 startScale;
        public override void Run()
        {
            Validate();
            startScale = transform.localScale;
        }
        
        float maxSize = 1.5f;
        float minSize = 0.5f;
        
        void Update ()
        {
            if (mode == ActionMode.Run)
            {
                timer += Time.deltaTime;
                var range = maxSize - minSize;
                transform.localScale =startScale* ((Mathf.Sin(timer) + 1.0f) / 2.0f * range + minSize);
            }
        }
    }
}