 #if UNITY_EDITOR
    using UnityEditor;
    using UnityEngine.SceneManagement;
    using System.IO;
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MTAssets
{
    public class CombinedMeshesManager : MonoBehaviour
    {
#if UNITY_EDITOR

        public enum UndoMethod
        {
            EnableOriginalMeshes,
            ReactiveOriginalGameObjects
        }
        [HideInInspector]
        public UndoMethod undoMethod;
        [HideInInspector]
        public Transform[] listToReactive;
        [HideInInspector]
        public string[] pathsOfAssetsToDelete;
        [HideInInspector]
        public bool undoConfirmation = false;

        //The UI of this component
        [UnityEditor.CustomEditor(typeof(CombinedMeshesManager)), CanEditMultipleObjects]
        public class ConfiguracaoUI : UnityEditor.Editor
        {
            public override void OnInspectorGUI()
            {
                DrawDefaultInspector();
                serializedObject.Update();
                CombinedMeshesManager script = (CombinedMeshesManager)target;

                GUILayout.Space(20);

                GUILayout.BeginHorizontal();
                GUILayout.Space(30);
                EditorGUILayout.HelpBox("This GameObject contains the meshes you previously combined. If you want to undo the merge quickly, and restore the original Meshes/GameObjects, click the button below.\n\nKeep in mind that when you undo the merge, the original meshes or gameobjects will be re-enabled according to your preference at the time of the merge.", MessageType.Info);
                GUILayout.Space(30);
                GUILayout.EndHorizontal();

                GUILayout.Space(20);

                //Verify if has missing files of merge, if data save in assets option is enabled
                if(script.pathsOfAssetsToDelete.Length > 0)
                {
                    MeshFilter[] meshesMerged = script.GetComponentsInChildren<MeshFilter>();
                    bool hasMissingMeshes = false;
                    foreach(MeshFilter mesh in meshesMerged)
                    {
                        if(mesh.sharedMesh == null)
                        {
                            hasMissingMeshes = true;
                        }
                    }
                    if(hasMissingMeshes == true)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(30);
                        EditorGUILayout.HelpBox("\n\nOops! It looks like there are missing mesh files in this merge. To solve this problem, you can undo this merge and re-do it again!\n\n", MessageType.Error);
                        GUILayout.Space(30);
                        GUILayout.EndHorizontal();

                        GUILayout.Space(20);
                    }
                }

                if (script.undoConfirmation == false)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(30);
                    if (GUILayout.Button("Undo And Delete This Merge!", GUILayout.Height(40)))
                    {
                        script.undoConfirmation = true;
                    }
                    GUILayout.Space(30);
                    GUILayout.EndHorizontal();
                }
                if (script.undoConfirmation == true)
                {
                    var styleCenter = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold };
                    EditorGUILayout.LabelField("Undo and delete this merge?", styleCenter, GUILayout.ExpandWidth(true));

                    GUILayout.BeginHorizontal();
                    GUILayout.Space(30);
                    if (GUILayout.Button("Yes", GUILayout.Height(40)))
                    {
                        script.UndoMerge();
                    }
                    if (GUILayout.Button("No", GUILayout.Height(40)))
                    {
                        script.undoConfirmation = false;
                    }
                    GUILayout.Space(30);
                    GUILayout.EndHorizontal();
                }

                GUILayout.Space(30);
            }

        }

        void UndoMerge()
        {
            //Undo the merge, according the type of merge
            if(undoMethod == UndoMethod.EnableOriginalMeshes)
            {
                for(int i = 0; i < listToReactive.Length; i++)
                {
                    if(listToReactive[i] != null && listToReactive[i].GetComponent<MeshRenderer>() != null)
                    {
                        listToReactive[i].GetComponent<MeshRenderer>().enabled = true;
                    }
                }
            }
            if (undoMethod == UndoMethod.ReactiveOriginalGameObjects)
            {
                for (int i = 0; i < listToReactive.Length; i++)
                {
                    if (listToReactive != null)
                    {
                        listToReactive[i].gameObject.SetActive(true);
                    }
                }
            }
            //Delete all unused assets, if have
            if(pathsOfAssetsToDelete.Length > 0)
            {
                foreach(string str in pathsOfAssetsToDelete)
                {
                    if(File.Exists(str) == true)
                    {
                        AssetDatabase.DeleteAsset(str);
                    }
                }
            }
            //Show dialog
            EditorUtility.DisplayDialog("Undo Merge", "The merge was successfully undone. All of the original meshes that this Manager could still access have been restored!\n\nIf you had chosen to save the merged meshes to your project files, all useless mesh files were deleted automatically!", "Very Good!");
            //Destroy this merge
            DestroyImmediate(this.gameObject, true);
        }
#endif
    }
}