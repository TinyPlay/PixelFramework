/*
 * Hypercasual Game Template
 * 
 * This is a hypercasual game template developed on the Pixel Framework with the support of TinyPlay.
 * This template and its components will help you quickly create hypercasual games,
 * as it includes everything you need to get started quickly.
 *
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
namespace HyperSample.UI.Views
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.Events;
    using PixelFramework.UI.View;
    using PixelFramework.UI.Components;
    using PixelFramework.Managers;
    using PixelFramework.Components.Audio;
    
    /// <summary>
    /// Settings View Class
    /// </summary>
    internal class SettingsView : BaseView
    {
        // Context
        [System.Serializable]
        public class Context : IViewContext
        {
            // Events
            public UnityEvent OnSettingsOpen;

            public Action<float> OnMasterVolumeChanged;
            public Action<float> OnSoundsSettingsChanged;
            public Action<float> OnMusicSettingsChanged;
            public Action<float> OnVoiceSettingsChanged;

            public Action<int> OnGraphiscLevelChanged;
            public Action<string> OnLanguageChanged;
        }
        
        // View References
        [Header("View References")] 
        [SerializeField] private Button _closeButton;
        [SerializeField] private AudioClip _clickSoundFX;

        [Header("Settings Editors")] 
        [SerializeField] private Slider _masterVolumeSlider;
        [SerializeField] private Slider _soundsVolumeSlider;
        [SerializeField] private Slider _musicVolumeSlider;
        [SerializeField] private Slider _voiceVolumeSlider;

        [SerializeField] private List<LocaleEditorModel> _localeSwitchers = new List<LocaleEditorModel>();

        [SerializeField] private Switcher _hqGraphicsSwitcher;
        
        
        // Private Params
        private AudioSource _audioSource;

        /// <summary>
        /// On Context Initialized
        /// </summary>
        public override void OnContextInitialized()
        {
            Context ctx = (Context) GetContext();
            
            // Initialize Audio
            if (_audioSource == null)
            {
                _audioSource = gameObject.AddComponent<AudioSource>();
                gameObject.AddComponent<AudioSourceVolumeManager>().AudioSourceType = AudioSourceType.SoundFX;
                _audioSource.clip = _clickSoundFX;
                _audioSource.loop = false;
                _audioSource.playOnAwake = false;
            }
            
            // Add Audio Settings Switchers
            _masterVolumeSlider.value = AudioManager.Instance().GetMasterVolume();
            _masterVolumeSlider.onValueChanged.AddListener(val =>
            {
                ctx.OnMasterVolumeChanged.Invoke(val);
            });
            _soundsVolumeSlider.value = AudioManager.Instance().GetSoundsVolume();
            _soundsVolumeSlider.onValueChanged.AddListener(val =>
            {
                ctx.OnSoundsSettingsChanged.Invoke(val);
            });
            _musicVolumeSlider.value = AudioManager.Instance().GetMusicVolume();
            _musicVolumeSlider.onValueChanged.AddListener(val =>
            {
                ctx.OnMusicSettingsChanged.Invoke(val);
            });
            _voiceVolumeSlider.value = AudioManager.Instance().GetVoicesVolume();
            _voiceVolumeSlider.onValueChanged.AddListener(val =>
            {
                ctx.OnVoiceSettingsChanged.Invoke(val);
            });
            
            // Add Language Settings Switchers
            foreach (LocaleEditorModel editor in _localeSwitchers)
            {
                editor.localeButton.onClick.AddListener(() =>
                {
                    ctx.OnLanguageChanged.Invoke(editor.localeCode);
                    if(_audioSource.clip!=null) _audioSource.Play();
                });
            }
            
            // Add Graphics Switcher
            _hqGraphicsSwitcher.SetContext(new Switcher.Context()
            {
                DefaultValue = (GraphicsManager.Instance().GetQualityLevel() == 1),
                OnSwitcherUpdate = isHq =>
                {
                    ctx.OnGraphiscLevelChanged.Invoke(isHq?1:0);
                }
            });
            
            // Add Handlers
            _closeButton.onClick.AddListener(() =>
            {
                if(_audioSource.clip!=null) _audioSource.Play();
                HideView();
            });
            ctx.OnSettingsOpen.AddListener(() =>
            {
                ShowView();
            });
        }

        /// <summary>
        /// On View Destroyed
        /// </summary>
        public override void OnViewDestroyed()
        {
            Context ctx = (Context) GetContext();
            
            // General Handlers
            if(ctx.OnSettingsOpen!=null) ctx.OnSettingsOpen.RemoveAllListeners();

            // Additional Handlers
            _closeButton.onClick.RemoveAllListeners();
            _masterVolumeSlider.onValueChanged.RemoveAllListeners();
            _soundsVolumeSlider.onValueChanged.RemoveAllListeners();
            _musicVolumeSlider.onValueChanged.RemoveAllListeners();
            _voiceVolumeSlider.onValueChanged.RemoveAllListeners();
            
            foreach (LocaleEditorModel editor in _localeSwitchers)
            {
                editor.localeButton.onClick.RemoveAllListeners();
            }
        }
    }
}