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

    public bool damage;
    public bool attackSpeed;
    public bool maxHp;
    public bool autoHeal;
    public bool bulletSpeed;
    public bool multiShoot;
    public bool penetration;
    public bool bulletBounce;
    public bool knockback;
    public bool bulletFreeze;
    public bool bulletExplosion;
    public bool stealHp;
    public bool dashChance;
    public bool dashGod;

    public Sprite sprite;
    public string descString;
}
