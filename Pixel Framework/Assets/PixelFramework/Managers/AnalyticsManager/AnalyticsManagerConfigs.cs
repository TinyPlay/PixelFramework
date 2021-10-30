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
namespace PixelFramework.Managers
{
    using System;
    
    /// <summary>
    /// Analytics Manager Config
    /// </summary>
    [System.Serializable]
    public class AnalyticsManagerConfigs
    {
        /// <summary>
        /// On Analytics Initialize
        /// </summary>
        public Action OnAnalyticsInitialize;

        /// <summary>
        /// On Event Sended
        /// </summary>
        public Action<string> OnEventSended;

        /// <summary>
        /// On Event Sended with String Data
        /// </summary>
        public Action<string, string> OnStringEventSended;
        
        /// <summary>
        /// On Event Sended with Object
        /// </summary>
        public Action<string, object> OnObjectEventSended;

        /// <summary>
        /// On Revenue Sended
        /// </summary>
        public Action<string, double, string> OnRevenueSended;
    }
}