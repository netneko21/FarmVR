namespace ActionsIO
{
    [ComponentIdentifierAttribute(action = ItemActions.ClearAllActions)]
    public class ClearAllActions : ActionIO
    {
        public override void Run()
        {
            Validate();
            //base.Awake();//ignore action list

            io.ClearActions();
            Remove();
        }
    }
}

