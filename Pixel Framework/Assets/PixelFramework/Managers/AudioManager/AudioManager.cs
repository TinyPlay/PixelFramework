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
    using UnityEngine;
    using UnityEngine.Events;
    using PixelFramework.Core.ContentManagement;

    /// <summary>
    /// Audio Manager Class
    /// </summary>
    public class AudioManager : IGameManager
    {
        // Audio Events
        public UnityEvent<AudioManagerConfigs> OnAudioSettingsChanged = new UnityEvent<AudioManagerConfigs>();
        
        // Private Params
        private static AudioManager _instance;
        private AudioManagerConfigs _config = new AudioManagerConfigs();

        #region Base Manager Logic
        /// <summary>
        /// Audio Manager Constructor
        /// </summary>
        /// <param name="config"></param>
        private AudioManager(AudioManagerConfigs config = null)
        {
            if (config != null) _config = config;
        }
        
        /// <summary>
        /// Get Audio Manager Instance
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static AudioManager Instance(AudioManagerConfigs config = null)
        {
            if (_instance == null)
                _instance = new AudioManager(config);
            return _instance;
        }
        
        /// <summary>
        /// Set Current Audio Manager State
        /// </summary>
        /// <param name="config"></param>
        public AudioManager SetState(AudioManagerConfigs config)
        {
            _config = config;
            return _instance;
        }

        /// <summary>
        /// Get Current Audio Manager State
        /// </summary>
        /// <returns></returns>
        public AudioManagerConfigs GetCurrentState()
        {
            return _config;
        }
        
        /// <summary>
        /// Load Manager State
        /// </summary>
        public void LoadState()
        {
            string path = "/audio_settings.dat";
            _config = FileReader.ReadObjectFromFile<AudioManagerConfigs>(path, SerializationType.EncryptedJSON);
        }

        /// <summary>
        /// Save Manager State
        /// </summary>
        public void SaveState()
        {
            string path = "/audio_settings.dat";
            FileReader.SaveObjectToFile(path, _config, SerializationType.EncryptedJSON);
            if(OnAudioSettingsChanged!=null) OnAudioSettingsChanged.Invoke(_config);
        }
        #endregion

        #region Audio Manager Logic
        /// <summary>
        /// Set Master Volume
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public AudioManager SetMasterVolume(float value)
        {
            _config.MasterVolume = value;
            SaveState();
            return _instance;
        }

        /// <summary>
        /// Get Master Volume
        /// </summary>
        /// <returns></returns>
        public float GetMasterVolume()
        {
            return _config.MasterVolume;
        }
        
        /// <summary>
        /// Set Sounds Volume
        /// </summary>
        /// <param name="volume"></param>
        public AudioManager SetSoundsVolume(float volume)
        {
            _config.SoundsVolume = volume;
            SaveState();
            return _instance;
        }

        /// <summary>
        /// Get Sounds Volume
        /// </summary>
        /// <returns></returns>
        public float GetSoundsVolume()
        {
            return _config.SoundsVolume;
        }

        /// <summary>
        /// Set Music Volume
        /// </summary>
        /// <param name="volume"></param>
        public AudioManager SetMusicVolume(float volume)
        {
            _config.MusicVolume = volume;
            SaveState();
            return _instance;
        }

        /// <summary>
        /// Get Music Volume
        /// </summary>
        /// <returns></returns>
        public float GetMusicVolume()
        {
            return _config.MusicVolume;
        }
        
        /// <summary>
        /// Set Voices Volume
        /// </summary>
        /// <param name="volume"></param>
        public AudioManager SetVoicesVolume(float volume)
        {
            _config.VoicesVolume = volume;
            SaveState();
            return _instance;
        }

        /// <summary>
        /// Get Voices Volume
        /// </summary>
        /// <returns></returns>
        public float GetVoicesVolume()
        {
            return _config.VoicesVolume;
        }
        #endregion
    }
}