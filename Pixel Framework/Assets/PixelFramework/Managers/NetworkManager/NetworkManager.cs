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
namespace PixelFramework.Managers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;
    using PixelFramework.Core.ContentManagement;
    using PixelFramework.Core.Networking;
    using PixelFramework.Utils;

    /// <summary>
    /// Network Manager
    /// </summary>
    public class NetworkManager : IGameManager
    {
        // Network Manager Events
        public UnityEvent<NetworkManagerConfigs> OnNetworkSettingsChanged = new UnityEvent<NetworkManagerConfigs>();
        
        // Private Params
        private static NetworkManager _instance;
        private NetworkManagerConfigs _config = new NetworkManagerConfigs();

        // Requests Queue
        private List<INetRequest> _requestQueue = new List<INetRequest>();

        #region Base Manager Logic
        /// <summary>
        /// Network Manager Constructor
        /// </summary>
        /// <param name="config"></param>
        private NetworkManager(NetworkManagerConfigs config = null)
        {
            if (config != null) _config = config;
        }
        
        /// <summary>
        /// Get Network Manager Instance
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static NetworkManager Instance(NetworkManagerConfigs config = null)
        {
            if (_instance == null)
                _instance = new NetworkManager(config);
            return _instance;
        }

        /// <summary>
        /// Set Network Manager State
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public NetworkManager SetState(NetworkManagerConfigs config)
        {
            _config = config;
            return _instance;
        }
        
        /// <summary>
        /// Load Manager State
        /// </summary>
        public void LoadState()
        {
            string path = "/network_settings.dat";
            _config = FileReader.ReadObjectFromFile<NetworkManagerConfigs>(path,
                SerializationType.EncryptedJSON);
        }

        /// <summary>
        /// Save Manager State
        /// </summary>
        public void SaveState()
        {
            string path = "/network_settings.dat";
            FileReader.SaveObjectToFile(path, _config, SerializationType.EncryptedJSON);
            if (OnNetworkSettingsChanged != null) OnNetworkSettingsChanged.Invoke(_config);
        }
        
        /// <summary>
        /// Get Current State
        /// </summary>
        /// <returns></returns>
        public NetworkManagerConfigs GetCurrentState()
        {
            return _config;
        }
        #endregion

        #region Network Manager Queue
        /// <summary>
        /// Add Request to Queue
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public NetworkManager AddRequestToQueue(INetRequest request)
        {
            _requestQueue.Add(request);
            return _instance;
        }

        /// <summary>
        /// Remove Request from Queue
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public NetworkManager RemoveRequestFromQueue(INetRequest request)
        {
            _requestQueue.Remove(request);
            return _instance;
        }

        /// <summary>
        /// Clear Requests Queue
        /// </summary>
        /// <returns></returns>
        public NetworkManager ClearQueue()
        {
            _requestQueue.Clear();
            return _instance;
        }

        /// <summary>
        /// Send Queue
        /// </summary>
        /// <returns></returns>
        public NetworkManager SendQueue()
        {
            CoroutineProvider.Start(StartQueue());
            return _instance;
        }

        /// <summary>
        /// Stop Queue
        /// </summary>
        /// <returns></returns>
        public NetworkManager StopQueue()
        {
            CoroutineProvider.Stop(StartQueue());
            return _instance;
        }

        /// <summary>
        /// Start Queue
        /// </summary>
        /// <returns></returns>
        private IEnumerator StartQueue()
        {
            while (_requestQueue.Count > 0)
            {
                foreach (INetRequest request in _requestQueue)
                {
                    request.SendRequest();
                    request.OnDispose(() =>
                    {
                        _requestQueue.Remove(request);
                    });
                }
                
                yield return new WaitForSeconds(_config.QueueRequestsInterval);
            }
        }
        #endregion

        #region Network Manager Requests
        /// <summary>
        /// Send Request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public NetworkManager SendRequest(WebRequest request)
        {
            request.SendRequest();
            return _instance;
        }
        
        /// <summary>
        /// Send Request
        /// </summary>
        /// <param name="downloadRequest"></param>
        /// <returns></returns>
        public NetworkManager SendRequest(INetRequest downloadRequest)
        {
            downloadRequest.SendRequest();
            return _instance;
        }

        /// <summary>
        /// Downloading Wrapper
        /// </summary>
        /// <param name="downloadRequest"></param>
        /// <returns></returns>
        public NetworkManager Download(Core.Networking.AssetBundleRequest downloadRequest)
        {
            downloadRequest.SendRequest();
            return _instance;
        }
        
        /// <summary>
        /// Downloading Wrapper
        /// </summary>
        /// <param name="downloadRequest"></param>
        /// <returns></returns>
        public NetworkManager Download(AudioClipRequest downloadRequest)
        {
            downloadRequest.SendRequest();
            return _instance;
        }
        
        /// <summary>
        /// Downloading Wrapper
        /// </summary>
        /// <param name="downloadRequest"></param>
        /// <returns></returns>
        public NetworkManager Download(Texture2DRequest downloadRequest)
        {
            downloadRequest.SendRequest();
            return _instance;
        }
        
        /// <summary>
        /// Downloading Wrapper
        /// </summary>
        /// <param name="downloadRequest"></param>
        /// <returns></returns>
        public NetworkManager Download(SpriteRequest downloadRequest)
        {
            downloadRequest.SendRequest();
            return _instance;
        }
        
        /// <summary>
        /// Downloading Wrapper
        /// </summary>
        /// <param name="downloadRequest"></param>
        /// <returns></returns>
        public NetworkManager Download(INetRequest downloadRequest)
        {
            downloadRequest.SendRequest();
            return _instance;
        }
        #endregion
    }
}