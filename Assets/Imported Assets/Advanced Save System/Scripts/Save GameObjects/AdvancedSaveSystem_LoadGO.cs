using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System;
using System.Security.Cryptography;

public class AdvancedSaveSystem_LoadGO : MonoBehaviour {

    [SerializeField] private Board board;
    [SerializeField] private City city;
    [SerializeField] private UIController uIController;
    string specialPath = "";
    string folderName = "";
    GameObject[] ObjectsToLoad;
    String[] gameObjectTags;
	string keyWord = "";
    private Building tempBuilding;

    void Start()
    {
        PasswordHash = transform.GetComponent<AdvancedSaveSystem_EncryptDetails>().PasswordHash;
        SaltKey = transform.GetComponent<AdvancedSaveSystem_EncryptDetails>().SaltKey;
        VIKey = transform.GetComponent<AdvancedSaveSystem_EncryptDetails>().VIKey;
        specialPath = transform.GetComponent<AdvancedSaveSystem_SaveGO>().GetSpecialPath();
        folderName = transform.GetComponent<AdvancedSaveSystem_SaveGO>().folderName;
        ObjectsToLoad = transform.GetComponent<AdvancedSaveSystem_SaveGO>().gameObjectPrefabs;
        gameObjectTags = transform.GetComponent<AdvancedSaveSystem_SaveGO>().gameObjectIDs;
		keyWord = transform.GetComponent<AdvancedSaveSystem_SaveGO> ().keyWord;
    }

    static string PasswordHash;
    static string SaltKey;
    static string VIKey;

	public void RefreshGameObjects(int slot)
	{
	//Delete All Current Loaded Objects
		object[] allObjects = FindObjectsOfType (typeof(GameObject));
		foreach (GameObject obj in allObjects)
		{
			if (obj.name.Contains (keyWord))
			{
				GameObject.Destroy(obj);
			}
		}

		LoadGameObjects (slot);
	}

    public void LoadGameObjects(int slot)
    {
          //string path = Environment.ExpandEnvironmentVariables(specialPath) + "\\" + folderName + "\\Slot" + slot.ToString();
          string path = specialPath;
          Debug.Log("LOAD: " + path);
          for (int i = 0; i < gameObjectTags.Length; )
          {
            string cleanName = "";
            string[] fileEntries = Directory.GetFiles(path + gameObjectTags[i]);
            foreach (string fileName in fileEntries)
            {
                cleanName = fileName.Substring(0, fileName.Length - 3);
                AdvancedSaveSystem_FileEncryptor.DecryptFile(fileName, cleanName);
                string lines = File.ReadAllText(cleanName);
                lines = Decrypt(lines);
                string posx = ReadLine(lines, 1);
                string posy = ReadLine(lines, 2);
                string posz = ReadLine(lines, 3);
                string rotx = ReadLine(lines, 4);
                string roty = ReadLine(lines, 5);
                string rotz = ReadLine(lines, 6);
                string rotw = ReadLine(lines, 7);
				string scalex = ReadLine (lines,8);
				string scaley = ReadLine (lines,9);
				string scalez = ReadLine (lines,10);

                tempBuilding = ObjectsToLoad[i].GetComponent<Building>();

                Vector3 tempPos = new Vector3(float.Parse(posx), float.Parse(posy), float.Parse(posz));
                Quaternion tempRot = new Quaternion(float.Parse(rotx), float.Parse(roty), float.Parse(rotz), float.Parse(rotw));
				//ObjectsToLoad[i].transform.localScale = new Vector3(float.Parse (scalex),float.Parse(scaley),float.Parse (scalez));

                if (tempBuilding.buildingType == "Road")
                {
                    ObjectsToLoad[i].transform.localScale = new Vector3(.1f, 1f, .1f);
                }
                else
                {
                    ObjectsToLoad[i].transform.localScale = new Vector3(1f, 1f, 1f);
                }

				if (!ObjectsToLoad[i].name.Contains (keyWord))
				{
					ObjectsToLoad[i].name += " " + keyWord + " ";
				}
                //Instantiate(ObjectsToLoad[i], tempPos, tempRot);
//                board.LoadBuilding(tempBuilding, tempPos, tempRot);
                File.Delete(cleanName);
            }
              i++;
          }
          AdvancedSaveSystem_MessageSystem.msg = "Game Loaded!";

        city.Cash = PlayerPrefs.GetFloat("cash");
        city.Food = PlayerPrefs.GetFloat("food");
        uIController.UpdateCityDataGOAP();

    }


    public static string Encrypt(string plainText)
    {
        byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

        byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
        var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros };
        var encryptor = symmetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));

        byte[] cipherTextBytes;

        using (var memoryStream = new MemoryStream())
        {
            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
            {
                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                cryptoStream.FlushFinalBlock();
                cipherTextBytes = memoryStream.ToArray();
                cryptoStream.Close();
            }
            memoryStream.Close();
        }
        return Convert.ToBase64String(cipherTextBytes);
    }
    public static string Decrypt(string encryptedText)
    {
        byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);
        byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
        var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.None };

        var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));
        var memoryStream = new MemoryStream(cipherTextBytes);
        var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
        byte[] plainTextBytes = new byte[cipherTextBytes.Length];

        int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
        memoryStream.Close();
        cryptoStream.Close();
        return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
    }
    private static string ReadLine(string text, int lineNumber)
    {
        var reader = new StringReader(text);

        string line;
        int currentLineNumber = 0;

        do
        {
            currentLineNumber += 1;
            line = reader.ReadLine();
        }
        while (line != null && currentLineNumber < lineNumber);

        return (currentLineNumber == lineNumber) ? line :
            string.Empty;
    }
}
