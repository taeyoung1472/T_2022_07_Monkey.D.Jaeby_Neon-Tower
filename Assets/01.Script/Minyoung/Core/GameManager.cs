using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public PlayerStat playerStat;
    public BulletStat bulletStat;

    private void Awake()
    {
        Instance = this;
    }
}

[Serializable]
public class PlayerStat
{
    public int hp = 5;
    public int speed = 10;
    public int autoHealDelay = 16;
    public int stealHp;
    public int dashChance;
    public int dashGod;
}
[Serializable]
public class BulletStat
{
    public int knockback = 0;
    public int wallBounceCnt = 0;
    public int bulletSpd = 0;
    public int damage = 0;
    public int bulletDelay = 0;
    public int freeze = 0;

    public int multiShotCount = 0;
    public int penetrationShot = 0;
    public int explosion = 0;
}