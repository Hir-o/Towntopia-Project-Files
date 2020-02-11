using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System;
using System.Security.Cryptography;
public class AdvancedSaveSystem_SaveGO : MonoBehaviour {
    string specialPath; //Use : %SystemFolderName%
    public string folderName = "Advanced Save System";
	public string keyWord = "[Saveable]";
    public string[] gameObjectIDs;
    public GameObject[] gameObjectPrefabs;
    int tmp = 0;
    static string PasswordHash;
    static string SaltKey;
    static string VIKey;

    [SerializeField] private City city; 

    void Start()
    {
        PasswordHash = transform.GetComponent<AdvancedSaveSystem_EncryptDetails>().PasswordHash;
        SaltKey = transform.GetComponent<AdvancedSaveSystem_EncryptDetails>().SaltKey;
        VIKey = transform.GetComponent<AdvancedSaveSystem_EncryptDetails>().VIKey;

        specialPath = Application.persistentDataPath;
    }
	
    public void SaveGameObjects(int slot)
    {
        try
        {
            string path = specialPath;
            Debug.Log("SAVE: " + path);
            foreach (string x in gameObjectIDs)
            {
               if (Directory.Exists(path + x))
               {
                   Directory.Delete(path + x,true);
                   Directory.CreateDirectory(path + x);
               }
               else
               {
                   Directory.CreateDirectory(path + x);
               }
            }
            foreach (string x in gameObjectIDs)
            {
				object[] allObjects = FindObjectsOfType (typeof(GameObject));
				foreach (GameObject obj in allObjects)
				{
				if (obj.name.Contains (keyWord))
					{
						if (obj.GetComponent<GameObjectID>().ID == x)
						{
							tmp = UnityEngine.Random.Range(111111, 999999);
							string toSave = "";
							toSave = obj.transform.position.x.ToString() + Environment.NewLine + obj.transform.position.y.ToString() + Environment.NewLine + obj.transform.position.z.ToString() + Environment.NewLine + obj.transform.rotation.x.ToString() + Environment.NewLine + obj.transform.rotation.y.ToString() + Environment.NewLine + obj.transform.rotation.z.ToString() + Environment.NewLine + obj.transform.rotation.w.ToString() + Environment.NewLine + obj.transform.localScale.x.ToString() + Environment.NewLine + obj.transform.localScale.y.ToString() + Environment.NewLine + obj.transform.localScale.z.ToString();
							File.WriteAllText(path + x + "\\item" + tmp.ToString(), Encrypt(toSave));
							AdvancedSaveSystem_FileEncryptor.EncryptFile(path + x + "\\item" + tmp.ToString(), path + x + "\\item" + tmp.ToString() + ".nc");
						}
					}
				}
            }
            AdvancedSaveSystem_MessageSystem.msg = "Game Saved!";
        }
        catch { }

        PlayerPrefs.SetInt("levelSaved", 1);
        PlayerPrefs.SetFloat("cash", city.Cash);
        PlayerPrefs.SetFloat("food", city.Food);
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

    public string GetSpecialPath()
    {
        return Application.persistentDataPath;
    }
}
