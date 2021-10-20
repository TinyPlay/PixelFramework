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
    using UnityEngine.UI;
    using PixelFramework.Managers;
    using PixelFramework.Models.Locale;
    
    /// <summary>
    /// Localized Image
    /// </summary>
    [RequireComponent(typeof(Image))]
    [AddComponentMenu("Pixel Framework/Locale/Localized Image")]
    internal class LocalizedImage : MonoBehaviour
    {
        // Images Data
        [SerializeField] private LocalizedImagesModel images = new LocalizedImagesModel();
        
        // Private Params
        private Image _image;

        /// <summary>
        /// On Awake
        /// </summary>
        private void Awake()
        {
            _image = GetComponent<Image>();
        }
        
        /// <summary>
        /// On Start
        /// </summary>
        private void Start()
        {
            LocaleManager.Instance().OnLocalizationSettingsChanged.AddListener(UpdateImage);
            UpdateImage(LocaleManager.Instance().GetCurrentState());
        }

        /// <summary>
        /// On Destroy
        /// </summary>
        private void OnDestroy()
        {
            LocaleManager.Instance().OnLocalizationSettingsChanged.RemoveListener(UpdateImage);
        }
        
        /// <summary>
        /// Update Image
        /// </summary>
        /// <param name="localeConfig"></param>
        private void UpdateImage(LocaleManagerConfigs localeConfig)
        {
            foreach (LocalizedImagesModel.LocalizedImage imageData in images.ImagesData)
            {
                if (localeConfig.LocaleCode == imageData.Locale)
                {
                    _image.sprite = imageData.Sprite;
                    break;
                }
            }
        }
    }
}