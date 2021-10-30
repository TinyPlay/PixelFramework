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
    /// Ads Manager
    /// </summary>
    public class AdsManager : IGameManager
    {
        // ADS Manager Events
        public UnityEvent<AdsManagerConfigs> OnAnalyticsSettingsChanged =
            new UnityEvent<AdsManagerConfigs>();
        
        // Private Params
        private static AdsManager _instance;
        private AdsManagerConfigs _config = new AdsManagerConfigs();

        private Action<bool> _onAdsShown;
        
        #region Base Manager Logic
        /// <summary>
        /// Ads Manager Constructor
        /// </summary>
        /// <param name="config"></param>
        private AdsManager(AdsManagerConfigs config = null)
        {
            if (config != null) _config = config;
            if (_config.OnAdsInitialized!=null) _config.OnAdsInitialized.Invoke();
        }
        
        /// <summary>
        /// Get Ads Manager Instance
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static AdsManager Instance(AdsManagerConfigs config = null)
        {
            if (_instance == null)
                _instance = new AdsManager(config);
            return _instance;
        }
        
        /// <summary>
        /// Set Current Ads Manager State
        /// </summary>
        /// <param name="config"></param>
        public AdsManager SetState(AdsManagerConfigs config)
        {
            _config = config;
            return _instance;
        }

        /// <summary>
        /// Get Current Ads Manager State
        /// </summary>
        /// <returns></returns>
        public AdsManagerConfigs GetCurrentState()
        {
            return _config;
        }
        
        /// <summary>
        /// Load Manager State
        /// </summary>
        public void LoadState()
        {
            string path = "/ads_settins.dat";
            _config = FileReader.ReadObjectFromFile<AdsManagerConfigs>(path, SerializationType.EncryptedJSON);
        }

        /// <summary>
        /// Save Manager State
        /// </summary>
        public void SaveState()
        {
            string path = "/ads_settins.dat";
            FileReader.SaveObjectToFile(path, _config, SerializationType.EncryptedJSON);
            if(OnAnalyticsSettingsChanged!=null) OnAnalyticsSettingsChanged.Invoke(_config);
        }
        #endregion

        #region Ads Manager Logic
        /// <summary>
        /// Show Ads
        /// </summary>
        /// <param name="adsType"></param>
        public void ShowAds(AdsType adsType)
        {
            if(_config.OnAdsShown!=null) _config.OnAdsShown.Invoke(adsType);
        }

        /// <summary>
        /// Set Ads Shown Callback
        /// </summary>
        /// <param name="onAdsShownCallback"></param>
        public void SetAdsShownCallback(Action<bool> onAdsShownCallback)
        {
            _onAdsShown = onAdsShownCallback;
        }

        /// <summary>
        /// Get Ads Shown Callback
        /// </summary>
        /// <returns></returns>
        public Action<bool> GetAdsShownCallback()
        {
            return _onAdsShown;
        }
        #endregion
    }
}