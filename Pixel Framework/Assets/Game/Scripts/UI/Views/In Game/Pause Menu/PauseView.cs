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
    /// Pause View Class
    /// </summary>
    internal class PauseView : BaseView
    {
        // Context
        [System.Serializable]
        public class Context : IViewContext
        {
            // Events
            public UnityEvent<bool> OnGamePause;
            public Action OnMainMenuExit;
        }
        
        // View References
        [Header("View References")] 
        [SerializeField] private Button _continuePlay;
        [SerializeField] private Button _exitToMenu;
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
            
            // Add Handlers
            _continuePlay.onClick.AddListener(() =>
            {
                if(_audioSource.clip!=null) _audioSource.Play();
                ctx.OnGamePause.Invoke(false);
            });
            _exitToMenu.onClick.AddListener(() =>
            {
                if(_audioSource.clip!=null) _audioSource.Play();
                ctx.OnGamePause.Invoke(false);
                if(ctx.OnMainMenuExit!=null) ctx.OnMainMenuExit.Invoke();
            });
            ctx.OnGamePause.AddListener(isShown =>
            {
                if (isShown)
                    ShowView();
                else
                    HideView();
            });
        }

        /// <summary>
        /// On View Destroyed
        /// </summary>
        public override void OnViewDestroyed()
        {
            Context ctx = (Context) GetContext();
            if(ctx.OnGamePause!=null) ctx.OnGamePause.RemoveAllListeners();
        }
    }
}