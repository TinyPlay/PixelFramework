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
    using PixelFramework.UI.View;
    using UnityEngine.Events;
    using PixelFramework.Managers;
    
    /// <summary>
    /// Settings Controller
    /// </summary>
    public class SettingsPm : BasePm
    {
        [System.Serializable]
        public class Context : IPmContext
        {
            public UnityEvent SettingsOpen = new UnityEvent();

            public Transform ViewParent;
            public GameObject ViewPrefab;
        }
        
        /// <summary>
        /// On Context Initialized
        /// </summary>
        public override void OnContextInitialized()
        {
            // Get Context
            Context ctx = (Context) GetContext();
            
            // Initialize View
            GameObject viewPrefab = GameObject.Instantiate(ctx.ViewPrefab, ctx.ViewParent);
            SettingsView view = viewPrefab.GetComponent<SettingsView>();
            
            // Initialize and Show View
            view.SetContext(new SettingsView.Context()
            {
                OnSettingsOpen = ctx.SettingsOpen,
                OnGraphiscLevelChanged = level =>
                {
                    Debug.Log($"Switch Graphics Level To: {level}");
                    GraphicsManager.Instance().SetQualityLevel(level);
                },
                
                OnLanguageChanged = locale =>
                {
                    Debug.Log($"Switch locale To: {locale}");
                    LocaleManager.Instance().SwitchLanguage(locale);
                },
                
                OnMasterVolumeChanged = volume =>
                {
                    Debug.Log($"Switch Master Volume To: {volume}");
                    AudioManager.Instance().SetMasterVolume(volume);
                },
                OnMusicSettingsChanged = volume =>
                {
                    Debug.Log($"Switch Music Volume To: {volume}");
                    AudioManager.Instance().SetMusicVolume(volume);
                },
                OnSoundsSettingsChanged = volume =>
                {
                    Debug.Log($"Switch Sounds Volume To: {volume}");
                    AudioManager.Instance().SetSoundsVolume(volume);
                },
                OnVoiceSettingsChanged = volume =>
                {
                    Debug.Log($"Switch Voice Volume To: {volume}");
                    AudioManager.Instance().SetVoicesVolume(volume);
                }
            });
        }
    }
}