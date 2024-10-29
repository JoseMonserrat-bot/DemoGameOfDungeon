using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
//--------------------------Environment Variables-----------------------------------------------------------------//

    [SerializeField] private List<GameObject> backgrounds;
    [SerializeField] private Transform player;
    
    //---------------------------Awake--------------------------------------------------------------------------------//

    private void Awake()
    {
        ChangeBackgroundBasedOnPosition(player.position);
    }
    
    //--------------------------Update and FixedUpdate is called once per frame--------------------------------------//
    void Update()
    {
        if (player != null)
        {
            ChangeBackgroundBasedOnPosition(player.position);
        }
    }

    private void FixedUpdate()
    {
        
    }

    //----------------------------Methods-----------------------------------------------------------------------------//

    private void ChangeBackgroundBasedOnPosition(Vector2 position)
    {
        if (position.x <= -40)
        {
            ActivateBackground(1); // Activa BackGroundCemetery
        }
        else if (position.x >= 33)
        {
            ActivateBackground(2); // Activa BackGroundForest
        }
        else if (position.y <= -22)
        {
            ActivateBackground(1); // Activa BackGroundCrypt
        }
        else
        {
            ActivateBackground(0); // Activa BackGroundVillage
        }
    }

    private void ActivateBackground(int index)
    {
        for (int i = 0; i < backgrounds.Count; i++)
        {
            backgrounds[i].SetActive(i == index);
        }
    }
}
