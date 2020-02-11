using UnityEngine;
using System.Collections;
using System.Security.Cryptography;
using System.IO;
using System.Text;
public class AdvancedSaveSystem_FileEncryptor : MonoBehaviour {
    public static string skey;
    public static string vkey;

    void Start()
    {
        skey = "743$^%gF5%9@$#@H8bG5#@%^";
        vkey = "7%^Vc2835055HFy@";
    }
    public static void EncryptFile(string inputFile, string outputFile)
    {
       try
        {
            using (RijndaelManaged aes = new RijndaelManaged())
            {
                byte[] key = ASCIIEncoding.UTF8.GetBytes(skey);

                byte[] IV = ASCIIEncoding.UTF8.GetBytes(vkey);

                using (FileStream fsCrypt = new FileStream(outputFile, FileMode.Create))
                {
                    using (ICryptoTransform encryptor = aes.CreateEncryptor(key, IV))
                    {
                        using (CryptoStream cs = new CryptoStream(fsCrypt, encryptor, CryptoStreamMode.Write))
                        {
                            using (FileStream fsIn = new FileStream(inputFile, FileMode.Open))
                            {
                                int data;
                                while ((data = fsIn.ReadByte()) != -1)
                                {
                                    cs.WriteByte((byte)data);
                                }
                            }
                        }
                    }
                }
            }
            File.Delete(inputFile);
        }
        catch
        {
            // failed to encrypt file
        }
    }
   public static void DecryptFile(string inputFile, string outputFile)
    {
        try
        {
            using (RijndaelManaged aes = new RijndaelManaged())
            {
                byte[] key = ASCIIEncoding.UTF8.GetBytes(skey);

                byte[] IV = ASCIIEncoding.UTF8.GetBytes(vkey);

                using (FileStream fsCrypt = new FileStream(inputFile, FileMode.Open))
                {
                    using (FileStream fsOut = new FileStream(outputFile, FileMode.Create))
                    {
                        using (ICryptoTransform decryptor = aes.CreateDecryptor(key, IV))
                        {
                            using (CryptoStream cs = new CryptoStream(fsCrypt, decryptor, CryptoStreamMode.Read))
                            {
                                int data;
                                while ((data = cs.ReadByte()) != -1)
                                {
                                    fsOut.WriteByte((byte)data);
                                }
                            }
                        }
                    }
                }
            }
        }
        catch
        {
            // failed to decrypt file
        }
    }
}
