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
namespace PixelFramework.UI.View
{
    using System;
    using UnityEngine;
    using DG.Tweening;
    
    /// <summary>
    /// Base View Class
    /// </summary>
    internal class BaseView : MonoBehaviour, IBaseView
    {
        // View Context
        private IViewContext _ctx;
        
        // View Container
        [Header("View Container")]
        [SerializeField] private Canvas viewCanvas;
        
        // Container for Animations
        private CanvasGroup _viewGroup;
        private RectTransform _viewTransform;

        #region Setup View
        /// <summary>
        /// Set View Context
        /// </summary>
        /// <param name="ctx"></param>
        public IBaseView SetContext(IViewContext ctx)
        {
            _ctx = ctx;
            OnContextInitialized();
            return this;
        }

        /// <summary>
        /// Get Context
        /// </summary>
        /// <returns></returns>
        public IViewContext GetContext()
        {
            return _ctx;
        }

        /// <summary>
        /// On Context Initialized
        /// </summary>
        public virtual void OnContextInitialized()
        {
        }
        #endregion

        #region Base Mono
        /// <summary>
        /// On Awake
        /// </summary>
        private void Awake()
        {
            // View Canvas Detecting
            viewCanvas = gameObject.GetComponent<Canvas>();
            _viewTransform = gameObject.GetComponent<RectTransform>();
            _viewGroup = gameObject.GetComponent<CanvasGroup>() ?? gameObject.AddComponent<CanvasGroup>();
            
            
            // On Before Initialized
            OnViewInitialized();
        }
        
        /// <summary>
        /// Awake Event Wrapper
        /// </summary>
        public virtual void OnViewInitialized() {
        }
        
        /// <summary>
        /// On Destroy
        /// </summary>
        private void OnDestroy()
        {
            // Remove DOTween
            _viewTransform?.DOKill();
            _viewGroup?.DOKill();
            
            OnViewDestroyed();
        }
        
        /// <summary>
        /// On Destroy Wrapper
        /// </summary>
        public virtual void OnViewDestroyed() {
        }
        #endregion

        #region Base View Methods
        /// <summary>
        /// Update View
        /// </summary>
        public virtual IBaseView UpdateView()
        {
            return this;
        }
        
        /// <summary>
        /// Check is View Shown
        /// </summary>
        /// <returns></returns>
        public bool IsViewShown()
        {
            return viewCanvas.enabled;
        }

        /// <summary>
        /// Show View
        /// </summary>
        /// <param name="animationOptions"></param>
        /// <param name="onComplete"></param>
        public IBaseView ShowView(ViewAnimationOptions animationOptions = null, Action onComplete = null)
        {
            viewCanvas.enabled = true;
            if (animationOptions == null) animationOptions = new ViewAnimationOptions();
            if (animationOptions.IsAnimated) {

                if (animationOptions.AnimationType == ViewAnimationType.Fade)
                {
                    _viewGroup.DOFade(1f, animationOptions.AnimationLength).From(0f)
                        .SetDelay(animationOptions.AnimationDelay).OnComplete(() =>
                        {
                            if (onComplete != null) onComplete();
                        });
                }

                if (animationOptions.AnimationType == ViewAnimationType.Scale)
                {
                    _viewTransform.DOScale(1f, animationOptions.AnimationLength).From(0f)
                        .SetDelay(animationOptions.AnimationDelay).OnComplete(() =>
                        {
                            if (onComplete != null) onComplete();
                        });
                }
            } else {
                if (onComplete != null) onComplete();
            }
            return this;
        }

        /// <summary>
        /// Hide View
        /// </summary>
        /// <param name="animationOptions"></param>
        /// <param name="onComplete"></param>
        public IBaseView HideView(ViewAnimationOptions animationOptions = null, Action onComplete = null)
        {
            if (animationOptions == null) animationOptions = new ViewAnimationOptions();
            
            if (animationOptions.IsAnimated)
            {
                if (animationOptions.AnimationType == ViewAnimationType.Fade)
                {
                    _viewGroup.DOFade(0f, animationOptions.AnimationLength).From(1f)
                        .SetDelay(animationOptions.AnimationDelay).OnComplete(() =>
                        {
                            viewCanvas.enabled = false;
                            if (onComplete != null) onComplete();
                        });
                }

                if (animationOptions.AnimationType == ViewAnimationType.Scale)
                {
                    _viewTransform.DOScale(0f, animationOptions.AnimationLength).From(1f)
                        .SetDelay(animationOptions.AnimationDelay).OnComplete(() =>
                        {
                            viewCanvas.enabled = false;
                            if (onComplete != null) onComplete();
                        });
                }
            }
            else
            {
                viewCanvas.enabled = false;
                if (onComplete != null) onComplete();
            }
            return this;
        }
        #endregion
    }
}