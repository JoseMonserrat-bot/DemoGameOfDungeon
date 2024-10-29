using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    //--------------------------Environment Variables-----------------------------------------------------------------//

    [SerializeField] private AudioMixer audioMixer;
    public string saveGameFile = Application.dataPath + "/saveGame.json";
    private static readonly string keyFilePath = Application.dataPath + "/encryptionKey.txt";
    
    [SerializeField] private Button fullWindowButton;
    [SerializeField] private Sprite fullScreenSprite;
    [SerializeField] private Sprite windowedSprite; 
    [SerializeField] private Dropdown qualityDropdown;
    
    //----------------------------Methods-----------------------------------------------------------------------------//

    public void Delete()
    {
        string saveGameMetaFile = saveGameFile + ".meta";
        string keyMetaFile = keyFilePath + ".meta";
        
        if (File.Exists(saveGameFile))
        {
            File.Delete(saveGameFile);
            if (File.Exists(saveGameMetaFile))
            {
                File.Delete(saveGameMetaFile);
            }
        }

        if (File.Exists(keyFilePath))
        {
            File.Delete(keyFilePath);
            if (File.Exists(keyMetaFile))
            {
                File.Delete(keyMetaFile);
            }
        }
    }
    
    public void FullWindow(bool pantallaCompleta)
    {
        Screen.fullScreen = !Screen.fullScreen;
        UpdateFullWindowButtonImage();
    }
    
    private void UpdateFullWindowButtonImage()
    {
        if (Screen.fullScreen)
        {
            fullWindowButton.image.sprite = windowedSprite;
        }
        else
        {
            fullWindowButton.image.sprite = fullScreenSprite;
        }
    }

    public void Volume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
    }

    public void GraphicQuality(int index)
    {
        if (index >= 0 && index < QualitySettings.names.Length)
        {
            QualitySettings.SetQualityLevel(index);
        }
    }

}
