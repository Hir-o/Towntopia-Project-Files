using UnityEngine;
using System.Collections;

public class AdvancedSaveSystem_MessageSystem : MonoBehaviour {
    public bool disable = false;
    public GUISkin guiSkin;
    
    public static string msg = "";
    int timer = 150;

	void OnGUI()
    {
        if (disable == false)
        {
            GUI.skin = guiSkin;
            if (msg != "")
            {
                timer--;
                if (timer == 0)
                {
                    msg = "";
                    timer = 150;
                }
                else
                {
                    GUI.Label(new Rect(10, 5, 500, 50), msg);
                }
            }
        }
    }
}
