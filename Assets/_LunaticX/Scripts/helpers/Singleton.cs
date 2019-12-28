using UnityEngine;
public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T _instance;
	private static readonly object Lock = new object();
	public static T instance
	{
		get
		{
			if (applicationIsQuitting) {return null;}
			lock(Lock)
			{
				if (_instance == null)
				{
					_instance = (T) FindObjectOfType(typeof(T));
					if (_instance == null)
					{
						GameObject singleton = null;
						GameObject singletonPrefab = (GameObject)Resources.Load(typeof(T).ToString(), typeof(GameObject));
						if (singletonPrefab != null) 
						{
							singleton = Instantiate (singletonPrefab);
							_instance = singleton.GetComponent<T> ();
						} else {
							singleton = new GameObject();
							_instance = singleton.AddComponent<T> ();
						}
						
						singleton.name = "_"+ typeof(T).ToString();						
						DontDestroyOnLoad(singleton);
					}
					else 
					{
						DontDestroyOnLoad(_instance.gameObject);
						_instance.gameObject.name = "_"+ typeof(T);	
					}
				}
				
				return _instance;
			}
		}
	}

	private static bool applicationIsQuitting = false;
	public void OnDestroy () {applicationIsQuitting = true;}
	protected void Awake ()
	{
		if(instance!=null&&instance!=this){Destroy(this);Destroy(gameObject);}
		OnAwake();
	}

	protected virtual void OnAwake(){}
}



