using UnityEngine;

public class FireBall : MonoBehaviour
{
    //--------------------------Environment Variables-----------------------------------------------------------------//

    private Animator _animator;
    public float speed;
    public int damage;
    public float lifeTime;
    
    //---------------------------Awake--------------------------------------------------------------------------------//

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    
    //--------------------------Update and FixedUpdate is called once per frame--------------------------------------//
    void Update()
    {
        transform.Translate(Time.deltaTime * speed * Vector2.left);
        Destroy(gameObject, lifeTime);
    }

    private void FixedUpdate()
    {
        
    }

    //----------------------------Methods-----------------------------------------------------------------------------//

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out NewPlayerController _newPlayerController))
        {
            if (_newPlayerController != null)
            {
                _newPlayerController.TakeDamage(damage);
                other.gameObject.GetComponent<NewPlayerController>().TakeDamage(damage);
                _animator.SetTrigger("Explosion");
            }
        }
    }
    
    private void Explosion()
    {
        Destroy(gameObject);
    }
}
