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
namespace PixelFramework.UI.Components
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.Events;
    
    /// <summary>
    /// Switcher Class
    /// </summary>
    [AddComponentMenu("Pixel Framework/UI/Switcher")]
    [RequireComponent(typeof(Button))]
    public class Switcher : MonoBehaviour, IBaseComponent
    {
        // Context
        [System.Serializable]
        public class Context : IComponentContext
        {
            public bool DefaultValue = false;
            public Action<bool> OnSwitcherUpdate;
        }
        private Context _ctx;
        
        // Public Params
        [Header("View Instances")]
        [SerializeField] private Sprite _enabledImage;
        [SerializeField] private Sprite _disabledImage;
        [SerializeField] private AudioClip _clickSoundFX;
        
        // Private Params
        private Button _currentButtonHolder;
        private Image _currentImageHolder;
        private AudioSource _audioSource;
        private bool _currentValue = false;
        
        /// <summary>
        /// Set Progress Bar Context
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public void SetContext(IComponentContext ctx)
        {
            // Setup Context
            _ctx = (Context)ctx;
            _currentValue = _ctx.DefaultValue;
            
            // Setup Components
            _currentButtonHolder = GetComponent<Button>();
            _currentImageHolder = GetComponent<Image>();
            
            // Initialize Audio
            if (_audioSource == null)
            {
                _audioSource = gameObject.AddComponent<AudioSource>();
                _audioSource.clip = _clickSoundFX;
                _audioSource.loop = false;
                _audioSource.playOnAwake = false;
            }
            
            // Add Handlers
            _currentButtonHolder.onClick.AddListener(() =>
            {
                if(_audioSource.clip!=null) _audioSource.Play();
                Toggle();
            });
            
            // Update View
            UpdateView();
        }
        
        /// <summary>
        /// On Destroy
        /// </summary>
        private void OnDestroy()
        {
            if(_currentButtonHolder!=null) _currentButtonHolder.onClick.RemoveAllListeners();
        }
        
        /// <summary>
        /// Update View
        /// </summary>
        private void UpdateView()
        {
            _currentImageHolder.sprite = (_currentValue) ? _enabledImage : _disabledImage;
            if(_ctx.OnSwitcherUpdate!=null) _ctx.OnSwitcherUpdate.Invoke(_currentValue);
        }

        /// <summary>
        /// Set Switcher Enabled
        /// </summary>
        /// <param name="enabled"></param>
        public void SetEnabled(bool enabled)
        {
            _currentValue = enabled;
            UpdateView();
        }

        /// <summary>
        /// Toggle Enabled / Disabled
        /// </summary>
        public void Toggle()
        {
            _currentValue = !_currentValue;
            UpdateView();
        }
    }
}