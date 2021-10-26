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
    /// Transition Controller
    /// </summary>
    public class TransitionPm : BasePm
    {
        [System.Serializable]
        public class Context : IPmContext
        {
            public UnityEvent ShowEvent = new UnityEvent();
            public UnityEvent HideEvent = new UnityEvent();

            public UnityEvent OnShown = new UnityEvent();
            public UnityEvent OnHidden = new UnityEvent();
            
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
            TransitionView view = viewPrefab.GetComponent<TransitionView>();
            view.SetContext(new TransitionView.Context());
            
            // Work with Events
            ctx.ShowEvent.AddListener(() =>
            {
                view.ShowView(new ViewAnimationOptions()
                {
                    IsAnimated = true,
                    AnimationDelay = 0,
                    AnimationLength = 2f,
                    AnimationType = ViewAnimationType.Fade
                }, () =>
                {
                    ctx.OnShown.Invoke();
                });
            });
            ctx.HideEvent.AddListener(() =>
            {
                view.HideView(new ViewAnimationOptions()
                {
                    IsAnimated = true,
                    AnimationDelay = 0,
                    AnimationLength = 2f,
                    AnimationType = ViewAnimationType.Fade
                }, () =>
                {
                    ctx.OnHidden.Invoke();
                });
            });
        }
    }
}