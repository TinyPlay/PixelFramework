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
    using UnityEngine.Networking;
    using System.Collections.Generic;
    
    /// <summary>
    /// Web Request Config
    /// </summary>
    [System.Serializable]
    public class WebRequestConfig
    {
        // Web Requests Config
        public string Url = "";
        public WebRequestType RequestType = WebRequestType.GET;
        public Dictionary<string, string> RequestData = new Dictionary<string, string>();
        public Dictionary<string, string> RequestHeaders = new Dictionary<string, string>();
        public bool CacheRequest = true;
        public int CacheLifetime = 300;
        
        // Web Request Callbacks
        public Action<string> OnComplete;
        public Action<string> OnError;
    }

    /// <summary>
    /// Web Request Type
    /// </summary>
    public enum WebRequestType
    {
        GET,
        POST,
        PUT,
        DELETE,
        HEAD
    }
}