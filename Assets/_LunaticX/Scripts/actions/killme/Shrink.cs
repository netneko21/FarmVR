using UnityEngine;

namespace ActionsIO
{
    [ComponentIdentifierAttribute(action = ItemActions.Shrink)]
    public class Shrink : ActionIO
    {
        private Vector3 startScale;
        private float timer;
        public override void Run()
        {
            Validate();
            startScale = transform.localScale;
            timer = 1;
        }
        
        void Update ()
        {
            if (mode == ActionMode.Run)
            {
                timer -= Time.deltaTime;
                transform.localScale = startScale * timer;
                if (timer <= 0)
                {
                    Finished();
                }
            }
        }

        void Finished()
        {
            mode = ActionMode.Stop;
            transform.localScale = Vector3.zero;
            Remove();
        }
    }
}