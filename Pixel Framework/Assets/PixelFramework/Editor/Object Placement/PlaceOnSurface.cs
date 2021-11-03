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
namespace PixelFramework.Editor
{
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;
    
    #if UNITY_EDITOR
    /// <summary>
    /// Place object on Surface
    /// By alexmelyon (https://gist.github.com/alexmelyon/)
    /// 
    /// Modified by Ilya Rastorguev
    /// </summary>
    [ExecuteInEditMode]
    public class PlaceOnSurface : MonoBehaviour
    {
        [MenuItem("Pixel Framework/Tools/Place Object to Surface _END", true, 3)]
        static bool ValidatePlaceSelectionOnTerrain()
        {
            return Selection.activeTransform != null;
        }

        [MenuItem("Pixel Framework/Tools/Place Object to Surface _END", false, 103)]
        static void PlaceSelectionOnTerrain()
        {
            foreach (Transform t in Selection.transforms)
            {
                Undo.RecordObject(t, "Move " + t.name);

                var meshComponents = t.GetComponentsInChildren<MeshRenderer>();
                if (meshComponents.Length != 0)
                {
                    var vs = GetVertices(meshComponents);
                    for(int i = 0; i < vs.Length; i++) { vs[i].y += t.position.y; }
                    var min = LowestVertice(vs);
                    var origin = t.position;
                    RaycastHit hit;
                    if(Physics.Raycast(origin, Vector3.down, out hit))
                    {
                        float distance = min.y - hit.point.y;
                        t.Translate(Vector3.down * distance);
                    }
                }
            }
        }

        /// <summary>
        /// Lowest Verticle
        /// </summary>
        /// <param name="vs"></param>
        /// <returns></returns>
        private static Vector3 LowestVertice(Vector3[] vs)
        {
            Vector3 min = vs[0];
            foreach(var v in vs)
            {
                if(v.y < min.y)
                {
                    min = v;
                }
            }
            return min;
        }

        /// <summary>
        /// Get Verticles
        /// </summary>
        /// <param name="meshes"></param>
        /// <returns></returns>
        private static Vector3[] GetVertices(MeshRenderer[] meshes)
        {
            var vertices = new List<Vector3>();
            foreach (var m in meshes)
            {
                var vs = m.GetComponent<MeshFilter>().sharedMesh.vertices;
                vertices.AddRange(vs);
            }
            return vertices.ToArray();
        }
    }
    #endif
}