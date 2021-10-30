/*
 * Pixel Framework
 * 
 * This framework allows you to develop games for mobile devices even faster than you normally do.
 * It includes a lot of useful classes, resources, and examples to get your project started.
 * The framework will be especially useful for developers of hyper-casual games.
 *
 * This framework was developed specifically for Pixel Incubator members with the support of TinyPlay.
 * You can read more about Incubator at the official website:
 * https://pixinc.club/
 *
 * @developer       Ilya Rastorguev
 * @autor           TinyPlay, Inc.
 * @version         1.0
 * @url             https://pixinc.club/
 * @support         https://github.com/TinyPlay/PixelFramework/issues
 * @discord         https://discord.gg/wE67T7Vm
 */
namespace PixelFramework.Utils
{
    using UnityEngine;
    using System.Collections;
    using System;
    
    /// <summary>
    /// Unix Time Class
    /// </summary>
    public class UnixTime
    {
        /// <summary>
        /// Get Current Unix Time
        /// </summary>
        /// <returns></returns>
        public static int Current()
        {
            DateTime epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            int currentEpochTime = (int)(DateTime.UtcNow - epochStart).TotalSeconds;
            return currentEpochTime;
        }
 
        /// <summary>
        /// Get Seconds elapsed from Unix Timestamp
        /// </summary>
        /// <param name="t1"></param>
        /// <returns></returns>
        public static int SecondsElapsed(int t1)
        {
            int difference = Current() - t1;
            return Mathf.Abs(difference);
        }
 
        /// <summary>
        /// Get Difference between two Unix Timestamps (in Seconds)
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        public static int SecondsElapsed(int t1, int t2)
        {
            int difference = t1 - t2;
            return Mathf.Abs(difference);
        }
    }
}