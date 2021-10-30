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
    
    /// <summary>
    /// Base View Interface
    /// </summary>
    public interface IBaseView
    {
        /// <summary>
        /// Setup View Context
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public IBaseView SetContext(IViewContext ctx);

        /// <summary>
        /// Get Context
        /// </summary>
        /// <returns></returns>
        public IViewContext GetContext();
        
        /// <summary>
        /// Show View
        /// </summary>
        /// <param name="animationOptions"></param>
        /// <param name="onComplete"></param>
        public IBaseView ShowView(ViewAnimationOptions animationOptions = null, Action onComplete = null);

        /// <summary>
        /// Hide View
        /// </summary>
        /// <param name="animationOptions"></param>
        /// <param name="onComplete"></param>
        public IBaseView HideView(ViewAnimationOptions animationOptions = null, Action onComplete = null);

        /// <summary>
        /// Update View
        /// </summary>
        public IBaseView UpdateView();

        /// <summary>
        /// On View Initialized
        /// </summary>
        public void OnViewInitialized();

        /// <summary>
        /// On View Destroyed
        /// </summary>
        public void OnViewDestroyed();

        /// <summary>
        /// On Context Initialized
        /// </summary>
        public void OnContextInitialized();
    }
}