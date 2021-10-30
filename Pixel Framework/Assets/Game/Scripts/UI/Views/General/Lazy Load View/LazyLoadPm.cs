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
    using UnityEngine.Events;
    using PixelFramework.UI.View;
    
    /// <summary>
    /// Lazy Load Pm
    /// </summary>
    public class LazyLoadPm : BasePm
    {
        [System.Serializable]
        public class Context : IPmContext
        {
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
            Context ctx = (Context) GetContext();
            
            // Initialize View
            GameObject viewPrefab = GameObject.Instantiate(ctx.ViewPrefab, ctx.ViewParent);
            LazyLoadView view = viewPrefab.GetComponent<LazyLoadView>();
            view.SetContext(new LazyLoadView.Context());
            
            // Work with Events
            ctx.ShowEvent.AddListener(() =>
            {
                view.ShowView(new ViewAnimationOptions()
                {
                    IsAnimated = true,
                    AnimationDelay = 0,
                    AnimationLength = 0.5f,
                    AnimationType = ViewAnimationType.Fade
                });
            });
            ctx.HideEvent.AddListener(() =>
            {
                view.HideView(new ViewAnimationOptions()
                {
                    IsAnimated = true,
                    AnimationDelay = 0,
                    AnimationLength = 0.5f,
                    AnimationType = ViewAnimationType.Fade
                });
            });
        }
    }
}