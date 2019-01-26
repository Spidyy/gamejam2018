//
// Copyright (c) 2016 Tag Games Ltd. All rights reserved
//

// #define DEBUG	// force enable logging
// #undef DEBUG		// force disable logging

namespace TagFramework
{

	using UnityEngine;
	using ConditionalAttribute = System.Diagnostics.ConditionalAttribute;

	//------------------------------------------------------------------
	/// Collection of logging methods for different severity of logs.
	/// Debug logs only occur in DEBUG mode.
	//------------------------------------------------------------------
	public static class LogUtils
	{
		//------------------------------------------------------------------
		/// Log to the console in DEBUG mode only. Limit calls to this
		/// to avoid console spam.
		///
		/// @author M Canala
		///
		/// @param in_format - the format of the message string
		/// @param in_args - the arguments to pass to the format string
		//------------------------------------------------------------------
		[ConditionalAttribute("DEBUG")]
		public static void DebugLog(string in_format, params object[] in_args)
		{
			Debug.LogFormat(in_format, in_args);
		}

		//------------------------------------------------------------------
		/// Log to the console as a warning. Used to flag issues that
		/// are unexpected but can be handled gracefully.
		///
		/// @author M Canala
		///
		/// @param in_format - the format of the message string
		/// @param in_args - the arguments to pass to the format string
		//------------------------------------------------------------------
		[ConditionalAttribute("DEBUG")]
		public static void WarningLog(string in_format, params object[] in_args)
		{
			Debug.LogWarningFormat(in_format, in_args);
		}
		//------------------------------------------------------------------
		/// Log to the console as an error. Used to flag issues that
		/// are unexpected and will result in no operation.
		///
		/// @author M Canala
		///
		/// @param in_format - the format of the message string
		/// @param in_args - the arguments to pass to the format string
		//------------------------------------------------------------------
		[ConditionalAttribute("DEBUG")]
		public static void ErrorLog(string in_format, params object[] in_args)
		{
			Debug.LogErrorFormat(in_format, in_args);
		}

		//------------------------------------------------------------------
		/// If the condition is false then an error is thrown and execution
		/// stopped. Used for issues that cannot be recovered from or that
		/// highlights issues in usage. Called in DEBUG mode only.
		///
		/// @author M Canala
		///
		/// @param in_condition - the condition to check
		/// @param in_format - the format of the message string
		/// @param in_args - the arguments to pass to the format string
		//------------------------------------------------------------------
		[ConditionalAttribute("DEBUG")]
		public static void Assert(bool in_condition, string in_format, params object[] in_args)
		{
			Debug.AssertFormat(in_condition, in_format, in_args);
		}
	}
}
