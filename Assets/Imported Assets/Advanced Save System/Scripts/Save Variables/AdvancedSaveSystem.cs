//Advanced Save System - nDev Studios
using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System;
using System.Security.Cryptography;
public class AdvancedSaveSystem : MonoBehaviour {

	//Public variables
    public string specialPath = "%appdata%"; //Use : %SystemFolderName%
    public string folderName = "Advanced Save System";
    public string[] variablesName;
    public string[] variablesValue;
    //
    static string PasswordHash;
    static string SaltKey;
    static string VIKey;
    public void SaveData(int slot)
    {
        try
        {

            string path = Environment.ExpandEnvironmentVariables(specialPath) + "\\" + folderName + "\\Slot" + slot.ToString();
            for (int i = 0; i < variablesName.Length; )
            {
                string unE = variablesValue[i];

                File.WriteAllText(path + "\\" + variablesName[i], Encrypt(unE));
                AdvancedSaveSystem_FileEncryptor.EncryptFile(path + "\\" + variablesName[i], path + "\\" + variablesName[i] + ".nc");
                i++;
            }
           
            AdvancedSaveSystem_MessageSystem.msg = "Game Saved!";

        }
        catch 
        { 
        }
    }

    public void LoadData(int slot)
    {
        string path = Environment.ExpandEnvironmentVariables(specialPath) + "\\" + folderName + "\\Slot" + slot.ToString();
          for (int i = 0; i < variablesName.Length; )
          {
              AdvancedSaveSystem_FileEncryptor.DecryptFile(path + "\\" + variablesName[i] + ".nc", (path + "\\" + variablesName[i]));
              string toD = File.ReadAllText(path + "\\" + variablesName[i]);
              variablesValue[i] = Decrypt(toD);
              File.Delete(path + "\\" + variablesName[i]);
              i++;
          }
          AdvancedSaveSystem_MessageSystem.msg = "Game Loaded!";
    }
    void Start()
    {
        PasswordHash = transform.GetComponent<AdvancedSaveSystem_EncryptDetails>().PasswordHash;
        SaltKey = transform.GetComponent<AdvancedSaveSystem_EncryptDetails>().SaltKey;
        VIKey = transform.GetComponent<AdvancedSaveSystem_EncryptDetails>().VIKey;
        try
        {
            Directory.CreateDirectory(Environment.ExpandEnvironmentVariables(specialPath) + "\\" + folderName);
            Directory.CreateDirectory(Environment.ExpandEnvironmentVariables(specialPath) + "\\" + folderName + "\\Slot1");
            Directory.CreateDirectory(Environment.ExpandEnvironmentVariables(specialPath) + "\\" + folderName + "\\Slot2");
            Directory.CreateDirectory(Environment.ExpandEnvironmentVariables(specialPath) + "\\" + folderName + "\\Slot3");
        }
        catch 
        { 
        }
        File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Decrypt_Me", Encrypt("If you finally decrypt this message then PM me in ArmedUnity sending me this code : 10xHgc$"));
        AdvancedSaveSystem_FileEncryptor.EncryptFile(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Decrypt_Me", Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Decrypt_Me.nc");
    }
    void Update()
    {
       
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
}
