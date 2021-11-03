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
namespace PixelFramework.Components.Camera
{
    using UnityEngine;
    
    /// <summary>
    /// Third Person Camera Controller Component
    /// </summary>
    [RequireComponent(typeof(Camera))]
    [AddComponentMenu("Pixel Framework/Camera/Smooth Third Person")]
    public class SmoothThirdPersonCamera : MonoBehaviour
    {
        [Header("Camera Params")]
        [SerializeField] private Transform Target;
        [SerializeField] private float CameraSpeed = 0.125f;
        [SerializeField] private Vector3 CameraOffset = new Vector3();
        
        /// <summary>
        /// Fixed Update
        /// </summary>
        private void FixedUpdate ()
        {
            Vector3 desiredPosition = Target.position + CameraOffset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, CameraSpeed);
            transform.position = smoothedPosition;
            transform.LookAt(Target);
        }
    }
}