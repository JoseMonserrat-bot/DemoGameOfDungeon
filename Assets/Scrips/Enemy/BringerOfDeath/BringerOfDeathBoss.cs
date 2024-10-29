using UnityEngine;
using UnityEngine.Tilemaps;

public class BringerOfDeathBoss : MonoBehaviour, I_Damage
{
//--------------------------Environment Variables-----------------------------------------------------------------//
    private Animator _animator;
    public Rigidbody2D Erb2D;
    public Transform Player;
    private bool rightLooking = true;

    [Header("Life")] 
    [SerializeField] public float enemyLife;
    [SerializeField] public float maxEnemyLife;
    [SerializeField] public Bar lifeBar;
    
    [Header("Attack")] 
    [SerializeField] private Transform attackController;
    [SerializeField] private float attackRadius;
    [SerializeField] private int damageAttack;
    
    [Header("Tilemap Collider")]
    public TilemapCollider2D _tilemapCollider2D;
    
    [Header("Audio")]
    [SerializeField] private AudioClip attackAudio;
    [SerializeField] private AudioClip hitAudio;
    [SerializeField] private AudioClip deadAudio;
    [SerializeField] private AudioClip spawnAudio;
    
//--------------------------Start is called before the first frame update-----------------------------------------//
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        Erb2D = GetComponent<Rigidbody2D>();
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        enemyLife = maxEnemyLife;
        lifeBar = GameObject.FindGameObjectWithTag("BringerOfDeathBossBar").GetComponent<Bar>();
        _tilemapCollider2D = GameObject.FindGameObjectWithTag("Doors").GetComponent<TilemapCollider2D>();
        lifeBar.initializeBar(enemyLife, maxEnemyLife);
        SoundsController.Instance.PlayAudio(spawnAudio);
    }

//---------------------------Update and FixedUpdate is called once per frame--------------------------------------//
    void Update()
    {
        if (Player != null)
        {
            LookPlayer();
            float distancePlayer = Vector2.Distance(transform.position, Player.position);
            _animator.SetFloat("DistancePlayer", distancePlayer);
        }
    }
    
//----------------------------Methods-----------------------------------------------------------------------------//

    public void TakeDamage(float damage)
    {
        if (enemyLife > 0)
        {
            _animator.SetTrigger("Hurt");
            enemyLife -= damage;
            lifeBar.changeCurrent(enemyLife);
            SoundsController.Instance.PlayAudio(hitAudio);
        }
        
        if(enemyLife <= 0)
        {
            Destroy(lifeBar.gameObject);
            _tilemapCollider2D.isTrigger = true;
            _animator.SetTrigger("Dead-Bringer-Of-Death");
            SoundsController.Instance.PlayAudio(deadAudio);
        }
    }
    
    private void Dead()
    {
        Destroy(gameObject);
    }

    public void LookPlayer()
    {
        if (Player != null)
        {
            if ((Player.position.x > transform.position.x && !rightLooking) || (Player.position.x < transform.position.x && rightLooking))
            {
                rightLooking = !rightLooking;
                transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
            }
        }
    }

    public void Attack()
    {
        Collider2D[] objects = Physics2D.OverlapCircleAll(attackController.position, attackRadius);

        foreach (Collider2D collider in objects)
        {
            if (collider.CompareTag("Player"))
            {
                NewPlayerController playerController = collider.GetComponent<NewPlayerController>();

                if (playerController != null && playerController.life > 0)
                {
                    SoundsController.Instance.PlayAudio(attackAudio);
                    playerController.TakeDamage(damageAttack);
                }
            }
        }
            
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackController.position, attackRadius);
    }
}
