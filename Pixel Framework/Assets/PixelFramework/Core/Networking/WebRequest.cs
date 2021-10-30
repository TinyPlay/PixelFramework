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
    using System.Text;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Networking;
    using PixelFramework.Utils;
    using PixelFramework.Core.Security;
    using PixelFramework.Core.ContentManagement;
    
    /// <summary>
    /// Web Request Class
    /// </summary>
    public class WebRequest : INetRequest
    {
        // Private Params
        private WebRequestConfig _config;
        
        // Private Events
        private Action _onRequestDispose;

        /// <summary>
        /// Web Request Constructor
        /// </summary>
        /// <param name="requestData"></param>
        public WebRequest(WebRequestConfig requestData)
        {
            _config = requestData;
        }

        /// <summary>
        /// Send Request
        /// </summary>
        /// <returns></returns>
        public INetRequest SendRequest()
        {
            CoroutineProvider.Start(Send());
            return this;
        }

        /// <summary>
        /// Cancel Request
        /// </summary>
        /// <returns></returns>
        public INetRequest CancelRequest()
        {
            CoroutineProvider.Stop(Send());
            return this;
        }

        /// <summary>
        /// Request Coroutine
        /// </summary>
        /// <returns></returns>
        private IEnumerator Send()
        {
            // Load Request Cache
            if (_config.CacheRequest)
            {
                string requestCache = GetRequestCache(_config.Url);
                if (requestCache != null)
                {
                    if (_config.OnComplete != null) 
                        _config.OnComplete.Invoke(requestCache);
                    if(_onRequestDispose!=null) 
                        _onRequestDispose.Invoke();
                }
                
                yield return null;
            }
            
            // Detect Method
            string requestMethod = GetRequestMethod(_config.RequestType);
            UnityWebRequest webRequest = new UnityWebRequest(_config.Url, requestMethod);
            DownloadHandlerBuffer dH = new DownloadHandlerBuffer();
            webRequest.downloadHandler = dH;
            foreach (KeyValuePair<string, string> header in _config.RequestHeaders)
            {
                webRequest.SetRequestHeader(header.Key, header.Value);
            }
            if (_config.RequestType == WebRequestType.POST)
            {
                foreach (KeyValuePair<string, string> formParameter in _config.RequestData)
                {
                    webRequest.SetRequestHeader(formParameter.Key, formParameter.Value);
                }
            }
            
            // Send Request
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                if (_config.OnComplete != null) 
                    _config.OnComplete.Invoke(webRequest.downloadHandler.text);
                if (_config.CacheRequest) SaveRequestCache(_config.Url, webRequest.downloadHandler.text);
            }
            else
            {
                if (_config.OnError != null) _config.OnError(webRequest.error);
            }
            
            
            if(_onRequestDispose!=null) 
                _onRequestDispose.Invoke();
            webRequest.Dispose();
        }

        /// <summary>
        /// Get Request Cache
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private string GetRequestCache(string url)
        {
            string cacheFilePath = $"{Application.persistentDataPath}/{Base64.Encode(url)}.cache";
            string cacheStampPath = $"{Application.persistentDataPath}/{Base64.Encode(url)}.cachestamp";

            string cacheFile = FileReader.LoadText(cacheFilePath, Encoding.UTF8);
            string cacheStamp = FileReader.LoadText(cacheStampPath, Encoding.UTF8);

            // Has Cache
            if (cacheFile != null && cacheStamp != null)
            {
                int cacheCreationTime = Int32.Parse(cacheStamp);
                if (UnixTime.SecondsElapsed(cacheCreationTime) > _config.CacheLifetime)
                {
                    cacheFile = null;
                    cacheStamp = null;
                    FileReader.DeleteFile(cacheFilePath);
                    FileReader.DeleteFile(cacheStampPath);
                    return null;
                }

                return cacheFile;
            }

            return null;
        }

        /// <summary>
        /// Save Request Cache
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        private void SaveRequestCache(string url, string data)
        {
            string cacheFilePath = $"{Application.persistentDataPath}/{Base64.Encode(url)}.cache";
            string cacheStampPath = $"{Application.persistentDataPath}/{Base64.Encode(url)}.cachestamp";
            FileReader.SaveText(cacheFilePath, data, Encoding.UTF8);
            FileReader.SaveText(cacheStampPath, UnixTime.Current().ToString(), Encoding.UTF8);
        }

        /// <summary>
        /// On Dispose Action
        /// </summary>
        /// <param name="onDispose"></param>
        /// <returns></returns>
        public INetRequest OnDispose(Action onDispose)
        {
            _onRequestDispose = onDispose;
            return this;
        }
        
        /// <summary>
        /// Get Request method name from Enum
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        private string GetRequestMethod(WebRequestType method)
        {
            switch (method)
            {
                case WebRequestType.POST:
                    return "POST";
                case WebRequestType.PUT:
                    return "PUT";
                case WebRequestType.DELETE:
                    return "DELETE";
                case WebRequestType.HEAD:
                    return "HEAD";
                default:
                    return "GET";
            }
        }
    }
}