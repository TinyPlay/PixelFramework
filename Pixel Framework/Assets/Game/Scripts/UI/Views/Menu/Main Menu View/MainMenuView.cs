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
            public UnityEvent OnPlayButtonClicked;
            public UnityEvent OnSettingsButtonClicked;
            public UnityEvent OnStoreButtonClicked;
        }
        
        // View References
        [Header("View References")] 
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _settingButton;
        [SerializeField] private Button _storeButton;
        [SerializeField] private AudioClip _clickSoundFX;
        
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
                _audioSource.clip = _clickSoundFX;
                _audioSource.loop = false;
                _audioSource.playOnAwake = false;
            }
            
            // Add Handlers
            _playButton.onClick.AddListener(() =>
            {
                if(_audioSource.clip!=null) _audioSource.Play();
                if(ctx.OnPlayButtonClicked!=null) ctx.OnPlayButtonClicked.Invoke();
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
    }
}