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
namespace PixelFramework.Components.Audio
{
    using UnityEngine;
    using PixelFramework.Managers;
    
    /// <summary>
    /// Audio Source Volume Manager Component
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    [AddComponentMenu("Pixel Framework/Audio/AudioSource Volume")]
    internal class AudioSourceVolumeManager : MonoBehaviour
    {
        // Audio Type
        public AudioSourceType AudioSourceType = AudioSourceType.SoundFX;
        
        // Private Params
        private AudioSource _audioSource;
        private float _baseVolume;
        
        /// <summary>
        /// On Awake
        /// </summary>
        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _baseVolume = _audioSource.volume;
        }

        /// <summary>
        /// On Start
        /// </summary>
        private void Start()
        {
            AudioManager.Instance().OnAudioSettingsChanged.AddListener(OnAudioSettingsUpdated);
            OnAudioSettingsUpdated(AudioManager.Instance().GetCurrentState());
        }

        /// <summary>
        /// On Destroy
        /// </summary>
        private void OnDestroy()
        {
            AudioManager.Instance().OnAudioSettingsChanged.RemoveListener(OnAudioSettingsUpdated);
        }

        /// <summary>
        /// On Audio Settings Updated
        /// </summary>
        private void OnAudioSettingsUpdated(AudioManagerConfigs configs)
        {
            float newVolume = _baseVolume * configs.MasterVolume;
            
            switch (AudioSourceType)
            {
                case AudioSourceType.SoundFX:
                    newVolume *= configs.SoundsVolume;
                    break;
                    
                case AudioSourceType.Music:
                    newVolume *= configs.MusicVolume;
                    break;
                
                case AudioSourceType.Voice:
                    newVolume *= configs.VoicesVolume;
                    break;
            }

            _audioSource.volume = newVolume;
        }
    }
}