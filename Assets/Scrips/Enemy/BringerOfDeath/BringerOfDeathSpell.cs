using UnityEngine;

public class BringerOfDeathSpell : MonoBehaviour
{
//--------------------------Environment Variables-----------------------------------------------------------------//

    [SerializeField] private int damageSpell;
    [SerializeField] private Vector2 boxDimension;
    [SerializeField] private Transform boxPosition;
    [SerializeField] private float timeOfLife;
    
    [Header("Audio")]
    [SerializeField] private AudioClip attackAudio;
    [SerializeField] private AudioClip spawnAudio;

//---------------------------Start--------------------------------------------------------------------------------//

    private void Start()
    {
        SoundsController.Instance.PlayAudio(spawnAudio);
        Destroy(gameObject, timeOfLife);
    }

//----------------------------Methods-----------------------------------------------------------------------------//

    public void hit()
    {
        Collider2D[] objects = Physics2D.OverlapBoxAll(boxPosition.position, boxDimension, 0f);

        foreach (Collider2D collider in objects)
        {
            if (collider.CompareTag("Player"))
            {
                NewPlayerController playerController = collider.GetComponent<NewPlayerController>();

                if (playerController != null && playerController.life > 0)
                {
                    SoundsController.Instance.PlayAudio(attackAudio);
                    playerController.TakeDamage(damageSpell);
                }
            }
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxPosition.position, boxDimension);
    }

}

