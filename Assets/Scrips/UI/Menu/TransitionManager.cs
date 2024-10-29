using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    //--------------------------Environment Variables-----------------------------------------------------------------//

    [SerializeField] private TextMeshProUGUI progressText;
    [SerializeField] private TextMeshProUGUI pressToContinue;
    [SerializeField] private TextMeshProUGUI GameInformation;
    [SerializeField] private Slider progressSlider;
    
    //---------------------------Awake--------------------------------------------------------------------------------//

    public void Awake()
    {
        StartCoroutine(Load());
    }
    
    //----------------------------Methods-----------------------------------------------------------------------------//

    private IEnumerator Load()
    {
        progressSlider.gameObject.SetActive(true);
        pressToContinue.gameObject.SetActive(false);
        
        float simulatedLoadTime = 5f;
        float progress = 0f;
        
        //AsyncOperation loadOperation = SceneManager.LoadSceneAsync(1);
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        
        loadOperation.allowSceneActivation = false;

        while (!loadOperation.isDone || progress < 1)
        {
            progress = Mathf.Clamp01(progress + Time.deltaTime / simulatedLoadTime);
            
            progressSlider.value = progress;
            progressText.text = $"{progress * 100:F0}%";
            
            if (progress >= 1)
            {
                pressToContinue.gameObject.SetActive(true);
                yield return new WaitUntil(() => Input.anyKeyDown);
                loadOperation.allowSceneActivation = true;
            }
            
            yield return null;
        }
        
    }
    
}
