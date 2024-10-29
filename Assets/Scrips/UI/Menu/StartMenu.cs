using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine;
using System.IO;

public class StartMenu : MonoBehaviour
{
    //--------------------------Environment Variables-----------------------------------------------------------------//
    
    public GameObject startMenu;
    public GameObject optionsMenu;
    public GameObject infoMenu;
    
    public GameObject returnOptionsButton;
    public GameObject optionsButton;
    public GameObject returnInfoButton;
    
    public GameObject transitionManager;
    
    public string saveGameFile;
    
    //---------------------------Awake--------------------------------------------------------------------------------//

    public void Awake()
    {
        saveGameFile = Application.dataPath + "/saveGame.json";
    }

    
    //----------------------------Methods-----------------------------------------------------------------------------//

    public void OpenStartsMenu()
    {
        startMenu.SetActive(true);
        optionsMenu.SetActive(false);
        infoMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(optionsButton);
    }
    
    public void OpenOptionsMenu()
    {
        startMenu.SetActive(false);
        optionsMenu.SetActive(true);
        infoMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(returnOptionsButton);
    }
    
    public void OpenInfoMenu()
    {
        startMenu.SetActive(false);
        optionsMenu.SetActive(false);
        infoMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(returnInfoButton);
    }
    
    public void Play()
    {
        transitionManager.gameObject.SetActive(true);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
    public void Quit()
    {
        Application.Quit();
    }
    
}
