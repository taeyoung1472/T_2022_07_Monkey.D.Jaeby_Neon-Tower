using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerHan : MonoBehaviour
{
    public static GameManagerHan Instance;

    public int hp = 5;
    public int damage = 1;
    public int speed = 10;
    private void Awake()
    {
        Instance = this;
    }
}
