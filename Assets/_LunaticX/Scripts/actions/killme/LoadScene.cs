using UnityEngine.SceneManagement;

namespace ActionsIO
{
    [ComponentIdentifierAttribute(action = ItemActions.LoadScene)]
    public class LoadScene : ActionIO
    {
        public override void Run()
        {
            Validate();
            //  SceneLoader.instance.Load(UiControls.instance.sceneToLoad,LoadSceneMode.Single);
           
        }
    }
}

