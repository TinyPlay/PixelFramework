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
    /// Preloader View Class
    /// </summary>
    internal class PreloaderView : BaseView
    {
        // Context
        [System.Serializable]
        public class Context : IViewContext
        {
            // Default Values
            public string DefaultStateText = "";
            public float DefaultProgress = 0f;
            
            // Events
            public UnityEvent<float, string> OnProgressUpdated = new UnityEvent<float, string>();
        }

        // View References
        [Header("View References")] 
        [SerializeField] private ProgressBar _progressBar;
        [SerializeField] private Text _progressStateLabel;

        // Private Params
        private float _curretProgress = 0f;
        private string _currentStateText = "";

        /// <summary>
        /// On Context Initialized
        /// </summary>
        public override void OnContextInitialized()
        {
            Context ctx = (Context) GetContext();
            
            // Setup Default Values
            _curretProgress = ctx.DefaultProgress;
            _currentStateText = ctx.DefaultStateText;
            
            // Initialize Progress Bar
            _progressBar.SetContext(new ProgressBar.Context()
            {
                DefaultProgress = _curretProgress
            });
            
            // On Progress Updated
            ctx.OnProgressUpdated.AddListener((progress, state) =>
            {
                _curretProgress = progress;
                _currentStateText = state;
                UpdateView();
            });
            
            UpdateView();
        }

        /// <summary>
        /// On View Destroyed
        /// </summary>
        public override void OnViewDestroyed()
        {
            Context ctx = (Context) GetContext();
            ctx.OnProgressUpdated.RemoveAllListeners();
        }

        /// <summary>
        /// Update View
        /// </summary>
        /// <returns></returns>
        public override IBaseView UpdateView()
        {
            _progressBar.SetValue(_curretProgress);
            _progressStateLabel.text = _currentStateText;
            return this;
        }

        /// <summary>
        /// Set Value
        /// </summary>
        public void SetValue(float progress, string stateText)
        {
            Context ctx = (Context) GetContext();
            ctx.OnProgressUpdated.Invoke(progress, stateText);
        }

        /// <summary>
        /// Get Progress Value
        /// </summary>
        /// <returns></returns>
        public float GetValue()
        {
            return _curretProgress;
        }

        /// <summary>
        /// Get State Text
        /// </summary>
        /// <returns></returns>
        public string GetState()
        {
            return _currentStateText;
        }
    }
}