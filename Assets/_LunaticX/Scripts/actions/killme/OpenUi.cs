namespace ActionsIO
{
    [ComponentIdentifierAttribute(action = ItemActions.OpenUI)]
    public class OpenUI : ActionIO
    {
        public override void Run()
        {
            Validate();
            UiControls.instance.Show(io.gameObject.GetComponent<Marker>());
        }
    }
}

