using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using UnityEngine.Video;

public class NewPlayerController : MonoBehaviour
{
    //--------------------------Environment Variables-----------------------------------------------------------------//
    
    [Header("Movements")]
    private PlayerImput _input;
    public Vector2 direction;
    public Rigidbody2D prb2D;
    public float movementSpeed;
    public float baseMovementSpeed;

    private Rigidbody2D _rb;

    public bool canMove;
    [SerializeField] private Vector2 reboundSpeed;
    
    
    [Header("Look")]
    public bool rightLooking = true;
    
    [Header("Jump")]
    public float strengthJump;
    public LayerMask whatIsFloor;
    public Transform groundController;
    
    public bool inFloor;
    public Vector3 boxDimensionsJump;
    
    [Header("Crouch")] 
    public Transform ceilingController;
    public float crouchSpeed;
    private bool wasCrouched = false;
    private bool inCeiling;
    
    public Vector3 boxDimensionsCrouch;
    
    [Header("Animation")] 
    private Animator _animator;

    [Header("Attack")] 
    [SerializeField] private Transform attackController;
    [SerializeField] private float attackRadius;
    [SerializeField] private float damageAttack;
    [SerializeField] private float timeBetweenAttacks;
    [SerializeField] private float nextAttackTime;

    [Header("ClimbingLadder")] 
    public Transform ladderController;
    private Vector2 climbing;
    private bool inLadder;
    private bool wasClimbing = false;
    public float climbingSpeed;
    public Vector3 boxDimensionsClimbing;
    public LayerMask whatIsLadder;
    private float initialGravity;
    
    [Header("Block")] 
    private bool wasBlocking = false;

    [Header("Fire")] 
    [SerializeField] private Transform FireController;
    [SerializeField] private GameObject projectile;
    
    [Header("Mana")] 
    [SerializeField] public int mana;
    [SerializeField] private int maximumMana;
    [SerializeField] private Bar manaBar;
    
    [Header("Life")] 
    [SerializeField] public int life;
    [SerializeField] private int maximumLife;
    [SerializeField] private Bar lifeBar;
    
    [Header("ManaPotion")] 
    [SerializeField] public int manaPotion;
    [SerializeField] private int maximumManaPotion;
    [SerializeField] private GameObject manaBarPotion;
    
    [Header("LifePotion")] 
    [SerializeField] public int lifePotion;
    [SerializeField] private int maximumLifePotion;
    [SerializeField] private GameObject lifeBarPotion;
    
    //[Header("Interacting")] 
    //private bool isInteracting = false;
    //private bool wasInteracting = false;
    
    [Header("Tilemap Collider")]
    public TilemapCollider2D _tilemapCollider2D;

    public event EventHandler PlayerDead;
    
    [Header("Audio")]
    [SerializeField] private AudioClip jumpAudio;
    [SerializeField] private AudioClip attackAudio;
    [SerializeField] private AudioClip fireAudio;
    [SerializeField] private AudioClip blockAudio;
    [SerializeField] private AudioClip hitAudio;
    [SerializeField] private AudioClip climbingAudio;
    [SerializeField] private AudioClip recoverManaAudio;
    [SerializeField] private AudioClip recoverLifeAudio;
    
    //---------------------------Awake--------------------------------------------------------------------------------//

    private void Awake()
    {
        _input = new PlayerImput();
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        initialGravity = prb2D.gravityScale;
        lifeBar.initializeBar(life, maximumLife);
        manaBar.initializeBar(mana, maximumMana);

        lifeBarPotion.GetComponent<PotionsBar>().UpdatePotionSprite(lifePotion);
        manaBarPotion.GetComponent<PotionsBar>().UpdatePotionSprite(manaPotion);
    }

    public void OnEnable()
    {
        _input.Enable();
        _input.Movement.Jump.started += OnJump;
        _input.Movement.Crouch.performed += OnCrouch;
        _input.Movement.Crouch.canceled += OnCrouchCancel;
        _input.Movement.MeleeAttack.started += OnAttack;
        _input.Movement.Fire.started += OnFire;
        _input.Movement.Interaction.performed += _ => OnClimbing();
        //_input.Movement.Interaction.canceled += OnClimbing;
        //_input.Movement.Interaction.started += OnInteracting;
        _input.Movement.Block.performed += OnBlock;
        _input.Movement.Block.canceled += OnBlockCancel;
        _input.Movement.LifePotion.started += _ => Heal();
        _input.Movement.ManaPotion.started += _ => RecoverMana();
    }
    
    public void OnDisable()
    {
        _input.Disable();
        _input.Movement.Jump.started -= OnJump;
        _input.Movement.Crouch.performed -= OnCrouch;
        _input.Movement.Crouch.canceled -= OnCrouchCancel;
        _input.Movement.MeleeAttack.started -= OnAttack;
        _input.Movement.Fire.started -= OnFire;
        _input.Movement.Interaction.performed -= _ => OnClimbing();
        //_input.Movement.Interaction.canceled -= OnClimbing;
        //_input.Movement.Interaction.started -= OnInteracting;
        _input.Movement.Block.performed += OnBlock;
        _input.Movement.Block.canceled -= OnBlockCancel;
        _input.Movement.LifePotion.started -= _ => Heal();
        _input.Movement.ManaPotion.started -= _ => RecoverMana();
    }

    //--------------------------Update and FixedUpdate is called once per frame--------------------------------------//
    void Update()
    {
        if (gameObject != null)
        {
            direction = _input.Movement.Move.ReadValue<Vector2>();
        
            if (nextAttackTime > 0)
            {
                nextAttackTime -= Time.deltaTime;
            }
        
            AdjustRotation(direction.x);

        }
    }

    private void FixedUpdate()
    {
        if (wasClimbing)
        {
            HandleClimbing();
        }
        else
        {
            HandleMovement();
        }
        
        
        
        CheckEnvironment();
        UpdateAnimatorParameters();
        
    }

    //----------------------------Methods-----------------------------------------------------------------------------//

    //----------------------------HandleMovement-----------------------//
    private void HandleMovement()
    {
        if (!wasBlocking)
        {
            prb2D.velocity = new Vector2(direction.x * movementSpeed, prb2D.velocity.y);
        }
        else if(inFloor)
        {
            prb2D.velocity = new Vector2(0, prb2D.velocity.y);
        }
    }

    //----------------------------HandleClimbing-----------------------//
    private void HandleClimbing()
    {
        prb2D.velocity = new Vector2(direction.x * climbingSpeed, direction.y * climbingSpeed);

        if (!inLadder)
        {
            OnClimbing();
        }
    }
    
    //----------------------------CheckEnvironment-----------------------//
    private void CheckEnvironment()
    {
        inFloor = Physics2D.OverlapBox(groundController.position, boxDimensionsJump, 0.0f, whatIsFloor);
        inCeiling = Physics2D.OverlapBox(ceilingController.position, boxDimensionsCrouch, 0.0f, whatIsFloor);
        inLadder = Physics2D.OverlapBox(ladderController.position, boxDimensionsClimbing, 0.0f, whatIsLadder);

        //if (!inCeiling || !wasCrouched)
        //{
        //    movementSpeed = baseMovementSpeed;
        //}
    }
    
    //----------------------------UpdateAnimatorParameters-----------------------//
    private void UpdateAnimatorParameters()
    {
        _animator.SetBool("InFloor", inFloor);
        _animator.SetBool("InCeiling", inCeiling);
        _animator.SetBool("WasCrouched", wasCrouched);
        _animator.SetFloat("Horizontal", Mathf.Abs(direction.x));
        _animator.SetFloat("SpeedY", prb2D.velocity.y);
        _animator.SetBool("InLadder", inLadder);
        _animator.SetBool("WasClimbing", wasClimbing);
        _animator.SetBool("WasBlocking", wasBlocking);
    }
    
    //----------------------------Rotate-----------------------//
    private void AdjustRotation(float directionX)
    {
        if (directionX > 0 && !rightLooking || directionX < 0 && rightLooking)
        {
            rightLooking = !rightLooking;
            //Vector3 scale = transform.localScale;
            //scale.x *= -1;
            //transform.localScale = scale;
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
        }
    }
    
    //----------------------------Jump-----------------------//
    private void OnJump(InputAction.CallbackContext context)
    {
        if (inFloor && !wasCrouched && !wasBlocking)
        {
            prb2D.AddForce(new Vector2(0.0f, strengthJump), ForceMode2D.Impulse);
            SoundsController.Instance.PlayAudio(jumpAudio);
        }
    }
    
    //----------------------------Crouch-----------------------//
    private void OnCrouch(InputAction.CallbackContext context)
    {
        if (!inCeiling && !wasBlocking)
        {
            wasCrouched = !wasCrouched;

            if (movementSpeed == baseMovementSpeed)
            {
                movementSpeed /= crouchSpeed;
            }
        }
    }

    private void OnCrouchCancel(InputAction.CallbackContext context)
    {
        if (!inCeiling)
        {
            wasCrouched = false;
            movementSpeed = baseMovementSpeed;
        }
    }

    //----------------------------Rebound-----------------------//
    private void Rebound()
    {
        
    }
    
    //----------------------------Interacting-----------------------//
    //private void OnInteracting(InputAction.CallbackContext context)
    //{
    //    if (!isInteracting)
    //    {
    //        wasInteracting = !wasInteracting;
    //    }
    //    else
    //    {
    //        wasInteracting = false;
    //    }
    //}
    
    //----------------------------Attack-----------------------//
    private void OnAttack(InputAction.CallbackContext context)
    {
        if (nextAttackTime <= 0 && !wasCrouched && !wasBlocking)
        {
            if (attackController!= null)
            {
                _animator.SetTrigger("MeleeAttack");
            
        
            Collider2D[] objects = Physics2D.OverlapCircleAll(attackController.position, attackRadius);

            SoundsController.Instance.PlayAudio(attackAudio);
            
            foreach (Collider2D collider in objects)
            {
                I_Damage d_Object = collider.GetComponent<I_Damage>();
                if (d_Object != null)
                {
                    d_Object.TakeDamage(damageAttack);
                }
            }

            nextAttackTime = timeBetweenAttacks;
            
            }
            else
            {
                Debug.LogError("attackController is null!");
            }
        }
    }
    
    //----------------------------Fire-----------------------//
    private void OnFire(InputAction.CallbackContext context)
    {
        if (mana != 0)
        {
            if (nextAttackTime <= 0 && !wasCrouched && !wasBlocking && _animator.GetComponent("MeleeAttack") == false && !wasClimbing)
            {
                UseMana(1);
                Instantiate(projectile, FireController.position, FireController.rotation);
                _animator.SetTrigger("Fire");
                
                SoundsController.Instance.PlayAudio(fireAudio);
                nextAttackTime = timeBetweenAttacks;
            }
        }
        
        
    }

    //----------------------------Block-----------------------//
    private void OnBlock(InputAction.CallbackContext context)
    {
        wasBlocking = true;
    }
    
    private void OnBlockCancel(InputAction.CallbackContext context)
    {
        wasBlocking = false;
    }
    

    //----------------------------Climbingladders-----------------------//

    private void OnClimbing()
    {
        if (inLadder && !wasClimbing)
        {
            wasClimbing = true;
            prb2D.gravityScale = 0;
            _tilemapCollider2D.isTrigger = true;
            SoundsController.Instance.PlayRepeatAudio(climbingAudio, wasClimbing);
        }
        else
        {
            wasClimbing = false;
            prb2D.gravityScale = initialGravity;
            _tilemapCollider2D.isTrigger = false;
            SoundsController.Instance.PlayRepeatAudio(climbingAudio, wasClimbing);
        }
    }
    
    //----------------------------TakeDamage-----------------------//
    public void TakeDamage(int damage)
    {
        if (!wasBlocking)
        {
            life -= damage;
            _animator.SetTrigger("IsKicked");
            lifeBar.changeCurrent(life);
            SoundsController.Instance.PlayAudio(hitAudio);
        }
        else
        {
            SoundsController.Instance.PlayAudio(blockAudio);
        }
        
        if (life <= 0)
        {
            //_rb.constraints = RigidbodyConstraints2D.FreezeAll;
            _animator.SetTrigger("IsDead");
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), true);
            //Destroy(gameObject);
        }
    }

    //----------------------------PlayerDead-----------------------//    
    
    public void PlayerDeadEvent()
    {
        PlayerDead?.Invoke(this, EventArgs.Empty);
    }

    public void Destroy()
    {
        //Destroy(gameObject);
    }

    //----------------------------Heal-----------------------//
    public void Heal()
    {
        if (lifePotion <= 0) return;

        life = maximumLife;

        lifeBar.changeCurrent(life);
        lifePotion--;
        lifeBarPotion.GetComponent<PotionsBar>().UpdatePotionSprite(lifePotion);
        SoundsController.Instance.PlayAudio(recoverLifeAudio);
    }
    
//----------------------------UseMana-----------------------//
    public void UseMana(int useMana)
    {
        if (mana > 0)
        {
            mana -= useMana;
            manaBar.changeCurrent(mana);
        }
    }

    //----------------------------RecoverMana-----------------------//
    public void RecoverMana()
    {
        if (manaPotion <= 0) return;

        mana = maximumMana;

        manaBar.changeCurrent(mana);
        manaPotion--;
        manaBarPotion.GetComponent<PotionsBar>().UpdatePotionSprite(manaPotion);
        SoundsController.Instance.PlayAudio(recoverManaAudio);
    }

    //----------------------------AddPotion-----------------------//
    public void AddPotion(float tipeOfPotion)
    {
        switch (tipeOfPotion)
        {
            case 1:
                if ((lifePotion + 1) >= maximumLifePotion)
                {
                    lifePotion = maximumLifePotion;
                    lifeBarPotion.GetComponent<PotionsBar>().UpdatePotionSprite(lifePotion);
                }
                else
                {
                    lifePotion++;
                    lifeBarPotion.GetComponent<PotionsBar>().UpdatePotionSprite(lifePotion);
                }
                break;
            case 2:
                if ((manaPotion + 1) >= maximumManaPotion)
                {
                    manaPotion = maximumManaPotion;
                    manaBarPotion.GetComponent<PotionsBar>().UpdatePotionSprite(manaPotion);
                }
                else
                {
                    manaPotion++;
                    manaBarPotion.GetComponent<PotionsBar>().UpdatePotionSprite(manaPotion);
                }
                break;
        }
    }
    
    //----------------------------DrawGizmos-----------------------//
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(groundController.position, boxDimensionsJump);
        
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(ceilingController.position, boxDimensionsCrouch);
        
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(ladderController.position, boxDimensionsClimbing);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackController.position, attackRadius);
    }
}