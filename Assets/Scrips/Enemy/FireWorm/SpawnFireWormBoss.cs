using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnFireWormBoss : MonoBehaviour
{ 
//--------------------------Environment Variables-----------------------------------------------------------------//

    public static SpawnFireWormBoss instance;    

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

            Instantiate(bossPrefab, transform.position, Quaternion.identity);
            _tilemapCollider2D.isTrigger = false;
            bossSpawned = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
