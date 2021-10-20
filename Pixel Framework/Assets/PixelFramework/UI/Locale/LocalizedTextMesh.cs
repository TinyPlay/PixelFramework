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
namespace PixelFramework.UI.Locale
{
    using UnityEngine;
    using TMPro;
    using PixelFramework.Managers;
    using PixelFramework.Models.Locale;
    
    /// <summary>
    /// Localized Text Mesh
    /// </summary>
    [RequireComponent(typeof(TextMeshPro))]
    [AddComponentMenu("Pixel Framework/Locale/Localized TextMesh")]
    internal class LocalizedTextMesh : MonoBehaviour
    {
        // UI Settings
        [SerializeField] private string localizationCode = "";
        
        // Private Params
        private TextMeshPro _text;
        
        /// <summary>
        /// On Awake
        /// </summary>
        private void Awake()
        {
            _text = GetComponent<TextMeshPro>();
        }
        
        /// <summary>
        /// On Start
        /// </summary>
        private void Start()
        {
            LocaleManager.Instance().OnLocalizationSettingsChanged.AddListener(UpdateText);
            UpdateText(LocaleManager.Instance().GetCurrentState());
        }

        /// <summary>
        /// On Destroy
        /// </summary>
        private void OnDestroy()
        {
            LocaleManager.Instance().OnLocalizationSettingsChanged.RemoveListener(UpdateText);
        }

        /// <summary>
        /// Update Text
        /// </summary>
        private void UpdateText(LocaleManagerConfigs localeConfig)
        {
            string localizedText = LocaleManager.Instance().GetItem(localizationCode);
            if (localizedText != null)
                _text.SetText(localizedText);
        }
    }
}