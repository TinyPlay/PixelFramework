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
    using UnityEngine.Events;
    using UnityEngine.Audio;
    using PixelFramework.Managers;
    using HyperSample.Models;
    using HyperSample.UI.Views;
    using DG.Tweening;
    
    /// <summary>
    /// Menu Installer Class
    /// </summary>
    internal class MenuInstaller : MonoBehaviour
    {
        [Header("General References")] 
        [SerializeField] private Transform ViewContainer;
        [SerializeField] private AudioMixer MainMixer;
        
        [Header("View References")] 
        [SerializeField] private GameObject PreloaderViewPrefab;
        [SerializeField] private GameObject TransitionViewPrefab;
        [SerializeField] private GameObject LazyLoadViewPrefab;
        [SerializeField] private GameObject MainMenuViewPrefab;
        [SerializeField] private GameObject SettingsViewPrefab;
        [SerializeField] private GameObject StoreViewPrefab;
        [SerializeField] private GameObject LevelsViewPrefab;

        [Header("Levels Settings")] 
        [SerializeField] private List<string> LevelsNames = new List<string>();
        
        // Events
        private UnityEvent<float, string> LoadingProgressEvent = new UnityEvent<float, string>();
        
        private UnityEvent ShowLoaderEvent = new UnityEvent();
        private UnityEvent ShowTransitionEvent = new UnityEvent();
        private UnityEvent HideTransitionEvent = new UnityEvent();
        private UnityEvent CompleteLoadingEvent = new UnityEvent();
        
        private UnityEvent<bool> StartGameEvent = new UnityEvent<bool>();
        private UnityEvent SettingsWindowEvent = new UnityEvent();
        private UnityEvent StoreWindowEvent = new UnityEvent();

        private UnityEvent<int> LevelSelected = new UnityEvent<int>();

        /// <summary>
        /// On Start
        /// </summary>
        private void Start()
        {
            // Initialize Data
            GameStateModel state = (GameStateModel) GameManager.Instance().GetCurrentState();
            
            // Initialize Audio
            MainMixer.SetFloat("MainGroup", -80f);
            MainMixer.DOSetFloat("MainGroup", 0f, 2f);

            // Initialize Preloader
            PreloaderPm loaderPm = new PreloaderPm();
            loaderPm.SetContext(new PreloaderPm.Context()
            {
                PreloaderEvent = LoadingProgressEvent,
                ViewParent = ViewContainer,
                ViewPrefab = PreloaderViewPrefab,
                ShowEvent = ShowLoaderEvent
            });
            
            // Initialize Transition
            TransitionPm transitionPm = new TransitionPm();
            transitionPm.SetContext(new TransitionPm.Context()
            {
                ViewParent = ViewContainer,
                ViewPrefab = TransitionViewPrefab,
                
                ShowEvent = ShowTransitionEvent,
                OnShown = CompleteLoadingEvent,
                HideEvent = HideTransitionEvent
            });
            
            // Initialize Main Menu
            MainMenuPm menuPm = new MainMenuPm();
            menuPm.SetContext(new MainMenuPm.Context()
            {
                ViewParent = ViewContainer,
                ViewPrefab = MainMenuViewPrefab,
                
                PlayButtonClicked = StartGameEvent,
                SettingsButtonClicked = SettingsWindowEvent,
                StoreButtonClicked = StoreWindowEvent
            });

            // Initialize Settings
            SettingsPm settingsPm = new SettingsPm();
            settingsPm.SetContext(new SettingsPm.Context()
            {
                SettingsOpen = SettingsWindowEvent,
                ViewParent = ViewContainer,
                ViewPrefab = SettingsViewPrefab
            });
            
            // Initialize Store
            
            
            // Initialize Level Selector
            LevelSelectorPm levelsPm = new LevelSelectorPm();
            levelsPm.SetContext(new LevelSelectorPm.Context()
            {
                ShowLevelsPanel = StartGameEvent,
                LevelSelected = LevelSelected,
                levelsNames = LevelsNames,
                ViewParent = ViewContainer,
                ViewPrefab = LevelsViewPrefab
            });
            LevelSelected.AddListener(levelId =>
            {
                LoadGameLevel(LevelsNames[levelId]);
            });
            
            // Initialize Logic
            HideTransitionEvent.Invoke();
        }

        /// <summary>
        /// Load Game Level
        /// </summary>
        private void LoadGameLevel(string level)
        {
            ShowLoaderEvent.Invoke();
            GameManager.Instance().LoadScene(level, progress =>
            {
                LoadingProgressEvent.Invoke(progress, LocaleManager.Instance().GetItem("loading_state_1"));
            }, operation =>
            {
                MainMixer.SetFloat("MainGroup", 0f);
                MainMixer.DOSetFloat("MainGroup", -80f, 2f);
                
                LoadingProgressEvent.Invoke(1f, LocaleManager.Instance().GetItem("loading_state_1"));
                CompleteLoadingEvent.AddListener(() =>
                {
                    operation.allowSceneActivation = true;
                });
                ShowTransitionEvent.Invoke();
            });
        }
    }
}