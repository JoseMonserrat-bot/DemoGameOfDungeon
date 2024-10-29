using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveGame
{
    //--------------------------Environment Variables-----------------------------------------------------------------//

    [Header("PlayerData")]
    public Vector3 playerPosition;
    public int life;
    public int mana;
    public int lifePotion;
    public int manaPotion;
    
    [Header("EnemyData")]
    public bool Spawn_BringerOfDeathBoss;
    public bool Spawn_Cthulu;
    public bool Spawn_FireWorm;

    //---------------------------Awake--------------------------------------------------------------------------------//



    //----------------------------Methods-----------------------------------------------------------------------------//



}
