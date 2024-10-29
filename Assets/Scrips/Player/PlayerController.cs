using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //--------------------------Environment Variables-----------------------------------------------------------------//

    private Rigidbody2D prb2D;

    [Header("Movement")] 
    private float horizontalMovement = 0.0f;
    [SerializeField] private float speedMovement;
    [Range(0.0f, 0.3f)][SerializeField] private float motionSmoothing;
    private Vector3 speed = Vector3.zero;
    private bool rightLooking = true;

    [Header("Jump")] 
    [SerializeField] private float strengthJump;
    [SerializeField] private LayerMask whatIsFloor;
    [SerializeField] private Transform controllerFloor;
    [SerializeField] private Vector3 boxDimensions;
    [SerializeField] private bool inFloor;
    private bool jump = false;
    
    //--------------------------Start is called before the first frame update-----------------------------------------//
    
    private void Start()
    {
        prb2D = GetComponent<Rigidbody2D>();

    }

    //---------------------------Update and FixedUpdate is called once per frame--------------------------------------//
    
    void Update()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal") * speedMovement;

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }

    }

    private void FixedUpdate()
    {
        inFloor = Physics2D.OverlapBox(controllerFloor.position, boxDimensions, 0f, whatIsFloor);
        
        //PlayerMovement
        PlayerMovement(horizontalMovement * Time.fixedDeltaTime, jump);

        jump = false;
    }

    //----------------------------Methods-----------------------------------------------------------------------------//
    
    private void PlayerMovement(float move, bool jump) 
    {
        Vector3 speedObject = new Vector2(move, prb2D.velocity.y);
        prb2D.velocity = Vector3.SmoothDamp(prb2D.velocity, speedObject, ref speed, motionSmoothing);

        if (move > 0 && !rightLooking)
        {
            PlayerOrientation();
        }
        else if (move < 0 && rightLooking)
        {
            PlayerOrientation();
        }

        if (inFloor && jump)
        {
            inFloor = false;
            prb2D.AddForce(new Vector2(0f, strengthJump));
        }
        
    }

    void PlayerOrientation()
    {
        rightLooking = !rightLooking;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(controllerFloor.position, boxDimensions); 
    }
    
    //----------------------------------------------------------------------------------------------------------------//
    
    
}
