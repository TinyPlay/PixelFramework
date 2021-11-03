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
    using System;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.Audio;
    using PixelFramework.Managers;
    using HyperSample.Models;
    using HyperSample.UI.Views;
    using DG.Tweening;
    
    /// <summary>
    /// Game Level Installer Class
    /// </summary>
    internal class GameLevelInstaller : MonoBehaviour
    {
        [Header("General References")] 
        [SerializeField] private Transform ViewContainer;
        [SerializeField] private AudioMixer MainMixer;
        
        [Header("View References")] 
        [SerializeField] private GameObject PreloaderViewPrefab;
        [SerializeField] private GameObject TransitionViewPrefab;
        [SerializeField] private GameObject MainUIPrefab;
        [SerializeField] private GameObject WinUIPrefab;
        [SerializeField] private GameObject LoseUIPrefab;
        [SerializeField] private GameObject PauseUIPrefab;
        [SerializeField] private GameObject TutorialUIPrefab;
        
        [Header("Gameplay References")]
        
        // Private Params
        private int CoinsColleted = 0;
        private int CoinsHighScore = 0;
        private int StarsColleted = 0;
        private int StarsHighScore = 0;
        
        // Events
        private UnityEvent<float, string> LoadingProgressEvent = new UnityEvent<float, string>();
        private UnityEvent ShowTransitionEvent = new UnityEvent();
        private UnityEvent CompleteLoadingEvent = new UnityEvent();

        private UnityEvent OnMenuExit = new UnityEvent();
        
        private UnityEvent ShowLoaderEvent = new UnityEvent();
        private UnityEvent OnTutorialShown = new UnityEvent();
        private UnityEvent<bool> OnGamePaused = new UnityEvent<bool>();
        private UnityEvent OnGameWin = new UnityEvent();
        private UnityEvent OnGameLose = new UnityEvent();
        
        private UnityEvent<int> OnCoinColleted = new UnityEvent<int>();

        /// <summary>
        /// On Start
        /// </summary>
        private void Start()
        {
            // Initialize Game Data
            GameStateModel gameData = (GameStateModel) GameManager.Instance().GetCurrentState();
            GameLevelModel levelData = (gameData.LevelDatas.Count - 1 <= gameData.CurrentLevel && gameData.LevelDatas.Count > 0)
                ? gameData.LevelDatas[gameData.CurrentLevel]
                : new GameLevelModel();
            
            // Initialize Loop Data
            CoinsColleted = 0;
            StarsColleted = 0;
            CoinsHighScore = (int) levelData.ScoresCollected;
            StarsHighScore = levelData.StarsCount;
            
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
            
            // Initialize Main UI
            MainUIPm mainUIPm = new MainUIPm();
            mainUIPm.SetContext(new MainUIPm.Context()
            {
                GamePaused = OnGamePaused,
                OnCoinColleted = OnCoinColleted,
                ViewParent = ViewContainer,
                ViewPrefab = MainUIPrefab
            });
            
            // Initialize Transition
            TransitionPm transitionPm = new TransitionPm();
            transitionPm.SetContext(new TransitionPm.Context()
            {
                ViewParent = ViewContainer,
                ViewPrefab = TransitionViewPrefab,
                
                ShowEvent = ShowTransitionEvent,
                OnShown = CompleteLoadingEvent
            });
            
            // Initialize Pause Menu
            PausePm pausePm = new PausePm();
            pausePm.SetContext(new PausePm.Context()
            {
                OnMenuExit = OnMenuExit,
                GamePaused = OnGamePaused,
                ViewParent = ViewContainer,
                ViewPrefab = PauseUIPrefab
            });

            // Initialize Win UI

            // Initialize Lose UI
            
            // Initialize Player Controller
            
            // Initialize Finish Controller
            
            // Initialize Enemy Controller
            
            // Initialize Coins Controller
            
            // Initialize Hanlders
            OnMenuExit.AddListener(GoToTheMenu);
            OnGamePaused.AddListener(isPaused =>
            {
                if(isPaused)
                    GameManager.Instance().PauseGame();
                else
                    GameManager.Instance().UnpauseGame();
            });
        }

        /// <summary>
        /// On Destroy
        /// </summary>
        private void OnDestroy()
        {
            OnMenuExit.RemoveAllListeners();
            OnGamePaused.RemoveAllListeners();
        }

        /// <summary>
        /// Go to the Next Level
        /// </summary>
        private void GoToTheNextLevel()
        {
            // Update Game Data
            string nextLevelName = "";
            

            // Load Next Level
            LoadScene(nextLevelName);
        }

        /// <summary>
        ///  Go to the Menu
        /// </summary>
        private void GoToTheMenu()
        {
            LoadScene("MenuScene");
        }

        /// <summary>
        /// Load Scene
        /// </summary>
        /// <param name="scene"></param>
        private void LoadScene(string scene)
        {
            ShowLoaderEvent.Invoke();
            GameManager.Instance().LoadScene(scene, progress =>
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