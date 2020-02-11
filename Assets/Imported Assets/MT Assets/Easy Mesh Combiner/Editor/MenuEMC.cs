using UnityEngine;
using UnityEditor;

namespace MTAssets{

    /*
     * This class is responsible for creating the menu for this asset. 
     */

    public class MenuEMC : MonoBehaviour
    {
        [MenuItem("GameObject/Combine Meshes", false, 36)]
        static void OpenMeshCombinerToolWithHierarchy()
        {
            MeshCombinerTool.OpenWindow();
        }

        [MenuItem("Tools/MT Assets/Easy Mesh Combiner/Mesh Combiner Tool", false, 10)]
        static void OpenMeshCombinerTool()
        {
            MeshCombinerTool.OpenWindow();
        }

        [MenuItem("Tools/MT Assets/Easy Mesh Combiner/Read Documentation", false, 30)]
        static void ReadDocumentation()
        {
            EditorUtility.DisplayDialog("Read Documentation", "The documentation is located inside the \n\"MT Assets/Easy Mesh Combiner\" folder. Just unzip \"Documentation.zip\" on your desktop and open the \"Documentation.html\" file with your preferred browser.", "Cool!");
        }

        [MenuItem("Tools/MT Assets/Easy Mesh Combiner/Support", false, 30)]
        static void GetSupport()
        {
            EditorUtility.DisplayDialog("Support", "If you have any questions, problems or want to contact me, just contact me by email (mtassets@windsoft.xyz).", "Got it!");
        }
    }
}