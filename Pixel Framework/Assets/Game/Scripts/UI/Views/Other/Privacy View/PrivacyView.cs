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
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.Events;
    using PixelFramework.UI.View;
    using PixelFramework.UI.Components;
    using PixelFramework.Components.Audio;
    using PixelFramework.Managers;
    
    /// <summary>
    /// Privacy View Class
    /// </summary>
    internal class PrivacyView : BaseView
    {
        // Context
        [System.Serializable]
        public class Context : IViewContext
        {
            // Events
            public UnityEvent OnPrivacyShown = new UnityEvent();
            public Action OnPrivacyAccepted;
            public Action OnPrivacyDelinced;
        }
        
        // View References
        [Header("View References")] 
        [SerializeField] private Button _acceptButton;
        [SerializeField] private Button _declineButton;
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
                gameObject.AddComponent<AudioSourceVolumeManager>().AudioSourceType = AudioSourceType.SoundFX;
                _audioSource.clip = _clickSoundFX;
                _audioSource.loop = false;
                _audioSource.playOnAwake = false;
            }
            
            // Initialize Handlers
            _acceptButton.onClick.AddListener(() =>
            {
                if(_audioSource.clip!=null) _audioSource.Play();
                if(ctx.OnPrivacyAccepted!=null) ctx.OnPrivacyAccepted.Invoke();
                HideView(new ViewAnimationOptions()
                {
                    AnimationDelay = 0f,
                    AnimationLength = 0.5f,
                    IsAnimated = true,
                    AnimationType = ViewAnimationType.Fade
                });
            });
            _declineButton.onClick.AddListener(() =>
            {
                if(_audioSource.clip!=null) _audioSource.Play();
                if(ctx.OnPrivacyDelinced!=null) ctx.OnPrivacyDelinced.Invoke();
            });
            
            // Initialize Events
            ctx.OnPrivacyShown.AddListener(() =>
            {
                ShowView(new ViewAnimationOptions()
                {
                    IsAnimated = true,
                    AnimationLength = 0.5f,
                    AnimationDelay = 0f,
                    AnimationType = ViewAnimationType.Fade
                });
            });
        }
        
        /// <summary>
        /// On View Destroyed
        /// </summary>
        public override void OnViewDestroyed()
        {
            Context ctx = (Context) GetContext();
            ctx.OnPrivacyShown.RemoveAllListeners();
            
            _acceptButton.onClick.RemoveAllListeners();
            _declineButton.onClick.RemoveAllListeners();
        }
    }
}