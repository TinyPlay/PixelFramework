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

using PixelFramework.Managers;

namespace HyperSample.UI.Views
{
    using System.Collections.Generic;
    using UnityEngine;
    using PixelFramework.UI.View;
    using UnityEngine.Events;
    using HyperSample.Models;
    
    /// <summary>
    /// Level Selector Controller
    /// </summary>
    public class LevelSelectorPm : BasePm
    {
        [System.Serializable]
        public class Context : IPmContext
        {
            public UnityEvent<int> LevelSelected = new UnityEvent<int>();
            public UnityEvent<bool> ShowLevelsPanel = new UnityEvent<bool>();

            public List<string> levelsNames = new List<string>();

            public Transform ViewParent;
            public GameObject ViewPrefab;
        }

        private LevelSelectorView view;
        
        /// <summary>
        /// On Context Initialized
        /// </summary>
        public override void OnContextInitialized()
        {
            // Get Context
            Context ctx = (Context) GetContext();
            GameStateModel gameData = (GameStateModel) GameManager.Instance().GetCurrentState();
            
            // Load Levels
            List<GameLevelModel> levelsData = new List<GameLevelModel>();
            foreach (string levelMap in ctx.levelsNames)
            {
                levelsData.Add(new GameLevelModel()
                {
                    IsCompleted = false,
                    ScoresCollected = 0,
                    StarsCount = 0
                });
            }

            int levelIndex = 0;
            foreach (GameLevelModel levelData in gameData.LevelDatas)
            {
                levelsData[levelIndex].IsCompleted = levelData.IsCompleted;
                levelsData[levelIndex].ScoresCollected = levelData.ScoresCollected;
                levelsData[levelIndex].StarsCount = levelData.StarsCount;
                levelIndex++;
            }
            
            // Initialize View
            GameObject viewPrefab = GameObject.Instantiate(ctx.ViewPrefab, ctx.ViewParent);
            view = viewPrefab.GetComponent<LevelSelectorView>();
            
            // Initialize and Show View
            view.SetContext(new LevelSelectorView.Context()
            {
                OnLevelLaunched = levelId =>
                {
                    ctx.LevelSelected.Invoke(levelId);
                },
                OnLevelsOpen = ctx.ShowLevelsPanel,
                CurrentLevel = gameData.CurrentLevel,
                Levels = levelsData,
                StarsValue = gameData.SoftCurrency
            });
        }

        /// <summary>
        /// Get Current Panel State
        /// </summary>
        /// <returns></returns>
        public bool GetCurrentPanelState()
        {
            if (view != null)
            {
                if (view.IsViewShown())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}