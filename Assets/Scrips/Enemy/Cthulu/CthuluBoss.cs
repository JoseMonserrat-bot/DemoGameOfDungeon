using UnityEngine;
using UnityEngine.Tilemaps;

public class CthuluBoss : MonoBehaviour, I_Damage
{
//--------------------------Environment Variables-----------------------------------------------------------------//
    private Animator _animator;
    public Rigidbody2D Crb2D;
    public Transform Player;
    private bool rightLooking = true;

    [Header("Life")] 
    [SerializeField] public float enemyLife;
    [SerializeField] public float maxEnemyLife;
    [SerializeField] public Bar lifeBar;
    
    [Header("Attack")] 
    [SerializeField] private Vector2 boxDimension;
    [SerializeField] private Transform boxPosition;
    [SerializeField] private int damageAttack;
    [SerializeField] private int strongAttackMultiplier;
    
    [Header("Tilemap Collider")]
    public TilemapCollider2D _tilemapCollider2D;
    
    [Header("Audio")]
    [SerializeField] private AudioClip attack1Audio;
    [SerializeField] private AudioClip attack2Audio;
    [SerializeField] private AudioClip hitAudio;
    [SerializeField] private AudioClip deadAudio;
    
//--------------------------Start is called before the first frame update-----------------------------------------//
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        Crb2D = GetComponent<Rigidbody2D>();
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        enemyLife = maxEnemyLife;
        lifeBar = GameObject.FindGameObjectWithTag("CthuluBar").GetComponent<Bar>();
        _tilemapCollider2D = GameObject.FindGameObjectWithTag("Doors").GetComponent<TilemapCollider2D>();
        lifeBar.initializeBar(enemyLife, maxEnemyLife);
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
            _animator.SetBool("Dead-Cthulu", true);
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
    
    public void Attack()
    {
        Collider2D[] objects = Physics2D.OverlapBoxAll(boxPosition.position, boxDimension, 0f);

        float randomValue = Random.Range(0f, 1f);
        
        foreach (Collider2D collider in objects)
        {
            if (collider.CompareTag("Player"))
            {
                NewPlayerController playerController = collider.GetComponent<NewPlayerController>();

                if (playerController != null && playerController.life > 0)
                {
                    if (randomValue < 0.75f)
                    {
                        SoundsController.Instance.PlayAudio(attack1Audio);
                        _animator.SetBool("StrongAttack", false);
                        collider.GetComponent<NewPlayerController>().TakeDamage(damageAttack);
                    }
                    else
                    {
                        SoundsController.Instance.PlayAudio(attack2Audio);
                        _animator.SetBool("StrongAttack", true);
                        collider.GetComponent<NewPlayerController>().TakeDamage(damageAttack * strongAttackMultiplier);
                    }
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
