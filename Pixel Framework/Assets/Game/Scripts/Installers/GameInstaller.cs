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
namespace HyperSample.Installers
{
    using UnityEngine;
    using PixelFramework.Managers;
    using HyperSample.Models;
    
    /// <summary>
    /// Game Installer Class
    /// </summary>
    internal class GameInstaller : MonoBehaviour
    {
        /// <summary>
        /// On Scene Started
        /// </summary>
        private void Start()
        {
            // Initialize Managers
            LocaleManager.Instance(new LocaleManagerConfigs()).LoadState();
            AudioManager.Instance(new AudioManagerConfigs()).LoadState();
            NetworkManager.Instance(new NetworkManagerConfigs()).LoadState();
            GraphicsManager.Instance(new GraphicsManagerConfigs()).LoadState();
            IAPManager.Instance(new IAPManagerConfigs()).LoadState();
            
            
            GameManager.Instance(new GameStateModel()).LoadState();
        }
    }
}