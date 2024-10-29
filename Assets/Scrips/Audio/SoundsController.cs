using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsController : MonoBehaviour
{
    //--------------------------Environment Variables-----------------------------------------------------------------//

    public static SoundsController Instance;
    private AudioSource _audioSource;
    
    private Coroutine repeatCoroutine;
    
    //---------------------------Awake--------------------------------------------------------------------------------//

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        _audioSource = GetComponent<AudioSource>();
    }
    
    //----------------------------Methods-----------------------------------------------------------------------------//

    public void PlayAudio(AudioClip audio)
    {
        _audioSource.PlayOneShot(audio);
    }

    public void PlayRepeatAudio(AudioClip audio, bool playAudio)
    {
        if (repeatCoroutine!= null)
        {
            StopCoroutine(repeatCoroutine);
        }
        repeatCoroutine = StartCoroutine(RepeatAudioCoroutine(audio, playAudio));
    }

    private IEnumerator RepeatAudioCoroutine(AudioClip audio, bool playAudio)
    {
        while (playAudio)
        {
            _audioSource.PlayOneShot(audio);
            yield return new WaitForSeconds(audio.length);
        }
    }

    public void StopRepeatAudio()
    {
        if (repeatCoroutine!= null)
        {
            StopCoroutine(repeatCoroutine);
            repeatCoroutine = null;
        }
    }
    
}
