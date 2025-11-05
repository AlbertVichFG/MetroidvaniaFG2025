using System;
using UnityEngine;

[Serializable]
public class GameData 
{
    [SerializeField]
    private float playerLife;
    [SerializeField]
    private float playerMaxLife;
    [SerializeField]
    private float playerMana;
    [SerializeField] 
    private float playerMaxMana;
    [SerializeField]
    private float playerDmg;
    [SerializeField]
    private float fireballDmg;
    [SerializeField]
    private float heavyDmg;
    [SerializeField]
    private int sceneSave;
    [SerializeField]
    private int maxJumps;

    public float PlayerLIFE
    {
        get { return playerLife; }
        set { playerLife = value; }
    }

    public float PlayerMaxLife
    {
        get { return playerMaxLife; }
        set { playerMaxLife = value; }
    }

    public float PlayerMana
    {
        get { return playerMana; }  
        set { playerMana = value; }
    }

    public float PlayerMaxMana
    {
        get { return playerMaxMana; }
        set { playerMaxMana = value; }
    }

    public float PlayerDmg
    {
        get { return playerDmg; }
        set { playerDmg = value; }
    }

    public float FireballDmg
    {
        get { return fireballDmg; }
        set { fireballDmg = value; }
    }
    public float HeavyDmg
    {
        get { return heavyDmg; }
        set { heavyDmg = value; }
    }

   public int SceneSave
   {
        get { return sceneSave; }
        set { sceneSave = value; }
   }

    public int MaxJumps
    {
        get { return maxJumps; }
        set { maxJumps = value; }
    }
}
