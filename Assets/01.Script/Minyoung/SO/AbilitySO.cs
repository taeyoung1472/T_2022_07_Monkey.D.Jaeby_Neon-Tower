using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Ability", menuName = "SO/Abilty")]
public class AbilitySO : ScriptableObject
{
    public enum Category
    {
        Player,
        Bullet,
        ETC
    }
    public Category myCategory;

    public enum AttackType
    {
        Basic,
        MultiShot,
        BouncingShot,
        ExplosionShot,
        PenetrationShot
    }
    public AttackType attackType;

    public bool stiffen;//°æÁ÷

    public int reflexPower;
    public int knockbackPower;
    public int multiShotCnt;
    public int hp;
    public int damage;
    public int speed;
    public int bulletSpeed;
    public int wallCnt;

    public Sprite sprite;
    public string abilTxt;
}
