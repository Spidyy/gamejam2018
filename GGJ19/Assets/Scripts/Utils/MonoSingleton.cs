using UnityEngine;

/// MonoBehaviour persistant Singleton class
/// 
public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T s_instance = null;
    private static bool s_appExiting = false;

    public static T Instance
	{
		get 
		{
            if(s_appExiting)
            {
                return null;
            }

            if(s_instance == null)
			{
				FindInstance();
			}

			return s_instance;
		 }
	}

	/// Unity Awake 
	///
	private void Awake()
	{
		if(s_instance == null)
		{
			FindInstance();
		}

        if(s_instance != null)
        {
            DontDestroyOnLoad(s_instance.gameObject);
        }
	}

    /// Application is quitting 
    ///
    protected virtual void OnApplicationQuit()
    {
        s_appExiting = true;
    }

    /// Find and mitigate multiple instances of the same singleton behavour 
    ///
    private static void FindInstance()
	{
		T[] instances = GameObject.FindObjectsOfType<T>();

		int instanceCount = instances.Length;
		if(instanceCount== 0)
		{
			//--Create a new gameobject singleton instance 
			string[] delimited = typeof(T).ToString().Split(new char[] { '.' });
            string name = delimited[delimited.Length - 1];

            GameObject go = new GameObject(name);
            s_instance = go.AddComponent<T>();

            Debug.LogWarning("New MonoSingleton created: " + name);
        }
		else if(instanceCount > 0)
		{
			s_instance = instances[0];

			//--Cleat surplus instances found 
			if(instanceCount >= 1)
			{
				for(int i = 1; i < instanceCount; ++i)
				{
                    GameObject.Destroy(instances[i]);
				}
			}
		}
	}
}