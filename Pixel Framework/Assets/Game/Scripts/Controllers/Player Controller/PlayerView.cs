namespace HyperSample.Controllers
{
    using UnityEngine;
    using PixelFramework.UI.View;
    
    /// <summary>
    /// Player View
    /// </summary>
    internal class PlayerView : MonoBehaviour
    {
        [System.Serializable]
        public class Context
        {
            
        }
        private Context _ctx;
        
        
        /// <summary>
        /// Set View Context
        /// </summary>
        /// <returns></returns>
        public PlayerView SetContext(Context ctx)
        {
            _ctx = ctx;
            return this;
        }
    }
}