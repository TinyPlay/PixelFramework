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
    using PixelFramework.Components.Audio;
    using PixelFramework.Managers;
    using HyperSample.Models;
    using HyperSample.UI.Components;
    
    /// <summary>
    /// Level Selector View
    /// </summary>
    internal class LevelSelectorView : BaseView
    {
        // Context
        [System.Serializable]
        public class Context : IViewContext
        {
            // Events
            public UnityEvent<bool> OnLevelsOpen;
            public Action<int> OnLevelLaunched;

            public int StarsValue = 0;
            public int CurrentLevel = 0;
            public List<GameLevelModel> Levels = new List<GameLevelModel>();
        }
        
        // View References
        [Header("View References")]
        [SerializeField] private AudioClip _clickSoundFX;
        [SerializeField] private Transform _levelsContainer;
        [SerializeField] private GameObject _levelItemTemplate;
        
        [Header("Currency References")] 
        [SerializeField] private Text _starsField;
        
        // Private Params
        private AudioSource _audioSource;
        private bool _isLevelsInitialized = false;

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
            
            // Initialize Levels
            if (!_isLevelsInitialized)
            {
                _isLevelsInitialized = true;

                int levelIndex = 0;
                foreach (GameLevelModel levelData in ctx.Levels)
                {
                    GameObject levelItemObject = Instantiate(_levelItemTemplate, _levelsContainer);
                    levelItemObject.transform.SetAsLastSibling();
                    levelItemObject.GetComponent<LevelItemView>().SetContext(new LevelItemView.Context()
                    {
                        IsLocked = (ctx.CurrentLevel < levelIndex)?true:false,
                        CurrentLevel = (levelIndex),
                        OnLevelClicked = level =>
                        {
                            ctx.OnLevelLaunched.Invoke(level);
                        },
                        StarsCount = levelData.StarsCount
                    });
                    
                    levelIndex++;
                }
            }
            
            // Add Handlers
            ctx.OnLevelsOpen.AddListener(isOpen =>
            {
                if (isOpen) 
                    ShowView();
                else
                    HideView();
                
                if(_audioSource.clip!=null) _audioSource.Play();
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
            
            // General Handlers
            if(ctx.OnLevelsOpen!=null) ctx.OnLevelsOpen.RemoveAllListeners();
        }

        /// <summary>
        /// Update View
        /// </summary>
        /// <returns></returns>
        public override IBaseView UpdateView()
        {
            Context ctx = (Context) GetContext();
            _starsField.text = ctx.StarsValue.ToString("N0");
            return this;
        }
    }
}