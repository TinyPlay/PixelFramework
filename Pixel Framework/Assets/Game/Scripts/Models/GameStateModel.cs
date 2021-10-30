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
namespace HyperSample.Models
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using PixelFramework.Managers;

    [System.Serializable]
    public class GameStateModel : IGameState
    {
        // Base Game Params
        public bool IsPrivacyAccepted = false;
        
        // General Data
        public int HardCurrency = 0;
        public int SoftCurrency = 0;
        
        // Levels Data
        public int CurrentLevel = 0;
        public List<GameLevelModel> LevelDatas = new List<GameLevelModel>();
    }
}