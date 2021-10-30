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
    /// Progress Bar Class
    /// </summary>
    [AddComponentMenu("Pixel Framework/UI/Image Progress Bar")]
    internal class ProgressBar : MonoBehaviour, IBaseComponent
    {
        // Context
        [System.Serializable]
        public class Context : IComponentContext
        {
            public float DefaultProgress = 0f;
            public UnityEvent<float> OnProgressUpdated = new UnityEvent<float>();
        }
        private Context _ctx;
        
        // Public Params
        [Header("View Instances")] 
        [SerializeField] private Image _progressBarImage;
        [SerializeField] private Text _progressBarValueText;
        
        // Private Params
        private float _currentProgress = 0f;

        /// <summary>
        /// Set Progress Bar Context
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public void SetContext(IComponentContext ctx)
        {
            _ctx = (Context)ctx;
            _currentProgress = _ctx.DefaultProgress;
        }

        /// <summary>
        /// On Destroy
        /// </summary>
        private void OnDestroy()
        {
            if(_ctx.OnProgressUpdated!=null) _ctx.OnProgressUpdated.RemoveAllListeners();
        }

        /// <summary>
        /// Update View
        /// </summary>
        private void UpdateView()
        {
            _progressBarImage.fillAmount = _currentProgress;
            _progressBarValueText.text = $"{GetPercentage()}%";
            if(_ctx.OnProgressUpdated!=null) _ctx.OnProgressUpdated.Invoke(_currentProgress);
        }

        /// <summary>
        /// Set Value
        /// </summary>
        /// <param name="value"></param>
        public void SetValue(float value)
        {
            _currentProgress = value;
            UpdateView();
        }

        /// <summary>
        /// Set Percentage
        /// </summary>
        /// <param name="percentage"></param>
        public void SetPercentage(float percentage)
        {
            _currentProgress = Mathf.Floor(percentage / 100f);
            UpdateView();
        }
        
        /// <summary>
        /// Get Value
        /// </summary>
        /// <returns></returns>
        public float GetValue()
        {
            return _currentProgress;
        }

        /// <summary>
        /// Get Percentage
        /// </summary>
        /// <returns></returns>
        public float GetPercentage()
        {
            return Mathf.Floor(_currentProgress * 100f);
        }
    }
}