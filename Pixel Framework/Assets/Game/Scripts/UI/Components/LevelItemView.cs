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
namespace HyperSample.UI.Components
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.Events;
    using PixelFramework.UI.Components;
    using PixelFramework.Components.Audio;
    using PixelFramework.Managers;
    
    /// <summary>
    /// Level Item View
    /// </summary>
    internal class LevelItemView : MonoBehaviour, IBaseComponent
    {
        // Context
        [System.Serializable]
        public class Context : IComponentContext
        {
            public int CurrentLevel = 0;
            public bool IsLocked = false;
            public int StarsCount = 0;

            public Action<int> OnLevelClicked;
        }
        private Context _ctx;
        
        // Public Params
        [Header("View Instances")] 
        [SerializeField] private Text _levelText;
        [SerializeField] private Image _starsImage;
        [SerializeField] private Image _lockedImage;
        [SerializeField] private List<Sprite> _starsSprite;
        [SerializeField] private AudioClip _clickSoundFX;
        
        // Private Params
        private Button _currentButtonHolder;
        private AudioSource _audioSource;

        /// <summary>
        /// Set Progress Bar Context
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public void SetContext(IComponentContext ctx)
        {
            // Setup Context
            _ctx = (Context)ctx;

            // Setup Components
            _currentButtonHolder = GetComponent<Button>();
            
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
            _currentButtonHolder.onClick.AddListener(() =>
            {
                if(_audioSource.clip!=null) _audioSource.Play();
                if(_ctx.OnLevelClicked!=null) _ctx.OnLevelClicked.Invoke(_ctx.CurrentLevel);
            });
            _currentButtonHolder.interactable = (_ctx.IsLocked) ? false : true;
            
            // Update View
            UpdateView();
        }
        
        /// <summary>
        /// On Destroy
        /// </summary>
        private void OnDestroy()
        {
            _currentButtonHolder.onClick.RemoveAllListeners();
        }
        
        /// <summary>
        /// Update View
        /// </summary>
        private void UpdateView()
        {
            _levelText.text = (_ctx.CurrentLevel + 1).ToString("N0");
            _lockedImage.enabled = _ctx.IsLocked;
            _starsImage.enabled = (_ctx.IsLocked) ? false : true;
            _starsImage.sprite = _starsSprite[_ctx.StarsCount];
        }
    }
}