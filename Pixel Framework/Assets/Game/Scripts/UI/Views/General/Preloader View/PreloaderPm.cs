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
    using PixelFramework.UI.View;
    using UnityEngine.Events;
    
    /// <summary>
    /// Preloader Controller
    /// </summary>
    public class PreloaderPm : BasePm
    {
        [System.Serializable]
        public class Context : IPmContext
        {
            public UnityEvent<float, string> PreloaderEvent = new UnityEvent<float, string>();
            
            public UnityEvent ShowEvent = new UnityEvent();
            public UnityEvent HideEvent = new UnityEvent();
            
            public Transform ViewParent;
            public GameObject ViewPrefab;
        }

        /// <summary>
        /// On Context Initialized
        /// </summary>
        public override void OnContextInitialized()
        {
            // Get Context
            Context ctx = (Context) GetContext();
            
            // Initialize View
            GameObject viewPrefab = GameObject.Instantiate(ctx.ViewPrefab, ctx.ViewParent);
            PreloaderView view = viewPrefab.GetComponent<PreloaderView>();

            // Initialize and Show View
            view.SetContext(new PreloaderView.Context()
            {
                DefaultProgress = 0f,
                DefaultStateText = "Please Wait",
                OnProgressUpdated = ctx.PreloaderEvent
            });
            
            // Add Handlers
            ctx.ShowEvent.AddListener(() =>
            {
                view.ShowView(new ViewAnimationOptions()
                {
                    AnimationType = ViewAnimationType.Fade,
                    AnimationDelay = 0,
                    AnimationLength = 0.5f,
                    IsAnimated = true
                });
            });
            ctx.HideEvent.AddListener(() =>
            {
                view.HideView(new ViewAnimationOptions()
                {
                    AnimationType = ViewAnimationType.Fade,
                    AnimationDelay = 0,
                    AnimationLength = 0.5f,
                    IsAnimated = true
                });
            });
        }
    }
}