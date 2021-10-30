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
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.SceneManagement;
    using PixelFramework.Core.ContentManagement;
    using PixelFramework.Utils;
    
    /// <summary>
    /// Game Manager Class
    /// </summary>
    public class GameManager : IGameManager
    {
        // Game Manager Events
        public UnityEvent<IGameState> OnGameStateChanged = new UnityEvent<IGameState>();
        public UnityEvent<bool> OnGamePaused = new UnityEvent<bool>();

        // Private Params
        private static GameManager _instance;
        private IGameState _config;

        #region Base Manager Logic
        /// <summary>
        /// Game Manager Constructor
        /// </summary>
        /// <param name="config"></param>
        private GameManager(IGameState config = null)
        {
            if (config != null) _config = config;
        }
        
        /// <summary>
        /// Get Game Manager Instance
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static GameManager Instance(IGameState config = null)
        {
            if (_instance == null)
                _instance = new GameManager(config);
            return _instance;
        }
        
        /// <summary>
        /// Set Current Game Manager State
        /// </summary>
        /// <param name="config"></param>
        public GameManager SetState(IGameState config)
        {
            _config = config;
            return _instance;
        }

        /// <summary>
        /// Get Current Game Manager State
        /// </summary>
        /// <returns></returns>
        public IGameState GetCurrentState()
        {
            return _config;
        }
        
        /// <summary>
        /// Load Manager State
        /// </summary>
        public void LoadState()
        {
            string path = "/game_state.dat";
            FileReader.ReadObjectFromFile(_config, path, SerializationType.EncryptedJSON);
        }

        /// <summary>
        /// Save Manager State
        /// </summary>
        public void SaveState()
        {
            string path = "/game_state.dat";
            FileReader.SaveObjectToFile(path, _config, SerializationType.EncryptedJSON);
            if(OnGameStateChanged!=null) OnGameStateChanged.Invoke(_config);
        }
        #endregion

        #region Game Pause
        /// <summary>
        /// Pause Game
        /// </summary>
        public void PauseGame()
        {
            if(OnGamePaused!=null) OnGamePaused.Invoke(true);
        }

        /// <summary>
        /// Unpause Game
        /// </summary>
        public void UnpauseGame()
        {
            if(OnGamePaused!=null) OnGamePaused.Invoke(false);
        }
        #endregion
        
        #region Scene Management
        /// <summary>
        /// Load Scene by Name
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="onProgress"></param>
        /// <param name="onComplete"></param>
        public void LoadScene(string sceneName, Action<float> onProgress = null, Action<AsyncOperation> onComplete = null)
        {
            CoroutineProvider.Start(LoadSceneAsync(sceneName, onProgress, onComplete));
        }
        
        /// <summary>
        /// Load Scene Async
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="onProgress"></param>
        /// <param name="onComplete"></param>
        /// <returns></returns>
        private IEnumerator LoadSceneAsync(string sceneName, Action<float> onProgress = null, Action<AsyncOperation> onComplete = null)
        {
            yield return null;
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
            asyncOperation.allowSceneActivation = false;
            while (!asyncOperation.isDone)
            {
                //Output the current progress
                if (onProgress != null) onProgress(asyncOperation.progress);
                // Check if the load has finished
                if (asyncOperation.progress >= 0.9f)
                {
                    if (onComplete != null)
                    {
                        onComplete(asyncOperation);
                    }
                    else
                    {
                        asyncOperation.allowSceneActivation = true;
                    }
                }

                yield return null;
            }
        }
        #endregion
    }
}