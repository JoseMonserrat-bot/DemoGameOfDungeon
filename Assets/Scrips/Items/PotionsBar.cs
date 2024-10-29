using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionsBar : MonoBehaviour
{
//--------------------------Environment Variables-----------------------------------------------------------------//

    public SpriteRenderer spriteRenderer;
    public List<Sprite> potionSprites;

//---------------------------Awake--------------------------------------------------------------------------------//

    private void Awake()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
    } 
    
//----------------------------Methods-----------------------------------------------------------------------------//

    public void UpdatePotionSprite(int amount)
    {
        if (amount >= 0 && amount < potionSprites.Count)
        {
            spriteRenderer.sprite = potionSprites[amount];
        }
    }

}
