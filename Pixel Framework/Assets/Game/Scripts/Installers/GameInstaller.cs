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
    using System.Collections.Generic;
    using UnityEngine;
    using PixelFramework.Managers;
    using HyperSample.Models;
    using UnityEngine.Purchasing;
    
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
            LocaleManager.Instance().SwitchLanguage(LocaleManager.Instance().GetCurrentState().LocaleCode);
            AudioManager.Instance(new AudioManagerConfigs()).LoadState();
            NetworkManager.Instance(new NetworkManagerConfigs()).LoadState();
            GraphicsManager.Instance(new GraphicsManagerConfigs()).LoadState();

            // Add IAP Manager
            List<IAPProduct> products = new List<IAPProduct>();
            products.Add(new IAPProduct()
            {
                productID = "noads",
                productIDList = new IDs()
                {
                    {"noads", GooglePlay.Name},
                    {"noads", AppleAppStore.Name}
                }
            });
            IAPManager.Instance(new IAPManagerConfigs()
            {
                Products = products,
                PaymentVerification = true
            });
            
            // Add Ads Manager
            AdsManager.Instance(new AdsManagerConfigs()
            {
                OnAdsInitialized = () =>
                {
                    /* TODO: Initialize Ads */
                },
                OnAdsShown = adsType =>
                {
                    /* TODO: Show Ads */
                }
            });
            
            // Add Analytics Manager
            AnalyticsManager.Instance(new AnalyticsManagerConfigs()
            {
                OnAnalyticsInitialize = () =>
                {
                    /* TODO: Initialize Analytics */
                },
                OnEventSended = eventName =>
                {
                    /* TODO: Send Event */
                },
                OnObjectEventSended = (eventName, eventObject) =>
                {
                    /* TODO: Send Event with Object Data */
                },
                OnRevenueSended = (productID, price, currency) =>
                {
                    /* TODO: Send Revenue Data */
                },
                OnStringEventSended = (eventName, eventData) =>
                {
                    /* TODO: Send Event with String Data */
                }
            });
            
            // Initialize Game Manager
            GameManager.Instance(new GameStateModel()).LoadState();
        }
    }
}