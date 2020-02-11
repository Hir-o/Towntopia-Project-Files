using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text;
using System;
using UnityEngine.SceneManagement;

namespace MTAssets
{
    public class MeshCombinerTool : EditorWindow
    {

        /*
         This class is responsible for the functioning of the "Easy Mesh Combiner" component, and all its functions.
        */
        /*
         * The Easy Mesh Combiner was developed by Marcos Tomaz in 2018.
         * Need help? Contact me (mtassets@windsoft.xyz)
         */

        Vector2 scrollPos;
        private enum AfterMerge
        {
            DisableOriginalMeshes,
            DeactiveOriginalGameObjects
        }
        private AfterMerge afterMerge;
        private bool combineChildrens = true;
        private bool combineInactives = false;
        private bool saveMeshInAssets = true;

        private GameObject[] selectedGameObjects;
        private List<string> listOfInvalidGameObjects = new List<string>();
        private List<string> listOfManyMaterials = new List<string>();
        private int verticesCountInSelection = 0;
        private int meshesToMergeCount = 0;
        private List<Material> listOfMaterials = new List<Material>();

        private List<Transform> transformsToCombine = new List<Transform>();
        private List<MeshFilter> meshFiltersToCombine = new List<MeshFilter>();
        private List<MeshRenderer> meshRenderersToCombine = new List<MeshRenderer>();

        public static void OpenWindow()
        {
            //Method to open the Window
            var window = GetWindow<MeshCombinerTool>("Combiner Tool");
            window.minSize = new Vector2(350, 650);
            window.maxSize = new Vector2(355, 655);
            var position = window.position;
            position.center = new Rect(0f, 0f, Screen.currentResolution.width, Screen.currentResolution.height).center;
            window.position = position;
            window.Show();
        }

        void OnGUI()
        {
            //Get selected GameObjects
            selectedGameObjects = Selection.gameObjects;

            //Valid the objects
            listOfInvalidGameObjects.Clear();
            listOfManyMaterials.Clear();
            transformsToCombine.Clear();
            meshFiltersToCombine.Clear();
            meshRenderersToCombine.Clear();
            listOfMaterials.Clear();
            verticesCountInSelection = 0;
            meshesToMergeCount = 0;
            for (int i = 0; i < selectedGameObjects.Length; i++)
            {
                MeshFilter[] meshFilters = new MeshFilter[0];
                MeshRenderer[] meshRenderers = new MeshRenderer[0];

                if (combineChildrens == true)
                {
                    meshFilters = selectedGameObjects[i].GetComponentsInChildren<MeshFilter>(combineInactives);
                    meshRenderers = selectedGameObjects[i].GetComponentsInChildren<MeshRenderer>(combineInactives);
                }
                if (combineChildrens == false)
                {
                    meshFilters = new MeshFilter[] { selectedGameObjects[i].GetComponent<MeshFilter>() };
                    meshRenderers = new MeshRenderer[] { selectedGameObjects[i].GetComponent<MeshRenderer>() };
                }

                //Verify if count of components is same
                if (meshFilters.Length != meshRenderers.Length)
                {
                    AddInListOfInvalidsIfNotExists(selectedGameObjects[i].name + "\n    (Mesh Filter or Mesh Renderer component missing)");
                }
                //Verify if each component is correctly filled
                if (meshFilters.Length == meshRenderers.Length)
                {
                    //Verify if each MeshFilter if filled, and not null
                    foreach (MeshFilter mf in meshFilters)
                    {
                        if (mf == null)
                        {
                            AddInListOfInvalidsIfNotExists(selectedGameObjects[i].name + "\n    (Mesh Filter component missing)");
                            continue;
                        }
                        if (mf.sharedMesh == null)
                        {
                            AddInListOfInvalidsIfNotExists(mf.name + "\n    (Have null mesh in Mesh Filter)");
                        }
                        if (mf.sharedMesh.subMeshCount != mf.GetComponent<MeshRenderer>().sharedMaterials.Length)
                        {
                            AddInListOfInvalidsIfNotExists(mf.name + "\n    (There are insufficient materials, or in excess)");
                        }
                    }
                    //Verify if each MeshRenderer has materials, and not null
                    foreach (MeshRenderer mr in meshRenderers)
                    {
                        if (mr == null)
                        {
                            AddInListOfInvalidsIfNotExists(selectedGameObjects[i].name + "\n    (Mesh Renderer component missing)");
                            continue;
                        }
                        for (int ii = 0; ii < mr.sharedMaterials.Length; ii++)
                        {
                            if (mr.sharedMaterials[ii] == null)
                            {
                                AddInListOfInvalidsIfNotExists(mr.name + "\n    (Have null materials in Mesh Renderer)");
                            }
                        }
                    }
                }
                if (listOfInvalidGameObjects.Count == 0)
                {
                    //Store meshes with more than 2 materials in list of warning
                    foreach(MeshFilter mf in meshFilters)
                    {
                        if(mf.sharedMesh.subMeshCount > 3 && mf.sharedMesh.vertexCount > 1500)
                        {
                            listOfManyMaterials.Add(mf.gameObject.name);
                        }
                    }
                    //Store the valid meshes data, to merge, if not exists in each list
                    AddInListOfMeshFiltersToMergeIfNotExists(meshFilters);
                    AddInListOfMeshRenderersToMergeIfNotExists(meshRenderers);
                }
            }

            //Support reminder
            GUILayout.Space(10);
            EditorGUILayout.HelpBox("Remember to read the Easy Mesh Combiner documentation to understand how to use it.\nGet support at: mtassets@windsoft.xyz", MessageType.None);

            GUILayout.Space(10);
            GUILayout.BeginVertical("box");

            GUILayout.BeginVertical("box");
            var styleCenter = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold };
            EditorGUILayout.LabelField(selectedGameObjects.Length.ToString() + " selected GameObjects!", styleCenter, GUILayout.ExpandWidth(true));

            //Verify if limit of vertices has been reached
            if (verticesCountInSelection <= 65000)
            {
                //Verify if exists invalid GameObjects
                if (listOfInvalidGameObjects.Count > 0)
                {
                    GUILayout.EndVertical();
                    EditorGUILayout.HelpBox("There are invalid GameObjects among the selected ones. Each GameObject that has a mesh to be combined must contain a \"Mesh Renderer\" and a \"Mesh Filter\". Make sure that all GameObjects to be combined have the two components listed. Also make sure that all GameObjects selected not have null mesh in their \"Mesh Filter\" and that all \"Mesh Renderers\" do not have null materials.", MessageType.Error);
                    StringBuilder stringBuilder = new StringBuilder();
                    foreach (string str in listOfInvalidGameObjects)
                    {
                        stringBuilder.Append(str + "\n");
                    }
                    scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(Screen.width - 15), GUILayout.Height(425));
                    EditorGUILayout.HelpBox("List of errors detected in selected GameObjects (" + listOfInvalidGameObjects.Count.ToString() + ")\n\n" + stringBuilder.ToString(), MessageType.Warning);
                    EditorGUILayout.EndScrollView();
                    EditorGUILayout.HelpBox("You can fix the errors of the GameObjects listed above, or you can simply remove them from the selection.", MessageType.Info);
                }
                if (listOfInvalidGameObjects.Count == 0)
                {
                    //Show stats if insuficient GameObjects
                    if (transformsToCombine.Count < 2)
                    {
                        GUILayout.EndVertical();
                        EditorGUILayout.HelpBox("Please select in the scene hierarchy, two or more GameObjects with meshes to be combined.", MessageType.Error);
                    }

                    //Show stats if suficiente GameObjects
                    if (transformsToCombine.Count >= 2)
                    {
                        EditorGUILayout.LabelField(transformsToCombine.Count.ToString() + " valid meshes found in this selection", styleCenter, GUILayout.ExpandWidth(true));
                        GUILayout.EndVertical();

                        GUILayout.Space(10);

                        float optimizationRate = (1 - ((float)listOfMaterials.Count / (float)meshesToMergeCount)) * (float)100;
                        EditorGUILayout.HelpBox("Statistics before optimization\n\nVertex Count: " + verticesCountInSelection.ToString() + "\nMeshes count: " + meshesToMergeCount.ToString() + "\nMaterials count: " + listOfMaterials.Count.ToString() + "\nDraw calls ± " + meshesToMergeCount.ToString() + "\nOptimization rate: 0%", MessageType.Info);
                        EditorGUILayout.HelpBox("Statistics expected after optimization\n\nVertex Count: " + verticesCountInSelection.ToString() + "\nMeshes count: " + listOfMaterials.Count.ToString() + "\nMaterials count: " + listOfMaterials.Count.ToString() + "\nDraw calls = " + listOfMaterials.Count.ToString() + "\nOptimization rate: " + optimizationRate.ToString("F2") + "%", MessageType.Info);
                        EditorGUILayout.HelpBox("The statistics are an approximation and do not take Static/Dynamic Batching into consideration", MessageType.Info);

                        GUILayout.Space(10);

                        afterMerge = (AfterMerge)EditorGUILayout.EnumPopup(new GUIContent("After Combine",
                                "What do you do after you complete the merge?\n\nDisable Original Meshes - The original meshes will be deactivated, so all the colliders and other components of the scenario will be kept intact, but the meshes will still be combined!\n\nDeactive Original GameObjects - All original GameObjects will be disabled. When you do not need to keep colliders and other active components in the scene, this is a good option!\n\nRelax, however, it is possible to undo the merge later and re-activate everything again!"),
                                afterMerge);

                        combineChildrens = (bool)EditorGUILayout.Toggle(new GUIContent("Combine Childrens",
                                    "If you want to combine children's of the selected GameObjects, enable this option!"),
                                    combineChildrens);

                        if (combineChildrens == true)
                        {
                            combineInactives = (bool)EditorGUILayout.Toggle(new GUIContent("Combine Inactives",
                                    "If you want to combine the GameObjects children that are disabled, just enable this option."),
                                    combineInactives);
                        }

                        saveMeshInAssets = (bool)EditorGUILayout.Toggle(new GUIContent("Save Mesh In Assets",
                                    "After matching the meshes, the resulting mesh will be saved in your project files. That way, you will not lose the combined mesh and you can still build your game with the combined scenario!"),
                                    saveMeshInAssets);
                        
                        //Warning of Unity limitation
                        if (listOfManyMaterials.Count > 0)
                        {
                            GUILayout.Space(8);
                            StringBuilder stringBuilder = new StringBuilder();
                            foreach (string str in listOfManyMaterials)
                            {
                                stringBuilder.Append("- " + str + "\n");
                            }
                            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(Screen.width - 15), GUILayout.Height(170));
                            EditorGUILayout.HelpBox("The GameObjects meshes listed below have many materials and many vertices, due to a limitation of Unity, some of these meshes may generate an error during merge. If this happens, try reducing the amount of materials they use.\nThis is just a warning, if none of these meshes fail during merge, do not worry, everything worked as expected!\n\n" + stringBuilder.ToString(), MessageType.Warning);
                            EditorGUILayout.EndScrollView();
                        }

                        GUILayout.Space(10);

                        if (GUILayout.Button("Combine Meshes Now!", GUILayout.Height(40)))
                        {
                            CombineNow_OneMeshPerMaterial();
                        }
                    }
                }
            }
            if (verticesCountInSelection > 65000)
            {
                GUILayout.EndVertical();
                EditorGUILayout.HelpBox("Oops! It looks like the 65.000 vertex limit for the selected GameObjects has been reached. Due to a limitation of Unity, if you combine meshes and the result mesh has more than 65.000 vertices, it may contain artifacts. Please unselect object by object until the vertex counter is below 65.000.\n\nVertices count of selected GameObjects: " + verticesCountInSelection.ToString(), MessageType.Error);
            }

            GUILayout.EndVertical();
        }

        void AddInListOfInvalidsIfNotExists(string str)
        {
            //Add the name if not exists in list
            bool existInList = false;
            for (int ii = 0; ii < listOfInvalidGameObjects.Count; ii++)
            {
                if (listOfInvalidGameObjects[ii].Equals(str) == true)
                {
                    existInList = true;
                }
            }
            if (existInList == false)
            {
                listOfInvalidGameObjects.Add(str);
            }
        }

        void AddInListOfMeshFiltersToMergeIfNotExists(MeshFilter[] meshFilters)
        {
            //Add the Meshfilter in list to merg if not exists
            foreach (MeshFilter mf in meshFilters)
            {
                //Verify if the current mesh filter exists in list
                bool existInList = false;
                for (int ii = 0; ii < meshFiltersToCombine.Count; ii++)
                {
                    if (meshFiltersToCombine[ii] == mf)
                    {
                        existInList = true;
                    }
                }
                if (existInList == false)
                {
                    verticesCountInSelection += mf.sharedMesh.vertexCount;
                    meshesToMergeCount += mf.sharedMesh.subMeshCount;
                    meshFiltersToCombine.Add(mf);
                }
            }
        }

        void AddInListOfMeshRenderersToMergeIfNotExists(MeshRenderer[] meshRenderers)
        {
            //Add the Meshrenderer in list to merge if not exists
            foreach (MeshRenderer mr in meshRenderers)
            {
                //Verify if the current mesh renderer exists in list
                bool existInList = false;
                for (int ii = 0; ii < meshRenderersToCombine.Count; ii++)
                {
                    if (meshRenderersToCombine[ii] == mr)
                    {
                        existInList = true;
                    }
                }
                if (existInList == false)
                {
                    AddInListOfMaterialsIfNotExists(mr.sharedMaterials);
                    transformsToCombine.Add(mr.GetComponent<Transform>());
                    meshRenderersToCombine.Add(mr);
                }
            }
        }

        void AddInListOfMaterialsIfNotExists(Material[] materials)
        {
            //Add the maerial in list if not exists
            foreach (Material mat in materials)
            {
                //Verify if the current material exists in list
                bool existInList = false;
                for (int ii = 0; ii < listOfMaterials.Count; ii++)
                {
                    if (listOfMaterials[ii] == mat)
                    {
                        existInList = true;
                    }
                }
                if (existInList == false)
                {
                    listOfMaterials.Add(mat);
                }
            }
        }

        void CombineNow_OneMeshPerMaterial()
        {
            //Separate each mesh according to your material
            Dictionary<Material, List<MeshFilter>> meshesPerMaterial = new Dictionary<Material, List<MeshFilter>>();
            for (int i = 0; i < meshFiltersToCombine.Count; i++)
            {
                for (int ii = 0; ii < meshFiltersToCombine[i].sharedMesh.subMeshCount; ii++)
                {
                    Material currentMaterial = meshRenderersToCombine[i].sharedMaterials[ii];

                    if (meshesPerMaterial.ContainsKey(currentMaterial) == true)
                    {
                        //Verify if this meshe is already added in list to merge, in relation to this material
                        bool exists = false;
                        foreach(MeshFilter mf in meshesPerMaterial[currentMaterial])
                        {
                            if(mf == meshFiltersToCombine[i])
                            {
                                exists = true;
                            }
                        }
                        if(exists == false)
                        {
                            meshesPerMaterial[currentMaterial].Add(meshFiltersToCombine[i]);
                        }
                    }
                    if (meshesPerMaterial.ContainsKey(currentMaterial) == false)
                    {
                        meshesPerMaterial.Add(currentMaterial, new List<MeshFilter>() { meshFiltersToCombine[i] });
                    }
                }
            }

            //Create the root GameObject
            GameObject rootGameObject = new GameObject("Combined Meshes");

            //List that contains path to combined meshes in assets
            List<string> pathsOfMeshesInAssets = new List<string>();

            //Combine the meshes
            List<GameObject> listOfGameObjects = new List<GameObject>();
            int materialCount = 0;
            foreach (var key in meshesPerMaterial)
            {
                //Get the meshes of current material
                List<MeshFilter> meshesWithSameMaterial = key.Value;

                //Process meshes and submeshes
                List<CombineInstance> combineInstances = new List<CombineInstance>();
                for (int i = 0; i < meshesWithSameMaterial.Count; i++)
                {
                    //Get current mesh of list
                    MeshFilter currentMesh = meshesWithSameMaterial[i];

                    //Verifies all submeshes
                    for (int ii = 0; ii < currentMesh.sharedMesh.subMeshCount; ii++)
                    {
                        //If this submesh is diferente of current material, ignore it
                        if (currentMesh.GetComponent<MeshRenderer>().sharedMaterials[ii].name != key.Key.name)
                        {
                            continue;
                        }

                        CombineInstance combineInstance = new CombineInstance();
                        combineInstance.mesh = currentMesh.sharedMesh;
                        combineInstance.subMeshIndex = ii;
                        combineInstance.transform = currentMesh.transform.localToWorldMatrix;
                        combineInstances.Add(combineInstance);
                    }
                }

                //Create the final mesh
                Mesh mesh = new Mesh();
                mesh.name = "Combined Mesh Material " + materialCount.ToString();
                mesh.CombineMeshes(combineInstances.ToArray(), true, true);

                //Create the GameObject to store this mesh
                GameObject gameObject = new GameObject("Material " + materialCount.ToString());
                MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
                meshRenderer.sharedMaterials = new Material[] { key.Key };
                MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
                meshFilter.sharedMesh = mesh;

                //Set to static GameObject
                GameObjectUtility.SetStaticEditorFlags(gameObject, StaticEditorFlags.BatchingStatic | StaticEditorFlags.NavigationStatic);

                //Get current date
                DateTime dateTime = new DateTime();
                dateTime = DateTime.Now;

                //Create the asset
                if (saveMeshInAssets == true)
                {
                    CreateDirectory();
                    string sceneName = SceneManager.GetActiveScene().name;
                    AssetDatabase.CreateAsset(meshFilter.sharedMesh, "Assets/MT Assets/Easy Mesh Combiner/Combined/" + sceneName + "/Mesh/" + rootGameObject.name + " (Material " + materialCount.ToString() + ") (" + dateTime.Year + dateTime.Month + dateTime.Day + dateTime.Hour + dateTime.Minute + dateTime.Second + dateTime.Millisecond.ToString() + ").asset");
                    pathsOfMeshesInAssets.Add("Assets/MT Assets/Easy Mesh Combiner/Combined/" + sceneName + "/Mesh/" + rootGameObject.name + " (Material " + materialCount.ToString() + ") (" + dateTime.Year + dateTime.Month + dateTime.Day + dateTime.Hour + dateTime.Minute + dateTime.Second + dateTime.Millisecond.ToString() + ").asset");
                }

                listOfGameObjects.Add(gameObject);
                materialCount += 1;
            }

            //Group the combined meshes in a parent gameobject root
            foreach (GameObject obj in listOfGameObjects)
            {
                obj.transform.SetParent(rootGameObject.transform);
            }

            //Configure the Combined Mesh Manager, and manage the original meshes
            CombinedMeshesManager combinedMeshesManager = rootGameObject.AddComponent<CombinedMeshesManager>();
            combinedMeshesManager.listToReactive = transformsToCombine.ToArray();
            combinedMeshesManager.pathsOfAssetsToDelete = pathsOfMeshesInAssets.ToArray();

            if(afterMerge == AfterMerge.DeactiveOriginalGameObjects)
            {
                combinedMeshesManager.undoMethod = CombinedMeshesManager.UndoMethod.ReactiveOriginalGameObjects;

                for(int i = 0; i < transformsToCombine.Count; i++)
                {
                    transformsToCombine[i].gameObject.SetActive(false);
                }
            }
            if (afterMerge == AfterMerge.DisableOriginalMeshes)
            {
                combinedMeshesManager.undoMethod = CombinedMeshesManager.UndoMethod.EnableOriginalMeshes;

                for (int i = 0; i < meshRenderersToCombine.Count; i++)
                {
                    meshRenderersToCombine[i].enabled = false;
                }
            }

            //Close this Window
            this.Close();

            //Show the alert dialog
            EditorUtility.DisplayDialog("GameObjects combined!", "The selected GameObjects were combined. If you enable \"Save Mesh In Assets\", the data can be found in the following directory...\n\n\"MT Assets/Easy Mesh Combiner/Combined/"+ SceneManager.GetActiveScene().name + "/\n\nIf you prefer to undo the merge quickly, just click on the GameObject result and use the \"Combined Meshes Manager\" to do this.\n\nThe GameObject result of this merge is being selected for you!\"", "Cool!");

            //Select Combined Meshes
            Selection.activeGameObject = rootGameObject;

            //Set to static root GameObject
            GameObjectUtility.SetStaticEditorFlags(rootGameObject, StaticEditorFlags.BatchingStatic);
        }

        private void CreateDirectory()
        {
            //Get scene name
            string sceneName = SceneManager.GetActiveScene().name;

            //Create the directory in project
            if (!AssetDatabase.IsValidFolder("Assets/MT Assets"))
            {
                AssetDatabase.CreateFolder("Assets", "MT Assets");
            }
            if (!AssetDatabase.IsValidFolder("Assets/MT Assets/Easy Mesh Combiner"))
            {
                AssetDatabase.CreateFolder("Assets/MT Assets", "Easy Mesh Combiner");
            }
            if (!AssetDatabase.IsValidFolder("Assets/MT Assets/Easy Mesh Combiner/Combined"))
            {
                AssetDatabase.CreateFolder("Assets/MT Assets/Easy Mesh Combiner", "Combined");
            }
            if (!AssetDatabase.IsValidFolder("Assets/MT Assets/Easy Mesh Combiner/Combined/" + sceneName))
            {
                AssetDatabase.CreateFolder("Assets/MT Assets/Easy Mesh Combiner/Combined", sceneName);
            }
            if (!AssetDatabase.IsValidFolder("Assets/MT Assets/Easy Mesh Combiner/Combined/" + sceneName + "/Material"))
            {
                AssetDatabase.CreateFolder("Assets/MT Assets/Easy Mesh Combiner/Combined/" + sceneName, "Material");
            }
            if (!AssetDatabase.IsValidFolder("Assets/MT Assets/Easy Mesh Combiner/Combined/" + sceneName + "/Texture"))
            {
                AssetDatabase.CreateFolder("Assets/MT Assets/Easy Mesh Combiner/Combined/" + sceneName, "Texture");
            }
            if (!AssetDatabase.IsValidFolder("Assets/MT Assets/Easy Mesh Combiner/Combined/" + sceneName + "/Mesh"))
            {
                AssetDatabase.CreateFolder("Assets/MT Assets/Easy Mesh Combiner/Combined/" + sceneName, "Mesh");
            }
        }
    }
}