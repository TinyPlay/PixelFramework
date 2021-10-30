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
    /// Web Audio Class
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    [AddComponentMenu("Pixel Framework/Audio/Web Audio")]
    internal class WebAudio : MonoBehaviour
    {
        // Audio Params
        [Header("Audio Configs")] 
        [SerializeField] private string Url = "";
        [SerializeField] private bool IsCaching = true;
        [SerializeField] private int CacheLifetime = 300;
        [SerializeField] private bool PlayOnDownloaded = false;
        
        // Audio Container
        private AudioSource _audio;
        
        /// <summary>
        /// On Awake
        /// </summary>
        private void Awake()
        {
            _audio = gameObject.GetComponent<AudioSource>();
        }
        
        /// <summary>
        /// On Start
        /// </summary>
        private void Start()
        {
            NetworkManager.Instance().Download(new AudioClipRequest(new AudioClipRequestConfig()
            {
                Url = Url,
                CacheRequest = IsCaching,
                CacheLifetime = CacheLifetime,
                OnComplete = clip =>
                {
                    _audio.clip = clip;
                    if(PlayOnDownloaded) _audio.Play();
                },
                OnError = error =>{
                    Debug.Log($"Failed to Download Audio: {Url}");
                }
            }));
        }
    }
}