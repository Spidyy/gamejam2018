//
// Copyright (c) 2016 Tag Games Ltd. All rights reserved
//

namespace TagFramework
{
	﻿using UnityEngine;
	using System.Collections.Generic;

	//-----------------------------------------------------------------
	/// Extension class for adding additional functionality to GameObject
	/// 
	/// @author M Canala
	//-----------------------------------------------------------------
	public static class GameObjectUtils
	{
		//-----------------------------------------------------------------
		/// Search recursively the first instance of a child with the given name
		/// 
		/// @author M Canala
		/// 
		/// @param in_childName - The name of the child to search for
		///
		/// @return The first instance of a child with the given name, or null
		//-----------------------------------------------------------------
		public static GameObject FindChildByName(this GameObject in_gameObject, string in_childName)
		{
			GameObject returnValue = null;
		
			Transform rootTransform = in_gameObject.transform;
			for(int childIndex = 0; childIndex < rootTransform.childCount && (returnValue == null); ++childIndex)
			{
				Transform childTransform = rootTransform.GetChild(childIndex);
				if(childTransform.name == in_childName)
					returnValue = childTransform.gameObject;
				else
					returnValue = childTransform.gameObject.FindChildByName(in_childName);
			}
			
			return returnValue;
		}

		//-----------------------------------------------------------------
		/// Search recursively the first instance of a child with the given tag
		/// 
		/// @author M Canala
		/// 
		/// @param in_childTag - The tag of the child to search for
		///
		/// @return The first instance of a child with the given tag, or null
		//-----------------------------------------------------------------
		public static GameObject FindChildByTag(this GameObject in_gameObject, string in_childTag)
		{
			GameObject returnValue = null;
			
			Transform rootTransform = in_gameObject.transform;
			for (int childIndex = 0; childIndex < rootTransform.childCount && (returnValue == null); ++childIndex)
			{
				Transform childTransform = rootTransform.GetChild(childIndex);
				if(childTransform.tag == in_childTag)
					returnValue = childTransform.gameObject;
				else
					returnValue = childTransform.gameObject.FindChildByTag(in_childTag);
			}
			
			return returnValue;
		}

		//-----------------------------------------------------------------
		/// Search recursively for instances of children with the given name
		/// 
		/// @author A Maclean
		/// 
		/// @param in_childName - The name of the child to search for
		///
		/// @return The instances of children with the given name, or an empty list
		//-----------------------------------------------------------------
		public static GameObject[] FindChildrenByName(this GameObject in_gameObject, string in_childName)
		{
			List<GameObject> returnChildren = new List<GameObject>();
			
			Transform rootTransform = in_gameObject.transform;
			for(int childIndex = 0; childIndex < rootTransform.childCount; ++childIndex)
			{
				Transform childTransform = rootTransform.GetChild(childIndex);
				if(childTransform.name == in_childName)
				{
					if(!returnChildren.Contains(childTransform.gameObject))
					{
						returnChildren.Add(childTransform.gameObject);
					}
				}
				else
				{
					GameObject[] found = childTransform.gameObject.FindChildrenByName(in_childName);
					if(found.Length > 0)
					{
						returnChildren.AddRange(found);
					}
				}
			}
			
			return returnChildren.ToArray();
		}

		//-----------------------------------------------------------------
		/// Search recursively for instances of children with the given tag
		/// 
		/// @author A Maclean
		/// 
		/// @param in_childName - The tag to search for
		///
		/// @return The instances of children with the given tag, or an empty list
		//-----------------------------------------------------------------
		public static GameObject[] FindChildrenByTag(this GameObject in_gameObject, string in_tag)
		{
			List<GameObject> returnChildren = new List<GameObject>();
			
			Transform rootTransform = in_gameObject.transform;
			for(int childIndex = 0; childIndex < rootTransform.childCount; ++childIndex)
			{
				Transform childTransform = rootTransform.GetChild(childIndex);
				if(childTransform.tag == in_tag)
				{
					if(!returnChildren.Contains(childTransform.gameObject))
					{
						returnChildren.Add(childTransform.gameObject);
					}
				}
				else
				{
					GameObject[] found = childTransform.gameObject.FindChildrenByTag(in_tag);
					if(found.Length > 0)
					{
						returnChildren.AddRange(found);
					}
				}
			}
			
			return returnChildren.ToArray();
		}

		//------------------------------------------------------------------
		/// Uses "FindChildByName" utility function then returns the desired
		/// component
		/// 
		/// @author A Maclean
		/// 
		/// @param in_childName - The name of the child game object
		/// @param in_parentObject - The parent of the game object to start the search from
		/// 
		/// @return The instance holding the found component, or default(T)
		//------------------------------------------------------------------ 
		public static T FindComponentFromChildObject<T>(this GameObject in_parentObject, string in_childName)
		{
			GameObject foundObject = in_parentObject.FindChildByName(in_childName);
			if(foundObject == null)
			{
				LogUtils.WarningLog("GameObjectUtils::FindComponentFromChildObject - Cannot find {0} as a child of {1} ", in_childName, in_parentObject.name);
				return default(T);
			}

			T component = foundObject.GetComponent<T>();
			if(component == null)
			{
				LogUtils.WarningLog("GameObjectUtils::FindComponentFromChildObject - Cannot find component type: {0} on {1}", typeof(T), in_childName);
				return default(T);
			}
			
			return component;
		}

		//------------------------------------------------------------------
		/// Search recursively for instances of children with the given string
		/// in the name.
		///
		/// @author M Canala
		/// 
		/// @param in_stringTocheck - the string to check
		/// @param in_result - the dictionary filled wih all results
		/// 
		/// @return Gameobjects containing the passed string into the name
		//------------------------------------------------------------------ 
		public static void FindChildrenWithStringInName(this GameObject in_gameObject, string in_stringTocheck, ref Dictionary<string, GameObject> in_result)
		{
			if(in_result == null)
			{
				in_result = new Dictionary<string, GameObject>();
			}

			Transform rootTransform = in_gameObject.transform;
			for(int childIndex = 0; childIndex < rootTransform.childCount; ++childIndex)
			{
				Transform childTransform = rootTransform.GetChild(childIndex);
				if(childTransform.name.Contains(in_stringTocheck))
				{
					if(!in_result.ContainsKey(childTransform.gameObject.name))
					{
						in_result.Add(childTransform.gameObject.name, childTransform.gameObject);
					}
					else
					{
						LogUtils.ErrorLog("GameObjectUtils::FindChildrenWithStringInName -> the object {0} contains two nodes with the same name {1}", in_gameObject.name, childTransform.gameObject.name);
					}
				}

				childTransform.gameObject.FindChildrenWithStringInName(in_stringTocheck, ref in_result);
			}
		}

		/// Search upward through the hierarchy (including the provided object) to find the first instance of the component type T
		///
		public static T FindComponentInParents<T>(this GameObject go)
		{
			T component = default(T);
			Transform toCheck = go.transform;

			while(component == null && toCheck != null)
			{
				component = toCheck.gameObject.GetComponent<T>();
				toCheck = toCheck.parent;
			}

			return component;
		}

        /// Sets the layer of the provided gameobject of all children
        ///
        public static void SetLayer(GameObject go, int layer)
        {
            var allTransforms = go.GetComponentsInChildren<Transform>(true);
            foreach (Transform foundTransform in allTransforms)
            {
                foundTransform.gameObject.layer = layer;
            }
        }

        /// Perform action on a game object's hierarchy recursively
        /// 
        public static void DoActionOnGameObjectRecursively(GameObject go, System.Action<GameObject> action)
        {
            action(go);

            for(int i = 0; i < go.transform.childCount; ++i)
            {
                DoActionOnGameObjectRecursively(go.transform.GetChild(i).gameObject, action);
            }
        }
	}
}