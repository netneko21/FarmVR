namespace ActionsIO
{
    [ComponentIdentifierAttribute(action = ItemActions.GrabMe)]
    public class GrabMe : ActionIO
    {
        public override void Run()
        {
            Validate();
            TryGrab();
        }
        
        public void TryGrab()
        {
           //RaycasterFocus.TryGrab()
        }
        
    }
}

