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
    /// Strategy Camera Component
    /// </summary>
    [RequireComponent(typeof(Camera))]
    [AddComponentMenu("Pixel Framework/Camera/Strategy Camera")]
    public class StrategyCamera : MonoBehaviour
    {
        [Header("Camera Params")]
        [SerializeField] private bool CanCameraRotate;
        [SerializeField] private bool CanCameraZoom;

        [SerializeField] private float MinZoom = 0f;
        [SerializeField] private float MaxZoom = 10f;

        // Private Params
        protected Camera Camera;
        protected Plane Plane;
        
        /// <summary>
        /// On Awake
        /// </summary>
        private void Awake()
        {
            Camera = GetComponent<Camera>();
        }

        /// <summary>
        /// On Update
        /// </summary>
        private void Update()
        {
            // Update Plane
            if (Input.touchCount >= 1)
                Plane.SetNormalAndPosition(transform.up, transform.position);
            
            Vector3 Delta1 = Vector3.zero;
            Vector3 Delta2 = Vector3.zero;
            
            // Scroll Camera
            if (Input.touchCount >= 1)
            {
                Delta1 = PlanePositionDelta(Input.GetTouch(0));
                if (Input.GetTouch(0).phase == TouchPhase.Moved)
                    Camera.transform.Translate(Delta1, Space.World);
            }
            
            // Pinch Camera
            if (Input.touchCount >= 2)
            {
                Vector3 pos1  = PlanePosition(Input.GetTouch(0).position);
                Vector3 pos2  = PlanePosition(Input.GetTouch(1).position);
                Vector3 pos1b = PlanePosition(Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition);
                Vector3 pos2b = PlanePosition(Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition);

                //calc zoom
                float zoom = Vector3.Distance(pos1, pos2) /
                           Vector3.Distance(pos1b, pos2b);

                //edge case
                if (zoom == MinZoom || zoom > MaxZoom)
                    return;

                if (!CanCameraZoom)
                    zoom = 1;

                //Move cam amount the mid ray
                Camera.transform.position = Vector3.LerpUnclamped(pos1, Camera.transform.position, 1 / zoom);

                if (CanCameraRotate && pos2b != pos2)
                    Camera.transform.RotateAround(pos1, Plane.normal, Vector3.SignedAngle(pos2 - pos1, pos2b - pos1b, Plane.normal));
            }
        }
        
        /// <summary>
        /// Plane Position Delta
        /// </summary>
        /// <param name="touch"></param>
        /// <returns></returns>
        protected Vector3 PlanePositionDelta(Touch touch)
        {
            //not moved
            if (touch.phase != TouchPhase.Moved)
                return Vector3.zero;

            //delta
            var rayBefore = Camera.ScreenPointToRay(touch.position - touch.deltaPosition);
            var rayNow = Camera.ScreenPointToRay(touch.position);
            if (Plane.Raycast(rayBefore, out var enterBefore) && Plane.Raycast(rayNow, out var enterNow))
                return rayBefore.GetPoint(enterBefore) - rayNow.GetPoint(enterNow);

            //not on plane
            return Vector3.zero;
        }
        
        /// <summary>
        /// Plane Position
        /// </summary>
        /// <param name="screenPos"></param>
        /// <returns></returns>
        protected Vector3 PlanePosition(Vector2 screenPos)
        {
            //position
            var rayNow = Camera.ScreenPointToRay(screenPos);
            if (Plane.Raycast(rayNow, out var enterNow))
                return rayNow.GetPoint(enterNow);

            return Vector3.zero;
        }
        
        /// <summary>
        /// On Draw Gismos
        /// </summary>
        private void OnDrawGizmos()
        {
            Gizmos.DrawLine(transform.position, transform.position + transform.up);
        }
    }
}