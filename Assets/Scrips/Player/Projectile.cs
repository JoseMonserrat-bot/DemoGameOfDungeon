using UnityEngine;

public class Projectile : MonoBehaviour
{
    //--------------------------Environment Variables-----------------------------------------------------------------//

    [SerializeField] private float speed;
    [SerializeField] private float damage;
    
    [SerializeField] private float lifeTime;
    
    [Header("Animation")] 
    private Animator _animator;
    
    //---------------------------Awake--------------------------------------------------------------------------------//

    private void Awake()
    {
        Destroy(gameObject, lifeTime);
    }
    
    //--------------------------Update and FixedUpdate is called once per frame--------------------------------------//
    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        
    }

    //----------------------------Methods-----------------------------------------------------------------------------//

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Jar"))
        {
            I_Damage d_Object = other.GetComponent<I_Damage>();
            if (d_Object != null)
            {
                d_Object.TakeDamage(damage);
            }
            
            Destroy(gameObject);
        }
    }
}
