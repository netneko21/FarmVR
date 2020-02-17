public class ScriptsChecker : Singleton<ScriptsChecker>
{
	protected override void OnAwake ()
	{
		ScriptsCheck();
	}
	
	void ScriptsCheck()
	{
		if (XRTracking.instance){}
		if (ActionsManager.instance){}
	}
}
