namespace ActionsIO
{
    [ComponentIdentifierAttribute(action = ItemActions.Deactivate)]
    public class Deactivate : ActionIO
    {
        public override void Run ()
        {
            Validate();
            GetComponent<InteractiveObject>().UpdateState(ItemState.Activated,false);
        }
    }
}