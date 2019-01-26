using UnityEngine;

/// MonoBehaviour persistant Singleton class
/// 
public sealed class Singleton<T> where T : MonoBehaviour
{
	private static T s_instance = null;

	public T Instance
	{
		get 
		{
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
	}

	/// Find and mitigate multiple instances of the same singleton behavour 
	///
	private void FindInstance()
	{
		T[] instances = GameObject.FindObjectsOfType<T>();

		int instanceCount = instances.Length;
		if(instanceCount== 0)
		{
			//--Create a new gameobject singleton instance 
			GameObject go = new GameObject(typeof(T).ToString());
			s_instance = go.AddComponent<T>();
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