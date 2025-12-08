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
    [SerializeField]
    private bool boss1; 
    [SerializeField]
    private bool boss2;
    [SerializeField]
    private bool canDash;
    [SerializeField]
    private bool hasFireBall;
    [SerializeField]
    private bool canCrouch;
    [SerializeField]
    private bool canGrabWall;
    [SerializeField]
    private bool canHeal;


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

    public bool Boss1
    {
        get { return boss1; }
        set { boss1 = value; }
    }

    public bool Boss2
    {
        get { return boss2; }
        set { boss2 = value; }
    }

    public bool CanDash
    {
        get { return canDash; }
        set { canDash = value; }
    }

    public bool HasFireBall
    {
        get { return hasFireBall; }
        set { hasFireBall = value; }
    }

    public bool CanCrouch
    {
        get { return canCrouch; }
        set { canCrouch = value; }
    }

    public bool CanGrabWall
    {
        get { return canGrabWall; }
        set { canGrabWall = value; }
    }

    public bool CanHeal
    {
        get { return canHeal; }
        set { canHeal = value; }
    }


    //Safe a veure si funciona
    [SerializeField] 
    private Vector3 lastCheckpointPos;
    public Vector3 LastCheckpointPos
    {
        get { return lastCheckpointPos; }
        set { lastCheckpointPos = value; }
    }
}
