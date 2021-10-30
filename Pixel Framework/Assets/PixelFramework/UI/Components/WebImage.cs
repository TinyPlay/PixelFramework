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
namespace PixelFramework.UI.Components
{
    using UnityEngine;
    using UnityEngine.UI;
    using PixelFramework.Core.Networking;
    using PixelFramework.Managers;
    
    /// <summary>
    /// Web Image Class
    /// </summary>
    [RequireComponent(typeof(Image))]
    [AddComponentMenu("Pixel Framework/UI/Web Image")]
    internal class WebImage : MonoBehaviour
    {
        // Image Params
        [Header("Image Configs")] 
        [SerializeField] private string Url = "";
        [SerializeField] private bool IsCaching = true;
        [SerializeField] private int CacheLifetime = 300;

        [Header("Image Placeholder")] 
        [SerializeField] private Sprite ImagePlaceholder;
        
        // Image Container
        private Image _image;

        /// <summary>
        /// On Awake
        /// </summary>
        private void Awake()
        {
            _image = gameObject.GetComponent<Image>();
        }

        /// <summary>
        /// On Start
        /// </summary>
        private void Start()
        {
            _image.sprite = ImagePlaceholder;
            NetworkManager.Instance().Download(new SpriteRequest(new SpriteRequestConfig()
            {
                Url = Url,
                CacheRequest = IsCaching,
                CacheLifetime = CacheLifetime,
                OnComplete = sprite =>
                {
                    _image.sprite = sprite;
                },
                OnError = error =>{
                    _image.sprite = ImagePlaceholder;
                    Debug.Log($"Failed to Download Image: {Url}");
                }
            }));
        }
    }
}