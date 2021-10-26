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
    using UnityEngine.Events;
    using PixelFramework.Managers;
    using HyperSample.Models;
    using HyperSample.UI.Views;
    
    /// <summary>
    /// Loader Installer Class
    /// </summary>
    internal class LoaderInstaller : MonoBehaviour
    {
        [Header("General References")] 
        [SerializeField] private Transform ViewContainer;
        
        [Header("View References")] 
        [SerializeField] private GameObject PreloaderViewPrefab;
        [SerializeField] private GameObject PrivacyViewPrefab;
        [SerializeField] private GameObject TransitionViewPrefab;

        /// <summary>
        /// On Start
        /// </summary>
        private void Start()
        {
            // Initialize Events
            UnityEvent<float, string> LoadingProgressEvent = new UnityEvent<float, string>();
            
            UnityEvent ShowLoaderEvent = new UnityEvent();
            UnityEvent ShowPrivacyEvent = new UnityEvent();
            UnityEvent ShowTransitionEvent = new UnityEvent();
            
            // Initialize Data
            GameStateModel state = (GameStateModel) GameManager.Instance().GetCurrentState();

            // Initialize Preloader
            PreloaderPm loaderPm = new PreloaderPm();
            loaderPm.SetContext(new PreloaderPm.Context()
            {
                PreloaderEvent = LoadingProgressEvent,
                ViewParent = ViewContainer,
                ViewPrefab = PreloaderViewPrefab,
                ShowEvent = ShowLoaderEvent
            });
            
            // Initialize Privacy
            PrivacyPm privacyPm = new PrivacyPm();
            privacyPm.SetContext(new PrivacyPm.Context()
            {
                ViewParent = ViewContainer,
                ViewPrefab = PrivacyViewPrefab,
                
                OnPrivacyAccepted = () =>
                {
                    LoadingProgressEvent.Invoke(0, LocaleManager.Instance().GetItem("loading_state_1"));
                    state.IsPrivacyAccepted = true;
                    GameManager.Instance().SaveState();
                    LoadMenuScene();
                },
                OnPrivacyDelinced = () =>
                {
                    Application.Quit();
                },
                PrivacyShowEvent = ShowPrivacyEvent
            });
            
            // Initialize Transition
            TransitionPm transitionPm = new TransitionPm();
            transitionPm.SetContext(new TransitionPm.Context()
            {
                ViewParent = ViewContainer,
                ViewPrefab = TransitionViewPrefab,
                
                ShowEvent = ShowTransitionEvent
            });

            // Initialize Logic
            LoadingProgressEvent.Invoke(0, LocaleManager.Instance().GetItem("loading_state_0"));
            ShowLoaderEvent.Invoke();
            if (state.IsPrivacyAccepted)
            {
                LoadingProgressEvent.Invoke(0, LocaleManager.Instance().GetItem("loading_state_1"));
                LoadMenuScene();
            }
            else
            {
                ShowPrivacyEvent.Invoke();
            }
        }

        /// <summary>
        /// Load Menu Scene
        /// </summary>
        private void LoadMenuScene()
        {
            
        }
    }
}