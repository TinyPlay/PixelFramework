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
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.Events;
    using PixelFramework.UI.View;
    using PixelFramework.UI.Components;
    using PixelFramework.Managers;
    using PixelFramework.Components.Audio;
    
    /// <summary>
    /// Main Menu View
    /// </summary>
    internal class MainMenuView : BaseView
    {
        // Context
        [System.Serializable]
        public class Context : IViewContext
        {
            // Events
            public UnityEvent<bool> OnPlayButtonClicked;
            public UnityEvent OnSettingsButtonClicked;
            public UnityEvent OnStoreButtonClicked;
            
            // Datas
            public int CoinsValue = 0;
            public int StarsValue = 0;
        }
        
        // View References
        [Header("View References")] 
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _settingButton;
        [SerializeField] private Button _storeButton;
        [SerializeField] private AudioClip _clickSoundFX;

        [Header("Currency References")] 
        [SerializeField] private Text _coinsField;
        [SerializeField] private Text _starsField;
        
        
        // Private Params
        private AudioSource _audioSource;
        private bool _isPanelShown = false;
        
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
            
            // Add Handlers
            _playButton.onClick.AddListener(() =>
            {
                if(_audioSource.clip!=null) _audioSource.Play();
                _isPanelShown = !_isPanelShown;
                if(ctx.OnPlayButtonClicked!=null) ctx.OnPlayButtonClicked.Invoke(_isPanelShown);
            });
            _settingButton.onClick.AddListener(() =>
            {
                if(_audioSource.clip!=null) _audioSource.Play();
                if(ctx.OnSettingsButtonClicked!=null) ctx.OnSettingsButtonClicked.Invoke();
            });
            _storeButton.onClick.AddListener(() =>
            {
                if(_audioSource.clip!=null) _audioSource.Play();
                if(ctx.OnStoreButtonClicked!=null) ctx.OnStoreButtonClicked.Invoke();
            });
            
            // Update View
            UpdateView();
        }
        
        /// <summary>
        /// On View Destroyed
        /// </summary>
        public override void OnViewDestroyed()
        {
            Context ctx = (Context) GetContext();
            
            // Remove Events Listeners
            if(ctx.OnPlayButtonClicked!=null) ctx.OnPlayButtonClicked.RemoveAllListeners();
            if(ctx.OnSettingsButtonClicked!=null) ctx.OnSettingsButtonClicked.RemoveAllListeners();
            if(ctx.OnStoreButtonClicked!=null) ctx.OnStoreButtonClicked.RemoveAllListeners();
            
            // Remove Button Listeners
            _playButton.onClick.RemoveAllListeners();
            _settingButton.onClick.RemoveAllListeners();
            _storeButton.onClick.RemoveAllListeners();
        }

        /// <summary>
        /// Update View
        /// </summary>
        /// <returns></returns>
        public override IBaseView UpdateView()
        {
            Context ctx = (Context) GetContext();
            _coinsField.text = ctx.CoinsValue.ToString("N0");
            _starsField.text = ctx.StarsValue.ToString("N0");
            return this;
        }
    }
}