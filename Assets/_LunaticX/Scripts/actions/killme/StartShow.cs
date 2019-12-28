namespace ActionsIO
{
    [ComponentIdentifierAttribute(action = ItemActions.StartShow)]
    public class StartShow : ActionIO
    {
        public override void Run()
        {
            ShowArea.instance.ActivateForObject(io);
        }
    }
}

