using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyBehaviour_Cthulu : StateMachineBehaviour
{
    private CthuluBoss _cthuluBoss;
    private Rigidbody2D _erb2D;
    [SerializeField] private float speed;
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _cthuluBoss = animator.GetComponent<CthuluBoss>();
        _erb2D = _cthuluBoss.Crb2D;
        _cthuluBoss.LookPlayer();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector2 direction = (_cthuluBoss.Player.position - animator.transform.position).normalized;
        _erb2D.velocity = new Vector2(direction.x * speed, direction.y * speed);
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
