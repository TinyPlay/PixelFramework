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
namespace PixelFramework.Utils
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    
    /// <summary>
    /// Coroutine Provider
    /// This class provides access to a singleton to run coroutines outside of MonoBehavior Classes
    /// </summary>
    public class CoroutineProvider : MonoBehaviour
    {
        static CoroutineProvider _singleton;
        static Dictionary<string,IEnumerator> _routines = new Dictionary<string,IEnumerator>(100);

        [RuntimeInitializeOnLoadMethod( RuntimeInitializeLoadType.BeforeSceneLoad )]
        static void InitializeType ()
        {
            _singleton = new GameObject($"#{nameof(CoroutineProvider)}").AddComponent<CoroutineProvider>();
            DontDestroyOnLoad( _singleton );
        }

        /// <summary>
        /// Start Coroutine
        /// </summary>
        /// <param name="routine">Coroutine</param>
        /// <returns></returns>
        public static Coroutine Start ( IEnumerator routine ) => _singleton.StartCoroutine( routine );
        
        /// <summary>
        /// Start Coroutine with ID
        /// </summary>
        /// <param name="routine">Coroutine</param>
        /// <param name="id">Coroutine ID</param>
        /// <returns></returns>
        public static Coroutine Start ( IEnumerator routine , string id )
        {
            var coroutine = _singleton.StartCoroutine( routine );
            if( !_routines.ContainsKey(id) ) _routines.Add( id , routine );
            else
            {
                _singleton.StopCoroutine( _routines[id] );
                _routines[id] = routine;
            }
            return coroutine;
        }
        
        /// <summary>
        /// Stop Coroutine
        /// </summary>
        /// <param name="routine">Coroutine</param>
        public static void Stop ( IEnumerator routine ) => _singleton.StopCoroutine( routine );
        
        /// <summary>
        /// Stop Coroutine by ID
        /// </summary>
        /// <param name="id">Coroutine ID</param>
        public static void Stop ( string id )
        {
            if( _routines.TryGetValue(id,out var routine) )
            {
                _singleton.StopCoroutine( routine );
                _routines.Remove( id );
            }
            else Debug.LogWarning($"coroutine '{id}' not found");
        }
        
        /// <summary>
        /// Stop All Coroutines
        /// </summary>
        public static void StopAll () => _singleton.StopAllCoroutines();
    }
}