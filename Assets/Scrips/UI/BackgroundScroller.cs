using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
//--------------------------Environment Variables-----------------------------------------------------------------//

    [SerializeField] private Vector2 movementSpeed;
    private Vector2 offset;
    private Material material;
    
    //---------------------------Awake--------------------------------------------------------------------------------//

    private void Awake()
    {
        material = GetComponent<SpriteRenderer>().material;
    }
    
    //--------------------------Update and FixedUpdate is called once per frame--------------------------------------//
    void Update()
    {
        offset = movementSpeed * Time.deltaTime;
        material.mainTextureOffset += offset;
    }

    private void FixedUpdate()
    {
        
    }

    //----------------------------Methods-----------------------------------------------------------------------------//
}
