using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    #region
    [SerializeField]
    private int hp = 5;
    public int HP
    {
        get => hp;
        set
        {
            hp = value;
        }
    }

    private int damage = 1;
    public int Damage
    {
        get => damage;
        set
        {
            damage = value;
        }
    }
    private int speed = 10;
    public int Speed
    {
        get => speed;
        set
        {
            speed = value;
        }
    }
    #endregion 플레이어 관련

    #region

    private int bulletSpd = 5;

    public int BulletSpd
    {
        get => bulletSpd;
        set
        {
            bulletSpd = value;
        }
    }

    private int reflexPower = 5;
    public int ReflexPower
    {
        get => reflexPower;
        set
        {
            reflexPower = value;
        }
    }
    private int wallBounceCnt = 0;
    public int WallBounceCnt
    {
        get => wallBounceCnt;
        set
        {
            wallBounceCnt = value;
        }
    }
    private int knockbackPower = 1;
    public int KnowbackPower
    {
        get => knockbackPower;
        set
        {
            knockbackPower = value;
        }
    }
    #endregion 총알관련
    private void Awake()
    {
        Instance = this;
    }
}
