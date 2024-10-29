using UnityEngine;

public class Jar : MonoBehaviour, I_Damage
{
//--------------------------Environment Variables-----------------------------------------------------------------//
    public Animator _animator;
    public Rigidbody2D Crb2D;
    public Transform Player;
    
    [Header("Life")] 
    [SerializeField] public float enemyLife;
    [SerializeField] private AudioClip breakAudio;
    
    [Header("Potion Prefabs")] 
    public GameObject lifePotionPrefab;
    public GameObject manaPotionPrefab;
    
//--------------------------Start is called before the first frame update-----------------------------------------//
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        Crb2D = GetComponent<Rigidbody2D>();
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    
//----------------------------Methods-----------------------------------------------------------------------------//

    public void TakeDamage(float damage)
    {
        if (enemyLife > 0)
        {
            enemyLife -= damage;
        }
        
        if(enemyLife <= 0)
        {
            _animator.SetBool("Break", true);
        }
    }
    
    private void SpawnPotion()
    {
        GameObject potionPrefab = (Random.value < 0.5f) ? lifePotionPrefab : manaPotionPrefab;
        Instantiate(potionPrefab, transform.position, Quaternion.identity);
    }
    
    private void Break()
    {
        SpawnPotion();
        SoundsController.Instance.PlayAudio(breakAudio);
        Destroy(gameObject);
    }
    
}
