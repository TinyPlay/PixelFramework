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
    /// Transition View
    /// </summary>
    internal class TransitionView : BaseView
    {
        // Context
        [System.Serializable]
        public class Context : IViewContext
        {
        }
    }
}