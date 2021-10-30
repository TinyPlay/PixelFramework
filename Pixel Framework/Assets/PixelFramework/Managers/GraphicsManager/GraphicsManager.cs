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

    /// <summary>
    /// Graphics Manager
    /// </summary>
    public class GraphicsManager : IGameManager
    {
        // Graphics Events
        public UnityEvent<GraphicsManagerConfigs> OnGraphicsSettingsChanged = new UnityEvent<GraphicsManagerConfigs>();
        
        // Private Params
        private static GraphicsManager _instance;
        private GraphicsManagerConfigs _config = new GraphicsManagerConfigs();

        #region Base Manager Logic
        /// <summary>
        /// Graphics Manager Constructor
        /// </summary>
        /// <param name="config"></param>
        private GraphicsManager(GraphicsManagerConfigs config = null)
        {
            if (config != null) _config = config;
        }

        /// <summary>
        /// Get Graphics Manager Instance
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static GraphicsManager Instance(GraphicsManagerConfigs config = null)
        {
            if (_instance == null)
                _instance = new GraphicsManager(config);
            return _instance;
        }

        /// <summary>
        /// Set Graphics Manager State
        /// </summary>
        /// <param name="config"></param>
        public GraphicsManager SetState(GraphicsManagerConfigs config)
        {
            _config = config;
            return _instance;
        }

        /// <summary>
        /// Load Manager State
        /// </summary>
        public void LoadState()
        {
            string path = "/graphics_settings.dat";
            _config = FileReader.ReadObjectFromFile<GraphicsManagerConfigs>(path,
                SerializationType.EncryptedJSON);
        }

        /// <summary>
        /// Save Manager State
        /// </summary>
        public void SaveState()
        {
            string path = "/graphics_settings.dat";
            FileReader.SaveObjectToFile(path, _config, SerializationType.EncryptedJSON);
            if(OnGraphicsSettingsChanged!=null) OnGraphicsSettingsChanged.Invoke(_config);
        }
        
        /// <summary>
        /// Get Current State
        /// </summary>
        /// <returns></returns>
        public GraphicsManagerConfigs GetCurrentState()
        {
            return _config;
        }
        #endregion

        #region Graphics Manager Logic
        /// <summary>
        /// Set Quality Level
        /// </summary>
        /// <param name="qualityLevel"></param>
        public GraphicsManager SetQualityLevel(int qualityLevel)
        {
            QualitySettings.SetQualityLevel(qualityLevel);
            _config.QualityLevel = qualityLevel;
            SaveState();
            return _instance;
        }

        /// <summary>
        /// Get Quality Level
        /// </summary>
        /// <returns></returns>
        public int GetQualityLevel()
        {
            return _config.QualityLevel;
        }
        
        /* TODO: Extended Graphics Management */
        #endregion
    }
}