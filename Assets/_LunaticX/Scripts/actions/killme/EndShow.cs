namespace ActionsIO
{
    [ComponentIdentifierAttribute(action = ItemActions.EndShow)]
    public class EndShow : ActionIO
    {
        public override void Run()
        {
            Validate();
            ShowArea.instance.Deactivate();
        }
    }
}

