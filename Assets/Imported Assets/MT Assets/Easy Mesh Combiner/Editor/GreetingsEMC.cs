using UnityEngine;
using UnityEditor;

namespace MTAssets
{
    /*
     * This class is responsible for displaying the welcome message when installing this asset.
     */

    [InitializeOnLoad]
    class GreetingsEMC
    {
        static GreetingsEMC()
        {
            if (!PlayerPrefs.HasKey("GreetingsEMC"))
            {
                //Show greetings and save
                EditorUtility.DisplayDialog("Easy Mesh Combiner was imported!",
                    "The \"Easy Mesh Combiner\" was imported for your project. You should be able to locate it in the directory \n(MT Assets/Easy Mesh Combiner). \n\n Remember to read the documentation to learn how to use this asset. To read the documentation, extract the contents of \"Documentation.zip\" inside the \n\"MT Assets/Easy Mesh Combiner\" folder. Then just open the \"Documentation.html\" in your favorite browser. \n\n If you need help, contact me via email (mtassets@windsoft.xyz).",
                    "Cool!");
                PlayerPrefs.SetInt("GreetingsEMC", 1);
            }
        }
    }
}