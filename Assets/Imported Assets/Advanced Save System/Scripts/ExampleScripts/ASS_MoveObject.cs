using UnityEngine;
using System.Collections;

public class ASS_MoveObject : MonoBehaviour
{

    public Transform[] objects;
    int currentObj = 0;
	public KeyCode nextObj = KeyCode.Space, moveLeft = KeyCode.LeftArrow, moveRight = KeyCode.RightArrow, moveForward = KeyCode.UpArrow, moveBack = KeyCode.DownArrow, moveUp = KeyCode.PageUp, moveDown = KeyCode.PageDown, rotateMod = KeyCode.R, scaleLeft = KeyCode.J, scaleRight = KeyCode.L, scaleUp = KeyCode.I, scaleDown = KeyCode.K;
    bool rotateMode = false;
    int cSlot = 1;
    void OnGUI()
    {
        if (GUI.Button(new Rect(Screen.width / 2 - 87, 20, 150, 30), "Change Slot" + cSlot.ToString()))
        {
            if (cSlot < 3)
                cSlot++;
            else
                cSlot = 1;
        }
        if (GUI.Button(new Rect(Screen.width / 2 - 87, 55, 150, 25), "Save Slot" + cSlot.ToString()))
        {
            transform.GetComponent<AdvancedSaveSystem_SaveGO>().SaveGameObjects(cSlot);
        }
        if (GUI.Button(new Rect(Screen.width / 2 - 87, 85, 150, 25), "Load Slot" + cSlot.ToString()))
        {
            foreach (Transform obj in objects)
            {
                obj.GetComponent<Renderer>().enabled = false;
            }
            transform.GetComponent<AdvancedSaveSystem_LoadGO>().LoadGameObjects(cSlot);
        }
		if (GUI.Button(new Rect(Screen.width / 2 - 87, 115, 150, 25), "Refresh From Slot " + cSlot.ToString()))
		{
			transform.GetComponent<AdvancedSaveSystem_LoadGO>().RefreshGameObjects (cSlot);
		}
    }

    void Update()
    {
        if (Input.GetKeyDown(nextObj))
        {
            if (currentObj < objects.Length)
            {
                currentObj++;
            }
            else
                currentObj = 0;
        }
        if (Input.GetKeyDown(rotateMod))
        {
            if (rotateMode == true)
                rotateMode = false;
            else
                rotateMode = true;
        }
        {
            //Control OBJ
            if (rotateMode == false)
            {
                if (Input.GetKey(moveLeft))
                {
                    objects[currentObj].Translate(Vector3.left * 2 * Time.deltaTime);
                }
                if (Input.GetKey(moveRight))
                {
                    objects[currentObj].Translate(Vector3.right * 2 * Time.deltaTime);
                }
                if (Input.GetKey(moveForward))
                {
                    objects[currentObj].Translate(Vector3.forward * 2 * Time.deltaTime);
                }
                if (Input.GetKey(moveBack))
                {
                    objects[currentObj].Translate(Vector3.back * 2 * Time.deltaTime);
                }
                if (Input.GetKey(moveUp))
                {
                    objects[currentObj].Translate(Vector3.up * 2 * Time.deltaTime);
                }
                if (Input.GetKey(moveDown))
                {
                    objects[currentObj].Translate(Vector3.down * 2 * Time.deltaTime);
                }
            }
            else
            {
                if (Input.GetKey(moveLeft))
                {
                    objects[currentObj].Rotate(Vector3.up * 50 * Time.deltaTime);
                }
                if (Input.GetKey(moveRight))
                {
                    objects[currentObj].Rotate(Vector3.down * 50 * Time.deltaTime);
                }
                if (Input.GetKey(moveForward))
                {
                    objects[currentObj].Rotate(Vector3.forward * 50 * Time.deltaTime);
                }
                if (Input.GetKey(moveBack))
                {
                    objects[currentObj].Rotate(Vector3.back * 50 * Time.deltaTime);
                }
                if (Input.GetKey(moveUp))
                {
                    objects[currentObj].Rotate(Vector3.right * 50 * Time.deltaTime);
                }
                if (Input.GetKey(moveDown))
                {
                    objects[currentObj].Rotate(Vector3.left * 50 * Time.deltaTime);
                }
			}
			//Scale Object
			if (Input.GetKey (scaleLeft))
			{
				objects[currentObj].localScale += new Vector3(0.05f,0f,0f);
			}
			if (Input.GetKey (scaleRight))
			{
				objects[currentObj].localScale += new Vector3(-0.05f,0f,0f);
			}
			if (Input.GetKey (scaleUp))
			{
				objects[currentObj].localScale += new Vector3(0f,0.05f,0f);
			}
			if (Input.GetKey (scaleDown))
			{
				objects[currentObj].localScale += new Vector3(0f,-0.05f,0f);
			}
        }
    }
}
