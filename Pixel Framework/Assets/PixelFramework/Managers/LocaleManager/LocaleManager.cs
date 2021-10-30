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
    using System.IO;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;
    using PixelFramework.Core.ContentManagement;
    using PixelFramework.Models.Locale;

    /// <summary>
    /// Locale Manager
    /// </summary>
    public class LocaleManager : IGameManager
    {
        // Localization Manager Events
        public UnityEvent<LocaleManagerConfigs> OnLocalizationSettingsChanged = new UnityEvent<LocaleManagerConfigs>();
        
        // Private Params
        private static LocaleManager _instance;
        private LocaleManagerConfigs _config = new LocaleManagerConfigs();
        private Dictionary<string, string> _currentLocale = new Dictionary<string, string>();

        #region Base Manager Logic
        /// <summary>
        /// Localization Manager Constructor
        /// </summary>
        /// <param name="config"></param>
        private LocaleManager(LocaleManagerConfigs config = null)
        {
            if (config != null) _config = config;
        }

        /// <summary>
        /// Get Localization Manager Instance
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static LocaleManager Instance(LocaleManagerConfigs config = null)
        {
            if (_instance == null)
                _instance = new LocaleManager(config);
            return _instance;
        }

        /// <summary>
        /// Set Localization Manager State
        /// </summary>
        /// <param name="config"></param>
        public LocaleManager SetState(LocaleManagerConfigs config)
        {
            _config = config;
            return _instance;
        }

        /// <summary>
        /// Load Manager State
        /// </summary>
        public void LoadState()
        {
            string path = "/localization_settings.dat";
            _config = FileReader.ReadObjectFromFile<LocaleManagerConfigs>(path,
                SerializationType.EncryptedJSON);
        }

        /// <summary>
        /// Save Manager State
        /// </summary>
        public void SaveState()
        {
            string path = "/localization_settings.dat";
            FileReader.SaveObjectToFile(path, _config, SerializationType.EncryptedJSON);
            if(OnLocalizationSettingsChanged!=null) OnLocalizationSettingsChanged.Invoke(_config);
        }
        
        /// <summary>
        /// Get Current State
        /// </summary>
        /// <returns></returns>
        public LocaleManagerConfigs GetCurrentState()
        {
            return _config;
        }
        #endregion

        #region Localization Manager Logic
        /// <summary>
        /// Switch Language to
        /// </summary>
        /// <param name="locale"></param>
        /// <returns></returns>
        public LocaleManager SwitchLanguage(string locale)
        {
            string pathToLanguage = $"Localization/{locale.ToUpper()}";
            string localeData = FileReader.LoadTextFromResources(pathToLanguage);
            if (localeData != null)
            {
                LocaleData data = new LocaleData();
                data.LocalesData = new List<LocaleData.LocaleItem>();
                data = JsonUtility.FromJson<LocaleData>(localeData);
                _currentLocale = _convertToDictionary(data);
                _config.LocaleCode = locale;
                SaveState();
            }
            else
            {
                throw new Exception($"Failed to Load Language: {locale}");
            }
            
            return _instance;
        }
        
        /// <summary>
        /// Get Locale Item by Code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public string GetItem(string code)
        {
            if (_currentLocale.ContainsKey(code))
            {
                return _currentLocale[code];
            }
            else
            {
                return code;
            }
        }
        
        /// <summary>
        /// Convert List to Dictionary
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private Dictionary<string, string> _convertToDictionary(LocaleData data)
        {
            Dictionary<string, string> newLocale = new Dictionary<string, string>();
            foreach (LocaleData.LocaleItem item in data.LocalesData)
            {
                newLocale.Add(item.Code, item.Value);
            }

            return newLocale;
        }
        #endregion
    }
}