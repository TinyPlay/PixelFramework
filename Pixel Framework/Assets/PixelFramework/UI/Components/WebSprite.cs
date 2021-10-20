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
    using PixelFramework.Core.Networking;
    using PixelFramework.Managers;
    
    /// <summary>
    /// Web Sprite Class
    /// </summary>
    [RequireComponent(typeof(Sprite))]
    [AddComponentMenu("Pixel Framework/UI/Web Sprite")]
    internal class WebSprite : MonoBehaviour
    {
        // Image Params
        [Header("Sprite Configs")] 
        [SerializeField] private string Url = "";
        [SerializeField] private bool IsCaching = true;
        [SerializeField] private int CacheLifetime = 300;

        [Header("Sprite Placeholder")] 
        [SerializeField] private Sprite SpritePlaceholder;
        
        // Image Container
        private Sprite _image;

        /// <summary>
        /// On Awake
        /// </summary>
        private void Awake()
        {
            _image = gameObject.GetComponent<Sprite>();
        }

        /// <summary>
        /// On Start
        /// </summary>
        private void Start()
        {
            _image = SpritePlaceholder;
            NetworkManager.Instance().Download(new SpriteRequest(new SpriteRequestConfig()
            {
                Url = Url,
                CacheRequest = IsCaching,
                CacheLifetime = CacheLifetime,
                OnComplete = sprite =>
                {
                    _image = sprite;
                },
                OnError = error =>{
                    _image = SpritePlaceholder;
                    Debug.Log($"Failed to Download Image: {Url}");
                }
            }));
        }
    }
}