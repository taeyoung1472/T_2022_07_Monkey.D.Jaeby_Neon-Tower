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
}
[Serializable]
public class BulletStat
{
    public int knockbackPower = 1;
    public int wallBounceCnt = 0;
    public int reflexPower = 5;
    public int bulletSpd = 5;
    public int damage = 1;
    public int multiShotCount;

    public bool penetrationShot;
    public bool ExplosionShot;
    public bool BouncingShot;
}