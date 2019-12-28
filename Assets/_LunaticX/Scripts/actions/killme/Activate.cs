namespace ActionsIO
{
    [ComponentIdentifierAttribute(action = ItemActions.Activate)]
    public class Activate : ActionIO
    {
        public override void Run()
        {
            Validate();
            GetComponent<InteractiveObject>().UpdateState(ItemState.Activated,true);
        }
    }
}

