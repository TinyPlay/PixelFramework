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
    /// Asset Bundle Request Class
    /// </summary>
    public class AssetBundleRequest : INetRequest
    {
        // Private Params
        private AssetBundleRequestConfig _config;
        
        // Private Events
        private Action _onRequestDispose;
        
        /// <summary>
        /// AssetBundle Request Constructor
        /// </summary>
        /// <param name="requestData"></param>
        public AssetBundleRequest(AssetBundleRequestConfig requestData)
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
            // Get Bundle from Cache
            while (!Caching.ready) {
                if(_onRequestDispose!=null) 
                    _onRequestDispose.Invoke();
                yield return null;
            }
            
            UnityWebRequest manifestRequest = UnityWebRequest.Get(_config.ManifestUrl);
            yield return manifestRequest.SendWebRequest();
            if (manifestRequest.result == UnityWebRequest.Result.Success)
            {
                Hash128 hash = default;
                string hashRow = manifestRequest.downloadHandler.text.ToString().Split("\n".ToCharArray())[5];
                hash = Hash128.Parse(hashRow.Split(':')[1].Trim());
                if (hash.isValid == true)
                {
                    manifestRequest.Dispose();
                    UnityWebRequest bundleRequset = UnityWebRequestAssetBundle.GetAssetBundle(_config.BundleUrl, hash, 0);
                    yield return bundleRequset.SendWebRequest();
                    
                    if (manifestRequest.result == UnityWebRequest.Result.Success)
                    {
                        if (_config.OnComplete != null)
                            _config.OnComplete(DownloadHandlerAssetBundle.GetContent(bundleRequset));
                    }
                    else
                    {
                        if (_config.OnError != null) _config.OnError(bundleRequset.error);
                    }

                    bundleRequset.Dispose();
                }
                else
                {
                    if (_config.OnError != null) 
                        _config.OnError($"Wrong AssetBundle Manifest Hash for: {_config.ManifestUrl}");
                }
            }
            else
            {
                if (_config.OnError != null) _config.OnError(manifestRequest.error);
            }

            if(_onRequestDispose!=null) 
                _onRequestDispose.Invoke();
            manifestRequest.Dispose();
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
    }
}