//
// Copyright (c) 2018 Tag Games Ltd. All rights reserved
//

using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// A collection of methods to help deal with timestamps
/// 
public static class TimeUtils
{
    public static string GetFormattedTimeStringShort(int totalSeconds)
    {
        if(totalSeconds < 0)
        {
            return string.Empty;
        }

        TimeSpan remaining = TimeSpan.FromSeconds(totalSeconds);

        StringBuilder timeString = new StringBuilder();
        bool stringStarted = false;
        if(remaining.Days >= 1)
        {
            timeString.Append(remaining.Days.ToString () + "d");
            stringStarted = true;
        } 

        if(remaining.Hours >= 1)
        {
            if(stringStarted)
                timeString.Append(" ");
            timeString.Append( remaining.Hours.ToString () + "h");
            stringStarted = true;
        }

        if(remaining.Minutes >= 1)
        {
            if(stringStarted)
                timeString.Append( " " );
            timeString.Append(remaining.Minutes.ToString () + "m");
            stringStarted = true;
        }

        if(remaining.Seconds >= 1)
        {
            if(stringStarted)
                timeString.Append(" ");
            timeString.Append(remaining.Seconds.ToString () + "s");
            stringStarted = true;
        }

        return timeString.ToString();
    }
}
