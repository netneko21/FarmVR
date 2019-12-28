using UnityEngine;

namespace ActionsIO
{
    [ComponentIdentifierAttribute(action = ItemActions.Reset)]
    public class Reset : ActionIO
    {
        private Vector3 startScale,startPos;
            private float timer;
            public override void Run()
            {
                Validate();
                io.transform.parent = io.baseParent;
                timer = 0;
            }
        
            void Update ()
            {
                if (timer <= 1)
                {
                    timer += Time.deltaTime;
                 //       ..   io.transform.localPosition = Vector3.Lerp(io.transform.localPosition,io.baseLocalPos,0.1f);
               ////     io.transform.localScale = Vector3.Lerp(io.transform.localScale,io.baseScale,0.1f);
             //       io.transform.localEulerAngles = MathX.AngleLerp(io.transform.localEulerAngles,io.baseLocalRot,0.1f);
                }
                else
                {
                    Finished();
                }
            }

            void Finished()
            {
                mode = ActionMode.Stop;
             //   io.transform.localPosition = io.baseLocalPos;
           //     io.transform.localScale = io.baseScale;
           //     io.transform.localEulerAngles = io.baseLocalRot;
            }
        }
    }


