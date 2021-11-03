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
namespace PixelFramework.Components.Camera
{
    
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    
    /// <summary>
    /// Free Camera Movement Class
    /// By alexmelyon (https://gist.github.com/alexmelyon/)
    /// 
    /// Modified by Ilya Rastorguev
    /// </summary>
    [RequireComponent(typeof(Camera))]
    [AddComponentMenu("Pixel Framework/Camera/Free Camera")]
    public class FreeCamera : MonoBehaviour
    {
        [Header("Camera Params")]
        [SerializeField] private float lookSpeedH = 2f;
        [SerializeField] private float lookSpeedV = 2f;
        [SerializeField] private float zoomSpeed = 2f;
        [SerializeField] private float dragSpeed = 6f;
        
        [Header("Camera Movement Params")]
        [SerializeField] private float normalMoveSpeed = 1F;
        [SerializeField] private float fastMoveFactor = 5F;

        [Header("Camera Mouse Controls Params")] 
        [SerializeField] private int lookAroundMouseButtonIndex = 1;
        [SerializeField] private int dragCameraMouseButtonIndex = 2;
        

        // Private Params
        private float _yaw = 0f;
        private float _pitch = 0f;

        /// <summary>
        /// On Update
        /// </summary>
        private void Update()
        {
            // Look around with Right Mouse
            if (Input.GetMouseButton(lookAroundMouseButtonIndex))
            {
                _yaw += lookSpeedH * Input.GetAxis("Mouse X");
                _pitch -= lookSpeedV * Input.GetAxis("Mouse Y");

                transform.eulerAngles = new Vector3(_pitch, _yaw, 0f);
            }
            
            // Drag camera around with Middle Mouse
            if (Input.GetMouseButton(dragCameraMouseButtonIndex))
            {
                transform.Translate(-Input.GetAxisRaw("Mouse X") * Time.deltaTime * dragSpeed, -Input.GetAxisRaw("Mouse Y") * Time.deltaTime * dragSpeed, 0);
            }
            
            // Zoom in and out with Mouse Wheel
            transform.Translate(0, 0, Input.GetAxis("Mouse ScrollWheel") * zoomSpeed, Space.Self);

            _handleKeyboard();
        }

        /// <summary>
        /// Handle Keyboard Movement
        /// </summary>
        private void _handleKeyboard()
        {
            // Get Input Axis for move forward / backward
            if (Input.GetAxis("Vertical") !=0 || Input.GetAxis("Horizontal")!=0)
            {
                transform.position += transform.forward * (normalMoveSpeed * fastMoveFactor) * Input.GetAxis("Vertical") * Time.deltaTime;
                transform.position += transform.right * (normalMoveSpeed * fastMoveFactor) * Input.GetAxis("Horizontal") * Time.deltaTime;
            }
            
            // Get Keys fro Additional Movement
            if (Input.GetKey(KeyCode.Q)) 
            {
                transform.position -= transform.up * (normalMoveSpeed * fastMoveFactor) * Time.deltaTime; 
            }
            if (Input.GetKey(KeyCode.E))
            {
                transform.position += transform.up * (normalMoveSpeed * fastMoveFactor) * Time.deltaTime; 
            }
        }
    }
}