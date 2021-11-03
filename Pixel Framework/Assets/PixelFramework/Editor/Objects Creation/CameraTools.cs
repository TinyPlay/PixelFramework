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
namespace PixelFramework.Editor.Objects_Creation
{
    using UnityEditor;
    using UnityEngine;
    using PixelFramework.Components.Camera;
    
    #if UNITY_EDITOR
    /// <summary>
    /// Camera Generators
    /// </summary>
    [ExecuteInEditMode]
    public class CameraTools : MonoBehaviour
    {
        [MenuItem("Pixel Framework/Create/Free Camera", false, 0)]
        static void CreateFreeCamera()
        {
            GameObject freeCam = new GameObject("Free Camera");
            freeCam.transform.SetAsFirstSibling();
            freeCam.AddComponent<FreeCamera>();
            Selection.activeGameObject = freeCam;
        }
        
        [MenuItem("Pixel Framework/Create/Smooth Follow Camera", false, 0)]
        static void CreateSmoothFollowCamera()
        {
            GameObject freeCam = new GameObject("Smooth Follow Camera");
            freeCam.transform.SetAsFirstSibling();
            freeCam.AddComponent<SmoothThirdPersonCamera>();
            Selection.activeGameObject = freeCam;
        }
        
        [MenuItem("Pixel Framework/Create/Strategy Camera", false, 0)]
        static void CreateStrategyCamera()
        {
            GameObject freeCam = new GameObject("Strategy Camera");
            freeCam.transform.SetAsFirstSibling();
            freeCam.AddComponent<StrategyCamera>();
            Selection.activeGameObject = freeCam;
        }
    }
    #endif
}