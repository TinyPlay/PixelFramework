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
 * @autor           alexmelyon
 * @version         1.0
 * @url             https://gist.github.com/alexmelyon/
 * @support         https://github.com/TinyPlay/PixelFramework/issues
 * @discord         https://discord.gg/wE67T7Vm
 */
namespace PixelFramework.Components.TouchInput
{
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    /// Mouse Finger Event Structure
    /// By alexmelyon (https://gist.github.com/alexmelyon/)
    /// 
    /// Modified by Ilya Rastorguev
    /// </summary>
    public struct MouseFingerEvent
    {
        public Vector2 position;
        public int index;
    }
}