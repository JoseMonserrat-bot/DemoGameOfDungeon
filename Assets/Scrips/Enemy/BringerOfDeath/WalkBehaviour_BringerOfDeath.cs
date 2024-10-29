using UnityEngine;

public class WalkBehaviour_BringerOfDeath : StateMachineBehaviour
{

    private BringerOfDeathBoss _bringerOfDeathBoss;
    private Rigidbody2D Erb2D;
    [SerializeField] private float speed;
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _bringerOfDeathBoss = animator.GetComponent<BringerOfDeathBoss>();
        Erb2D = _bringerOfDeathBoss.Erb2D;

        _bringerOfDeathBoss.LookPlayer();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Erb2D.velocity = new Vector2(speed, Erb2D.velocity.y) * animator.transform.right;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Erb2D.velocity = new Vector2(0, Erb2D.velocity.y);
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
