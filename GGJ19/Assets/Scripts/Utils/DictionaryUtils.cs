//
// Copyright (c) 2016 Tag Games Ltd. All rights reserved
//

namespace TagFramework
{
	﻿using UnityEngine;
	using System.Collections.Generic;

	//-----------------------------------------------------------------
	/// Extension class for adding additional functionality to Dictionary
	/// 
	/// @author S Downie
	//-----------------------------------------------------------------
	public static class DictionaryUtils
	{
		//-----------------------------------------------------------------
		/// If the key already exists it is updated, otherwise a new key
		/// is added and the value set.
		/// 
		/// @author S Downie
		/// 
		/// @param in_key - Key to add or update
		/// @param in_value - Value to set
		//-----------------------------------------------------------------
		public static void AddOrUpdate<TKey, TValue>(this Dictionary<TKey, TValue> in_dict, TKey in_key, TValue in_value)
		{
			if(in_dict.ContainsKey(in_key) == true)
			{
				in_dict[in_key] = in_value;
			}
			else
			{
				in_dict.Add(in_key, in_value);
			}
		}

		//-----------------------------------------------------------------
		/// If the key already exists it is updated otherwise a new key
		/// is added and the value set. Both via the given delegate
		/// 
		/// @author S Downie
		/// 
		/// @param in_key - Key to add or update
		/// @param in_adder - Returns the new value
		/// @param in_updater - Takes the existing value and returns the new value
		//-----------------------------------------------------------------
		public static void AddOrUpdate<TKey, TValue>(this Dictionary<TKey, TValue> in_dict, TKey in_key, System.Func<TValue> in_adder, System.Func<TValue, TValue> in_updater)
		{
			if(in_dict.ContainsKey(in_key) == true)
			{
				in_dict[in_key] = in_updater(in_dict[in_key]);
			}
			else
			{
				in_dict.Add(in_key, in_adder());
			}
		}

		//-----------------------------------------------------------------
		/// Tries to get the value associated to passed key otherwise returns
		/// the default value
		/// 
		/// @author S Downie
		/// 
		/// @param in_key - Key to add or update
		/// @param in_default - the default value in case the get fails
		//-----------------------------------------------------------------
		public static TValue GetValueOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> in_dict, TKey in_key, TValue in_default)
		{
			TValue value;
			if(in_dict.TryGetValue(in_key, out value) == true)
			{
				return value;
			}

			return in_default;
		}

		//-----------------------------------------------------------------
		/// If the key already exists it is updated, otherwise a new key
		/// is added and the value set.
		/// 
		/// @author M Canala
		/// 
		/// @param in_key - Key to add or update
		//-----------------------------------------------------------------
		public static TValue TryGetValue<TKey, TValue>(this Dictionary<TKey, TValue> in_dict, TKey in_key)
		{
			TValue value;
			in_dict.TryGetValue(in_key, out value);
			return value;
		}
	}
}