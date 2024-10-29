using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaPotion : MonoBehaviour
{
    //--------------------------Environment Variables-----------------------------------------------------------------//

    [SerializeField] private AudioClip _audioClip;
    
    //----------------------------Methods-----------------------------------------------------------------------------//
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            NewPlayerController playerInventory = other.GetComponent<NewPlayerController>();
            if (playerInventory != null)
            {
                SoundsController.Instance.PlayAudio(_audioClip);
                playerInventory.AddPotion(2);
                Destroy(gameObject);
            }
        }
    }
}
