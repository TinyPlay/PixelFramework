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
namespace PixelFramework.Core.Networking
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Networking;
    
    
    /// <summary>
    /// Audio Clip Request Config
    /// </summary>
    [System.Serializable]
    public class AudioClipRequestConfig
    {
        // Web Requests Config
        public string Url = "";
        public AudioType AudioType = AudioType.OGGVORBIS;
        public bool CacheRequest = true;
        public int CacheLifetime = 300;
        
        // Web Request Callbacks
        public Action<AudioClip> OnComplete;
        public Action<string> OnError;
    }
}