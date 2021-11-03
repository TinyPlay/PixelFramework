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
 * @autor           alexmelyon
 * @version         1.0
 * @url             https://gist.github.com/alexmelyon/
 * @support         https://github.com/TinyPlay/PixelFramework/issues
 * @discord         https://discord.gg/wE67T7Vm
 */
namespace PixelFramework.Components.TouchInput
{
    using UnityEngine;
    using UnityEngine.Events;
    
    /// <summary>
    /// Mouse Finger Class
    /// By alexmelyon (https://gist.github.com/alexmelyon/)
    /// 
    /// Modified by Ilya Rastorguev
    /// </summary>
    [AddComponentMenu("Pixel Framework/Input/Mouse Finger")]
    public class MouseFinger : MonoBehaviour
    {
        // Mouse Finger Events
        public UnityEvent<MouseFingerEvent> OnInputBegin;
        public UnityEvent<MouseFingerEvent> OnInputMove;
        public UnityEvent<MouseFingerEvent> OnInputEnd;
        
        /// <summary>
        /// On Update
        /// </summary>
        private void Update()
        {
            _handleMouse(0);
            _handleMouse(1);
            _handleMouse(2);
            _handleTouches();
        }
        
        /// <summary>
        /// Handle Touches
        /// </summary>
        private void _handleTouches()
        {
            for (int finger = 0; finger < Input.touchCount; finger++)
            {
                var touch = Input.touches[finger];
                var ev = new MouseFingerEvent {
                    position = touch.position,
                    index = finger
                };
                if (touch.phase == TouchPhase.Began)
                {
                    OnInputBegin.Invoke(ev) ;
                }
                if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved)
                {
                    OnInputMove.Invoke(ev);
                }
                if (touch.phase == TouchPhase.Ended)
                {
                    OnInputEnd.Invoke(ev);
                }
            }
        }
        
        /// <summary>
        /// Handle Mouse
        /// </summary>
        /// <param name="button"></param>
        private void _handleMouse(int button)
        {
            var ev = new MouseFingerEvent
            {
                position = Input.mousePosition,
                index = button
            };
            if (Input.GetMouseButtonDown(button))
            {
                OnInputBegin.Invoke(ev);
            }
            if(Input.GetMouseButton(button))
            {
                OnInputMove.Invoke(ev);
            }
            if (Input.GetMouseButtonUp(button))
            {
                OnInputEnd.Invoke(ev);
            }
        }
    }
}