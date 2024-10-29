using UnityEngine;
using UnityEngine.Tilemaps;

public class FireWormBoss : MonoBehaviour, I_Damage
{
//--------------------------Environment Variables-----------------------------------------------------------------//
    public Animator _animator;
    public Rigidbody2D Crb2D;
    public Transform Player;
    private bool rightLooking = true;

    [Header("Life")] 
    [SerializeField] public float enemyLife;
    [SerializeField] public float maxEnemyLife;
    [SerializeField] public Bar lifeBar;
    
    [Header("FireAttack")] 
    public Transform fireAttackController;
    public float lineDistance;
    public LayerMask playerLayer;
    public bool playerInRange;
    public float timeBetweenFireAttacks;
    public float timeLastFireAttacks;
    public float nextFireAttackTime;
    public GameObject fireBall;
    
    [Header("Tilemap Collider")]
    public TilemapCollider2D _tilemapCollider2D;
    
    [Header("Audio")]
    [SerializeField] private AudioClip hitAudio;
    [SerializeField] private AudioClip fireAudio;
    [SerializeField] private AudioClip deadAudio;
    
//--------------------------Start is called before the first frame update-----------------------------------------//
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        Crb2D = GetComponent<Rigidbody2D>();
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        enemyLife = maxEnemyLife;
        lifeBar = GameObject.FindGameObjectWithTag("FireWormBar").GetComponent<Bar>();
        _tilemapCollider2D = GameObject.FindGameObjectWithTag("Doors").GetComponent<TilemapCollider2D>();
        lifeBar.initializeBar(enemyLife, maxEnemyLife);
    }

//---------------------------Update and FixedUpdate is called once per frame--------------------------------------//
    void Update()
    {
        if (Player != null)
        {
            playerInRange = Physics2D.Raycast(fireAttackController.position, transform.right, lineDistance, playerLayer);
            float distancePlayer = Vector2.Distance(transform.position, Player.position);
        
            LookPlayer();
        
            if (playerInRange)
            {
                if (Time.time > timeBetweenFireAttacks + timeLastFireAttacks)
                {
                    timeLastFireAttacks = Time.time;
                    _animator.SetTrigger("Attack");
                    Invoke(nameof(FireAttack), nextFireAttackTime);
                }
            }
        
        
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
            _animator.SetTrigger("Dead-FireWorm");
            _animator.SetBool("Dead", true);
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
            if ((Player.position.x < transform.position.x && !rightLooking) || (Player.position.x > transform.position.x && rightLooking))
            {
                rightLooking = !rightLooking;
                transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
            }
        }
    }
    
    public void FireAttack()
    {
        if (Player != null)
        {
            SoundsController.Instance.PlayAudio(fireAudio);
            Instantiate(fireBall, fireAttackController.position, fireAttackController.rotation);
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(fireAttackController.position, fireAttackController.position + transform.right * lineDistance);
    }
    
}
