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
    using UnityEngine.SceneManagement;
    using PixelFramework.Core.ContentManagement;
    using PixelFramework.Utils;

    /// <summary>
    /// Analytics Manager
    /// </summary>
    public class AnalyticsManager : IGameManager
    {
        // Analytics Manager Events
        public UnityEvent<AnalyticsManagerConfigs> OnAnalyticsSettingsChanged =
            new UnityEvent<AnalyticsManagerConfigs>();
        
        // Private Params
        private static AnalyticsManager _instance;
        private AnalyticsManagerConfigs _config = new AnalyticsManagerConfigs();

        #region Base Manager Logic
        /// <summary>
        /// Analytics Manager Constructor
        /// </summary>
        /// <param name="config"></param>
        private AnalyticsManager(AnalyticsManagerConfigs config = null)
        {
            if (config != null) _config = config;
            if (_config.OnAnalyticsInitialize!=null) _config.OnAnalyticsInitialize.Invoke();
        }
        
        /// <summary>
        /// Get Analytics Manager Instance
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static AnalyticsManager Instance(AnalyticsManagerConfigs config = null)
        {
            if (_instance == null)
                _instance = new AnalyticsManager(config);
            return _instance;
        }
        
        /// <summary>
        /// Set Current Analytics Manager State
        /// </summary>
        /// <param name="config"></param>
        public AnalyticsManager SetState(AnalyticsManagerConfigs config)
        {
            _config = config;
            return _instance;
        }

        /// <summary>
        /// Get Current Analytics Manager State
        /// </summary>
        /// <returns></returns>
        public AnalyticsManagerConfigs GetCurrentState()
        {
            return _config;
        }
        
        /// <summary>
        /// Load Manager State
        /// </summary>
        public void LoadState()
        {
            string path = "/analytics_settings.dat";
            _config = FileReader.ReadObjectFromFile<AnalyticsManagerConfigs>(path, SerializationType.EncryptedJSON);
        }

        /// <summary>
        /// Save Manager State
        /// </summary>
        public void SaveState()
        {
            string path = "/analytics_settings.dat";
            FileReader.SaveObjectToFile(path, _config, SerializationType.EncryptedJSON);
            if(OnAnalyticsSettingsChanged!=null) OnAnalyticsSettingsChanged.Invoke(_config);
        }
        #endregion

        #region Analytics Manager Logic
        /// <summary>
        /// Send Event
        /// </summary>
        /// <param name="eventName"></param>
        public void SendEvent(string eventName)
        {
            if(_config.OnEventSended!=null) _config.OnEventSended.Invoke(eventName);
        }

        /// <summary>
        /// Send Event with String Data
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="eventData"></param>
        public void SendEvent(string eventName, string eventData)
        {
            if(_config.OnStringEventSended!=null) _config.OnStringEventSended.Invoke(eventName, eventData);
        }


        /// <summary>
        /// Send Event with Object Data
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="eventData"></param>
        public void SendEvent(string eventName, object eventData)
        {
            if(_config.OnObjectEventSended!=null) _config.OnObjectEventSended.Invoke(eventName, eventData);
        }

        /// <summary>
        /// Send Revenue
        /// </summary>
        /// <param name="productID"></param>
        /// <param name="price"></param>
        /// <param name="currency"></param>
        public void SendRevenue(string productID, double price, string currency)
        {
            if (_config.OnRevenueSended != null) _config.OnRevenueSended(productID, price, currency);
        }
        #endregion
    }
}