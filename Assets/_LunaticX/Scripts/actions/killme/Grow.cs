using UnityEngine;

namespace ActionsIO
{
    [ComponentIdentifierAttribute(action = ItemActions.Grow)]
    public class Grow : ActionIO
    {
        private Vector3 startScale;
        private float timer;
        public override void Run()
        {
            Validate();
//            startScale = io.baseScale;
            transform.localScale = Vector3.zero;
        }
        
        void Update ()
        {
            if (mode == ActionMode.Run)
            {
                timer += Time.deltaTime;
                transform.localScale = startScale * timer;
                if (timer >= 1)
                {
                    Finished();
                }
            }
        }

        void Finished()
        {
            mode = ActionMode.Stop;
            transform.localScale = startScale;
        }
    }
}