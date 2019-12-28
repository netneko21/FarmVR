using UnityEngine;

namespace ActionsIO
{
    [ComponentIdentifierAttribute(action = ItemActions.Move)]
    public class Move : ActionIO
    {
        private float min=1f;
        private float max=3f;
        private float startPos;
        private float timer;
        public override void Run()
        {
            Validate();
            startPos = transform.position.x;
            min = startPos;
            max = startPos + 2;
        }
        
        void Update ()
        {
            if (mode == ActionMode.Run)
            {
                timer += Time.deltaTime*0.5f;
                transform.position = new Vector3( Mathf.PingPong(timer, max - min) + min,
                    transform.position.y, transform.position.z);
            }
        }
    }
}