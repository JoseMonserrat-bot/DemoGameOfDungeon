using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnCthuluBoss : MonoBehaviour
{ 
//--------------------------Environment Variables-----------------------------------------------------------------//

    public static SpawnCthuluBoss instance;    

    public GameObject bossPrefab;
    public Transform canvasTransform;
    public GameObject bossBarPrefab;
    private bool bossSpawned = false;
    
    [Header("Tilemap Collider")]
    public TilemapCollider2D _tilemapCollider2D;
    
//----------------------------Methods-----------------------------------------------------------------------------//
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !bossSpawned)
        {
            GameObject bossBarInstance = Instantiate(bossBarPrefab, canvasTransform);
            bossBarInstance.transform.SetSiblingIndex(0);
            
            float offset = 2f;
            Instantiate(bossPrefab, new Vector3(transform.position.x, transform.position.y + offset, transform.position.z), Quaternion.identity);
            _tilemapCollider2D.isTrigger = false;
            bossSpawned = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
