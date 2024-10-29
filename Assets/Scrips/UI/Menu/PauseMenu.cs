using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    //--------------------------Environment Variables-----------------------------------------------------------------//

    private MenuImput _input;
    private AudioSource _audioSource;
    
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private GameObject menuPause;

    private bool gameInPause = false;
    
    public GameObject pauseMenu;
    public GameObject infoMenu;
    public GameObject deadMenu;
    
    public GameObject deadReturnButton;
    public GameObject resumeButton;
    public GameObject returnInfoButton;
    
    [SerializeField] private NewPlayerController newPlayerController;
    
    [SerializeField] private SaveGameController saveGameController;
    
    //---------------------------Awake--------------------------------------------------------------------------------//

    private void Awake()
    {
        _input = new MenuImput();
        _audioSource = GetComponent<AudioSource>();
        newPlayerController = GameObject.FindGameObjectWithTag("Player").GetComponent<NewPlayerController>();
        newPlayerController.PlayerDead += DeadMenu;
        saveGameController = GameObject.FindGameObjectWithTag("SaveGameController").GetComponent<SaveGameController>();
    }

    private void OnEnable()
    {
        _input.Enable();
        _input.Menu.Pause.started += _ => TogglePause(gameInPause);
    }

    //----------------------------Methods-----------------------------------------------------------------------------//

    public void DeadMenu(object sender, EventArgs e)
    {
        newPlayerController.OnDisable();
        newPlayerController.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        pauseButton.SetActive(false);
        deadMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(deadReturnButton);
    }
    
    
    public void OpenPauseMenu()
    {
        newPlayerController.OnDisable();
        newPlayerController.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        pauseMenu.SetActive(true);
        infoMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(resumeButton);
    }
    
    public void OpenInfoMenu()
    {
        pauseMenu.SetActive(false);
        infoMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(returnInfoButton);
    }
    
    private void TogglePause(bool inPause)
    {
        if (!inPause)
        {
            Pause();
        }
        else
        {
            Resume();
        }
    }
    
    public void Pause()
    {
        _audioSource.Play();
        if (newPlayerController.GetComponent<Rigidbody2D>() != null)
        {
            newPlayerController.OnDisable();
        }
        Time.timeScale = 0f;
        gameInPause = true;
        pauseButton.SetActive(false);
        OpenPauseMenu();
    }

    public void Resume()
    {
        if (newPlayerController!= null)
        {
            Time.timeScale = 1f;
            gameInPause = false;
            newPlayerController.OnEnable();
            newPlayerController.GetComponent<Rigidbody2D>().constraints &= ~RigidbodyConstraints2D.FreezePosition;
        }
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), false);
        pauseButton.SetActive(true);
        menuPause.SetActive(false);
    }
    
    public void RestartDead()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        if (newPlayerController.GetComponent<Rigidbody2D>() != null)
        {
            newPlayerController.OnEnable();
            newPlayerController.GetComponent<Rigidbody2D>().constraints &= ~RigidbodyConstraints2D.FreezePosition;
        }
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), false);
        pauseButton.SetActive(true);
        deadMenu.SetActive(false);
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void SaveData()
    {
        saveGameController.Save();
    }

    public void Quit()
    {
        Time.timeScale = 1f;
        gameInPause = false;
        newPlayerController.OnEnable();
        newPlayerController.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

}
