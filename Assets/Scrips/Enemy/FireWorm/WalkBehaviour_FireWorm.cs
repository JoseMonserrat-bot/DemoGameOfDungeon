using UnityEngine;

public class WalkBehaviour_FireWorm : StateMachineBehaviour
{
    private FireWormBoss _fireWormBoss;
    private Rigidbody2D _erb2D;
    [SerializeField] private float speed;
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _fireWormBoss = animator.GetComponent<FireWormBoss>();
        _erb2D = _fireWormBoss.Crb2D;

        _fireWormBoss.LookPlayer();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector2 direction = (animator.transform.position - _fireWormBoss.Player.position).normalized;
        //Vector2 direction = (_fireWormBoss.Player.position - animator.transform.position).normalized;
        
        _erb2D.velocity = new Vector2(direction.x * speed, _erb2D.velocity.y);
        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _erb2D.velocity = new Vector2(0, _erb2D.velocity.y);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
