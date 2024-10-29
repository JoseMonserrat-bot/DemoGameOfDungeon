using System;
using UnityEngine;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class SaveGameController : MonoBehaviour
{
    //--------------------------Environment Variables-----------------------------------------------------------------//

    
    public GameObject player;
    public GameObject Spawn_BringerOfDeathBoss;
    public GameObject Spawn_Cthulu;
    public GameObject Spawn_FireWorm;
    
    public string saveGameFile;
    public SaveGame saveGame = new SaveGame();

    //---------------------------Awake--------------------------------------------------------------------------------//

    public void Awake()
    {
        saveGameFile = Application.dataPath + "/saveGame.json";

        player = GameObject.FindGameObjectWithTag("Player");
        Spawn_BringerOfDeathBoss = GameObject.FindGameObjectWithTag("Spawn_BringerOfDeathBoss");
        Spawn_Cthulu = GameObject.FindGameObjectWithTag("Spawn_Cthulu");
        Spawn_FireWorm = GameObject.FindGameObjectWithTag("Spawn_FireWorm");
        
        Load();
    }

    //----------------------------Methods-----------------------------------------------------------------------------//

    public void Load()
    {
        if (File.Exists(saveGameFile))
        {  
            //string data = File.ReadAllText(saveGameFile);
            //saveGame = JsonUtility.FromJson<SaveGame>(data);
            
            string encryptedData = File.ReadAllText(saveGameFile);
            string data = CryptoUtils.DecryptString(encryptedData);
            saveGame = JsonUtility.FromJson<SaveGame>(data);

            player.transform.position = saveGame.playerPosition;
            player.GetComponent<NewPlayerController>().life = saveGame.life;
            player.GetComponent<NewPlayerController>().mana = saveGame.mana;
            player.GetComponent<NewPlayerController>().lifePotion = saveGame.lifePotion;
            player.GetComponent<NewPlayerController>().manaPotion = saveGame.manaPotion;

            Spawn_BringerOfDeathBoss.SetActive(saveGame.Spawn_BringerOfDeathBoss);
            Spawn_Cthulu.SetActive(saveGame.Spawn_Cthulu);
            Spawn_FireWorm.SetActive(saveGame.Spawn_FireWorm);
        }
        else
        {
            saveGame = new SaveGame()
            {
                playerPosition = player.transform.position,
                life = player.GetComponent<NewPlayerController>().life,
                mana = player.GetComponent<NewPlayerController>().mana,
                lifePotion = player.GetComponent<NewPlayerController>().lifePotion,
                manaPotion = player.GetComponent<NewPlayerController>().manaPotion,
                Spawn_BringerOfDeathBoss = Spawn_BringerOfDeathBoss.activeSelf,
                Spawn_Cthulu = Spawn_Cthulu.activeSelf,
                Spawn_FireWorm = Spawn_FireWorm.activeSelf,
            };

            string json = JsonUtility.ToJson(saveGame);
            string encryptedJson = CryptoUtils.EncryptString(json);

            File.WriteAllText(saveGameFile, encryptedJson);
        }
    }
    
    public void Save()
    {
        SaveGame newData = new SaveGame()
        {
            playerPosition = player.transform.position,
            life = player.GetComponent<NewPlayerController>().life,
            mana = player.GetComponent<NewPlayerController>().mana,
            lifePotion = player.GetComponent<NewPlayerController>().lifePotion,
            manaPotion = player.GetComponent<NewPlayerController>().manaPotion,
            Spawn_BringerOfDeathBoss = GameObject.FindGameObjectWithTag("Spawn_BringerOfDeathBoss"),
            Spawn_Cthulu = GameObject.FindGameObjectWithTag("Spawn_Cthulu"),
            Spawn_FireWorm = GameObject.FindGameObjectWithTag("Spawn_FireWorm"),
        };

        //string chainJSON = JsonUtility.ToJson(newData);
        
        //File.WriteAllText(saveGameFile, chainJSON);
        
        string json = JsonUtility.ToJson(newData);
        string encryptedJson = CryptoUtils.EncryptString(json);

        File.WriteAllText(saveGameFile, encryptedJson);
    }
    
public static class CryptoUtils
{
    private static readonly string keyFilePath = Application.dataPath + "/encryptionKey.txt";

    public static void GenerateAndSaveKey()
    {
        using (Aes aesAlg = Aes.Create())
        {
            File.WriteAllText(keyFilePath, Convert.ToBase64String(aesAlg.Key));
        }
    }

    public static string GetKey()
    {
        if (File.Exists(keyFilePath))
        {
            return File.ReadAllText(keyFilePath);
        }
        else
        {
            GenerateAndSaveKey();
            return File.ReadAllText(keyFilePath);
        }
    }

    public static string EncryptString(string plainText)
    {
        string key = GetKey();
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Convert.FromBase64String(key);
            aesAlg.GenerateIV();

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msEncrypt = new MemoryStream())
            {
                msEncrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length);
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                {
                    swEncrypt.Write(plainText);
                }
                return Convert.ToBase64String(msEncrypt.ToArray());
            }
        }
    }

    public static string DecryptString(string cipherText)
    {
        string key = GetKey();
        byte[] fullCipher = Convert.FromBase64String(cipherText);
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Convert.FromBase64String(key);
            byte[] iv = new byte[aesAlg.BlockSize / 8];
            byte[] cipher = new byte[fullCipher.Length - iv.Length];

            Array.Copy(fullCipher, iv, iv.Length);
            Array.Copy(fullCipher, iv.Length, cipher, 0, cipher.Length);

            aesAlg.IV = iv;

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msDecrypt = new MemoryStream(cipher))
            using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
            {
                return srDecrypt.ReadToEnd();
            }
        }
    }
}
}
